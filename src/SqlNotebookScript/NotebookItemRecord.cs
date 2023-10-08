using System;
using System.Collections.Generic;
using SqlNotebookScript.Interpreter;

namespace SqlNotebookScript;

public abstract class NotebookItemRecord
{
    public string Name { get; set; }
}

public sealed class ScriptNotebookItemRecord : NotebookItemRecord
{
    public string Sql { get; set; }
    public List<string> Parameters { get; set; }
}

public sealed class PageNotebookItemRecord : NotebookItemRecord, IDisposable
{
    public List<PageBlockRecord> Blocks { get; set; }

    public void Dispose()
    {
        foreach (var block in Blocks)
        {
            if (block is IDisposable disposable)
                disposable.Dispose();
        }
    }
}

public abstract class PageBlockRecord { }

public sealed class QueryPageBlockRecord : PageBlockRecord, IDisposable
{
    public string Sql { get; set; }
    public ScriptOutput Output { get; set; }
    public QueryPageBlockOptions Options { get; set; }

    public void Dispose()
    {
        Output?.Dispose();
    }
}

public sealed class QueryPageBlockOptions
{
    public bool ShowSql { get; set; }
    public bool ShowResults { get; set; }
    public int MaxDisplayRows { get; set; }
}

public sealed class TextPageBlockRecord : PageBlockRecord
{
    public string Content;
}
