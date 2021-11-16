using SqlNotebookScript.Core.AdoModules;
using SqlNotebookScript.Core.GenericModules;
using SqlNotebookScript.Core.SqliteInterop;
using SqlNotebookScript.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using static SqlNotebookScript.Core.SqliteInterop.NativeMethods;

namespace SqlNotebookScript.Core;

public sealed class Notebook : IDisposable {
    private const int CURRENT_FILE_VERSION = 2;

    private delegate void FreeDelegate(IntPtr p);

    private static readonly Lazy<(IntPtr Ptr, FreeDelegate Delegate)> _freeFunc = new(() => {
        FreeDelegate @delegate = Marshal.FreeHGlobal;
        return (Marshal.GetFunctionPointerForDelegate(@delegate), @delegate);
    });

    private delegate void ExecuteGenericFunctionDelegate(IntPtr a, int b, IntPtr c);

    private static readonly Lazy<(IntPtr Ptr, ExecuteGenericFunctionDelegate Delegate)> _executeGenericFunctionFunc = new(() => {
        ExecuteGenericFunctionDelegate @delegate = ExecuteGenericFunction;
        return (Marshal.GetFunctionPointerForDelegate(@delegate), @delegate);
    });

    private bool _disposedValue;

    private readonly object _lock = new();
    private static readonly object _tokenizeLock = new();

    // Just hang onto these. They are used as unmanaged callbacks.
    private readonly List<object> _delegates = new();

    private List<AdoModuleProvider> _adoModuleProviders =
        new() {
            new MySqlAdoModuleProvider(),
            new PostgreSqlAdoModuleProvider(),
            new SqlServerAdoModuleProvider(),
        };
    private List<GenericModuleProvider> _genericModuleProviders = new();
    private string _originalFilePath;
    private readonly string _workingCopyFilePath;
    private IntPtr _sqlite; // sqlite3*
    private bool _cancelling;

    public NotebookUserData UserData { get; set; }
    public static string ErrorMessage { get; set; }

    public Notebook(string filePath, bool isNew) {
        _workingCopyFilePath = NotebookTempFiles.GetTempFilePath(".working-copy");
        if (!isNew) {
            try {
                File.Copy(filePath, _workingCopyFilePath, overwrite: true);
            } catch {
                File.Delete(_workingCopyFilePath);
                throw;
            }
        }

        _originalFilePath = filePath;
        Invoke(Init);
    }

    private void Dispose(bool disposing) {
        if (!_disposedValue) {
            if (disposing) {
                // dispose managed state (managed objects)
            }

            // free unmanaged resources (unmanaged objects) and override finalizer
            // set large fields to null
            if (_sqlite != IntPtr.Zero) {
                _ = sqlite3_close_v2(_sqlite);
                _sqlite = IntPtr.Zero;
            }
            
            foreach (var x in _adoModuleProviders) {
                x.Dispose();
            }
            _adoModuleProviders = null;

            foreach (var x in _genericModuleProviders) {
                x.Dispose();
            }
            _genericModuleProviders = null;

            UserData = null;

            _disposedValue = true;
        }
    }

    ~Notebook() {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose() {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private readonly record struct CustomFunctions(
        List<CustomTableFunction> CustomTableFunctions, List<CustomScalarFunction> CustomScalarFunctions);

    private static CustomFunctions FindCustomFunctions() {
        var customTableFunctionType = typeof(CustomTableFunction);
        var customScalarFunctionType = typeof(CustomScalarFunction);

        List<CustomTableFunction> customTableFunctions = new();
        List<CustomScalarFunction> customScalarFunctions = new();

        foreach (var type in typeof(CustomTableFunction).Assembly.GetExportedTypes()) {
            if (!type.IsAbstract) {
                if (type.IsAssignableTo(customTableFunctionType)) {
                    customTableFunctions.Add((CustomTableFunction)Activator.CreateInstance(type));
                } else if (type.IsAssignableTo(customScalarFunctionType)) {
                    customScalarFunctions.Add((CustomScalarFunction)Activator.CreateInstance(type));
                }
            }
        }

        return new(customTableFunctions, customScalarFunctions);
    }

    private void Init() {
        FileVersionMigrator.MigrateIfNeeded(_workingCopyFilePath);
            
        using NativeString filePathNative = new(_workingCopyFilePath);

        using NativeBuffer sqliteNative = new(IntPtr.Size);

        SqliteUtil.ThrowIfError(IntPtr.Zero,
            sqlite3_open(filePathNative.Ptr, sqliteNative.Ptr));
        _sqlite = Marshal.ReadIntPtr(sqliteNative.Ptr); // sqlite3*

        foreach (var x in _adoModuleProviders) {
            x.Install(_sqlite);
        }

        var (tableFunctions, scalarFunctions) = FindCustomFunctions();
        foreach (var tableFunction in tableFunctions) {
            GenericModuleProvider provider = new();
            provider.Install(_sqlite, tableFunction);
            _genericModuleProviders.Add(provider);
        }
        foreach (var scalarFunction in scalarFunctions) {
            RegisterGenericFunction(scalarFunction);
        }

        CustomFunctionsProvider.InstallCustomFunctions(_sqlite);

        ReadUserDataFromDatabase();

        Execute("DROP TABLE IF EXISTS _sqlnotebook_userdata;");
        Execute("DROP TABLE IF EXISTS _sqlnotebook_version;");
    }

    private delegate object GenericFunctionExecuteDelegate(IReadOnlyList<object> args);

    private void RegisterGenericFunction(CustomScalarFunction scalarFunction) {
        using NativeString nameNative = new(scalarFunction.Name);

        GenericFunctionExecuteDelegate @delegate = scalarFunction.Execute;
        _delegates.Add(@delegate);

        SqliteUtil.ThrowIfError(_sqlite,
            sqlite3_create_function_v2(
                db: _sqlite,
                zFunc: nameNative.Ptr,
                nArg: scalarFunction.ParamCount,
                enc: SQLITE_UTF16 | (scalarFunction.IsDeterministic ? SQLITE_DETERMINISTIC : 0),
                p: Marshal.GetFunctionPointerForDelegate(@delegate),
                xSFunc: _executeGenericFunctionFunc.Value.Ptr,
                xStep: IntPtr.Zero,
                xFinal: IntPtr.Zero,
                xDestroy: IntPtr.Zero));
    }

    private static void ExecuteGenericFunction(IntPtr ctx, int argc, IntPtr argv) {
        try {
            var @delegate = Marshal.GetDelegateForFunctionPointer<GenericFunctionExecuteDelegate>(
                sqlite3_user_data(ctx));
            List<object> args = new();
            for (var i = 0; i < argc; i++) {
                args.Add(GenericModuleProvider.GetArg(Marshal.ReadIntPtr(argv + i * IntPtr.Size)));
            }
            var result = @delegate(args);
            SqliteUtil.Result(ctx, result);
        } catch (Exception ex) {
            NativeString messageNative16 = new(ex.GetExceptionMessage(), utf16: true);
            sqlite3_result_error16(ctx, messageNative16.Ptr, -1);
        }
    }

    public void Save() {
        if (IsTransactionActive()) {
            throw new Exception("A transaction is active. Execute either \"COMMIT\" or \"ROLLBACK\" to end the transaction before saving.");
        }

        WriteFileVersionAndUserDataToDatabase();

        SqliteUtil.ThrowIfError(_sqlite, sqlite3_close_v2(_sqlite));
        _sqlite = IntPtr.Zero;

        try {
            File.Copy(_workingCopyFilePath, _originalFilePath, overwrite: true);
        } finally {
            Init();
        }
    }

    private void WriteFileVersionAndUserDataToDatabase() {
        // if there is no user data, then write a plain SQLite database without any special SQL Notebook tables
        if (UserData.Items.Count == 0) {
            Execute("DROP TABLE IF EXISTS _sqlnotebook_version;");
            Execute("DROP TABLE IF EXISTS _sqlnotebook_userdata;");
            return;
        }

        Execute("CREATE TABLE IF NOT EXISTS _sqlnotebook_version (version);");
        Execute("DELETE FROM _sqlnotebook_version;");
        Execute("INSERT INTO _sqlnotebook_version (version) VALUES (@version);",
            new Dictionary<string, object> {
                ["@version"] = CURRENT_FILE_VERSION
            });

        Execute("CREATE TABLE IF NOT EXISTS _sqlnotebook_userdata (json);");
        Execute("DELETE FROM _sqlnotebook_userdata;");
        Execute("INSERT INTO _sqlnotebook_userdata (json) VALUES (@json)",
            new Dictionary<string, object> {
                ["@json"] = JsonSerializer.Serialize(UserData, new JsonSerializerOptions())
            });
    }

    private void ReadUserDataFromDatabase() {
        Execute("CREATE TABLE IF NOT EXISTS _sqlnotebook_userdata (json);");
        var table = Query("SELECT json FROM _sqlnotebook_userdata", -1);
        if (table.Rows.Count == 0) {
            UserData = new();
            return;
        }
        try {
            var json = (string)table.Rows[0][0];
            UserData = JsonSerializer.Deserialize<NotebookUserData>(json, new JsonSerializerOptions());
        } catch {
            UserData = new();
        }
    }

    public void Invoke(Action action) {
        lock (_lock) {
            action();
        }
    }

    public bool TryInvoke(Action action) {
        if (Monitor.TryEnter(_lock)) {
            try {
                action();
                return true;
            } finally {
                Monitor.Exit(_lock);
            }
        } else {
            return false;
        }
    }

    public void Execute(string sql) {
        Execute(sql, Array.Empty<object>());
    }

    private static IReadOnlyDictionary<string, object> ToLowercaseKeys(IReadOnlyDictionary<string, object> dict) {
        var allLowercase = true;
        foreach (var key in dict.Keys) {
            if (key != key.ToLowerInvariant()) {
                allLowercase = false;
                break;
            }
        }

        if (allLowercase) {
            return dict;
        } else {
            Dictionary<string, object> newDict = new();
            foreach (var pair in dict) {
                newDict.Add(pair.Key.ToLowerInvariant(), pair.Value);
            }
            return newDict;
        }
    }

    public void Execute(string sql, IReadOnlyDictionary<string, object> args) {
        QueryCore(sql, ToLowercaseKeys(args), null, false, _sqlite, GetCancelling, -1);
    }

    public void Execute(string sql, IReadOnlyList<object> args) {
        QueryCore(sql, null, args, false, _sqlite, GetCancelling, -1);
    }

    public SimpleDataTable Query(string sql, int maxRows) {
        return Query(sql, Array.Empty<object>(), maxRows);
    }

    public SimpleDataTable Query(string sql, IReadOnlyDictionary<string, object> args, int maxRows) {
        return QueryCore(sql, ToLowercaseKeys(args), null, true, _sqlite, GetCancelling, maxRows);
    }

    public SimpleDataTable Query(string sql, IReadOnlyList<object> args, int maxRows) {
        return QueryCore(sql, null, args, true, _sqlite, GetCancelling, maxRows);
    }

    public SimpleDataTable SpecialReadOnlyQuery(string sql, IReadOnlyDictionary<string, object> args) {
        SimpleDataTable result = null;
        var success = TryInvoke(() => {
            result = Query(sql, args, -1);
        });
        if (success) {
            return result;
        }

        // Another operation is in progress, so open a new connection for this query.
        using NativeString filePathNative = new(_workingCopyFilePath);
        using NativeBuffer sqlitePtrNative = new(IntPtr.Size);
        SqliteUtil.ThrowIfError(IntPtr.Zero,
            sqlite3_open_v2(filePathNative.Ptr, sqlitePtrNative.Ptr,
                SQLITE_OPEN_READONLY, IntPtr.Zero));
        var tempSqlite = Marshal.ReadIntPtr(sqlitePtrNative.Ptr); // sqlite3*
        try {
            return QueryCore(sql, args, null, true, tempSqlite, null, -1);
        } finally {
            _ = sqlite3_close_v2(tempSqlite);
        }
    }

    public object QueryValue(string sql) {
        return QueryValue(sql, Array.Empty<object>());
    }

    public object QueryValue(string sql, IReadOnlyDictionary<string, object> args) =>
        GetSingleValue(Query(sql, args, -1));

    public object QueryValue(string sql, IReadOnlyList<object> args) =>
        GetSingleValue(Query(sql, args, -1));

    private static object GetSingleValue(SimpleDataTable dt) =>
        dt.Rows.Count == 1 && dt.Columns.Count == 1
        ? dt.Rows[0].GetValue(0)
        : null;

    private static SimpleDataTable QueryCore(
        string sql,
        IReadOnlyDictionary<string, object> namedArgs,
        IReadOnlyList<object> orderedArgs,
        bool returnResult,
        IntPtr db, // sqlite3*
        Func<bool> cancelling,
        int maxRows
        ) {
        System.Diagnostics.Debug.Assert(maxRows != 0); // -1 is unrestricted, not zero

        if (cancelling != null && cancelling()) {
            throw new OperationCanceledException();
        }

        // namedArgs has lowercase keys
        var stmt = IntPtr.Zero; // sqlite3_stmt*
        try {
            // prepare the statement
            using NativeString sqlNative = new(sql);
            using NativeBuffer stmtNative = new(IntPtr.Size);
            SqliteUtil.ThrowIfError(db,
                sqlite3_prepare_v2(
                    db, sqlNative.Ptr, sqlNative.ByteCount, stmtNative.Ptr, IntPtr.Zero));
            stmt = Marshal.ReadIntPtr(stmtNative.Ptr);
            if (stmt == IntPtr.Zero) {
                throw new Exception("Invalid statement.");
            }

            // bind the arguments
            SqliteUtil.ThrowIfError(db, sqlite3_clear_bindings(stmt));
            int paramCount = sqlite3_bind_parameter_count(stmt);
            for (int i = 1; i <= paramCount; i++) {
                object value;

                if (namedArgs != null) {
                    var paramName = Marshal.PtrToStringUTF8(sqlite3_bind_parameter_name(stmt, i));
                    if (!namedArgs.TryGetValue(paramName.ToLowerInvariant(), out value)) {
                        throw new ArgumentException($"Missing value for SQL parameter \"{paramName}\".");
                    }
                } else if (orderedArgs != null) {
                    value = orderedArgs[i - 1];
                } else {
                    throw new ArgumentException($"{nameof(namedArgs)} or {nameof(orderedArgs)} must be non-null.");
                }

                if (value == null) {
                    SqliteUtil.ThrowIfError(db, sqlite3_bind_null(stmt, i));
                } else {
                    switch (value) {
                        case DBNull:
                            SqliteUtil.ThrowIfError(db, sqlite3_bind_null(stmt, i));
                            break;
                        case int intValue:
                            SqliteUtil.ThrowIfError(db, sqlite3_bind_int(stmt, i, intValue));
                            break;
                        case long longValue:
                            SqliteUtil.ThrowIfError(db, sqlite3_bind_int64(stmt, i, longValue));
                            break;
                        case double dblValue:
                            SqliteUtil.ThrowIfError(db, sqlite3_bind_double(stmt, i, dblValue));
                            break;
                        case byte[] arr: 
                            {
                                // SQLite will take ownership of this blob and call our free function when it's done.
                                var copy = Marshal.AllocHGlobal(arr.Length);
                                Marshal.Copy(arr, 0, copy, arr.Length);
                                SqliteUtil.ThrowIfError(db,
                                    sqlite3_bind_blob64(
                                        stmt, i, copy, (ulong)arr.Length, _freeFunc.Value.Ptr));
                                break;
                            }
                        default:
                            {
                                var strValue = value.ToString();
                                var bytes = Encoding.Unicode.GetBytes(strValue);

                                // SQLite will take ownership of this string and call our free function when it's done.
                                var strNative = Marshal.AllocHGlobal(bytes.Length + 1);
                                Marshal.Copy(bytes, 0, strNative, bytes.Length);
                                Marshal.WriteByte(strNative + bytes.Length, 0);

                                SqliteUtil.ThrowIfError(db,
                                    sqlite3_bind_text16(
                                        stmt, i, strNative, bytes.Length, _freeFunc.Value.Ptr));
                                break;
                            }
                    }
                }
            }

            // execute the statement
            List<string> columnNames = new();
            int columnCount = 0;
            if (returnResult) {
                columnCount = sqlite3_column_count(stmt);
                for (int i = 0; i < columnCount; i++) {
                    var columnName = Marshal.PtrToStringUni(sqlite3_column_name16(stmt, i));
                    columnNames.Add(columnName);
                }
            }

            // read the results
            List<object[]> rows = new();
            long fullCount = 0;
            while (true) {
                if (cancelling != null && cancelling()) {
                    throw new OperationCanceledException();
                }

                int ret = sqlite3_step(stmt);
                if (ret == SQLITE_DONE) {
                    break;
                } else if (ret == SQLITE_ROW) {
                    fullCount++;
                    if (maxRows >= 0 && fullCount > maxRows) {
                        // Keep counting, but don't read the rows.
                        continue;
                    }
                    if (returnResult) {
                        var rowData = new object[columnCount];
                        for (int i = 0; i < columnCount; i++) {
                            switch (sqlite3_column_type(stmt, i)) {
                                case SQLITE_INTEGER:
                                    rowData[i] = sqlite3_column_int64(stmt, i);
                                    break;
                                case SQLITE_FLOAT:
                                    rowData[i] = sqlite3_column_double(stmt, i);
                                    break;
                                case SQLITE_TEXT:
                                    rowData[i] = Marshal.PtrToStringUni(sqlite3_column_text16(stmt, i));
                                    break;
                                case SQLITE_BLOB: {
                                    var cb = sqlite3_column_bytes(stmt, i);
                                    var sourceBuffer = sqlite3_column_blob(stmt, i);
                                    var copy = new byte[cb];
                                    Marshal.Copy(sourceBuffer, copy, 0, cb);
                                    rowData[i] = copy;
                                    break;
                                }
                                case SQLITE_NULL:
                                    rowData[i] = DBNull.Value;
                                    break;
                                default:
                                    throw new InvalidOperationException("Unrecognized result from sqlite3_column_type().");
                            }
                        }
                        rows.Add(rowData);
                    }
                } else if (ret == SQLITE_READONLY) {
                    throw new InvalidOperationException("Unable to write to a read-only notebook file.");
                } else if (ret == SQLITE_BUSY || ret == SQLITE_LOCKED) {
                    throw new InvalidOperationException("The notebook file is locked by another application.");
                } else if (ret == SQLITE_CORRUPT) {
                    throw new InvalidOperationException("The notebook file is corrupted.");
                } else if (ret == SQLITE_NOTADB) {
                    throw new InvalidOperationException("This is not an SQLite database file.");
                } else if (ret == SQLITE_INTERRUPT) {
                    throw new OperationCanceledException("SQL query canceled by the user.");
                } else if (ret == SQLITE_ERROR) {
                    SqliteUtil.ThrowIfError(db, SQLITE_ERROR);
                } else {
                    throw new InvalidOperationException($"Unrecognized result ({ret}) from sqlite3_step().");
                }
            }

            if (returnResult) {
                return new SimpleDataTable(columnNames, rows, fullCount);
            } else {
                return null;
            }
        } finally {
            if (stmt != IntPtr.Zero) {
                _ = sqlite3_finalize(stmt);
            }
        }
    }

    public void SaveAs(string filePath) {
        _originalFilePath = filePath;
        Save();
    }

    public string GetFilePath() {
        return _originalFilePath;
    }

    public IReadOnlyList<Token> Tokenize(string input) {
        using NativeBuffer scratch = new(IntPtr.Size);
        List<Token> list = new();
        using NativeString inputNative = new(input);
        lock (_tokenizeLock) {
            var tokenType = 0;
            var oldPos = 0;
            var pos = 0;
            var len = inputNative.ByteCount;
            while ((tokenType = GetToken(inputNative.Ptr, ref oldPos, ref pos, len, scratch)) > 0) {
                // Grab the substring from 'oldPos' to 'pos'
                var utf8TokenLength = pos - oldPos;
                var utf8TokenBytes = new byte[utf8TokenLength];
                Marshal.Copy(inputNative.Ptr + oldPos, utf8TokenBytes, 0, utf8TokenLength);
                var tokenText = Encoding.UTF8.GetString(utf8TokenBytes);

                list.Add(new() {
                    Type = (TokenType)tokenType,
                    Text = tokenText,
                    Utf8Start = (ulong)oldPos,
                    Utf8Length = (ulong)(pos - oldPos)
                });

                oldPos = pos;
            }
        }
        return list;
    }

    private static int GetToken(
        IntPtr str, // const char*
        ref int oldPos,
        ref int pos,
        int len,
        NativeBuffer scratch // IntPtr sized buffer
        ) {
        const int TK_SPACE = (int)TokenType.Space;
        if (pos >= len) {
            return 0;
        }
        int tokenType, tokenLen;
        do {
            tokenLen = SxGetToken(str + pos, scratch.Ptr);
            tokenType = (int)Marshal.ReadIntPtr(scratch.Ptr);
            oldPos = pos;
            pos += tokenLen;
        } while (tokenType == TK_SPACE && pos < len);

        return tokenType == TK_SPACE ? 0 : tokenType;
    }

    public void BeginUserCancel() {
        _cancelling = true;
        sqlite3_interrupt(_sqlite);
    }

    public void EndUserCancel() {
        _cancelling = false;
    }

    public IReadOnlyDictionary<string, string> GetScripts() {
        Dictionary<string, string> dict = new();
        foreach (var item in UserData.Items) {
            if (item.Type == "Script") {
                dict.Add(item.Name.ToLowerInvariant(), item.Data ?? "");
            }
        }
        return dict;
    }

    public bool IsTransactionActive() {
        return sqlite3_get_autocommit(_sqlite) == 0;
    }

    private bool GetCancelling() => _cancelling;
}
