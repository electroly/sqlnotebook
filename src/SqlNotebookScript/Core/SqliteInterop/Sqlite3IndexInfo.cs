using System;
using System.Runtime.InteropServices;

namespace SqlNotebookScript.Core.SqliteInterop;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct Sqlite3IndexInfo
{
    public int nConstraint;
    public IntPtr aConstraint;
    public int nOrderBy;
    public IntPtr aOrderBy;
    public IntPtr aConstraintUsage;
    public int idxNum;
    public IntPtr idxStr;
    public int needToFreeIdxStr;
    public int orderByConsumed;
    public double estimatedCost;
    public long estimatedRows;
    public int idxFlags;
    public ulong colUsed;
}
