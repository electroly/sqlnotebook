using System;
using System.Collections.Generic;
using SqlNotebookScript.Interpreter;

namespace SqlNotebookScript;

public abstract class NotebookItemRecord : IDisposable
{
    public string Name { get; set; }
    public abstract void Dispose();
}

public sealed class ScriptNotebookItemRecord : NotebookItemRecord
{
    public string Sql { get; set; }
    public List<string> Parameters { get; set; }

    public override void Dispose() { }
}

public sealed class PageNotebookItemRecord : NotebookItemRecord
{
    public List<PageBlockRecord> Blocks { get; set; }

    public override void Dispose()
    {
        foreach (var block in Blocks)
        {
            block.Dispose();
        }
    }
}

public abstract class PageBlockRecord : IDisposable
{
    public abstract void Dispose();
}

public sealed class QueryPageBlockRecord : PageBlockRecord
{
    public string Sql { get; set; }
    public ScriptOutput Output { get; set; }
    public QueryPageBlockOptions Options { get; set; }

    public override void Dispose()
    {
        Output?.Dispose();
        Output = null;
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

    public override void Dispose() { }
}
