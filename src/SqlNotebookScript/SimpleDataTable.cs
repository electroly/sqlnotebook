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
