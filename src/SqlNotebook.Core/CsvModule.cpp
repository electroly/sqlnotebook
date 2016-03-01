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
using namespace System::IO;
using namespace Microsoft::VisualBasic::FileIO;
using namespace System::Text;
using namespace System::Text::RegularExpressions;

static sqlite3_module s_csvModule = { 0 };

private struct CsvTable {
    sqlite3_vtab Super;
    std::vector<std::wstring> ColumnNames;
    std::vector<std::vector<std::wstring>> Rows;
    CsvTable() {
        memset(&Super, 0, sizeof(Super));
    }
};

private struct CsvCursor {
    sqlite3_vtab_cursor Super;
    CsvTable* Table;
    int RowIndex;
    CsvCursor() {
        memset(&Super, 0, sizeof(Super));
        Table = nullptr;
        RowIndex = 0;
    }
};

static String^ CreateUniqueName(String^ prefix, List<String^>^ existingNames) {
    auto name = prefix; // first try it without a number on the end
    int suffix = 2;
    while (existingNames->Contains(name)) {
        name = prefix + (suffix++);
    }
    return name;
}

static int CsvCreate(sqlite3* db, void* pAux, int argc, const char* const* argv, sqlite3_vtab** ppVTab, char** pzErr) {
    // from https://www.sqlite.org/vtab.html regarding argv:
    //      The first string, argv[0], is the name of the module being invoked. The module name is the name provided
    //      as the second argument to sqlite3_create_module() and as the argument to the USING clause of the CREATE
    //      VIRTUAL TABLE statement that is running. The second, argv[3], is the name of the database in which the new
    //      virtual table is being created. The database name is "main" for the primary database, or "temp" for TEMP
    //      database, or the name given at the end of the ATTACH statement for attached databases. The third element
    //      of the array, argv[4], is the name of the new virtual table, as specified following the TABLE keyword in
    //      the CREATE VIRTUAL TABLE statement. If present, the fourth and subsequent strings in the argv[] array
    //      report the arguments to the module name in the CREATE VIRTUAL TABLE statement.
    // our custom arguments are:
    //      argv[3] filePath: full path of the CSV file on disk
    //      argv[4] headerFlag: "HEADER" or "NO_HEADER"

    CsvTable* vtab = nullptr;

    try {
        if (argc != 5) {
            throw gcnew Exception("Wrong number of arguments.");
        }
        auto filePath = Util::Str(argv[3])->Trim(L'\'');
        auto headerFlag = Util::Str(argv[4])->ToUpper();
        if (!File::Exists(filePath)) {
            throw gcnew Exception("Could not find the file: " + filePath);
        }
        if (headerFlag != "HEADER" && headerFlag != "NO_HEADER") {
            throw gcnew Exception("Argument #2 must be HEADER or NO_HEADER.");
        }
        bool hasHeader = headerFlag->Equals("HEADER");

        // parse file
        List<String^>^ columnNames = nullptr;
        List<array<String^>^>^ rows = nullptr;
        TextFieldParser parser(filePath); // disposable
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;
        columnNames = gcnew List<String^>();
        if (hasHeader) {
            auto cells = parser.ReadFields();
            auto nonAlphaNumericUnderscoreRegex = gcnew Regex("[^A-Za-z0-9_]");
            for each (auto cell in cells) {
                auto prefix = nonAlphaNumericUnderscoreRegex->Replace(cell, "_");
                auto name = CreateUniqueName(prefix, columnNames);
                columnNames->Add(name);
            }
        }
        rows = gcnew List<array<String^>^>();
        while (!parser.EndOfData) {
            auto cells = parser.ReadFields();
            rows->Add(cells);
            // this row may have more cells than any previous row did.  if so, then assign default names to the extra columns.
            for (int i = columnNames->Count; i < cells->Length; i++) {
                columnNames->Add(CreateUniqueName("unnamed", columnNames));
            }
        }

        // store the file contents in the native CsvTable structure
        vtab = new CsvTable;
        vtab->ColumnNames.reserve(columnNames->Count);
        for each (auto columnName in columnNames) {
            vtab->ColumnNames.push_back(Util::WStr(columnName));
        }
        vtab->Rows.reserve(rows->Count);
        for each (auto row in rows) {
            std::vector<std::wstring> rowData;
            rowData.reserve(row->Length);
            for each (auto cell in row) {
                rowData.push_back(Util::WStr(cell));
            }
            vtab->Rows.push_back(rowData);
        }

        // inform sqlite of the column structure
        auto createTableLines = gcnew List<String^>();
        for each (auto col in columnNames) {
            createTableLines->Add(col + " TEXT");
        }
        auto createTableSql = gcnew StringBuilder("CREATE TABLE a (");
        createTableSql->Append(String::Join(",", createTableLines));
        createTableSql->Append(")");
        g_SqliteCall(db, sqlite3_declare_vtab(db, Util::CStr(createTableSql->ToString()).c_str()));

        *ppVTab = &vtab->Super;
        return SQLITE_OK;
    } catch (Exception^ ex) {
        delete vtab;
        *pzErr = sqlite3_mprintf("CsvCreate: %s", Util::CStr(ex->Message).c_str());
        return SQLITE_ERROR;
    }
}

static int CsvDestroy(sqlite3_vtab* pVTab) {
    delete (CsvTable*)pVTab;
    return SQLITE_OK;
}

static int CsvBestIndex(sqlite3_vtab* pVTab, sqlite3_index_info* info) {
    auto vtab = (CsvTable*)pVTab;
    // we don't have any indices of any kind.  a full dumb scan is all we can offer.
    info->idxNum = 1;
    info->estimatedCost = 1;
    info->estimatedRows = vtab->Rows.size();
    return SQLITE_OK;
}

static int CsvOpen(sqlite3_vtab* pVTab, sqlite3_vtab_cursor** ppCursor) {
    auto cursor = new CsvCursor;
    cursor->Table = (CsvTable*)pVTab;
    *ppCursor = &cursor->Super;
    return SQLITE_OK;
}

static int CsvClose(sqlite3_vtab_cursor* pCur) {
    delete (CsvCursor*)pCur;
    return SQLITE_OK;
}

static int CsvFilter(sqlite3_vtab_cursor* pCur, int idxNum, const char* idxStr, int argc, sqlite3_value** argv) {
    auto cursor = (CsvCursor*)pCur;
    cursor->RowIndex = 0;
    return SQLITE_OK;
}

static int CsvNext(sqlite3_vtab_cursor* pCur) {
    auto cursor = (CsvCursor*)pCur;
    cursor->RowIndex++;
    return SQLITE_OK;
}

static int CsvEof(sqlite3_vtab_cursor* pCur) {
    auto cursor = (CsvCursor*)pCur;
    auto totalRows = cursor->Table->Rows.size();
    return cursor->RowIndex >= totalRows ? 1 : 0;
}

static int CsvColumn(sqlite3_vtab_cursor* pCur, sqlite3_context* ctx, int n) {
    auto cursor = (CsvCursor*)pCur;
    auto& row = cursor->Table->Rows[cursor->RowIndex];
    if (n >= row.size()) {
        sqlite3_result_null(ctx);
    } else {
        auto value = row[n];
        auto valueCopy = _wcsdup(value.c_str()); // sqlite will free this
        auto lenB = value.size() * sizeof(wchar_t);
        sqlite3_result_text16(ctx, valueCopy, (int)lenB, free);
    }
    return SQLITE_OK;
}

static int CsvRowid(sqlite3_vtab_cursor* pCur, sqlite3_int64* pRowid) {
    auto cursor = (CsvCursor*)pCur;
    *pRowid = cursor->RowIndex;
    return SQLITE_OK;
}

static int CsvRename(sqlite3_vtab* pVtab, const char* zNew) {
    // don't care
    return SQLITE_OK;
}

void Notebook::InstallCsvModule() {
    if (s_csvModule.iVersion != 1) {
        s_csvModule.iVersion = 1;
        s_csvModule.xCreate = CsvCreate;
        s_csvModule.xConnect = CsvCreate;
        s_csvModule.xBestIndex = CsvBestIndex;
        s_csvModule.xDisconnect = CsvDestroy;
        s_csvModule.xDestroy = CsvDestroy;
        s_csvModule.xOpen = CsvOpen;
        s_csvModule.xClose = CsvClose;
        s_csvModule.xFilter = CsvFilter;
        s_csvModule.xNext = CsvNext;
        s_csvModule.xEof = CsvEof;
        s_csvModule.xColumn = CsvColumn;
        s_csvModule.xRowid = CsvRowid;
        s_csvModule.xRename = CsvRename;
    }
    SqliteCall(sqlite3_create_module_v2(_sqlite, "csv", &s_csvModule, NULL, NULL));
}
