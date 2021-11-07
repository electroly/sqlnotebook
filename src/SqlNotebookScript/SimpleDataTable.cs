using SqlNotebookScript.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SqlNotebookScript {
    public sealed class SimpleDataTable {
        private readonly IReadOnlyDictionary<string, int> _columnIndices;

        public IReadOnlyList<string> Columns { get; }
        public IReadOnlyList<object[]> Rows { get; }
        public long FullCount { get; }

        public SimpleDataTable(IReadOnlyList<string> columns, IReadOnlyList<object[]> rows, long fullCount) {
            Columns = columns;
            Rows = rows;
    
            var dict = new Dictionary<string, int>();
            int i = 0;
            foreach (var columnName in columns) {
                dict[columnName] = i++;
            }
            _columnIndices = dict;

            FullCount = fullCount == 0 ? rows.Count : fullCount;
        }

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
            List<object[]> rows = new(rowCount);
            for (var i = 0; i < rowCount; i++) {
                var row = new object[columnCount];
                for (var j = 0; j < columnCount; j++) {
                    row[j] = reader.ReadScalar();
                }
                rows.Add(row);
            }

            return new(columns, rows, fullCount);
        }
    }
}
