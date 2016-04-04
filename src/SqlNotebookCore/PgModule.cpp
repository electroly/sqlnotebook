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

#include "gcroot.h"
#include "SqlNotebookCore.h"
using namespace SqlNotebookCore;
using namespace System::IO;
using namespace System::Text;
using namespace Npgsql;

static sqlite3_module s_pgModule = { 0 };

private struct PgTable {
    sqlite3_vtab Super;
    gcroot<String^> ConnectionString;
    gcroot<String^> PgTableName;
    gcroot<List<String^>^> ColumnNames;
    int64_t InitialRowCount;
    PgTable() {
        memset(&Super, 0, sizeof(Super));
    }
};

private struct PgCursor {
    sqlite3_vtab_cursor Super;
    PgTable* Table;
    gcroot<NpgsqlConnection^> Connection;
    gcroot<NpgsqlCommand^> Command;
    gcroot<NpgsqlDataReader^> Reader;
    bool IsEof;

    PgCursor() {
        memset(&Super, 0, sizeof(Super));
        Table = nullptr;
    }
};

private ref class PgDataReader : public IDisposable { // for RAII
    public:
    PgDataReader(NpgsqlCommand^ command) {
        _reader = command->ExecuteReader();
    }

    !PgDataReader() {
        delete _reader;
        _reader = nullptr;
    }

    ~PgDataReader() {
        if (!_isDisposed) {
            (*this).!PgDataReader();
            _isDisposed = true;
        }
    }

    NpgsqlDataReader^ operator ->() {
        return _reader;
    }

    private:
    bool _isDisposed;
    NpgsqlDataReader^ _reader;
};

static int PgCreate(sqlite3* db, void* pAux, int argc, const char* const* argv, sqlite3_vtab** ppVTab, char** pzErr) {
    // argv[3]: connectionString
    // argv[4]: table name

    PgTable* vtab = nullptr;

    try {
        if (argc != 5) {
            throw gcnew Exception("Syntax: CREATE VIRTUAL TABLE <name> USING pgsql ('<connection string>', 'table name');");
        }
        auto connStr = Util::Str(argv[3])->Trim(L'\'');
        auto pgTableName = Util::Str(argv[4])->Trim(L'\'');
        NpgsqlConnection conn(connStr);
        conn.Open();

        // ensure the table exists and detect the column names
        auto columnNames = gcnew List<String^>();
        auto columnTypes = gcnew List<Type^>();
        {
            NpgsqlCommand cmd("SELECT * FROM \"" + pgTableName->Replace("\"", "\"\"") + "\" WHERE FALSE", %conn);
            PgDataReader reader(%cmd);
            for (int i = 0; i < reader->FieldCount; i++) {
                columnNames->Add(reader->GetName(i));
                columnTypes->Add(reader->GetFieldType(i));
            }
        }

        // get a row count too
        int64_t rowCount = 0;
        {
            NpgsqlCommand cmd("SELECT COUNT(*) FROM \"" + pgTableName->Replace("\"", "\"\"") + "\"", %conn);
            rowCount = (int64_t)cmd.ExecuteScalar();
        }

        // create sqlite structure
        auto vtab = new PgTable;
        vtab->ConnectionString = connStr;
        vtab->PgTableName = pgTableName;
        vtab->ColumnNames = columnNames;
        vtab->InitialRowCount = rowCount;

        // register the column names and types with pgsql
        auto columnLines = gcnew List<String^>();
        for (int i = 0; i < columnNames->Count; i++) {
            auto t = columnTypes[i];
            String^ sqlType;
            if (t == Int16::typeid || t == Int32::typeid || t == Int64::typeid || t == Byte::typeid || t == bool::typeid) {
                sqlType = "integer";
            } else if (t == float::typeid || t == double::typeid || t == Decimal::typeid) {
                sqlType = "real";
            } else {
                sqlType = "text";
            }
            columnLines->Add(columnNames[i] + " " + sqlType);
        }
        auto createSql = "CREATE TABLE a (" + String::Join(", ", columnLines) + ")";
        g_SqliteCall(db, sqlite3_declare_vtab(db, Util::CStr(createSql).c_str()));

        *ppVTab = &vtab->Super;
        return SQLITE_OK;
    } catch (Exception^ ex) {
        delete vtab;
        *pzErr = sqlite3_mprintf("PgCreate: %s", Util::CStr(ex->Message).c_str());
        return SQLITE_ERROR;
    }
}

static int PgDestroy(sqlite3_vtab* pVTab) {
    delete (PgTable*)pVTab;
    return SQLITE_OK;
}

static int PgBestIndex(sqlite3_vtab* pVTab, sqlite3_index_info* info) {
    auto vtab = (PgTable*)pVTab;
    
    // build a pgsql query corresponding to the request
    auto sb = gcnew StringBuilder();
    sb->Append("SELECT *, substring(md5(cast(ctid as text)) from 1 for 16) AS __rowid FROM \"");
    sb->Append(vtab->PgTableName);
    sb->Append("\"");

    // where clause
    if (info->nConstraint > 0) {
        sb->Append(" WHERE ");
        auto terms = gcnew List<String^>();
        int argvIndex = 1;
        for (int i = 0; i < info->nConstraint; i++) {
            if (info->aConstraint[i].iColumn == -1) {
                // rowid instead of a column. we don't support this type of constraint.
                continue;
            }

            String^ op;
            switch (info->aConstraint[i].op) {
                case SQLITE_INDEX_CONSTRAINT_EQ: op = " = "; break;
                case SQLITE_INDEX_CONSTRAINT_GT: op = " > "; break;
                case SQLITE_INDEX_CONSTRAINT_LE: op = " <= "; break;
                case SQLITE_INDEX_CONSTRAINT_LT: op = " < "; break;
                case SQLITE_INDEX_CONSTRAINT_GE: op = " >= "; break;
                case SQLITE_INDEX_CONSTRAINT_LIKE: op = " LIKE "; break;
                default: continue; // we don't support this operator
            }

            info->aConstraintUsage[i].argvIndex = argvIndex;
            info->aConstraintUsage[i].omit = true;
            terms->Add(vtab->ColumnNames->default[info->aConstraint[i].iColumn] + op + "@arg" + argvIndex);
            argvIndex++;
        }
        sb->Append(String::Join(" AND ", terms));
    }

    // order by clause
    if (info->nOrderBy > 0) {
        sb->Append(" ORDER BY ");
        auto terms = gcnew List<String^>();
        for (int i = 0; i < info->nOrderBy; i++) {
            terms->Add(vtab->ColumnNames->default[info->aOrderBy[i].iColumn] + (info->aOrderBy[i].desc ? " DESC" : ""));
        }
        sb->Append(String::Join(", ", terms));
        info->orderByConsumed = true;
    }

    auto sql = Util::CStr(sb->ToString());
    info->idxNum = 0;
    info->idxStr = sqlite3_mprintf("%s", sql.c_str());
    info->needToFreeIdxStr = true;
    info->estimatedRows = vtab->InitialRowCount / (1 + info->nConstraint);
    info->estimatedCost = (double)info->estimatedRows;
        // wild guess of the effect of each WHERE constraint. we just want to induce sqlite to give us as many
        // constraints as possible for a given query.

    return SQLITE_OK;
}

static int PgOpen(sqlite3_vtab* pVTab, sqlite3_vtab_cursor** ppCursor) {
    auto cursor = new PgCursor;
    cursor->Table = (PgTable*)pVTab;
    cursor->Connection = gcnew NpgsqlConnection(cursor->Table->ConnectionString);
    cursor->Connection->Open();
    *ppCursor = &cursor->Super;
    return SQLITE_OK;
}

static int PgClose(sqlite3_vtab_cursor* pCur) {
    auto cursor = (PgCursor*)pCur;
    
    NpgsqlDataReader^ reader = cursor->Reader;
    delete reader;
    cursor->Reader = nullptr;

    NpgsqlCommand^ command = cursor->Command;
    delete command;
    cursor->Command = nullptr;

    NpgsqlConnection^ conn = cursor->Connection;
    delete conn;
    cursor->Connection = nullptr;
    
    delete cursor;
    return SQLITE_OK;
}

static int PgFilter(sqlite3_vtab_cursor* pCur, int idxNum, const char* idxStr, int argc, sqlite3_value** argv) {
    try {
        auto cursor = (PgCursor*)pCur;
        auto sql = Util::Str(idxStr);
        if ((NpgsqlCommand^)cursor->Command != nullptr) {
            // why does this happen?
            cursor->Command->Cancel();
            delete cursor->Reader;
            delete cursor->Command;
            delete cursor->Connection;
            cursor->Connection = gcnew NpgsqlConnection(cursor->Table->ConnectionString);
            cursor->Connection->Open();
        }
        auto cmd = gcnew NpgsqlCommand(sql, cursor->Connection);
        for (int i = 0; i < argc; i++) {
            Object^ argVal = nullptr;
            switch (sqlite3_value_type(argv[i])) {
                case SQLITE_INTEGER:
                    argVal = sqlite3_value_int64(argv[i]);
                    break;
                case SQLITE_FLOAT:
                    argVal = sqlite3_value_double(argv[i]);
                    break;
                case SQLITE_NULL:
                    argVal = DBNull::Value;
                    break;
                case SQLITE_TEXT:
                    argVal = Util::Str((const wchar_t*)sqlite3_value_text16(argv[i]));
                    break;
                default:
                    throw gcnew Exception("Data type not supported.");
            }
            auto varName = "@arg" + (i + 1).ToString();
            cmd->Parameters->Add(gcnew NpgsqlParameter(varName, argVal));
        }
        auto reader = cmd->ExecuteReader();
        cursor->Command = cmd;
        cursor->Reader = reader;
        cursor->IsEof = !cursor->Reader->Read();
        return SQLITE_OK;
    } catch (Exception^) {
        return SQLITE_ERROR;
    }
}

static int PgNext(sqlite3_vtab_cursor* pCur) {
    auto cursor = (PgCursor*)pCur;
    cursor->IsEof = !cursor->Reader->Read();
    return SQLITE_OK;
}

static int PgEof(sqlite3_vtab_cursor* pCur) {
    auto cursor = (PgCursor*)pCur;
    return cursor->IsEof ? 1 : 0;
}

static void ResultText16(sqlite3_context* ctx, String^ str) {
    auto wstr = Util::WStr(str);
    auto wstrCopy = _wcsdup(wstr.c_str());
    auto lenB = wstr.size() * sizeof(wchar_t);
    sqlite3_result_text16(ctx, wstrCopy, (int)lenB, free);
}

static int PgColumn(sqlite3_vtab_cursor* pCur, sqlite3_context* ctx, int n) {
    try {
        auto cursor = (PgCursor*)pCur;
        if (cursor->IsEof) {
            return SQLITE_ERROR;
        }
        auto type = cursor->Reader->GetFieldType(n);
        if (type == Int16::typeid) {
            sqlite3_result_int(ctx, cursor->Reader->GetInt16(n));
        } else if (type == Int32::typeid) {
            sqlite3_result_int(ctx, cursor->Reader->GetInt32(n));
        } else if (type == Int64::typeid) {
            sqlite3_result_int64(ctx, cursor->Reader->GetInt64(n));
        } else if (type == Byte::typeid) {
            sqlite3_result_int(ctx, cursor->Reader->GetByte(n));
        } else if (type == Single::typeid) {
            sqlite3_result_double(ctx, cursor->Reader->GetFloat(n));
        } else if (type == Double::typeid) {
            sqlite3_result_double(ctx, cursor->Reader->GetDouble(n));
        } else if (type == Decimal::typeid) {
            sqlite3_result_double(ctx, (double)cursor->Reader->GetDecimal(n));
        } else if (type == String::typeid) {
            ResultText16(ctx, cursor->Reader->GetString(n));
        } else if (type == Char::typeid) {
            ResultText16(ctx, gcnew String(cursor->Reader->GetChar(n), 1));
        } else if (type == Boolean::typeid) {
            sqlite3_result_int(ctx, cursor->Reader->GetBoolean(n) ? 1 : 0);
        } else if (type == NpgsqlTypes::NpgsqlDate::typeid) {
            ResultText16(ctx, ((DateTime)cursor->Reader->GetDate(n)).ToString("yyyy-MM-dd"));
        } else if (type == NpgsqlTypes::NpgsqlDateTime::typeid) {
            ResultText16(ctx, ((DateTime)cursor->Reader->GetDateTime(n)).ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"));
        } else {
            ResultText16(ctx, cursor->Reader->GetValue(n)->ToString());
        }
        return SQLITE_OK;
    } catch (Exception^) {
        return SQLITE_ERROR;
    }
}

static int PgRowid(sqlite3_vtab_cursor* pCur, sqlite3_int64* pRowid) {
    auto cursor = (PgCursor*)pCur;
    auto hashHex = cursor->Reader->GetString(cursor->Table->ColumnNames->Count);
    *pRowid = Convert::ToInt64(hashHex, 16);
    return SQLITE_OK;
}

static int PgRename(sqlite3_vtab* pVtab, const char* zNew) {
    // don't care
    return SQLITE_OK;
}

void Notebook::InstallPgModule() {
    if (s_pgModule.iVersion != 1) {
        s_pgModule.iVersion = 1;
        s_pgModule.xCreate = PgCreate;
        s_pgModule.xConnect = PgCreate;
        s_pgModule.xBestIndex = PgBestIndex;
        s_pgModule.xDisconnect = PgDestroy;
        s_pgModule.xDestroy = PgDestroy;
        s_pgModule.xOpen = PgOpen;
        s_pgModule.xClose = PgClose;
        s_pgModule.xFilter = PgFilter;
        s_pgModule.xNext = PgNext;
        s_pgModule.xEof = PgEof;
        s_pgModule.xColumn = PgColumn;
        s_pgModule.xRowid = PgRowid;
        s_pgModule.xRename = PgRename;
    }
    SqliteCall(sqlite3_create_module_v2(_sqlite, "pgsql", &s_pgModule, NULL, NULL));
}
