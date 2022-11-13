using System.Runtime.InteropServices;

namespace SqlNotebookScript.Core.SqliteInterop;

[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Size = 12)]
public struct Sqlite3IndexConstraint
{
    [FieldOffset(0)]
    public int iColumn;

    [FieldOffset(4)]
    public byte op;

    [FieldOffset(5)]
    public byte usable;

    [FieldOffset(8)]
    public int iTermOffset;
}
