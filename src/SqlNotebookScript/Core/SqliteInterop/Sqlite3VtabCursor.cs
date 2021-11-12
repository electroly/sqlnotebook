using System;
using System.Runtime.InteropServices;

namespace SqlNotebookScript.Core.SqliteInterop;

[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Size = 12)]
internal class Sqlite3VtabCursor {
    // --- The prefix of this struct must match sqlite3_vtab_cursor
    // sizeof(sqlite3_vtab_cursor): 8

    // sqlite3_vtab.pVtab: offset 0, size 8
    [FieldOffset(0)] public IntPtr pVtab;

    // --- The suffix is up to us

    [FieldOffset(8)] public int MetadataKey;
}
