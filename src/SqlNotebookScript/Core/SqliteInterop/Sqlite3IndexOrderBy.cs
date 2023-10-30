using System.Runtime.InteropServices;

namespace SqlNotebookScript.Core.SqliteInterop;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct Sqlite3IndexOrderBy
{
    public int iColumn;
    public byte desc;
}
