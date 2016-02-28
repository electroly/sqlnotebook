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
using namespace System::Runtime::InteropServices;

Notebook::Notebook(String^ filePath) {
    _filePath = filePath;
    _modules = gcnew List<RegisteredModule^>();

    auto filePathWstr = Util::WStr(filePath);
    sqlite3* sqlite;
    SqliteCall(sqlite3_open16(filePathWstr.c_str(), &sqlite));
    _sqlite = sqlite;
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
    if (_modules) {
        for each (auto rm in _modules) {
            delete rm;
        }
        _modules = nullptr;
    }
}

void Notebook::RegisterModule(String^ moduleName, ISqliteModule^ module) {
    //TODO
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
    // prepare the statement
    auto sqlWstr = Util::WStr(sql);
    auto sqlWstrLenB = sqlWstr.size() * sizeof(wchar_t);
    sqlite3_stmt* stmt;
    const void* unusedTail;
    SqliteCall(sqlite3_prepare16_v2(_sqlite, sqlWstr.c_str(), (int)sqlWstrLenB, &stmt, &unusedTail));

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
            SqliteCall(sqlite3_bind_text16(stmt, i, strValue.c_str(), (int)lenB, free));
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
        } else if (ret == SQLITE_ERROR) {
            SqliteCall(SQLITE_ERROR);
        } else {
            delete table;
            throw gcnew InvalidOperationException(L"Unrecognized result from sqlite3_step().");
        }
    }

    if (returnResult) {
        return table;
    } else {
        delete table;
        return nullptr;
    }
}

void Notebook::SqliteCall(int result) {
    if (result != SQLITE_OK) {
        auto msg = Util::Str((const wchar_t*)sqlite3_errmsg16(_sqlite));
        throw gcnew SqliteException(msg);
    }
}

SqliteException::SqliteException(String^ message)
    : Exception(message) {
}

RegisteredModule::RegisteredModule(ISqliteModule^ clientModule, NativeSqliteModule* sqliteModule) {
    _clientModule = clientModule;
    _sqliteModule = sqliteModule;
}

RegisteredModule::~RegisteredModule() {
    if (_isDisposed) {
        return;
    }
    this->!RegisteredModule();
    _isDisposed = true;
}

RegisteredModule::!RegisteredModule() {
    if (_clientModule) {
        delete _clientModule;
        _clientModule = nullptr;
    }
    if (_sqliteModule) {
        delete _sqliteModule;
        _sqliteModule = nullptr;
    }
}
