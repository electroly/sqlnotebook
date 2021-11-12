using System;
using System.Runtime.InteropServices;

namespace SqlNotebookScript.Core.SqliteInterop;

[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Size = 28)]
public struct Sqlite3Vtab {
    // --- The prefix of this struct must match sqlite3_vtab
    // sizeof(sqlite3_vtab): 24

    // sqlite3_vtab.pModule: offset 0, size 8
    [FieldOffset(0)] public IntPtr pModule;

    // sqlite3_vtab.nRef: offset 8, size 4
    [FieldOffset(8)] public int nRef;

    // sqlite3_vtab.zErrMsg: offset 16, size 8
    [FieldOffset(16)] public IntPtr zErrMsg;

    // --- The suffix is up to us

    [FieldOffset(24)] public int MetadataKey;
}
