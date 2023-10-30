using System.Runtime.InteropServices;

namespace SqlNotebookScript.Core.SqliteInterop;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct Sqlite3IndexConstraintUsage
{
    public int argvIndex;
    public byte omit;
}
