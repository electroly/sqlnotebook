using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using Npgsql;
using SqlNotebookScript.Core;
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.Interpreter;

public sealed class ImportDatabaseStmtRunner {
    private readonly Notebook _notebook;
    private readonly ScriptEnv _env;
    private readonly ScriptRunner _runner;
    private readonly Ast.ImportDatabaseStmt _stmt;
    private readonly CancellationToken _cancel;

    private readonly bool _link = false;
    private readonly bool _truncateExistingTable = false;
    private readonly bool _temporaryTable = false;
    private readonly string _vendor;
    private readonly string _srcSchemaName = "";
    private readonly string _srcTableName;
    private readonly string _sql;
    private readonly string _dstTableName;
    private readonly string _connectionString;

    // must be run from the SQLite thread
    public static void Run(Notebook notebook, ScriptEnv env, ScriptRunner runner, Ast.ImportDatabaseStmt stmt,
        CancellationToken cancel
        ) {
        ImportDatabaseStmtRunner importer = new(notebook, env, runner, stmt, cancel);
        SqlUtil.WithTransaction(notebook, importer.Import);
    }

    private ImportDatabaseStmtRunner(Notebook notebook, ScriptEnv env, ScriptRunner runner, Ast.ImportDatabaseStmt stmt,
        CancellationToken cancel
        ) {
        _notebook = notebook;
        _env = env;
        _runner = runner;
        _stmt = stmt;
        _cancel = cancel;

        // vendor
        var vendorObj = _runner.EvaluateExpr(_stmt.VendorExpr, _env);
        if (vendorObj is not string) {
            throw new Exception("IMPORT DATABASE: The 'vendor' parameter must be a string.");
        }
        _vendor = ((string)vendorObj).ToLowerInvariant();
        switch (_vendor) {
            case "mssql":
            case "pgsql":
            case "mysql":
                break;
            default:
                throw new Exception($"IMPORT DATABASE: The vendor \"{_vendor}\" is not recognized.");
        }

        // src-table-name, sql
        if (stmt.SrcTableNameExprOrNull != null) {
            _srcTableName = _runner.EvaluateIdentifierOrExpr(stmt.SrcTableNameExprOrNull, _env);
            _sql = null;
            if (_vendor == "mssql" && _srcTableName.Contains('.')) {
                var parts = _srcTableName.Split(new[] { '.' }, 2);
                _srcSchemaName = parts[0];
                _srcTableName = parts[1];
            }
        } else if (stmt.SqlExprOrNull != null) {
            var sqlObj = _runner.EvaluateExpr(stmt.SqlExprOrNull, _env);
            if (sqlObj is not string) {
                throw new Exception("IMPORT DATABASE: The 'sql' parameter must be a string.");
            }
            _sql = (string)sqlObj;
            _srcTableName = null;
        } else {
            throw new Exception("IMPORT DATABASE: Internal error. No source table and no query specified.");
        }

        // dst-table-name
        if (_stmt.DstTableNameExprOrNull != null) {
            _dstTableName = _runner.EvaluateIdentifierOrExpr(_stmt.DstTableNameExprOrNull, _env);
        } else if (_stmt.SrcTableNameExprOrNull != null) {
            _dstTableName = _runner.EvaluateIdentifierOrExpr(_stmt.SrcTableNameExprOrNull, _env);
        } else if (_stmt.SqlExprOrNull != null) {
            throw new Exception("IMPORT DATABASE: A target table name (with INTO) is required when the source is a query.");
        } else {
            throw new Exception("IMPORT DATABASE: Internal error. No destination table name.");
        }

        // connection-string
        var connectionStringObj = _runner.EvaluateExpr(_stmt.ConnectionStringExpr, _env);
        if (connectionStringObj is not string) {
            throw new Exception("IMPORT DATABASE: The 'connection-string' parameter must be a string.");
        }
        _connectionString = (string)connectionStringObj;

        // OPTIONS
        foreach (var option in _stmt.OptionsList.GetOptionKeys()) {
            switch (option) {
                case "LINK":
                    _link = _stmt.OptionsList.GetOptionBool(option, _runner, _env, false);
                    break;

                case "TRUNCATE_EXISTING_TABLE":
                    _truncateExistingTable = _stmt.OptionsList.GetOptionBool(option, _runner, _env, false);
                    break;

                case "TEMPORARY_TABLE":
                    _temporaryTable = _stmt.OptionsList.GetOptionBool(option, _runner, _env, false);
                    break;

                default:
                    throw new Exception($"\"{option}\" is not a recognized option name.");
            }
        }

        // validate option LINK
        if (_link) {
            if (_sql != null) {
                throw new Exception("IMPORT DATABASE: When creating a live link, a source table name must be provided, not an SQL query.");
            }
            if (TableExists(_dstTableName)) {
                throw new Exception($"IMPORT DATABASE: The table \"{_dstTableName}\" already exists.");
            }
            if (_truncateExistingTable) {
                throw new Exception($"IMPORT DATABASE: When creating a live link, the TRUNCATE_EXISTING_TABLE option is not valid.");
            }
            if (_temporaryTable) {
                throw new Exception($"IMPORT DATABASE: When creating a live link, the TEMPORARY_TABLE option is not valid.");
            }
        }
    }

    private bool TableExists(string name) =>
        _notebook.Query($"PRAGMA TABLE_INFO({name.DoubleQuote()});").FullCount > 0;

    private void Import() {
        var truncatedDstTableName =
            _dstTableName.Length > 50
            ? _dstTableName[..50] + "..."
            : _dstTableName;
        using var status = ExecutionStatus.Start(truncatedDstTableName);
        if (_link) {
            ImportLink();
        } else {
            ImportCopy(status);
        }
    }

    private void ImportLink() {
        if (_vendor == "mssql") {
            _notebook.Execute(
                $"CREATE VIRTUAL TABLE {_dstTableName.DoubleQuote()} USING mssql " +
                $"('{_connectionString.Replace("'", "''")}', '{_srcTableName.Replace("'", "''")}', " +
                $"'{_srcSchemaName.Replace("'", "''")}')");
        } else {
            _notebook.Execute(
                $"CREATE VIRTUAL TABLE {_dstTableName.DoubleQuote()} USING {_vendor} " +
                $"('{_connectionString.Replace("'", "''")}', '{_srcTableName.Replace("'", "''")}')");
        }
    }

    private void ImportCopy(ExecutionStatus.InFlight status) {
        IDbConnection srcConnection = null;
        IDbCommand srcCommand = null;
        IDataReader reader = null;

        try {
            srcConnection =
                _vendor switch {
                    "mssql" => new SqlConnection(_connectionString),
                    "pgsql" => new NpgsqlConnection(_connectionString),
                    "mysql" => new MySqlConnection(_connectionString),
                    _ => throw new Exception("IMPORT DATABASE: Internal error. Invalid vendor.")
                };
            srcConnection.Open();

            srcCommand = srcConnection.CreateCommand();
            if (_sql != null) {
                srcCommand.CommandText = _sql;
            } else {
                var quotedSourceTable =
                    _vendor == "mssql" && !string.IsNullOrEmpty(_srcSchemaName)
                    ? $"{_srcSchemaName.DoubleQuote()}.{_srcTableName.DoubleQuote()}"
                    : _srcTableName.DoubleQuote();
                srcCommand.CommandText = $"SELECT * FROM {quotedSourceTable}";
            }
            reader = srcCommand.ExecuteReader();

            ImportCopyCore(reader, status);
        } finally {
            // Dispose on another thread; it can take time.
            _ = Task.Run(() => {
                reader?.Dispose();
                srcCommand?.Dispose();
                srcConnection?.Dispose();
            }, CancellationToken.None);
        }
    }

    private void ImportCopyCore(IDataReader reader, ExecutionStatus.InFlight status) {
        List<string> srcColumnNames = new();
        List<Type> srcColumnTypes = new();
        for (var i = 0; i < reader.FieldCount; i++) {
            srcColumnNames.Add(reader.GetName(i));
            srcColumnTypes.Add(reader.GetFieldType(i));
        }

        // Create table
        List<string> dstCreateColumns = new();
        for (int i = 0; i < srcColumnNames.Count; i++) {
            var t = srcColumnTypes[i];
            string sqlType;
            if (t == typeof(short) || t == typeof(int) || t == typeof(long) || t == typeof(byte) || t == typeof(bool)) {
                sqlType = "integer";
            } else if (t == typeof(float) || t == typeof(double) || t == typeof(decimal)) {
                sqlType = "real";
            } else {
                sqlType = "text";
            }
            dstCreateColumns.Add("\"" + srcColumnNames[i].Replace("\"", "\"\"") + "\" " + sqlType);
        }
        _notebook.Execute(
            $"CREATE {(_temporaryTable ? "TEMPORARY" : "")} TABLE IF NOT EXISTS {_dstTableName.DoubleQuote()} " +
            $"({string.Join(", ", dstCreateColumns)})");

        if (_truncateExistingTable) {
            _notebook.Execute($"DELETE FROM {_dstTableName.DoubleQuote()};");
        }

        // Copy data
        var insertSql = $"INSERT INTO {_dstTableName.DoubleQuote()} VALUES ({string.Join(", ", Enumerable.Range(0, reader.FieldCount).Select(x => "?"))})";
        using var insertStmt = _notebook.Prepare(insertSql);
        long rowsCopied = 0;
        using RowProgressUpdateTask statusUpdate = new(status, () => Interlocked.Read(ref rowsCopied));

        object[][] NewBuffer() {
            const int CAPACITY = 100; // empirically determined
            var buffer = new object[CAPACITY][];
            for (var i = 0; i < CAPACITY; i++) {
                buffer[i] = new object[reader.FieldCount];
            }
            return buffer;
        }

        void Produce(object[][] buffer, out int batchCount) {
            batchCount = 0;
            while (batchCount < buffer.Length && reader.Read()) {
                reader.GetValues(buffer[batchCount]);
                batchCount++;
            }
        }

        void Consume(object[][] buffer, int batchCount, long totalSoFar) {
            for (var i = 0; i < batchCount; i++) {
                insertStmt.ExecuteStream(buffer[i], null, null, _cancel);
                Interlocked.Increment(ref rowsCopied);
            }
        }

        OverlappedProducerConsumer.Go(NewBuffer, Produce, Consume, _cancel);
    }
}
