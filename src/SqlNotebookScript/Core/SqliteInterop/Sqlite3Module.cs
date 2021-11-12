using System;
using System.Runtime.InteropServices;

namespace SqlNotebookScript.Core.SqliteInterop;

[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Size = 192)]
public struct Sqlite3Module {
    [FieldOffset(0)] public int iVersion;
    [FieldOffset(8)] public IntPtr xCreate;
    [FieldOffset(16)] public IntPtr xConnect;
    [FieldOffset(24)] public IntPtr xBestIndex;
    [FieldOffset(32)] public IntPtr xDisconnect;
    [FieldOffset(40)] public IntPtr xDestroy;
    [FieldOffset(48)] public IntPtr xOpen;
    [FieldOffset(56)] public IntPtr xClose;
    [FieldOffset(64)] public IntPtr xFilter;
    [FieldOffset(72)] public IntPtr xNext;
    [FieldOffset(80)] public IntPtr xEof;
    [FieldOffset(88)] public IntPtr xColumn;
    [FieldOffset(96)] public IntPtr xRowid;
    [FieldOffset(104)] public IntPtr xUpdate;
    [FieldOffset(112)] public IntPtr xBegin;
    [FieldOffset(120)] public IntPtr xSync;
    [FieldOffset(128)] public IntPtr xCommit;
    [FieldOffset(136)] public IntPtr xRollback;
    [FieldOffset(144)] public IntPtr xFindFunction;
    [FieldOffset(152)] public IntPtr xRename;
    [FieldOffset(160)] public IntPtr xSavepoint;
    [FieldOffset(168)] public IntPtr xRelease;
    [FieldOffset(176)] public IntPtr xRollbackTo;
    [FieldOffset(184)] public IntPtr xShadowName;
}
