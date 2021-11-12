using System.Runtime.InteropServices;

namespace SqlNotebookScript.Core.SqliteInterop;

[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Size = 8)]
public struct Sqlite3IndexOrderBy {
    [FieldOffset(0)] public int iColumn;
    [FieldOffset(4)] public byte desc;
}
