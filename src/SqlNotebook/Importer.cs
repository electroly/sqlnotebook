// SQL Notebook
// Copyright (C) 2016 Brian Luft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using MySql.Data.MySqlClient;
using Npgsql;
using SqlNotebookCore;
using SqlNotebookScript.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlNotebook
{
    public sealed class RecentDataSource {
        public Type ImportSessionType;
        public string DisplayName;
        public string ConnectionString;
    }

    public sealed class Importer {
        private readonly NotebookManager _manager;
        private readonly Notebook _notebook;
        private readonly IWin32Window _owner;

        public Importer(NotebookManager manager, IWin32Window owner) {
            _manager = manager;
            _notebook = manager.Notebook;
            _owner = owner;
        }

        public static string CreateUniqueName(string prefix, IReadOnlyList<string> existingNames) {
            var name = prefix; // first try it without a number on the end
            int suffix = 2;
            while (existingNames.Contains(name)) {
                name = prefix + (suffix++);
            }
            return name;
        }

        public async Task DoDatabaseImport<T>() where T : IDatabaseImportSession, new() {
            var session = new T();
            if (session.FromConnectForm(_owner)) {
               await DoCommonImport(session);
            }
        }

        public async Task DoRecentImport(RecentDataSource recent) {
            var session = (IImportSession)Activator.CreateInstance(recent.ImportSessionType);
            session.FromRecent(recent, _owner);
            await DoCommonImport(session);
        }

        private async Task DoCommonImport(IImportSession session) {
            IReadOnlyList<ImportPreviewForm.SelectedTable> selectedTables = null;
            bool csvHasHeaderRow = false, copyData = false;

            using (var frm = new ImportPreviewForm(session)) {
                if (frm.ShowDialog(_owner) != DialogResult.OK) {
                    return;
                }
                selectedTables = frm.SelectedTables;
                csvHasHeaderRow = frm.CsvHasHeaderRow;
                copyData = frm.CopyData;
            }

            var fileSession = session as IFileImportSession;
            var dbSession = session as IDatabaseImportSession;
            if (fileSession != null) {
                fileSession.FileHasHeaderRow = csvHasHeaderRow;
            }

            Action import = () => {
                _notebook.Invoke(() => {
                    _notebook.Execute("BEGIN");

                    try {
                        if (dbSession != null) {
                            DoDatabaseImport(selectedTables, copyData, dbSession);
                        } else if (fileSession != null) {
                            DoDatabaseImport(selectedTables, fileSession);
                        } else {
                            throw new InvalidOperationException("Unexpected session type.");
                        }

                        _notebook.Execute("COMMIT");
                        session.AddToRecentlyUsed();
                    } catch {
                        try { _notebook.Execute("ROLLBACK"); } catch { }
                        throw;
                    }
                });
            };

            _manager.PushStatus("Importing the selected tables...");
            Exception exception = null;
            await Task.Run(() => {
                try {
                    import();
                } catch (Exception ex) {
                    exception = ex;
                }
            });
            _manager.PopStatus();
            _manager.Rescan();

            if (exception == null) {
                _manager.SetDirty();
            } else {
                MessageForm.ShowError(_owner, "Import Error", "The import failed.", exception.Message);
            }
        }

        private void DoDatabaseImport(IReadOnlyList<ImportPreviewForm.SelectedTable> selectedTables, IFileImportSession fileSession) {
            fileSession.PerformImport(
                _notebook,
                selectedTables.Select(x => x.SourceName).ToList(),
                selectedTables.Select(x => x.TargetName).ToList()
            );
        }

        private void DoDatabaseImport(IReadOnlyList<ImportPreviewForm.SelectedTable> selectedTables, bool copyData, IDatabaseImportSession dbSession) {
            foreach (var selectedTable in selectedTables) {
                if (copyData) {
                    if (_notebook.QueryValue("SELECT name FROM sqlite_master WHERE name = '__temp_import'") != null) {
                        _notebook.Execute("DROP TABLE __temp_import");
                    }

                    _notebook.Execute(dbSession.GetCreateVirtualTableStatement(selectedTable.SourceName, "__temp_import"));

                    // create the target physical table with the same schema as the virtual table
                    var tableCols = _notebook.Query($"PRAGMA table_info (__temp_import)");
                    var lines = new List<string>();
                    var pks = new string[tableCols.Rows.Count];
                    for (int i = 0; i < tableCols.Rows.Count; i++) {
                        var name = Convert.ToString(tableCols.Get(i, "name"));
                        var type = Convert.ToString(tableCols.Get(i, "type"));
                        var notNull = Convert.ToInt32(tableCols.Get(i, "notnull")) == 1;
                        var pk = Convert.ToInt32(tableCols.Get(i, "pk"));
                        if (pk > 0) {
                            pks[pk - 1] = name;
                        }
                        lines.Add($"{name.DoubleQuote()} {type} {(notNull ? "NOT NULL" : "")}");
                    }
                    var pkList = string.Join(" , ", pks.Where(x => x != null).Select(x => x.DoubleQuote()));
                    if (pkList.Any()) {
                        lines.Add($"PRIMARY KEY ( {pkList} )");
                    }
                    _notebook.Execute($"CREATE TABLE {selectedTable.TargetName.DoubleQuote()} ( {string.Join(" , ", lines)} )");

                    // copy all data from the virtual table to the physical table
                    _notebook.Execute($"INSERT INTO {selectedTable.TargetName.DoubleQuote()} SELECT * FROM __temp_import");

                    // we're done with this import
                    _notebook.Execute($"DROP TABLE __temp_import");
                } else {
                    // just create the virtual table
                    _notebook.Execute(dbSession.GetCreateVirtualTableStatement(selectedTable.SourceName, selectedTable.TargetName));
                }
            }
        }
    }

    public interface IImportSession {
        bool FromRecent(RecentDataSource recent, IWin32Window owner);
        IReadOnlyList<string> TableNames { get; }
        void AddToRecentlyUsed();
    }

    public interface IFileImportSession : IImportSession {
        IReadOnlyList<string> SupportedFileExtensions { get; }
        void FromFilePath(string filePath);
        bool FileHasHeaderRow { get; set; }
        void PerformImport(Notebook notebook, IReadOnlyList<string> sourceTableNames, IReadOnlyList<string> targetTableNames);
                // called on the SQLite thread
    }

    public interface IDatabaseImportSession : IImportSession {
        bool FromConnectForm(IWin32Window owner);
        string GetCreateVirtualTableStatement(string sourceTableName, string notebookTableName);
    }

    public sealed class PgImportSession : AdoImportSession<Npgsql.NpgsqlConnectionStringBuilder> {
        public override string ProductName { get; } = "PostgreSQL";

        public override string GetCreateVirtualTableStatement(string sourceTableName, string notebookTableName) {
            return $"CREATE VIRTUAL TABLE {notebookTableName.DoubleQuote()} USING pgsql ('{_builder.ConnectionString.Replace("'", "''")}', '{sourceTableName.Replace("'", "''")}')";
        }

        protected override IDbConnection CreateConnection(Npgsql.NpgsqlConnectionStringBuilder builder) {
            return new Npgsql.NpgsqlConnection(builder.ConnectionString);
        }

        protected override void ReadTableNames(IDbConnection connection) {
            var tableNames = new List<string>();
            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' ORDER BY table_name";
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        tableNames.Add(reader.GetString(0));
                    }
                }
            }
            TableNames = tableNames;
        }

        protected override Npgsql.NpgsqlConnectionStringBuilder CreateBuilder(string connStr) {
            return new Npgsql.NpgsqlConnectionStringBuilder(connStr);
        }

        protected override string GetDisplayName() {
            return $"{_builder.Database} on {_builder.Host} (PostgreSQL)";
        }

        protected override ImportConnectionForm.BasicOptions GetBasicOptions(NpgsqlConnectionStringBuilder builder) {
            return new ImportConnectionForm.BasicOptions {
                Server = builder.Host,
                Database = builder.Database,
                Username = builder.Username,
                Password = builder.Password
            };
        }

        protected override void SetBasicOptions(NpgsqlConnectionStringBuilder builder, ImportConnectionForm.BasicOptions opt) {
            builder.Host = opt.Server;
            builder.Database = opt.Database;
            builder.Username = opt.Username;
            builder.Password = opt.Password;
        }
    }

    public sealed class MsImportSession : AdoImportSession<SqlConnectionStringBuilder> {
        public override string ProductName { get; } = "Microsoft SQL Server";

        public override string GetCreateVirtualTableStatement(string sourceTableName, string notebookTableName) {
            string schema;
            string table;
            string combinedQuotedName;
            if (sourceTableName.Contains(".")) {
                var parts = sourceTableName.Split(new[] { '.' }, 2);
                schema = parts[0];
                table = parts[1];
                combinedQuotedName = $"{schema.DoubleQuote()}.{table.DoubleQuote()}";
            } else {
                schema = "";
                table = sourceTableName;
                combinedQuotedName = sourceTableName.DoubleQuote();
            }
            return $"CREATE VIRTUAL TABLE {notebookTableName.DoubleQuote()} USING mssql ('{_builder.ConnectionString.Replace("'", "''")}', '{table.Replace("'", "''")}', '{schema.Replace("'", "''")}')";
        }

        protected override IDbConnection CreateConnection(SqlConnectionStringBuilder builder) {
            return new SqlConnection(builder.ConnectionString);
        }

        protected override void ReadTableNames(IDbConnection connection) {
            var tableNames = new List<string>();
            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = "SELECT table_name, table_schema FROM information_schema.tables ORDER BY table_name";
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        var tableName = reader.GetString(0);
                        var tableSchema = reader.GetString(1);
                        var combinedName = tableSchema == "dbo" ? tableName : $"{tableSchema}.{tableName}";
                        tableNames.Add(combinedName);
                    }
                }
            }
            TableNames = tableNames;
        }

        protected override SqlConnectionStringBuilder CreateBuilder(string connStr) {
            return new SqlConnectionStringBuilder(connStr);
        }

        protected override string GetDisplayName() {
            return $"{_builder.InitialCatalog} on {_builder.DataSource} (Microsoft SQL Server)";
        }

        protected override ImportConnectionForm.BasicOptions GetBasicOptions(SqlConnectionStringBuilder builder) {
            return new ImportConnectionForm.BasicOptions {
                Server = builder.DataSource,
                Database = builder.InitialCatalog,
                Username = builder.UserID,
                Password = builder.Password,
                UseWindowsAuth = builder.IntegratedSecurity
            };
        }

        protected override void SetBasicOptions(SqlConnectionStringBuilder builder, ImportConnectionForm.BasicOptions opt) {
            builder.DataSource = opt.Server;
            builder.InitialCatalog = opt.Database;
            builder.UserID = opt.Username;
            builder.Password = opt.Password;
            builder.IntegratedSecurity = opt.UseWindowsAuth;
        }
    }

    public sealed class MyImportSession : AdoImportSession<MySqlConnectionStringBuilder> {
        public override string ProductName { get; } = "MySQL";

        public override string GetCreateVirtualTableStatement(string sourceTableName, string notebookTableName) {
            return $"CREATE VIRTUAL TABLE {notebookTableName.DoubleQuote()} USING mysql ('{_builder.ConnectionString.Replace("'", "''")}', '{sourceTableName.Replace("'", "''")}')";
        }

        protected override IDbConnection CreateConnection(MySqlConnectionStringBuilder builder) {
            return new MySqlConnection(builder.ConnectionString);
        }

        protected override void ReadTableNames(IDbConnection connection) {
            var tableNames = new List<string>();
            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = "SELECT table_name FROM information_schema.tables ORDER BY table_name";
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        tableNames.Add(reader.GetString(0));
                    }
                }
            }
            TableNames = tableNames;
        }

        protected override MySqlConnectionStringBuilder CreateBuilder(string connStr) {
            return new MySqlConnectionStringBuilder(connStr);
        }

        protected override string GetDisplayName() {
            return $"{_builder.Database} on {_builder.Server} (MySQL)";
        }

        protected override ImportConnectionForm.BasicOptions GetBasicOptions(MySqlConnectionStringBuilder builder) {
            return new ImportConnectionForm.BasicOptions {
                Server = builder.Server,
                Database = builder.Database,
                Username = builder.UserID,
                Password = builder.Password
            };
        }

        protected override void SetBasicOptions(MySqlConnectionStringBuilder builder, ImportConnectionForm.BasicOptions opt) {
            builder.Server = opt.Server;
            builder.Database = opt.Database;
            builder.UserID = opt.Username;
            builder.Password = opt.Password;
        }
    }

    public abstract class AdoImportSession<TConnectionStringBuilder> : IDatabaseImportSession
        where TConnectionStringBuilder : DbConnectionStringBuilder, new() {

        public abstract string ProductName { get; }
        public abstract string GetCreateVirtualTableStatement(string sourceTableName, string notebookTableName);
        protected abstract IDbConnection CreateConnection(TConnectionStringBuilder builder);
        protected abstract void ReadTableNames(IDbConnection connection);
        protected abstract TConnectionStringBuilder CreateBuilder(string connStr);
        protected abstract string GetDisplayName();
        protected abstract ImportConnectionForm.BasicOptions GetBasicOptions(TConnectionStringBuilder builder);
        protected abstract void SetBasicOptions(TConnectionStringBuilder builder, ImportConnectionForm.BasicOptions opt);

        protected TConnectionStringBuilder _builder = new TConnectionStringBuilder();

        public bool FromConnectForm(IWin32Window owner) {
            var successfulConnect = false;

            do {
                var f = new ImportConnectionForm(
                    $"Connect to {ProductName}", 
                    _builder,
                    b => GetBasicOptions((TConnectionStringBuilder)b),
                    (b, o) => SetBasicOptions((TConnectionStringBuilder)b, o));

                using (f) {
                    if (f.ShowDialog(owner) != DialogResult.OK) {
                        return false;
                    }
                }

                successfulConnect = DoConnect(owner);
            } while (!successfulConnect);

            return true;
        }

        private bool DoConnect(IWin32Window owner) {
            bool successfulConnect = false;
            Action action = () => {
                using (var connection = CreateConnection(_builder)) {
                    connection.Open();
                    ReadTableNames(connection);
                }
            };
            using (var f = new WaitForm("SQL Notebook", $"Accessing {ProductName}...", action)) {
                f.ShowDialog(owner);
                if (f.ResultException == null) {
                    successfulConnect = true;
                } else {
                    MessageForm.ShowError(owner,
                        "Connection Error",
                        $"The connection to {ProductName} failed.",
                        f.ResultException.Message);
                }
            }
            return successfulConnect;
        }

        public bool FromRecent(RecentDataSource recent, IWin32Window owner) {
            _builder = CreateBuilder(recent.ConnectionString);
            return DoConnect(owner);
        }

        public IReadOnlyList<string> TableNames { get; protected set; } = new string[0];

        public void AddToRecentlyUsed() {
            /*RecentDataSources.Add(new RecentDataSource {
                ConnectionString = _builder.ConnectionString,
                DisplayName = GetDisplayName(),
                ImportSessionType = this.GetType()
            });*/
        }
    }
}
