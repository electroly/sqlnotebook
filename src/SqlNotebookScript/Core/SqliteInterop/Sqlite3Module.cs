using System;
using System.Runtime.InteropServices;

namespace SqlNotebookScript.Core.SqliteInterop;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct Sqlite3Module
{
    public int iVersion;
    public IntPtr xCreate;
    public IntPtr xConnect;
    public IntPtr xBestIndex;
    public IntPtr xDisconnect;
    public IntPtr xDestroy;
    public IntPtr xOpen;
    public IntPtr xClose;
    public IntPtr xFilter;
    public IntPtr xNext;
    public IntPtr xEof;
    public IntPtr xColumn;
    public IntPtr xRowid;
    public IntPtr xUpdate;
    public IntPtr xBegin;
    public IntPtr xSync;
    public IntPtr xCommit;
    public IntPtr xRollback;
    public IntPtr xFindFunction;
    public IntPtr xRename;
    public IntPtr xSavepoint;
    public IntPtr xRelease;
    public IntPtr xRollbackTo;
    public IntPtr xShadowName;
}
