using System.Collections.Generic;

namespace SqlNotebookScript.DataTables;

public sealed class MemorySimpleDataTable : SimpleDataTable
{
    public MemorySimpleDataTable(IReadOnlyList<string> columns, IReadOnlyList<object[]> rows, long fullCount)
    {
        Columns = columns;
        Rows = rows;
        FullCount = fullCount;

        var dict = new Dictionary<string, int>();
        int i = 0;
        foreach (var columnName in columns)
        {
            dict[columnName] = i++;
        }
        _columnIndices = dict;
    }

    public override void Dispose() { }
}
