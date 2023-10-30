using System.Runtime.InteropServices;

namespace SqlNotebookScript.Core.SqliteInterop;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct Sqlite3IndexConstraint
{
    public int iColumn;
    public byte op;
    public byte usable;
    public int iTermOffset;
}
