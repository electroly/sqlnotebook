using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.DataTables;

public abstract class SimpleDataTable : IDisposable {
    protected IReadOnlyDictionary<string, int> _columnIndices;

    public IReadOnlyList<string> Columns { get; protected set; }
    public IReadOnlyList<object[]> Rows { get; protected set; }
    public long FullCount { get; protected set; }

    public object Get(int rowNumber, string column) {
        var row = Rows[rowNumber];
        var colIndex = _columnIndices[column];
        return row[colIndex];
    }

    public int GetIndex(string column) {
        return _columnIndices[column];
    }

    public void Serialize(BinaryWriter writer) {
        // FullCount
        writer.Write(FullCount);

        // Columns
        writer.Write(Columns.Count);
        foreach (var column in Columns) {
            writer.Write(column);
        }

        // Rows
        writer.Write(Rows.Count);
        foreach (var row in Rows) {
            Debug.Assert(row.Length == Columns.Count);
            foreach (var cell in row) {
                writer.WriteScalar(cell);
            }
        }
    }

    public static SimpleDataTable Deserialize(BinaryReader reader) {
        // FullCount
        var fullCount = reader.ReadInt64();

        // Columns
        var columnCount = reader.ReadInt32();
        List<string> columns = new(columnCount);
        for (var i = 0; i < columnCount; i++) {
            columns.Add(reader.ReadString());
        }

        // Rows
        var rowCount = reader.ReadInt32();
        using SimpleDataTableBuilder builder = new(columns);
        var row = new object[columnCount];
        for (var i = 0; i < rowCount; i++) {
            for (var j = 0; j < columnCount; j++) {
                row[j] = reader.ReadScalar();
            }
            builder.AddRow(row);
        }
        return builder.Build(fullCount);
    }

    public abstract void Dispose();
}
