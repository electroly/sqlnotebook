using System.Collections.Generic;

namespace SqlNotebookScript;

public abstract class CustomTableFunction
{
    public abstract string Name { get; }
    public abstract string CreateTableSql { get; }
    public abstract int HiddenColumnCount { get; }
    public abstract IEnumerable<object[]> Execute(object[] hiddenValues);
}
