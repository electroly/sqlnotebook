using System;
using System.Text;
using SqlNotebookScript.Core;

namespace Tests;

public sealed class GlobalInit
{
    private GlobalInit()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Notebook.InitSqlite();
    }

    private static readonly Lazy<GlobalInit> _instance = new(() => new());

    public static void Init() => _ = _instance.Value;
}
