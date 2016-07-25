// SQL Notebook
// Copyright (C) 2016 Brian Luft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#include <msclr/auto_handle.h>
#include "SqlNotebookCore.h"
using namespace SqlNotebookCore;
using namespace System::IO;
using namespace System::IO::Compression;
using namespace System::Text;
using namespace System::Runtime::InteropServices;
using namespace msclr;
using namespace Newtonsoft::Json;

#define SQLITE_ZIP_ENTRY_NAME "sqlite.db"
#define NOTEBOOK_ZIP_ENTRY_NAME "notebook.json"

Notebook::Notebook(String^ filePath, bool isNew) {
    _workingCopyFilePath = NotebookTempFiles::GetTempFilePath(".db");
    if (isNew) {
        _userData = gcnew NotebookUserData();
    } else {
        try {
            auto_handle<ZipArchive> zip(ZipFile::OpenRead(filePath));
            // load the sqlite database
            {
                auto entry = zip->GetEntry(SQLITE_ZIP_ENTRY_NAME);
                auto_handle<Stream> inStream(entry->Open());
                FileStream outStream(_workingCopyFilePath, FileMode::Create);
                inStream->CopyTo(%outStream);
            }
            // load the user items
            {
                auto entry = zip->GetEntry(NOTEBOOK_ZIP_ENTRY_NAME);
                auto_handle<Stream> inStream(entry->Open());
                StreamReader reader(inStream.get(), Encoding::UTF8);
                auto json = reader.ReadToEnd();
                _userData = (NotebookUserData^)JsonConvert::DeserializeObject(json, NotebookUserData::typeid);
            }
        } catch (Exception^) {
            File::Delete(_workingCopyFilePath);
            throw;
        }
    }

    _originalFilePath = filePath;
    _lock = gcnew Object();
    Invoke(gcnew Action(this, &Notebook::Init));
}

Notebook::~Notebook() {
    if (_isDisposed) {
        return;
    }
    this->!Notebook();
    _isDisposed = true;
}

Notebook::!Notebook() {
    if (_sqlite) {
        sqlite3_close_v2(_sqlite);
        _sqlite = nullptr;
    }
    for each (IntPtr ptr in _sqliteModules) {
        auto mod = (sqlite3_module*)(void*)ptr;
        delete mod;
    }
    _sqliteModules->Clear();
}

static Tuple<List<CustomTableFunction^>^, List<CustomScalarFunction^>^>^ FindCustomTableFunctions() {
    auto modules = gcnew List<CustomTableFunction^>();
    auto functions = gcnew List<CustomScalarFunction^>();
    auto assembly = CustomTableFunction::typeid->Assembly;
    auto types = assembly->GetExportedTypes();
    for each (auto type in types) {
        if (type->IsAbstract) {
            continue;
        }
        auto baseType = type->BaseType;
        while (baseType != nullptr) {
            if (baseType == CustomTableFunction::typeid) {
                modules->Add((CustomTableFunction^)Activator::CreateInstance(type));
                break;
            } else if (baseType == CustomScalarFunction::typeid) {
                functions->Add((CustomScalarFunction^)Activator::CreateInstance(type));
                break;
            }
            baseType = baseType->BaseType;
        }
    }
    return Tuple::Create(modules, functions);
}

void Notebook::Init() {
    auto filePathCstr = Util::CStr(_workingCopyFilePath);
    sqlite3* sqlite = nullptr;
    SqliteCall(sqlite3_open(filePathCstr.c_str(), &sqlite));
    _sqlite = sqlite;

    InstallPgModule();
    InstallMsModule();
    InstallMyModule();
    auto genericModulesAndFunctions = FindCustomTableFunctions();
    for each (auto genericModule in genericModulesAndFunctions->Item1) {
        InstallGenericModule(genericModule);
    }
    for each (auto genericFunction in genericModulesAndFunctions->Item2) {
        RegisterGenericFunction(genericFunction);
    }
    InstallCustomFunctions();
}

void Notebook::Invoke(Action^ action) {
    Monitor::Enter(_lock);
    try {
        action();
    } finally {
        Monitor::Exit(_lock);
    }
}

void Notebook::Execute(String^ sql) {
    Execute(sql, gcnew List<Object^>());
}

static IReadOnlyDictionary<String^, Object^>^ ToLowercaseKeys(IReadOnlyDictionary<String^, Object^>^ dict) {
    bool allLowercase = true;
    for each (auto key in dict->Keys) {
        if (key != key->ToLower()) {
            allLowercase = false;
            break;
        }
    }

    if (allLowercase) {
        return dict;
    } else {
        auto newDict = gcnew Dictionary<String^, Object^>();
        for each (auto pair in dict) {
            newDict->Add(pair.Key->ToLower(), pair.Value);
        }
        return newDict;
    }
}

void Notebook::Execute(String^ sql, IReadOnlyDictionary<String^, Object^>^ args) {
    QueryCore(sql, ToLowercaseKeys(args), nullptr, false, _sqlite, gcnew Func<bool>(this, &Notebook::GetCancelling));
}

void Notebook::Execute(String^ sql, IReadOnlyList<Object^>^ args) {
    QueryCore(sql, nullptr, args, false, _sqlite, gcnew Func<bool>(this, &Notebook::GetCancelling));
}

SimpleDataTable^ Notebook::Query(String^ sql) {
    return Query(sql, gcnew List<Object^>());
}

SimpleDataTable^ Notebook::Query(String^ sql, IReadOnlyDictionary<String^, Object^>^ args) {
    return QueryCore(sql, ToLowercaseKeys(args), nullptr, true, _sqlite,
        gcnew Func<bool>(this, &Notebook::GetCancelling));
}

SimpleDataTable^ Notebook::Query(String^ sql, IReadOnlyList<Object^>^ args) {
    return QueryCore(sql, nullptr, args, true, _sqlite, gcnew Func<bool>(this, &Notebook::GetCancelling));
}

// opens a new connection if another query is in progress.  the SQL command must be pure SQLite and must contain 
// a read-only query.
SimpleDataTable^ Notebook::SpecialReadOnlyQuery(String^ sql, IReadOnlyDictionary<String^, Object^>^ args) {
    if (Monitor::TryEnter(_lock)) {
        try {
            return Query(sql, args);
        } finally {
            Monitor::Exit(_lock);
        }
    } else {
        // another operation is in progress, so open a new connection for this query.
        auto filePathCstr = Util::CStr(_workingCopyFilePath);
        sqlite3* tempSqlite = nullptr;
        g_SqliteCall(tempSqlite, sqlite3_open_v2(filePathCstr.c_str(), &tempSqlite, SQLITE_OPEN_READONLY, nullptr));
        try {
            return QueryCore(sql, args, nullptr, true, tempSqlite, nullptr);
        } finally {
            sqlite3_close_v2(tempSqlite);
        }
    }
}

Object^ Notebook::QueryValue(String^ sql) {
    return QueryValue(sql, gcnew List<Object^>());
}

Object^ Notebook::QueryValue(String^ sql, IReadOnlyDictionary<String^, Object^>^ args) {
    auto dt = Query(sql, args);
    if (dt->Rows->Count == 1 && dt->Columns->Count == 1) {
        return dt->Rows[0]->GetValue(0);
    } else {
        return nullptr;
    }
}

Object^ Notebook::QueryValue(String^ sql, IReadOnlyList<Object^>^ args) {
    auto dt = Query(sql, args);
    if (dt->Rows->Count == 1 && dt->Columns->Count == 1) {
        return dt->Rows[0]->GetValue(0);
    } else {
        return nullptr;
    }
}

SimpleDataTable^ Notebook::QueryCore(String^ sql, IReadOnlyDictionary<String^, Object^>^ namedArgs,
IReadOnlyList<Object^>^ orderedArgs, bool returnResult, sqlite3* db, Func<bool>^ cancelling) {
    if (cancelling != nullptr && cancelling()) {
        throw gcnew OperationCanceledException();
    }

    // namedArgs has lowercase keys
    sqlite3_stmt* stmt = nullptr;
    try {
        // prepare the statement
        auto sqlWstr = Util::WStr(sql);
        auto sqlWstrLenB = sqlWstr.size() * sizeof(wchar_t);
        const void* unusedTail = nullptr;
        g_SqliteCall(db, sqlite3_prepare16_v2(db, sqlWstr.c_str(), (int)sqlWstrLenB, &stmt, &unusedTail));
        if (!stmt) {
            throw gcnew Exception("Invalid statement.");
        }

        // bind the arguments
        g_SqliteCall(db, sqlite3_clear_bindings(stmt));
        int paramCount = sqlite3_bind_parameter_count(stmt);
        for (int i = 1; i <= paramCount; i++) {
            Object^ value;

            if (namedArgs != nullptr) {
                auto paramName = Util::Str(sqlite3_bind_parameter_name(stmt, i));
                if (!namedArgs->TryGetValue(paramName->ToLower(), value)) {
                    auto errMsg = Util::Str("Missing value for SQL parameter \"") + paramName + Util::Str("\".");
                    throw gcnew ArgumentException(errMsg);
                }
            } else if (orderedArgs != nullptr) {
                value = orderedArgs[i - 1];
            } else {
                throw gcnew ArgumentException("namedArgs or orderedArgs must be non-null.");
            }

            if (value == nullptr) {
                g_SqliteCall(db, sqlite3_bind_null(stmt, i));
            } else {
                auto type = value->GetType();
                if (type == DBNull::typeid) {
                    g_SqliteCall(db, sqlite3_bind_null(stmt, i));
                } else if (type == Int32::typeid) {
                    int intValue = safe_cast<Int32>(value);
                    g_SqliteCall(db, sqlite3_bind_int(stmt, i, intValue));
                } else if (type == Int64::typeid) {
                    int64_t longValue = safe_cast<Int64>(value);
                    g_SqliteCall(db, sqlite3_bind_int64(stmt, i, longValue));
                } else if (type == Double::typeid) {
                    double dblValue = safe_cast<Double>(value);
                    g_SqliteCall(db, sqlite3_bind_double(stmt, i, dblValue));
                } else if (type == array<Byte>::typeid) {
                    auto arr = (array<Byte>^)value;
                    auto copy = (Byte*)malloc(arr->Length);
                    Marshal::Copy(arr, 0, IntPtr(copy), arr->Length);
                    g_SqliteCall(db, sqlite3_bind_blob64(stmt, i, copy, arr->Length, free));
                } else {
                    auto strValue = Util::WStr(value->ToString());
                    // sqlite will hang onto the string after the call returns so make a copy.  it
                    // will call our callback (we just use "free") to dispose of this copy.
                    auto strCopy = _wcsdup(strValue.c_str());
                    auto lenB = strValue.size() * sizeof(wchar_t);
                    g_SqliteCall(db, sqlite3_bind_text16(stmt, i, strCopy, (int)lenB, free));
                }
            }
        }

        // execute the statement
        auto columnNames = gcnew List<String^>();
        int columnCount = 0;
        if (returnResult) {
            columnCount = sqlite3_column_count(stmt);
            for (int i = 0; i < columnCount; i++) {
                auto columnName = Util::Str((const wchar_t*)sqlite3_column_name16(stmt, i));
                columnNames->Add(columnName);
            }
        }

        // read the results
        auto rows = gcnew List<array<Object^>^>();
        while (true) {
            if (cancelling != nullptr && cancelling()) {
                throw gcnew OperationCanceledException();
            }

            int ret = sqlite3_step(stmt);
            if (ret == SQLITE_DONE) {
                break;
            } else if (ret == SQLITE_ROW) {
                if (returnResult) {
                    array<Object^>^ rowData = gcnew array<Object^>(columnCount);
                    for (int i = 0; i < columnCount; i++) {
                        switch (sqlite3_column_type(stmt, i)) {
                            case SQLITE_INTEGER:
                                rowData[i] = sqlite3_column_int64(stmt, i);
                                break;
                            case SQLITE_FLOAT:
                                rowData[i] = sqlite3_column_double(stmt, i);
                                break;
                            case SQLITE_TEXT:
                                rowData[i] = Util::Str((const wchar_t*)sqlite3_column_text16(stmt, i));
                                break;
                            case SQLITE_BLOB: {
                                auto cb = sqlite3_column_bytes(stmt, i);
                                auto sourceBuffer = (Byte*)sqlite3_column_blob(stmt, i);
                                auto copy = gcnew array<Byte>(cb);
                                Marshal::Copy(IntPtr(sourceBuffer), copy, 0, cb);
                                rowData[i] = copy;
                                break;
                            }
                            case SQLITE_NULL:
                                rowData[i] = DBNull::Value;
                                break;
                            default:
                                throw gcnew InvalidOperationException(L"Unrecognized result from sqlite3_column_type().");
                        }
                    }
                    rows->Add(rowData);
                }
            } else if (ret == SQLITE_READONLY) {
                throw gcnew InvalidOperationException(L"Unable to write to a read-only notebook file.");
            } else if (ret == SQLITE_BUSY || ret == SQLITE_LOCKED) {
                throw gcnew InvalidOperationException(L"The notebook file is locked by another application.");
            } else if (ret == SQLITE_CORRUPT) {
                throw gcnew InvalidOperationException(L"The notebook file is corrupted.");
            } else if (ret == SQLITE_NOTADB) {
                throw gcnew InvalidOperationException(L"This is not an SQLite database file.");
            } else if (ret == SQLITE_INTERRUPT) {
                throw gcnew UserCancelException(L"SQL query canceled by the user.");
            } else if (ret == SQLITE_ERROR) {
                g_SqliteCall(db, SQLITE_ERROR);
            } else {
                throw gcnew InvalidOperationException(String::Format(L"Unrecognized result ({0}) from sqlite3_step().", ret));
            }
        }

        if (returnResult) {
            return gcnew SimpleDataTable(columnNames, rows);
        } else {
            return nullptr;
        }
    } finally {
        sqlite3_finalize(stmt);
    }
}

void Notebook::SqliteCall(int result) {
    g_SqliteCall(_sqlite, result);
}

void g_SqliteCall(sqlite3* sqlite, int result) {
    // handle the errcode by throwing if non-SQLITE_OK
    if (result != SQLITE_OK) {
        if (sqlite == nullptr) {
            throw gcnew SqliteException(String::Format("SQLite error {0}", result));
        } else {
            auto msg = Util::Str((const wchar_t*)sqlite3_errmsg16(sqlite));
            throw gcnew SqliteException(msg);
        }
    }
}

void Notebook::Save() {
    SqliteCall(sqlite3_close(_sqlite));
    _sqlite = nullptr;

    try {
        if (File::Exists(_originalFilePath)) {
            File::Delete(_originalFilePath);
        }
        auto_handle<ZipArchive> archive(ZipFile::Open(_originalFilePath, ZipArchiveMode::Create));
        // write the sqlite database
        {
            auto entry = archive->CreateEntry(SQLITE_ZIP_ENTRY_NAME, CompressionLevel::Fastest);
            auto_handle<Stream> outStream(entry->Open());
            FileStream inStream(_workingCopyFilePath, FileMode::Open);
            inStream.CopyTo(outStream.get());
        }
        // write the notebook items
        {
            auto entry = archive->CreateEntry(NOTEBOOK_ZIP_ENTRY_NAME, CompressionLevel::Fastest);
            auto_handle<Stream> outStream(entry->Open());
            auto json = JsonConvert::SerializeObject(_userData);
            StreamWriter writer(outStream.get(), Encoding::UTF8);
            writer.Write(json);
        }
    } finally {
        Init();
    }
}

void Notebook::SaveAs(String^ filePath) {
    _originalFilePath = filePath;
    Save();
}

String^ Notebook::GetFilePath() {
    return _originalFilePath;
}

static int GetToken(const char* str, int* oldPos, int* pos, int len) {
    const int TK_SPACE = (int)TokenType::Space;
    if (*pos >= len) {
        return 0;
    }
    int tokenType, tokenLen;
    do {
        tokenLen = SxGetToken((const unsigned char*)&str[*pos], &tokenType);
        *oldPos = *pos;
        *pos += tokenLen;
    } while (tokenType == TK_SPACE && *pos < len);

    return tokenType == TK_SPACE ? 0 : tokenType;
}

IReadOnlyList<Token^>^ Notebook::Tokenize(String^ input) {
    Monitor::Enter(_tokenizeLock);
    try {
        auto str = Util::CStr(input);
        auto cstr = str.c_str();
        auto list = gcnew List<Token^>();
        int tokenType = 0, oldPos = 0, pos = 0, len = (int)str.length();
        while ((tokenType = GetToken(cstr, &oldPos, &pos, len)) > 0) {
            char* utf8Token = (char*)calloc(pos - oldPos + 1, sizeof(char));
            memcpy(utf8Token, &cstr[oldPos], pos - oldPos);

            auto token = gcnew Token();
            token->Type = (TokenType)tokenType;
            token->Text = Util::Str(utf8Token);
            token->Utf8Start = oldPos;
            token->Utf8Length = pos - oldPos;
            list->Add(token);

            free(utf8Token);
            oldPos = pos;
        }
        return list;
    } finally {
        Monitor::Exit(_tokenizeLock);
    }
}

String^ Token::ToString() {
    return String::Format("{0}: \"{1}\"", Type, Text);
}

String^ Notebook::FindLongestValidStatementPrefix(String^ input) {
    const int TK_SEMI = 1;
    auto str = Util::CStr(input);
    auto cstr = str.c_str();
    auto parser = SxParserAlloc();
    auto parse = SxParseAlloc(_sqlite, cstr);

    int tokenType = 0, oldPos = 0, pos = 0, len = (int)str.length();
    while ((tokenType = GetToken(cstr, &oldPos, &pos, len)) > 0) {
        SxAdvanceParse(parser, tokenType, &cstr[oldPos], pos - oldPos, parse);
        if (SxGetParseErrorCount(parse) > 0) {
            break;
        }
        oldPos = pos;
        if (tokenType == TK_SEMI) {
            break;
        }
    }

    auto prefix = str.substr(0, oldPos);
    auto nprefix = Util::Str(prefix.c_str());
    SxParserFree(_sqlite, parser, parse);
    return nprefix;
}

void Notebook::BeginUserCancel() {
    _cancelling = true;
    sqlite3_interrupt(_sqlite);
}

void Notebook::EndUserCancel() {
    _cancelling = false;
}

static void ResultText16(sqlite3_context* ctx, String^ str) {
    auto wstr = Util::WStr(str);
    auto wstrCopy = _wcsdup(wstr.c_str());
    auto lenB = wstr.size() * sizeof(wchar_t);
    sqlite3_result_text16(ctx, wstrCopy, (int)lenB, free);
}

static void ResultBlob(sqlite3_context* ctx, array<Byte>^ bytes) {
    auto cb = bytes->Length;
    auto buffer = (Byte*)malloc(cb);
    Marshal::Copy(bytes, 0, IntPtr(buffer), cb);
    sqlite3_result_blob64(ctx, buffer, cb, free);
}

void Notebook::SqliteResult(sqlite3_context* ctx, Object^ value) {
    if (value == nullptr) {
        sqlite3_result_null(ctx);
        return;
    }
    auto type = value->GetType();
    if (type == DBNull::typeid) {
        sqlite3_result_null(ctx);
    } else if (type == Int16::typeid) {
        sqlite3_result_int(ctx, (Int16)value);
    } else if (type == Int32::typeid) {
        sqlite3_result_int(ctx, (Int32)value);
    } else if (type == Int64::typeid) {
        sqlite3_result_int64(ctx, (Int64)value);
    } else if (type == Byte::typeid) {
        sqlite3_result_int(ctx, (Byte)value);
    } else if (type == Single::typeid) {
        sqlite3_result_double(ctx, (Single)value);
    } else if (type == Double::typeid) {
        sqlite3_result_double(ctx, (Double)value);
    } else if (type == Decimal::typeid) {
        sqlite3_result_double(ctx, (double)(Decimal)value);
    } else if (type == String::typeid) {
        ResultText16(ctx, (String^)value);
    } else if (type == Char::typeid) {
        ResultText16(ctx, gcnew String((Char)value, 1));
    } else if (type == Boolean::typeid) {
        sqlite3_result_int(ctx, (Boolean)value ? 1 : 0);
    } else if (type == NpgsqlTypes::NpgsqlDate::typeid) {
        ResultText16(ctx, ((DateTime)(NpgsqlTypes::NpgsqlDate)value).ToString("yyyy-MM-dd"));
    } else if (type == NpgsqlTypes::NpgsqlDateTime::typeid) {
        ResultText16(ctx, ((DateTime)(NpgsqlTypes::NpgsqlDateTime)value).ToString("yyyy-MM-dd"));
    } else if (type == DateTime::typeid) {
        ResultText16(ctx, ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff zzz"));
    } else if (type == DateTimeOffset::typeid) {
        ResultText16(ctx, ((DateTimeOffset)value).ToString("yyyy-MM-dd HH:mm:ss.fff zzz"));
    } else if (type == array<Byte>::typeid) {
        ResultBlob(ctx, (array<Byte>^)value);
    } else {
        ResultText16(ctx, value->ToString());
    }
}

IReadOnlyDictionary<String^, String^>^ Notebook::GetScripts() {
    auto dict = gcnew Dictionary<String^, String^>();
    for each (auto item in UserData->Items) {
        if (item->Type == "Script") {
            dict->Add(item->Name->ToLower(), item->Data == nullptr ? "" : item->Data);
        }
    }
    return dict;
}

bool Notebook::IsTransactionActive() {
    return sqlite3_get_autocommit(_sqlite) == 0;
}

bool Notebook::GetCancelling() {
    return _cancelling;
}

SimpleDataTable::SimpleDataTable(IReadOnlyList<String^>^ columns, IReadOnlyList<array<Object^>^>^ rows) {
    Columns = columns;
    Rows = rows;
    
    auto dict = gcnew Dictionary<String^, int>();
    int i = 0;
    for each (auto columnName in columns) {
        dict[columnName] = i++;
    }
    _columnIndices = dict;
}

Object^ SimpleDataTable::Get(int rowNumber, String^ column) {
    auto row = Rows[rowNumber];
    auto colIndex = _columnIndices[column];
    return row[colIndex];
}

int SimpleDataTable::GetIndex(String^ column) {
    return _columnIndices[column];
}

void NotebookTempFiles::Init() {
    if (!_path) {
        _path = Path::Combine(Path::GetTempPath(), "SqlNotebookTemp");
        Directory::CreateDirectory(_path);
        DeleteFiles();
    }
}

String^ NotebookTempFiles::GetTempFilePath(String^ extension) {
    Init();
    extension = extension->TrimStart('.');
    auto filePath = Path::Combine(_path, Guid::NewGuid().ToString() + "." + extension);
    File::WriteAllBytes(filePath, gcnew array<Byte>(0));
    File::AppendAllText(Path::Combine(_path, "delete.lst"), String::Format("{0}\r\n", filePath));
    return filePath;
}

void NotebookTempFiles::DeleteFiles() {
    auto deleteLstPath = Path::Combine(_path, "delete.lst");
    if (File::Exists(deleteLstPath)) {
        auto deleteFilePaths = File::ReadAllLines(deleteLstPath);
        auto couldNotDelete = gcnew List<String^>();
        for each (auto filePath in deleteFilePaths) {
            if (filePath->Length > 0) {
                try {
                    File::Delete(filePath);
                } catch (Exception^) {
                    couldNotDelete->Add(filePath);
                }
            }
        }
        // try again next time with the couldNotDelete files
        File::WriteAllLines(deleteLstPath, couldNotDelete);
    }
}