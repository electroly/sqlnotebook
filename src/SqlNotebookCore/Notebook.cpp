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

void Notebook::Init() {
    auto filePathCstr = Util::CStr(_workingCopyFilePath);
    sqlite3* sqlite = nullptr;
    SqliteCall(sqlite3_open(filePathCstr.c_str(), &sqlite));
    _sqlite = sqlite;

    InstallPgModule();
    InstallMsModule();
    InstallMyModule();
    InstallGenericModule(gcnew ListFilesModule());
    InstallGenericModule(gcnew RangeModule());
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

// the blob format for arrays is:
// - 32-bit integer: number of elements
// - for each element:
//    - 32-bit integer: length of the element record to follow (excluding this size integer itself)
//    - 1 byte: SQLITE_INTEGER/FLOAT/BLOB/NULL/TEXT
//    - for null: nothing
//    - for integer: the 64-bit integer value
//    - for float: the 64-bit floating point value
//    - for text: a 32-bit integer byte count of the UTF-8 text to follow, then the UTF-8 text
//    - for blob: a 32-bit integer byte count of the blob to follow, then the blob data

// write the element records (but not the initial count)
static void ConvertToSqlArrayCore(IReadOnlyList<Object^>^ objects, BinaryWriter^ writer) {
    const int cbByte = sizeof(Byte);
    const int cbInt32 = sizeof(Int32);
    const int cbInt64 = sizeof(Int64);
    const int cbDouble = sizeof(Double);

    for (int i = 0; i < objects->Count; i++) {
        auto o = objects[i];
        auto t = o->GetType();
        if (t == Int64::typeid) {
            writer->Write((Int32)(cbByte + cbInt64));
            writer->Write((Byte)SQLITE_INTEGER);
            writer->Write((Int64)o);
        } else if (t == Double::typeid) {
            writer->Write((Int32)(cbByte + cbDouble));
            writer->Write((Byte)SQLITE_FLOAT);
            writer->Write((Double)o);
        } else if (t == String::typeid) {
            auto utf8 = Encoding::UTF8->GetBytes((String^)o);
            writer->Write((Int32)(cbByte + cbInt32 + utf8->Length));
            writer->Write((Byte)SQLITE_TEXT);
            writer->Write((Int32)(utf8->Length));
            writer->Write(utf8);
        } else if (t == array<Byte>::typeid) {
            auto bytes = (array<Byte>^)o;
            writer->Write((Int32)(cbByte + cbInt32 + bytes->Length));
            writer->Write((Byte)SQLITE_BLOB);
            writer->Write((Int32)(bytes->Length));
            writer->Write(bytes);
        } else if (t == DBNull::typeid) {
            writer->Write((Int32)(cbByte));
            writer->Write((Byte)SQLITE_NULL);
        } else {
            throw gcnew ArgumentException(String::Format(
                "Unrecognized object type \"{0}\" found at index {1} when building SQL array.", t->Name, i));
        }
    }
}

array<Byte>^ Notebook::ConvertToSqlArray(IReadOnlyList<Object^>^ objects) {
    MemoryStream memoryStream;
    BinaryWriter writer{ %memoryStream };
    writer.Write((Int32)objects->Count);
    ConvertToSqlArrayCore(objects, %writer);
    return memoryStream.ToArray();
}

static Int32 ReadInt32(array<Byte>^ blob, int offset) {
    if (offset + sizeof(Int32) > blob->Length) {
        throw gcnew Exception("Unexpected end of blob data.");
    } else {
        Int32 value;
        Marshal::Copy(blob, offset, IntPtr(&value), sizeof(Int32));
        return value;
    }
}

static Int64 ReadInt64(array<Byte>^ blob, int offset) {
    if (offset + sizeof(Int64) > blob->Length) {
        throw gcnew Exception("Unexpected end of blob data.");
    } else {
        Int64 value;
        Marshal::Copy(blob, offset, IntPtr(&value), sizeof(Int64));
        return value;
    }
}

static double ReadDouble(array<Byte>^ blob, int offset) {
    if (offset + sizeof(Double) > blob->Length) {
        throw gcnew Exception("Unexpected end of blob data.");
    } else {
        Double value;
        Marshal::Copy(blob, 0, IntPtr(&value), sizeof(Double));
        return value;
    }
}

static Byte ReadByte(array<Byte>^ blob, int offset) {
    if (offset + sizeof(Byte) > blob->Length) {
        throw gcnew Exception("Unexpected end of blob data.");
    } else {
        return blob[offset];
    }
}

int Notebook::GetArrayCount(array<Byte>^ arrayBlob) {
    if (arrayBlob->Length < sizeof(Int32)) {
        throw gcnew Exception("This blob is not an array.");
    } else {
        return ReadInt32(arrayBlob, 0);
    }
}

static Object^ ReadArrayElement(array<Byte>^ arrayBlob, int& position) {
    auto elementLength = ReadInt32(arrayBlob, position);
    position += sizeof(Int32);
    auto dataType = ReadByte(arrayBlob, position);
    position += sizeof(Byte);
    switch (dataType) {
        case SQLITE_INTEGER: {
            auto value = ReadInt64(arrayBlob, position);
            position += sizeof(Int64);
            return value;
        }

        case SQLITE_FLOAT: {
            auto value = ReadDouble(arrayBlob, position);
            position += sizeof(Double);
            return value;
        }

        case SQLITE_TEXT: {
            auto cbText = ReadInt32(arrayBlob, position);
            position += sizeof(Int32);
            Byte* utf8 = new Byte[cbText + 1];
            utf8[cbText] = 0;
            Marshal::Copy(arrayBlob, position, IntPtr(utf8), cbText);
            position += cbText;
            auto str = Util::Str((const char*)utf8);
            delete utf8;
            return str;
        }

        case SQLITE_BLOB: {
            auto cbBlob = ReadInt32(arrayBlob, position);
            position += sizeof(Int32);
            auto blob = gcnew array<Byte>(cbBlob);
            Array::Copy(arrayBlob, position, blob, 0, (int)cbBlob);
            position += cbBlob;
            return blob;
        }

        case SQLITE_NULL:
            return DBNull::Value;

        default:
            throw gcnew Exception(String::Format("Unrecognized data type in array blob: {0}", dataType));
    }
}

Object^ Notebook::GetArrayElement(array<Byte>^ arrayBlob, int elementIndex) {
    auto count = GetArrayCount(arrayBlob);
    if (elementIndex < 0 || elementIndex >= count) {
        throw gcnew Exception(String::Format(
            "The index {0} is out of bounds for the array, which has {1} elements.", elementIndex, count));
    }

    int position = sizeof(Int32);
    for (int i = 0; i < elementIndex; i++) {
        // skip this element
        auto skipElementLength = ReadInt32(arrayBlob, position);
        position += sizeof(Int32) + skipElementLength;
    }

    return ReadArrayElement(arrayBlob, position);
}

array<Object^>^ Notebook::GetArrayElements(array<Byte>^ arrayBlob) {
    auto count = GetArrayCount(arrayBlob);
    auto output = gcnew array<Object^>(count);
    int position = sizeof(Int32);
    for (int i = 0; i < count; i++) {
        output[i] = ReadArrayElement(arrayBlob, position);
    }
    return output;
}

array<Byte>^ Notebook::SliceArrayElements(array<Byte>^ originalArrayBlob, int index, int removeElements,
IReadOnlyList<Object^>^ insertElements) {
    auto oldCount = GetArrayCount(originalArrayBlob);
    if (index < 0 || index > oldCount) {
        throw gcnew Exception(String::Format(
            "Array index {0} is out of range. The array has {1} elements.",
            index, oldCount));
    }
    if (index + removeElements > oldCount) {
        throw gcnew Exception(String::Format(
            "Array index {0} - removed elements {1} must not exceed the length of the array.",
            index, removeElements));
    }

    auto newCount = oldCount - removeElements + insertElements->Count;
    MemoryStream memoryStream;
    BinaryWriter writer { %memoryStream };
    writer.Write((Int32)newCount);

    // copy the elements up to the edit point as-is
    int position = sizeof(Int32);
    for (int i = 0; i < index; i++) {
        auto skipLength = ReadInt32(originalArrayBlob, position);
        writer.Write(originalArrayBlob, position, sizeof(Int32) + skipLength);
        position += sizeof(Int32) + skipLength;
    }

    // skip the removed elements
    for (int i = index; i < index + removeElements; i++) {
        auto skipLength = ReadInt32(originalArrayBlob, position);
        position += sizeof(Int32) + skipLength;
    }

    // insert the new elements
    ConvertToSqlArrayCore(insertElements, %writer);

    // copy the rest of the elements as-is
    for (int i = index + removeElements; i < oldCount; i++) {
        auto skipLength = ReadInt32(originalArrayBlob, position);
        writer.Write(originalArrayBlob, position, sizeof(Int32) + skipLength);
        position += sizeof(Int32) + skipLength;
    }

    return memoryStream.ToArray();
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