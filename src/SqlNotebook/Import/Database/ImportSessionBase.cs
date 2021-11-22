using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlNotebook.Properties;
using SqlNotebookScript.Core;
using SqlNotebookScript.Utils;

namespace SqlNotebook.Import.Database;

public abstract partial class ImportSessionBase<TConnectionStringBuilder> : IImportSession
    where TConnectionStringBuilder : DbConnectionStringBuilder, new() {

    public abstract string ProductName { get; }
    public abstract string GetCreateVirtualTableStatement(string sourceTableName, string notebookTableName);
    public IReadOnlyList<string> TableNames { get; protected set; } = Array.Empty<string>();
    protected abstract IDbConnection CreateConnection(TConnectionStringBuilder builder);
    protected abstract void ReadTableNames(IDbConnection connection);
    protected abstract TConnectionStringBuilder CreateBuilder(string connStr);
    protected abstract string GetDisplayName();
    protected abstract DatabaseConnectionForm.BasicOptions GetBasicOptions(TConnectionStringBuilder builder);
    protected abstract void SetBasicOptions(TConnectionStringBuilder builder, DatabaseConnectionForm.BasicOptions opt);
    protected abstract string GetDefaultConnectionString();
    protected abstract void SetDefaultConnectionString(string str);

    protected TConnectionStringBuilder _builder = new();

    public bool FromConnectForm(IWin32Window owner) {
        var successfulConnect = false;

        do {
            var initialConnectionString = GetDefaultConnectionString();
            if (!string.IsNullOrWhiteSpace(initialConnectionString)) {
                try {
                    _builder.ConnectionString = initialConnectionString;
                } catch { }
            }

            using DatabaseConnectionForm f = new(
                $"Connect to {ProductName}", 
                _builder,
                b => GetBasicOptions((TConnectionStringBuilder)b),
                (b, o) => SetBasicOptions((TConnectionStringBuilder)b, o));
            if (f.ShowDialog(owner) != DialogResult.OK) {
                return false;
            }

            // Save the connection string for next time, even if it fails.
            SetDefaultConnectionString(_builder.ConnectionString);
            Settings.Default.Save();

            successfulConnect = DoConnect(owner);
        } while (!successfulConnect);

        return true;
    }

    private bool DoConnect(IWin32Window owner) {
        WaitForm.GoWithCancelByWalkingAway(
            owner, "Database Connection", $"Accessing {ProductName}...", out var success,
            () => {
            using var connection = CreateConnection(_builder);
            connection.Open();
            ReadTableNames(connection);
        });
        return success;
    }

    public void ImportTableByCopyingData(string quotedSourceTable, string targetTable, Notebook notebook,
        CancellationToken cancel) {
        IDbConnection srcConnection = null;
        IDbCommand srcCommand = null;
        IDataReader reader = null;
        
        try {
            srcConnection = CreateConnection(_builder);
            srcConnection.Open();

            srcCommand = srcConnection.CreateCommand();
            srcCommand.CommandText = $"SELECT * FROM {quotedSourceTable}";
            reader = srcCommand.ExecuteReader();

            ImportTableByCopyingDataCore(targetTable, notebook, reader, cancel);
        } finally {
            // dispose on another thread; it can take time
            _ = Task.Run(() => {
                reader?.Dispose();
                srcCommand?.Dispose();
                srcConnection?.Dispose();
            }, CancellationToken.None);
        }
    }

    private static void ImportTableByCopyingDataCore(string targetTable, Notebook notebook, IDataReader reader,
        CancellationToken cancel
        ) {
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
        notebook.Execute($"CREATE TABLE {targetTable.DoubleQuote()} (" + string.Join(", ", dstCreateColumns) + ")");

        // Copy data
        var insertSql = $"INSERT INTO {targetTable.DoubleQuote()} VALUES ({string.Join(", ", Enumerable.Range(0, reader.FieldCount).Select(x => "?"))})";
        using var insertStmt = notebook.Prepare(insertSql);
        long rowsCopied = 0;
        using RowsCopiedProgressUpdateTask statusUpdate = new(targetTable, () => Interlocked.Read(ref rowsCopied));

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
                insertStmt.ExecuteStream(buffer[i], null, null, cancel);
                Interlocked.Increment(ref rowsCopied);
            }
        }

        OverlappedProducerConsumer.Go(NewBuffer, Produce, Consume, cancel);
    }
}
