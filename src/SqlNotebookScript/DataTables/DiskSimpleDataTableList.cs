using System.Collections;
using System.Collections.Generic;

namespace SqlNotebookScript.DataTables;

public sealed class DiskSimpleDataTableList : IReadOnlyList<object[]> {
    private readonly DiskSimpleDataTable _table;

    public DiskSimpleDataTableList(DiskSimpleDataTable table, int count) {
        Count = count;
        _table = table;
    }

    public object[] this[int index] {
        get {
            var row = new object[_table.Columns.Count];
            _table.GetRow(index, row);
            return row;
        }
    }

    public int Count { get; }

    public IEnumerator<object[]> GetEnumerator() {
        return new DiskSimpleDataTableEnumerator(_table, _table.Columns.Count, Count);
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return new DiskSimpleDataTableEnumerator(_table, _table.Columns.Count, Count);
    }
}
