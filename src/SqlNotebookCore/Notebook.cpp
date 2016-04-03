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

#include "SqlNotebookCore.h"
using namespace SqlNotebookCore;

Notebook::Notebook(String^ filePath) {
    _filePath = filePath;
    _threadCancellationTokenSource = gcnew CancellationTokenSource();
    _threadQueue = gcnew BlockingCollection<Action^>();
    _thread = Task::Run(gcnew Action(this, &Notebook::SqliteThread));
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
}

void Notebook::SqliteThread() {
    auto stopToken = _threadCancellationTokenSource->Token;
    try {
        while (!stopToken.IsCancellationRequested) {
            auto action = _threadQueue->Take(stopToken);
            action();
        }
    } catch (OperationCanceledException^) {
        // ok
    }
}

void Notebook::Init() {
    auto filePathWstr = Util::WStr(_filePath);
    sqlite3* sqlite;
    SqliteCall(sqlite3_open16(filePathWstr.c_str(), &sqlite));
    _sqlite = sqlite;

    // ensure we have an exclusive lock on the database file
    Execute("PRAGMA locking_mode = EXCLUSIVE");
    Execute("BEGIN EXCLUSIVE");
    Execute("COMMIT");

    InstallCsvModule();
    InstallPgModule();
}

private ref class InvokeClosure {
    public:
    Action^ UserAction = nullptr;
    BlockingCollection<Action^>^ ThreadQueue = nullptr;
    Exception^ CaughtException = nullptr;
    
    void Invoke() {
        _sem = gcnew Semaphore(0, 1);
        ThreadQueue->Add(gcnew Action(this, &InvokeClosure::RunOnThread));
        // RunOnThread will release the semaphore when it completes
        _sem->WaitOne();
        if (CaughtException != nullptr) {
            throw CaughtException;
        }
    }

    private:
    Semaphore^ _sem;
    
    void RunOnThread() {
        try {
            UserAction();
        } catch (Exception^ ex) {
            CaughtException = ex;
        }
        _sem->Release();
    }
};

void Notebook::Invoke(Action^ action) {
    auto x = gcnew InvokeClosure();
    x->UserAction = action;
    x->ThreadQueue = _threadQueue;
    x->Invoke();
}

void Notebook::Execute(String^ sql) {
    Execute(sql, gcnew List<Object^>());
}

void Notebook::Execute(String^ sql, IReadOnlyDictionary<String^, Object^>^ args) {
    QueryCore(sql, args, nullptr, false);
}

void Notebook::Execute(String^ sql, IReadOnlyList<Object^>^ args) {
    QueryCore(sql, nullptr, args, false);
}

DataTable^ Notebook::Query(String^ sql) {
    return Query(sql, gcnew List<Object^>());
}

DataTable^ Notebook::Query(String^ sql, IReadOnlyDictionary<String^, Object^>^ args) {
    return QueryCore(sql, args, nullptr, true);
}

DataTable^ Notebook::Query(String^ sql, IReadOnlyList<Object^>^ args) {
    return QueryCore(sql, nullptr, args, true);
}

DataTable^ Notebook::QueryCore(String^ sql, IReadOnlyDictionary<String^, Object^>^ namedArgs,
        IReadOnlyList<Object^>^ orderedArgs, bool returnResult) {
    sqlite3_stmt* stmt = nullptr;
    try {
        // prepare the statement
        auto sqlWstr = Util::WStr(sql);
        auto sqlWstrLenB = sqlWstr.size() * sizeof(wchar_t);
        const void* unusedTail = nullptr;
        SqliteCall(sqlite3_prepare16_v2(_sqlite, sqlWstr.c_str(), (int)sqlWstrLenB, &stmt, &unusedTail));
        if (!stmt) {
            throw gcnew Exception("Invalid statement.");
        }

        // bind the arguments
        SqliteCall(sqlite3_clear_bindings(stmt));
        int paramCount = sqlite3_bind_parameter_count(stmt);
        for (int i = 1; i <= paramCount; i++) {
            Object^ value;

            if (namedArgs != nullptr) {
                auto paramName = Util::Str(sqlite3_bind_parameter_name(stmt, i));
                if (!namedArgs->TryGetValue(paramName, value)) {
                    auto errMsg = Util::Str("Missing value for SQL parameter \"") + paramName + Util::Str("\".");
                    throw gcnew ArgumentException(errMsg);
                }
            } else if (orderedArgs != nullptr) {
                value = orderedArgs[i];
            } else {
                throw gcnew ArgumentException("namedArgs or orderedArgs must be non-null.");
            }

            auto type = value->GetType();
            if (type == int::typeid) {
                auto intValue = safe_cast<int>(value);
                SqliteCall(sqlite3_bind_int(stmt, i, intValue));
            } else if (type == long::typeid) {
                auto longValue = safe_cast<long>(value);
                SqliteCall(sqlite3_bind_int64(stmt, i, longValue));
            } else if (type == double::typeid) {
                auto dblValue = safe_cast<double>(value);
                SqliteCall(sqlite3_bind_double(stmt, i, dblValue));
            } else {
                auto strValue = Util::WStr(value->ToString());
                // sqlite will hang onto the string after the call returns so make a copy.  it
                // will call our callback (we just use "free") to dispose of this copy.
                auto strCopy = _wcsdup(strValue.c_str());
                auto lenB = strValue.size() * sizeof(wchar_t);
                SqliteCall(sqlite3_bind_text16(stmt, i, strCopy, (int)lenB, free));
            }
        }

        // execute the statement
        auto table = gcnew DataTable();
        int columnCount = 0;
        if (returnResult) {
            columnCount = sqlite3_column_count(stmt);
            for (int i = 0; i < columnCount; i++) {
                auto columnName = Util::Str((const wchar_t*)sqlite3_column_name16(stmt, i));
                table->Columns->Add(columnName);
            }
        }

        // read the results into a DataTable
        while (true) {
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
                            case SQLITE_BLOB:
                                rowData[i] = Util::Str("BLOB"); // not supported
                                break;
                            case SQLITE_NULL:
                                rowData[i] = DBNull::Value;
                                break;
                            default:
                                delete table;
                                throw gcnew InvalidOperationException(L"Unrecognized result from sqlite3_column_type().");
                        }
                    }
                    table->Rows->Add(rowData);
                }
            } else if (ret == SQLITE_READONLY) {
                delete table;
                throw gcnew InvalidOperationException(L"Unable to write to a read-only notebook file.");
            } else if (ret == SQLITE_BUSY || ret == SQLITE_LOCKED) {
                delete table;
                throw gcnew InvalidOperationException(L"The notebook file is locked by another application.");
            } else if (ret == SQLITE_CORRUPT) {
                delete table;
                throw gcnew InvalidOperationException(L"The notebook file is corrupted.");
            } else if (ret == SQLITE_NOTADB) {
                delete table;
                throw gcnew InvalidOperationException(L"This is not an SQLite database file.");
            } else if (ret == SQLITE_ERROR) {
                delete table;
                SqliteCall(SQLITE_ERROR);
            } else {
                delete table;
                throw gcnew InvalidOperationException(String::Format(L"Unrecognized result ({0}) from sqlite3_step().", ret));
            }
        }

        if (returnResult) {
            return table;
        } else {
            delete table;
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
        auto msg = Util::Str((const wchar_t*)sqlite3_errmsg16(sqlite));
        throw gcnew SqliteException(msg);
    }
}

void Notebook::MoveTo(String^ newFilePath) {
    SqliteCall(sqlite3_close_v2(_sqlite));
    _sqlite = nullptr;
    System::IO::File::Copy(_filePath, newFilePath, true);
    _filePath = newFilePath;
    Init();
}

String^ Notebook::GetFilePath() {
    return _filePath;
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
}

String^ Token::ToString() {
    return String::Format("{0}: \"{1}\"", Type, Text);
}

String^ Notebook::FindLongestValidStatementPrefix(String^ input) {
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
    }

    SxParserFree(_sqlite, parser, parse);

    auto prefix = str.substr(0, oldPos);
    return Util::Str(prefix.c_str());
}
