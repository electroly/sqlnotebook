using System;
using System.Runtime.InteropServices;

namespace SqlNotebookScript.Core.SqliteInterop;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
internal class Sqlite3VtabCursor
{
    // --- The prefix of this struct must match sqlite3_vtab_cursor

    public IntPtr pVtab;

    // --- The suffix is up to us

    public int MetadataKey;
}
