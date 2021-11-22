using System.Collections;
using System.Collections.Generic;

namespace SqlNotebookScript.DataTables;

public sealed class DiskSimpleDataTableEnumerator : IEnumerator<object[]> {
    private readonly DiskSimpleDataTable _table;
    private readonly int _rowCount;
    private long _rowIndex = -1;

    public DiskSimpleDataTableEnumerator(DiskSimpleDataTable table, int columnCount, int rowCount) {
        _table = table;
        _rowCount = rowCount;
        Current = new object[columnCount];
    }

    public object[] Current { get; }

    object IEnumerator.Current => Current;

    public void Dispose() {}

    public bool MoveNext() {
        _rowIndex++;
        if (_rowIndex >= _rowCount) {
            return false;
        }
        
        _table.GetRow(_rowIndex, Current);
        return true;
    }

    public void Reset() {
        _rowIndex = -1;
    }
}
