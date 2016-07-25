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

using System;
using System.Collections.Generic;
using System.Linq;
using SqlNotebookCore;
using SqlNotebookCoreModules.Script;

namespace SqlNotebook {
    // store a snapshot of the database schema for quick reference
    public sealed class DatabaseSchema {
        // lowercase table name -> TableSchema
        public Dictionary<string, TableSchema> Tables { get; }

        // the lowercase name of anything in sqlite_master that isn't a table
        public HashSet<string> NonTables { get; }

        public DatabaseSchema(IEnumerable<TableSchema> tables, IEnumerable<string> nonTables) {
            Tables = tables.ToDictionary(x => x.Name.ToLower());
            NonTables = new HashSet<string>(nonTables.Select(x => x.ToLower()));
        }
        
        public static DatabaseSchema FromNotebook(Notebook notebook) {
            var tableSchemas = new List<TableSchema>();
            var nonTables = new List<string>();

            notebook.Invoke(() => {
                var tablesDt = notebook.Query("SELECT name, type FROM sqlite_master");
                for (int i = 0; i < tablesDt.Rows.Count; i++) {
                    var tableName = tablesDt.Get(i, "name").ToString();
                    var tableType = tablesDt.Get(i, "type").ToString();
                    if (tableType == "table") {
                        tableSchemas.Add(GetTableSchema(notebook, tableName));
                    } else {
                        nonTables.Add(tableName);
                    }
                }
            });

            return new DatabaseSchema(tableSchemas, nonTables);
        }

        private static TableSchema GetTableSchema(Notebook notebook, string tableName) {
            // precondition: must be inside notebook.Invoke()
            var columnSchemas = new List<ColumnSchema>();

            var dt = notebook.Query($"PRAGMA table_info ({tableName.DoubleQuote()})");
            for (int i = 0; i < dt.Rows.Count; i++) {
                var name = dt.Get(i, "name").ToString();
                var type = dt.Get(i, "type").ToString();
                var notNull = Convert.ToInt32(dt.Get(i, "notnull")) != 0;
                var primaryKey = Convert.ToInt32(dt.Get(i, "pk"));
                columnSchemas.Add(new ColumnSchema(name, type, notNull, primaryKey));
            }

            return new TableSchema(tableName, columnSchemas);
        }
    }

    public sealed class TableSchema {
        // from sqlite_master
        public string Name { get; }

        // from pragma table_info
        public IReadOnlyList<ColumnSchema> Columns { get; }

        public TableSchema(string name, IReadOnlyList<ColumnSchema> columns) {
            Name = name;
            Columns = columns;
        }
    }

    public sealed class ColumnSchema {
        public string Name { get; }
        public string Type { get; }
        public bool NotNull { get; }
        public int PrimaryKey { get; }

        public ColumnSchema(string name, string type, bool notNull, int primaryKey) {
            Name = name;
            Type = type;
            NotNull = notNull;
            PrimaryKey = primaryKey;
        }
    }
}
