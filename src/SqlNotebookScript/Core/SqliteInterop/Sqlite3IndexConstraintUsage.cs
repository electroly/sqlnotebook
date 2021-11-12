using System.Runtime.InteropServices;

namespace SqlNotebookScript.Core.SqliteInterop;

[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Size = 8)]
public struct Sqlite3IndexConstraintUsage {
    [FieldOffset(0)] public int argvIndex;
    [FieldOffset(4)] public byte omit;
}
