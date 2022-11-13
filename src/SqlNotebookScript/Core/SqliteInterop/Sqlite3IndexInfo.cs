using System;
using System.Runtime.InteropServices;

namespace SqlNotebookScript.Core.SqliteInterop;

[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Size = 96)]
public struct Sqlite3IndexInfo
{
    [FieldOffset(0)]
    public int nConstraint;

    [FieldOffset(8)]
    public IntPtr aConstraint;

    [FieldOffset(16)]
    public int nOrderBy;

    [FieldOffset(24)]
    public IntPtr aOrderBy;

    [FieldOffset(32)]
    public IntPtr aConstraintUsage;

    [FieldOffset(40)]
    public int idxNum;

    [FieldOffset(48)]
    public IntPtr idxStr;

    [FieldOffset(56)]
    public int needToFreeIdxStr;

    [FieldOffset(60)]
    public int orderByConsumed;

    [FieldOffset(64)]
    public double estimatedCost;

    [FieldOffset(72)]
    public long estimatedRows;

    [FieldOffset(80)]
    public int idxFlags;

    [FieldOffset(88)]
    public ulong colUsed;
}
