using System;
using System.Collections.Generic;
using System.Linq;
using SqlNotebookScript.Core;
using SqlNotebookScript.Utils;

namespace SqlNotebook;

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

        Notebook.Invoke(() => {
            using var tablesDt = notebook.Query("SELECT name, type FROM sqlite_master");
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

        using var dt = notebook.Query($"PRAGMA table_info ({tableName.DoubleQuote()})");
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
