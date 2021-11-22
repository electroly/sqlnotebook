using System;
using System.Runtime.InteropServices;

namespace SqlNotebookScript.Core.SqliteInterop;

#pragma warning disable CA1401 // P/Invokes should not be visible
public static class NativeMethods {
    private const string SQLITE_DLL = "sqlite3.dll";

    public const int SQLITE_OK = 0;
    public const int SQLITE_ERROR = 1;
    public const int SQLITE_INTERNAL = 2;
    public const int SQLITE_PERM = 3;
    public const int SQLITE_ABORT = 4;
    public const int SQLITE_BUSY = 5;
    public const int SQLITE_LOCKED = 6;
    public const int SQLITE_NOMEM = 7;
    public const int SQLITE_READONLY = 8;
    public const int SQLITE_INTERRUPT = 9;
    public const int SQLITE_IOERR = 10;
    public const int SQLITE_CORRUPT = 11;
    public const int SQLITE_NOTFOUND = 12;
    public const int SQLITE_FULL = 13;
    public const int SQLITE_CANTOPEN = 14;
    public const int SQLITE_PROTOCOL = 15;
    public const int SQLITE_EMPTY = 16;
    public const int SQLITE_SCHEMA = 17;
    public const int SQLITE_TOOBIG = 18;
    public const int SQLITE_CONSTRAINT = 19;
    public const int SQLITE_MISMATCH = 20;
    public const int SQLITE_MISUSE = 21;
    public const int SQLITE_NOLFS = 22;
    public const int SQLITE_AUTH = 23;
    public const int SQLITE_FORMAT = 24;
    public const int SQLITE_RANGE = 25;
    public const int SQLITE_NOTADB = 26;
    public const int SQLITE_NOTICE = 27;
    public const int SQLITE_WARNING = 28;

    public const int SQLITE_ROW = 100;
    public const int SQLITE_DONE = 101;

    public const int SQLITE_INDEX_CONSTRAINT_EQ = 2;
    public const int SQLITE_INDEX_CONSTRAINT_GT = 4;
    public const int SQLITE_INDEX_CONSTRAINT_LE = 8;
    public const int SQLITE_INDEX_CONSTRAINT_LT = 16;
    public const int SQLITE_INDEX_CONSTRAINT_GE = 32;
    public const int SQLITE_INDEX_CONSTRAINT_MATCH = 64;
    public const int SQLITE_INDEX_CONSTRAINT_LIKE = 65;
    public const int SQLITE_INDEX_CONSTRAINT_GLOB = 66;
    public const int SQLITE_INDEX_CONSTRAINT_REGEXP = 67;
    public const int SQLITE_INDEX_CONSTRAINT_NE = 68;
    public const int SQLITE_INDEX_CONSTRAINT_ISNOT = 69;
    public const int SQLITE_INDEX_CONSTRAINT_ISNOTNULL = 70;
    public const int SQLITE_INDEX_CONSTRAINT_ISNULL = 71;
    public const int SQLITE_INDEX_CONSTRAINT_IS = 72;
    public const int SQLITE_INDEX_CONSTRAINT_FUNCTION = 150;

    public const int SQLITE_INTEGER = 1;
    public const int SQLITE_FLOAT = 2;
    public const int SQLITE_TEXT = 3;
    public const int SQLITE_BLOB = 4;
    public const int SQLITE_NULL = 5;

    public const int SQLITE_UTF16 = 4;

    public const int SQLITE_DETERMINISTIC = 0x800;

    public const int SQLITE_OPEN_READONLY = 0x1;

    [DllImport("kernel32.dll", EntryPoint = "RtlZeroMemory", SetLastError = false)]
    public static extern void ZeroMemory(IntPtr dest, IntPtr size);

    [DllImport(SQLITE_DLL, EntryPoint = "SxGetToken", SetLastError = false)]
    public static extern int SxGetToken(IntPtr z, IntPtr tokenType);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_open", SetLastError = false)]
    public static extern int sqlite3_open(IntPtr zFilename, IntPtr ppDb);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_open_v2", SetLastError = false)]
    public static extern int sqlite3_open_v2(IntPtr filename, IntPtr ppDb, int flags, IntPtr zVfs);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_close", SetLastError = false)]
    public static extern int sqlite3_close(IntPtr db);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_malloc", SetLastError = false)]
    public static extern IntPtr sqlite3_malloc(int n);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_free", SetLastError = false)]
    public static extern void sqlite3_free(IntPtr p);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_errmsg16", SetLastError = false)]
    public static extern IntPtr sqlite3_errmsg16(IntPtr db);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_declare_vtab", SetLastError = false)]
    public static extern int sqlite3_declare_vtab(IntPtr db, IntPtr zCreateTable);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_value_type", SetLastError = false)]
    public static extern int sqlite3_value_type(IntPtr pVal);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_value_int64", SetLastError = false)]
    public static extern long sqlite3_value_int64(IntPtr pVal);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_value_double", SetLastError = false)]
    public static extern double sqlite3_value_double(IntPtr pVal);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_value_text16", SetLastError = false)]
    public static extern IntPtr sqlite3_value_text16(IntPtr pVal);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_value_bytes", SetLastError = false)]
    public static extern int sqlite3_value_bytes(IntPtr pVal);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_value_blob", SetLastError = false)]
    public static extern IntPtr sqlite3_value_blob(IntPtr pVal);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_result_text16", SetLastError = false)]
    public static extern void sqlite3_result_text16(IntPtr pCtx, IntPtr z, int n, IntPtr xDel);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_result_blob64", SetLastError = false)]
    public static extern void sqlite3_result_blob64(IntPtr pCtx, IntPtr z, ulong n, IntPtr xDel);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_result_null", SetLastError = false)]
    public static extern void sqlite3_result_null(IntPtr pCtx);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_result_int", SetLastError = false)]
    public static extern void sqlite3_result_int(IntPtr pCtx, int iVal);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_result_int64", SetLastError = false)]
    public static extern void sqlite3_result_int64(IntPtr pCtx, long iVal);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_result_double", SetLastError = false)]
    public static extern void sqlite3_result_double(IntPtr pCtx, double rVal);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_result_error16", SetLastError = false)]
    public static extern void sqlite3_result_error16(IntPtr pCtx, IntPtr z, int n);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_create_module_v2", SetLastError = false)]
    public static extern int sqlite3_create_module_v2(IntPtr db, IntPtr zName, IntPtr pModule, IntPtr pAux,
        IntPtr xDestroy);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_create_function_v2", SetLastError = false)]
    public static extern int sqlite3_create_function_v2(IntPtr db, IntPtr zFunc, int nArg, int enc, IntPtr p,
        IntPtr xSFunc, IntPtr xStep, IntPtr xFinal, IntPtr xDestroy);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_user_data", SetLastError = false)]
    public static extern IntPtr sqlite3_user_data(IntPtr pCtx);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_prepare_v2", SetLastError = false)]
    public static extern int sqlite3_prepare_v2(IntPtr db, IntPtr zSql, int nBytes, IntPtr ppStmt, IntPtr pzTail);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_prepare16_v2", SetLastError = false)]
    public static extern int sqlite3_prepare16_v2(IntPtr db, IntPtr zSql, int nBytes, IntPtr ppStmt, IntPtr pzTail);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_reset", SetLastError = false)]
    public static extern int sqlite3_reset(IntPtr pStmt);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_clear_bindings", SetLastError = false)]
    public static extern int sqlite3_clear_bindings(IntPtr pStmt);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_bind_parameter_count", SetLastError = false)]
    public static extern int sqlite3_bind_parameter_count(IntPtr pStmt);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_bind_parameter_name", SetLastError = false)]
    public static extern IntPtr sqlite3_bind_parameter_name(IntPtr pStmt, int i);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_bind_null", SetLastError = false)]
    public static extern int sqlite3_bind_null(IntPtr pStmt, int i);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_bind_int", SetLastError = false)]
    public static extern int sqlite3_bind_int(IntPtr pStmt, int i, int iValue);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_bind_int64", SetLastError = false)]
    public static extern int sqlite3_bind_int64(IntPtr pStmt, int i, long iValue);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_bind_double", SetLastError = false)]
    public static extern int sqlite3_bind_double(IntPtr pStmt, int i, double rValue);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_bind_blob64", SetLastError = false)]
    public static extern int sqlite3_bind_blob64(IntPtr pStmt, int i, IntPtr zData, ulong nData, IntPtr xDel);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_bind_text16", SetLastError = false)]
    public static extern int sqlite3_bind_text16(IntPtr pStmt, int i, IntPtr zData, int nData, IntPtr xDel);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_step", SetLastError = false)]
    public static extern int sqlite3_step(IntPtr pStmt);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_column_count", SetLastError = false)]
    public static extern int sqlite3_column_count(IntPtr pStmt);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_column_name16", SetLastError = false)]
    public static extern IntPtr sqlite3_column_name16(IntPtr pStmt, int N);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_column_type", SetLastError = false)]
    public static extern int sqlite3_column_type(IntPtr pStmt, int i);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_column_int", SetLastError = false)]
    public static extern int sqlite3_column_int(IntPtr pStmt, int i);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_column_int64", SetLastError = false)]
    public static extern long sqlite3_column_int64(IntPtr pStmt, int i);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_column_double", SetLastError = false)]
    public static extern double sqlite3_column_double(IntPtr pStmt, int i);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_column_text16", SetLastError = false)]
    public static extern IntPtr sqlite3_column_text16(IntPtr pStmt, int i);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_column_blob", SetLastError = false)]
    public static extern IntPtr sqlite3_column_blob(IntPtr pStmt, int i);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_column_bytes", SetLastError = false)]
    public static extern int sqlite3_column_bytes(IntPtr pStmt, int i);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_finalize", SetLastError = false)]
    public static extern int sqlite3_finalize(IntPtr pStmt);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_interrupt", SetLastError = false)]
    public static extern void sqlite3_interrupt(IntPtr db);

    [DllImport(SQLITE_DLL, EntryPoint = "sqlite3_get_autocommit", SetLastError = false)]
    public static extern int sqlite3_get_autocommit(IntPtr db);
}   
#pragma warning restore CA1401 // P/Invokes should not be visible
