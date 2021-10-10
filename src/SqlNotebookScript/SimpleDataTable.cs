using System.Collections.Generic;

namespace SqlNotebookScript {
    public sealed class SimpleDataTable {
        public IReadOnlyList<string> Columns { get; }
        public IReadOnlyList<object[]> Rows { get; }
        private IReadOnlyDictionary<string, int> _columnIndices;

        public SimpleDataTable(IReadOnlyList<string> columns, IReadOnlyList<object[]> rows) {
            Columns = columns;
            Rows = rows;
    
            var dict = new Dictionary<string, int>();
            int i = 0;
            foreach (var columnName in columns) {
                dict[columnName] = i++;
            }
            _columnIndices = dict;
        }

        public object Get(int rowNumber, string column) {
            var row = Rows[rowNumber];
            var colIndex = _columnIndices[column];
            return row[colIndex];
        }

        public int GetIndex(string column) {
            return _columnIndices[column];
        }
    }
}
