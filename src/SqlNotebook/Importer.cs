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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using MySql.Data.MySqlClient;
using SqlNotebookCore;

namespace SqlNotebook {
    public sealed class RecentDataSource {
        public Type ImportSessionType;
        public string DisplayName;
        public string ConnectionString;
    }

    public sealed class Importer {
        private readonly NotebookManager _manager;
        private readonly Notebook _notebook;
        private readonly IWin32Window _owner;

        private readonly IReadOnlyList<Type> _fileSessionTypes = new[] {
            typeof(CsvImportSession)
        };

        public List<RecentDataSource> RecentlyUsed = new List<RecentDataSource>();

        public Importer(NotebookManager manager, IWin32Window owner) {
            _manager = manager;
            _notebook = manager.Notebook;
            _owner = owner;
        }

        public async Task DoFileImport() {
            var fileSessions =
                (from type in _fileSessionTypes
                 let typeSession = (IFileImportSession)Activator.CreateInstance(type)
                 from extension in typeSession.SupportedFileExtensions
                 select new { Extension = extension, Session = typeSession })
                .ToDictionary(x => x.Extension, x => x.Session);
            var openFrm = new OpenFileDialog {
                AutoUpgradeEnabled = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DereferenceLinks = true,
                Filter = $"Supported data files|{string.Join(";", from x in fileSessions.Keys select "*" + x)}",
                SupportMultiDottedExtensions = true,
                Title = "Import from File"
            };
            string filePath;
            using (openFrm) {
                if (openFrm.ShowDialog(_owner) == DialogResult.OK) {
                    filePath = openFrm.FileName;
                } else {
                    return;
                }
            }
            var fileExt = Path.GetExtension(filePath).ToLower();
            IFileImportSession session;
            if (!fileSessions.TryGetValue(fileExt, out session)) {
                throw new Exception("Unrecognized file extension: " + fileExt);
            }
            session.FromFilePath(filePath);
            await DoCommonImport(session);
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
            if (fileSession != null) {
                fileSession.FileHasHeaderRow = csvHasHeaderRow;
            }

            Action import = () => {
                _notebook.Invoke(() => {
                    _notebook.Execute("BEGIN");

                    try {
                        foreach (var selectedTable in selectedTables) {
                            if (copyData) {
                                if (_notebook.QueryValue("SELECT name FROM sqlite_master WHERE name = '__temp_import'") != null) {
                                    _notebook.Execute("DROP TABLE __temp_import");
                                }
                                
                                _notebook.Execute(session.GetCreateVirtualTableStatement(selectedTable.SourceName, "__temp_import"));

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
                                    lines.Add($"\"{name.Replace("\"", "\"\"")}\" {type} {(notNull ? "NOT NULL" : "")}");
                                }
                                var pkList = string.Join(" , ", pks.Where(x => x != null).Select(x => $"\"{x.Replace("\"", "\"\"")}\""));
                                if (pkList.Any()) {
                                    lines.Add($"PRIMARY KEY ( {pkList} )");
                                }
                                _notebook.Execute($"CREATE TABLE \"{selectedTable.TargetName.Replace("\"", "\"\"")}\" ( {string.Join(" , ", lines)} )");

                                // copy all data from the virtual table to the physical table
                                _notebook.Execute($"INSERT INTO \"{selectedTable.TargetName.Replace("\"", "\"\"")}\" SELECT * FROM __temp_import");

                                // we're done with this import
                                _notebook.Execute($"DROP TABLE __temp_import");
                            } else {
                                // just create the virtual table
                                _notebook.Execute(session.GetCreateVirtualTableStatement(selectedTable.SourceName, selectedTable.TargetName));
                            }
                        }

                        _notebook.Execute("COMMIT");
                    } catch {
                        try { _notebook.Execute("ROLLBACK"); } catch { }
                        throw;
                    }
                });
            };

            _manager.PushStatus("Importing the selected tables. Press ESC to cancel.");
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
                var td = new TaskDialog {
                    Cancelable = true,
                    Caption = "Import Error",
                    Icon = TaskDialogStandardIcon.Error,
                    InstructionText = "The import failed.",
                    StandardButtons = TaskDialogStandardButtons.Ok,
                    OwnerWindowHandle = _owner.Handle,
                    StartupLocation = TaskDialogStartupLocation.CenterOwner,
                    Text = exception.Message
                };
                td.Show();
            }
        }
    }

    public interface IImportSession {
        bool FromRecent(RecentDataSource recent, IWin32Window owner);
        IReadOnlyList<string> TableNames { get; }
        string GetCreateVirtualTableStatement(string sourceTableName, string notebookTableName);
        void AddToRecentlyUsed(Importer importer);
    }

    public interface IFileImportSession : IImportSession {
        IReadOnlyList<string> SupportedFileExtensions { get; }
        void FromFilePath(string filePath);
        bool FileHasHeaderRow { get; set; }
    }

    public interface IDatabaseImportSession : IImportSession {
        bool FromConnectForm(IWin32Window owner);
    }

    public sealed class CsvImportSession : IFileImportSession {
        private string _filePath;
        private string _tableName;

        public IReadOnlyList<string> SupportedFileExtensions {
            get {
                return new[] { ".csv", ".txt" };
            }
        }

        public void FromFilePath(string filePath) {
            _filePath = filePath;
            var regex = new Regex("[^A-Za-z0-9_]");
            _tableName = regex.Replace(Path.GetFileNameWithoutExtension(_filePath), "_").ToLower();
        }

        public bool FromRecent(RecentDataSource recent, IWin32Window owner) {
            FromFilePath(recent.ConnectionString);
            return true;
        }

        public IReadOnlyList<string> TableNames {
            get {
                return new[] { _tableName };
            }
        }

        public bool FileHasHeaderRow { get; set; }

        public string GetCreateVirtualTableStatement(string sourceTableName, string notebookTableName) {
            return $"CREATE VIRTUAL TABLE \"{notebookTableName.Replace("\"", "\"\"")}\" USING csv ('{_filePath.Replace("'", "''")}', {(FileHasHeaderRow ? "HEADER" : "NO_HEADER")})";
        }

        public void AddToRecentlyUsed(Importer importer) {
            importer.RecentlyUsed.Add(new RecentDataSource {
                ConnectionString = _filePath,
                DisplayName = Path.GetFileName(_filePath),
                ImportSessionType = typeof(CsvImportSession)
            });
        }
    }

    public sealed class PgImportSession : AdoImportSession<Npgsql.NpgsqlConnectionStringBuilder> {
        public override string ProductName { get; } = "PostgreSQL";

        public override string GetCreateVirtualTableStatement(string sourceTableName, string notebookTableName) {
            return $"CREATE VIRTUAL TABLE \"{notebookTableName.Replace("\"", "\"\"")}\" USING pgsql ('{_builder.ConnectionString.Replace("'", "''")}', '{sourceTableName.Replace("'", "''")}')";
        }

        protected override IDbConnection CreateConnection(Npgsql.NpgsqlConnectionStringBuilder builder) {
            return new Npgsql.NpgsqlConnection(builder);
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
    }

    public sealed class MsImportSession : AdoImportSession<SqlConnectionStringBuilder> {
        public override string ProductName { get; } = "Microsoft SQL Server";

        public override string GetCreateVirtualTableStatement(string sourceTableName, string notebookTableName) {
            return $"CREATE VIRTUAL TABLE \"{notebookTableName.Replace("\"", "\"\"")}\" USING mssql ('{_builder.ConnectionString.Replace("'", "''")}', '{sourceTableName.Replace("'", "''")}')";
        }

        protected override IDbConnection CreateConnection(SqlConnectionStringBuilder builder) {
            return new SqlConnection(builder.ConnectionString);
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

        protected override SqlConnectionStringBuilder CreateBuilder(string connStr) {
            return new SqlConnectionStringBuilder(connStr);
        }

        protected override string GetDisplayName() {
            return $"{_builder.InitialCatalog} on {_builder.DataSource} (Microsoft SQL Server)";
        }
    }

    public sealed class MyImportSession : AdoImportSession<MySqlConnectionStringBuilder> {
        public override string ProductName { get; } = "MySQL";

        public override string GetCreateVirtualTableStatement(string sourceTableName, string notebookTableName) {
            return $"CREATE VIRTUAL TABLE \"{notebookTableName.Replace("\"", "\"\"")}\" USING mysql ('{_builder.ConnectionString.Replace("'", "''")}', '{sourceTableName.Replace("'", "''")}')";
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
    }

    public abstract class AdoImportSession<TConnectionStringBuilder> : IDatabaseImportSession
        where TConnectionStringBuilder : DbConnectionStringBuilder, new() {

        public abstract string ProductName { get; }
        public abstract string GetCreateVirtualTableStatement(string sourceTableName, string notebookTableName);
        protected abstract IDbConnection CreateConnection(TConnectionStringBuilder builder);
        protected abstract void ReadTableNames(IDbConnection connection);
        protected abstract TConnectionStringBuilder CreateBuilder(string connStr);
        protected abstract string GetDisplayName();

        protected TConnectionStringBuilder _builder = new TConnectionStringBuilder();

        public bool FromConnectForm(IWin32Window owner) {
            var successfulConnect = false;

            do {
                using (var f = new ImportConnectionForm($"Connect to {ProductName}", _builder)) {
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
                    var td = new TaskDialog {
                        Cancelable = true,
                        Caption = "Connection Error",
                        Icon = TaskDialogStandardIcon.Error,
                        InstructionText = $"The connection to {ProductName} failed.",
                        StandardButtons = TaskDialogStandardButtons.Ok,
                        OwnerWindowHandle = owner.Handle,
                        StartupLocation = TaskDialogStartupLocation.CenterOwner,
                        Text = f.ResultException.Message
                    };
                    td.Show();
                }
            }
            return successfulConnect;
        }

        public bool FromRecent(RecentDataSource recent, IWin32Window owner) {
            _builder = CreateBuilder(recent.ConnectionString);
            return DoConnect(owner);
        }

        public IReadOnlyList<string> TableNames { get; protected set; } = new string[0];

        public void AddToRecentlyUsed(Importer importer) {
            importer.RecentlyUsed.Add(new RecentDataSource {
                ConnectionString = _builder.ConnectionString,
                DisplayName = GetDisplayName(),
                ImportSessionType = this.GetType()
            });
        }
    }
}
