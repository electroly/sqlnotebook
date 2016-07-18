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
#include "gcroot.h"
#include "SqlNotebookCore.h"
using namespace SqlNotebookCore;
using namespace SqlNotebookCoreModules;
using namespace System::Data;
using namespace System::Data::SqlClient;
using namespace System::IO;
using namespace System::Linq;
using namespace System::Text;

private struct GenericTable {
    sqlite3_vtab Super;
    gcroot<GenericSqliteModule^> Module;

    GenericTable() {
        memset(&Super, 0, sizeof(Super));
        Module = nullptr;
    }

    void SetError(String^ message) {
        sqlite3_free(Super.zErrMsg);
        auto messageCstr = Util::CStr(message);
        Super.zErrMsg = sqlite3_mprintf("%s", messageCstr.c_str());
    }
};

private struct GenericCursor {
    sqlite3_vtab_cursor Super;
    GenericTable* Table;
    gcroot<IEnumerator<array<Object^>^>^> Enumerator;
    bool Eof;

    GenericCursor() {
        memset(&Super, 0, sizeof(Super));
        Table = nullptr;
        Enumerator = nullptr;
        Eof = false;
    }
};

static int GenericCreate(sqlite3* db, void* pAux, int argc, const char* const* argv, sqlite3_vtab** ppVTab, char** pzErr) {
    try {
        auto module = *(gcroot<GenericSqliteModule^>*)pAux;
        GenericTable* vtab = new GenericTable;
        vtab->Module = module;
        *ppVTab = &vtab->Super;
        auto sql = module->CreateTableSql;
        auto sqlCstr = Util::CStr(sql);
        g_SqliteCall(db, sqlite3_declare_vtab(db, sqlCstr.c_str()));
        return SQLITE_OK;
    } catch (Exception^) {
        return SQLITE_INTERNAL;
    }
}

static int GenericDestroy(sqlite3_vtab* pVTab) {
    delete (GenericTable*)pVTab;
    return SQLITE_OK;
}

static int GenericBestIndex(sqlite3_vtab* pVTab, sqlite3_index_info* info) {
    auto table = (GenericTable*)pVTab;
    auto filter = gcnew List<int>();
    
    // where clause
    int argvIndex = 1;
    for (int i = 0; i < info->nConstraint; i++) {
        auto colIndex = info->aConstraint[i].iColumn;
        auto op = info->aConstraint[i].op;
        if (colIndex == -1) {
            continue;
        } else if (!info->aConstraint[i].usable) {
            continue;
        } else if (colIndex > table->Module->HiddenColumnCount) {
            continue;
        } else if (op != SQLITE_INDEX_CONSTRAINT_EQ) {
            continue;
        }

        info->aConstraintUsage[i].argvIndex = argvIndex;
        info->aConstraintUsage[i].omit = true;

        filter->Add(colIndex);
        argvIndex++;
    }

    info->orderByConsumed = false;
    info->idxNum = 0;
    auto filterCstr = Util::CStr(String::Join(",", filter));
    info->idxStr = sqlite3_mprintf("%s", filterCstr.c_str());
    info->needToFreeIdxStr = true;
    info->estimatedRows = 1000;
    info->estimatedCost = 1000;
    return SQLITE_OK;
}

static int GenericOpen(sqlite3_vtab* pVTab, sqlite3_vtab_cursor** ppCursor) {
    auto cursor = new GenericCursor;
    cursor->Table = (GenericTable*)pVTab;
    *ppCursor = &cursor->Super;
    return SQLITE_OK;
}

static int GenericClose(sqlite3_vtab_cursor* pCur) {
    auto cursor = (GenericCursor*)pCur;
    delete cursor;
    return SQLITE_OK;
}

static int GenericNext(sqlite3_vtab_cursor* pCur) {
    auto cursor = (GenericCursor*)pCur;
    try {
        cursor->Eof = !cursor->Enumerator->MoveNext();
        return SQLITE_OK;
    } catch (Exception^ ex) {
        cursor->Table->SetError(ex->Message);
        return SQLITE_ERROR;
    }
}

static Object^ GetArg(sqlite3_value* arg) {
    switch (sqlite3_value_type(arg)) {
        case SQLITE_INTEGER:
            return (Int64)sqlite3_value_int64(arg);
        case SQLITE_FLOAT:
            return (Double)sqlite3_value_double(arg);
        case SQLITE_NULL:
            return DBNull::Value;
        case SQLITE_TEXT:
            return Util::Str((const wchar_t*)sqlite3_value_text16(arg));
        case SQLITE_BLOB:
        {
            auto cb = sqlite3_value_bytes(arg);
            auto inputArray = (Byte*)sqlite3_value_blob(arg);
            auto outputArray = gcnew array<Byte>(cb);
            Marshal::Copy(IntPtr(inputArray), outputArray, 0, cb);
            return outputArray;
        }
        default:
            throw gcnew Exception("Data type not supported.");
    }
}

static int GenericFilter(sqlite3_vtab_cursor* pCur, int idxNum, const char* idxStr, int argc, sqlite3_value** argv) {
    auto cursor = (GenericCursor*)pCur;
    try {
        auto filter = gcnew List<int>();
        for each (auto numStr in Util::Str(idxStr)->Split(',')) {
            if (numStr->Length > 0) {
                filter->Add(int::Parse(numStr));
            }
        }

        auto hiddenCount = cursor->Table->Module->HiddenColumnCount;
        auto hiddenValues = gcnew array<Object^>(hiddenCount);
        for (int i = 0; i < argc; i++) {
            hiddenValues[filter[i]] = GetArg(argv[i]);
        }

        cursor->Enumerator = cursor->Table->Module->Execute(hiddenValues)->GetEnumerator();
        cursor->Eof = false;
        return GenericNext(pCur);
    } catch (Exception^ ex) {
        cursor->Table->SetError(ex->Message);
        return SQLITE_ERROR;
    }
}

static int GenericEof(sqlite3_vtab_cursor* pCur) {
    auto cursor = (GenericCursor*)pCur;
    return cursor->Eof ? 1 : 0;
}

static int GenericColumn(sqlite3_vtab_cursor* pCur, sqlite3_context* ctx, int n) {
    auto cursor = (GenericCursor*)pCur;
    try {
        auto row = cursor->Enumerator->Current;
        Notebook::SqliteResult(ctx, row[n]);
        return SQLITE_OK;
    } catch (Exception^ ex) {
        cursor->Table->SetError(ex->Message);
        return SQLITE_ERROR;
    }
}

static int GenericRowid(sqlite3_vtab_cursor* pCur, sqlite3_int64* pRowid) {
    auto cursor = (GenericCursor*)pCur;
    try {
        Int64 hash = 0;
        auto arr = cursor->Enumerator->Current;
        for each (auto value in arr) {
            hash <<= 1;
            hash |= value->GetHashCode();
        }
        *pRowid = hash;
        return SQLITE_OK;
    } catch (Exception^ ex) {
        cursor->Table->SetError(ex->Message);
        return SQLITE_ERROR;
    }
}

static int GenericRename(sqlite3_vtab* pVtab, const char* zNew) {
    return SQLITE_OK; // don't care
}

static void DeleteGcrootModule(void* ptr) {
    delete (gcroot<GenericSqliteModule^>*)ptr;
}

void Notebook::InstallGenericModule(GenericSqliteModule^ impl) {
    auto staticModule = new sqlite3_module;
    _sqliteModules->Add(IntPtr(staticModule));

    staticModule->iVersion = 1;
    staticModule->xCreate = nullptr;
    staticModule->xConnect = GenericCreate;
    staticModule->xBestIndex = GenericBestIndex;
    staticModule->xDisconnect = GenericDestroy;
    staticModule->xDestroy = GenericDestroy;
    staticModule->xOpen = GenericOpen;
    staticModule->xClose = GenericClose;
    staticModule->xFilter = GenericFilter;
    staticModule->xNext = GenericNext;
    staticModule->xEof = GenericEof;
    staticModule->xColumn = GenericColumn;
    staticModule->xRowid = GenericRowid;
    staticModule->xRename = GenericRename;
    auto gcrootModule = new gcroot<GenericSqliteModule^>(impl);
    auto nameCstr = Util::CStr(impl->Name);
    SqliteCall(sqlite3_create_module_v2(_sqlite, nameCstr.c_str(), staticModule, gcrootModule, DeleteGcrootModule));
}
