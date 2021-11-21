using SqlNotebookScript.Core.AdoModules;
using SqlNotebookScript.Core.GenericModules;
using SqlNotebookScript.Core.SqliteInterop;
using SqlNotebookScript.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using static SqlNotebookScript.Core.SqliteInterop.NativeMethods;

namespace SqlNotebookScript.Core;

public sealed class Notebook : IDisposable {
    public static bool CancelInProgress { get; set; } = false;

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

    public object QueryValue(string sql) =>
        QueryValue(sql, Array.Empty<object>());

    public object QueryValue(string sql, IReadOnlyDictionary<string, object> args) =>
        GetSingleValue(Query(sql, args, -1));

    public object QueryValue(string sql, IReadOnlyList<object> args) =>
        GetSingleValue(Query(sql, args, -1));

    private static object GetSingleValue(SimpleDataTable dt) =>
        dt.Rows.Count == 1 && dt.Columns.Count == 1
        ? dt.Rows[0].GetValue(0)
        : null;

    public PreparedStatement Prepare(string sql) => new(_sqlite, sql);

    private static SimpleDataTable QueryCore(
        string sql,
        IReadOnlyDictionary<string, object> namedArgs,
        IReadOnlyList<object> orderedArgs,
        bool returnResult,
        IntPtr db, // sqlite3*
        Func<bool> cancelling,
        int maxRows
        ) {
        if (cancelling != null && cancelling()) {
            throw new OperationCanceledException();
        }

        using PreparedStatement statement = new(db, sql);
        var argArray =
            namedArgs != null ? statement.GetArgs(namedArgs) :
            orderedArgs is object[] a ? a :
            orderedArgs.ToArray();
        return statement.Execute(argArray, returnResult, maxRows, CancellationToken.None);
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
        CancelInProgress = true;
        sqlite3_interrupt(_sqlite);
    }

    public void EndUserCancel() {
        CancelInProgress = false;
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

    private bool GetCancelling() => CancelInProgress;
}
