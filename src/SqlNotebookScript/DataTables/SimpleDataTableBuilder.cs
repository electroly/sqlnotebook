using System;
using System.Collections.Generic;

namespace SqlNotebookScript.DataTables;

public sealed class SimpleDataTableBuilder : IDisposable
{
    private readonly IReadOnlyList<string> _columns;
    private readonly int _columnCount;

    // We start by keeping rows in-memory for awhile. If it gets too big, then we null out the rows list and create a
    // disk-based table.
    private List<object[]> _rows = new();
    private DiskSimpleDataTable _disk = null;

    public SimpleDataTableBuilder(IReadOnlyList<string> columns)
    {
        _columns = columns;
        _columnCount = _columns.Count;
    }

    public void AddRow(object[] row)
    {
        if (_rows != null)
        {
            var copy = new object[_columnCount];
            Array.Copy(row, copy, _columnCount);
            _rows.Add(copy);

            if (_rows.Count >= 50_000)
            {
                SpillOntoDisk();
            }
        }
        else
        {
            _disk.LoadRow(row);
        }
    }

    private void SpillOntoDisk()
    {
        _disk = new(_columns);
        foreach (var row in _rows)
        {
            _disk.LoadRow(row);
        }
        _rows = null;
    }

    public SimpleDataTable Build(long? fullCount = null)
    {
        if (_rows != null)
        {
            return new MemorySimpleDataTable(_columns, _rows, fullCount ?? _rows.Count);
        }
        var disk = _disk;
        _disk = null;
        disk.FinishLoading(fullCount);
        return disk;
    }

    public void Dispose()
    {
        _disk?.Dispose();
    }
}
