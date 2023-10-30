using System;
using System.Runtime.InteropServices;

namespace SqlNotebookScript.Core.SqliteInterop;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct Sqlite3Vtab
{
    // --- The prefix of this struct must match sqlite3_vtab
    public IntPtr pModule;
    public int nRef;
    public IntPtr zErrMsg;

    // --- The suffix is up to us

    public int MetadataKey;
}
