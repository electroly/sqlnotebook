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
using Excel;
using Microsoft.VisualBasic.FileIO;
using Microsoft.WindowsAPICodePack.Dialogs;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using Npgsql;
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
            typeof(CsvImportSession),
            typeof(ExcelImportSession),
            typeof(JsonImportSession)
        };

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

        public void AddToRecentlyUsed() {
            RecentDataSources.Add(new RecentDataSource {
                ConnectionString = _filePath,
                DisplayName = Path.GetFileName(_filePath),
                ImportSessionType = this.GetType()
            });
        }

        public static Regex _nonAlphaNumericUnderscoreRegex = new Regex("[^A-Za-z0-9_]");
        public void PerformImport(Notebook notebook, IReadOnlyList<string> sourceTableNames, IReadOnlyList<string> targetTableNames) {
            var sourceTableName = sourceTableNames.Single();
            var targetTableName = targetTableNames.Single();

            using (var parser = NewParser(_filePath)) {
                var columnNames = new List<string>();
                if (FileHasHeaderRow) {
                    var cells = parser.ReadFields();
                    foreach (var cell in cells) {
                        var prefix = _nonAlphaNumericUnderscoreRegex.Replace(cell, "_");
                        var name = Importer.CreateUniqueName(prefix, columnNames);
                        columnNames.Add(name);
                    }
                }

                var firstRow = true;
                string insertSql = null;
                var row = new object[columnNames.Count];
                while (!parser.EndOfData) {
                    var cells = parser.ReadFields();

                    if (firstRow) {
                        if (!FileHasHeaderRow) {
                            for (int i = 0; i < cells.Length; i++) {
                                columnNames.Add(Importer.CreateUniqueName("col", columnNames));
                            }
                        }

                        var columnList = string.Join(" , ", columnNames.Select(x => x.DoubleQuote()));
                        notebook.Execute($"CREATE TABLE {targetTableName.DoubleQuote()} ( {columnList} )");
                        insertSql = $"INSERT INTO {targetTableName.DoubleQuote()} VALUES " +
                            $"( {string.Join(",", columnNames.Select((x, i) => $"?{i + 1}"))} )";
                        firstRow = false;
                    }

                    for (int i = 0; i < cells.Length; i++) {
                        row[i] = cells[i];
                    }
                    for (int i = cells.Length; i < row.Length; i++) {
                        row[i] = null;
                    }
                    notebook.Execute(insertSql, row);
                }
            }
        }

        private static string CreateUniqueName(string prefix, IReadOnlyList<string> existingNames) {
            var name = prefix; // first try it without a number on the end
            int suffix = 2;
            while (existingNames.Contains(name)) {
                name = prefix + (suffix++);
            }
            return name;
        }

        private static TextFieldParser NewParser(string filePath) {
            var x = new TextFieldParser(filePath);
            x.HasFieldsEnclosedInQuotes = true;
            x.SetDelimiters(",");
            return x;
        }

    }

    public sealed class ExcelImportSession : IFileImportSession {
        private string _filePath;

        public IReadOnlyList<string> SupportedFileExtensions {
            get {
                return new[] { ".xls", ".xlsx" };
            }
        }

        private IExcelDataReader CreateReader(string filePath, Stream stream) {
            IExcelDataReader x;
            if (Path.GetExtension(filePath).ToLower() == ".xls") {
                x = ExcelReaderFactory.CreateBinaryReader(stream);
            } else {
                x = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            x.IsFirstRowAsColumnNames = FileHasHeaderRow;
            return x;
        }

        public void FromFilePath(string filePath) {
            _filePath = filePath;

            using (var stream = File.OpenRead(filePath))
            using (var reader = CreateReader(filePath, stream)) {
                var tableNames = new List<string>();
                for (int i = 0; i < reader.ResultsCount; i++) {
                    tableNames.Add(Importer.CreateUniqueName(reader.Name, tableNames));
                    reader.NextResult();
                }
                TableNames = tableNames;
            }
        }

        public bool FromRecent(RecentDataSource recent, IWin32Window owner) {
            FromFilePath(recent.ConnectionString);
            return true;
        }

        public IReadOnlyList<string> TableNames { get; private set; } = new string[0];

        public bool FileHasHeaderRow { get; set; }

        public void AddToRecentlyUsed() {
            RecentDataSources.Add(new RecentDataSource {
                ConnectionString = _filePath,
                DisplayName = Path.GetFileName(_filePath),
                ImportSessionType = this.GetType()
            });
        }

        public void PerformImport(Notebook notebook, IReadOnlyList<string> sourceTableNames, IReadOnlyList<string> targetTableNames) {
            using (var stream = File.OpenRead(_filePath))
            using (var reader = CreateReader(_filePath, stream))
            using (var dataSet = reader.AsDataSet()) {
                for (int i = 0; i < sourceTableNames.Count; i++) {
                    var sourceName = sourceTableNames[i];
                    var targetName = targetTableNames[i];
                    var dt = dataSet.Tables[IndexOf(sourceTableNames, sourceName)];
                    var columnNames = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                    var quotedCols = columnNames.Select(x => x.DoubleQuote());
                    notebook.Execute($"CREATE TABLE {targetName.DoubleQuote()} ({string.Join(",", quotedCols)})");
                    var insertSql = $"INSERT INTO {targetName.DoubleQuote()} VALUES " +
                        $"( {string.Join(",", columnNames.Select((x, j) => $"?{j + 1}"))} )";
                    foreach (DataRow row in dt.Rows) {
                        notebook.Execute(insertSql, row.ItemArray);
                    }
                }
            }
        }

        private static int IndexOf(IReadOnlyList<string> list, string item) {
            for (int i = 0; i < list.Count; i++) {
                if (list[i] == item) {
                    return i;
                }
            }
            return -1;
        }
    }

    public sealed class JsonImportSession : IFileImportSession {
        private string _filePath;
        private JToken _root;
        private IReadOnlyDictionary<string, JArray> _arrays;

        public IReadOnlyList<string> SupportedFileExtensions {
            get {
                return new[] { ".json" };
            }
        }

        public void FromFilePath(string filePath) {
            _filePath = filePath;

            _root = JToken.Parse(File.ReadAllText(filePath), new JsonLoadSettings {
                CommentHandling = CommentHandling.Ignore,
                LineInfoHandling = LineInfoHandling.Ignore
            });
            var arrays = new Dictionary<string, JArray>(); // path -> array

            if (_root.Type == JTokenType.Array) {
                arrays.Add("root", (JArray)_root);
            } else if (_root.Type == JTokenType.Object) {
                var stack = new Stack<Tuple<string, JObject>>(); // path, object
                stack.Push(Tuple.Create("root", (JObject)_root));
                while (stack.Any()) {
                    var pair = stack.Pop();
                    var path = pair.Item1;
                    var token = pair.Item2;
                    foreach (var prop in token.Children<JProperty>()) {
                        var propPath = $"{path}_{prop.Name}";
                        if (prop.Value.Type == JTokenType.Array) {
                            var arr = (JArray)prop.Value;

                            if (arr.Count > 0) {
                                arrays.Add(propPath, arr);
                            }

                            int n = Math.Min(100, arr.Count);
                            int digits = n.ToString().Length;
                            for (int i = 0; i < n; i++) {
                                if (arr[i].Type == JTokenType.Object) {
                                    var itemPath = $"{propPath}_{i.ToString().PadLeft(digits, '0')}";
                                    stack.Push(Tuple.Create(itemPath, (JObject)arr[i]));
                                }
                            }
                        } else if (prop.Value.Type == JTokenType.Object) {
                            stack.Push(Tuple.Create(propPath, (JObject)prop.Value));
                        }
                    }
                }
            } else {
                throw new Exception("No arrays were found in the JSON data.");
            }

            // remove "root_" prefix from all table names
            _arrays = arrays.ToDictionary(x => RemovePrefix(x.Key, "root_"), x => x.Value);
            TableNames = _arrays.Select(x => x.Key).OrderBy(x => x).ToList();
        }

        private static string RemovePrefix(string str, string prefix) {
            if (str.StartsWith(prefix)) {
                return str.Substring(prefix.Length);
            } else {
                return str;
            }
        }

        public bool FromRecent(RecentDataSource recent, IWin32Window owner) {
            FromFilePath(recent.ConnectionString);
            return true;
        }

        public IReadOnlyList<string> TableNames { get; private set; } = new string[0];

        public bool FileHasHeaderRow { get; set; } = false;

        public void AddToRecentlyUsed() {
            RecentDataSources.Add(new RecentDataSource {
                ConnectionString = _filePath,
                DisplayName = Path.GetFileName(_filePath),
                ImportSessionType = this.GetType()
            });
        }

        public void PerformImport(Notebook notebook, IReadOnlyList<string> sourceTableNames, IReadOnlyList<string> targetTableNames) {
            for (int i = 0; i < sourceTableNames.Count; i++) {
                var sourceName = sourceTableNames[i];
                var targetName = targetTableNames[i];
                var arr = _arrays[sourceName];
                var rows = arr.Select(ParseRow).ToList();

                var columnNames =
                    (from row in rows
                     from col in row.Keys
                     select col)
                    .Distinct()
                    .ToList();

                var quotedCols = columnNames.Select(x => x.DoubleQuote());
                notebook.Execute($"CREATE TABLE {targetName.DoubleQuote()} ({string.Join(",", quotedCols)})");
                var insertSql = $"INSERT INTO {targetName.DoubleQuote()} VALUES " +
                    $"( {string.Join(",", columnNames.Select((x, j) => $"?{j + 1}"))} )";
                foreach (var row in rows) {
                    var itemArray = new object[columnNames.Count];
                    for (int j = 0; j < itemArray.Length; j++) {
                        object value;
                        if (row.TryGetValue(columnNames[j], out value)) {
                            itemArray[j] = value;
                        }
                    }
                    notebook.Execute(insertSql, itemArray);
                }
            }
        }

        private static IReadOnlyDictionary<string, object> ParseRow(JToken root) {
            var row = new Dictionary<string, object>();
            var stack = new Stack<Tuple<string, JToken>>();
            stack.Push(Tuple.Create("", root));

            while (stack.Any()) {
                var pair = stack.Pop();
                var prefix = pair.Item1;
                var token = pair.Item2;
                if (token.Type == JTokenType.Object) {
                    var obj = (JObject)token;
                    foreach (var prop in obj.Properties()) {
                        stack.Push(Tuple.Create($"{prefix}{prop.Name}_", prop.Value));
                    }
                } else {
                    var col = $"{prefix.TrimEnd('_')}";
                    if (!col.Any()) {
                        col = "value";
                    }
                    row.Add(col, token.ToString());
                }
            }

            return row;
        }
    }

    public sealed class PgImportSession : AdoImportSession<Npgsql.NpgsqlConnectionStringBuilder> {
        public override string ProductName { get; } = "PostgreSQL";

        public override string GetCreateVirtualTableStatement(string sourceTableName, string notebookTableName) {
            return $"CREATE VIRTUAL TABLE {notebookTableName.DoubleQuote()} USING pgsql ('{_builder.ConnectionString.Replace("'", "''")}', '{sourceTableName.Replace("'", "''")}')";
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
            return $"CREATE VIRTUAL TABLE {notebookTableName.DoubleQuote()} USING mssql ('{_builder.ConnectionString.Replace("'", "''")}', '{sourceTableName.Replace("'", "''")}')";
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

        public void AddToRecentlyUsed() {
            RecentDataSources.Add(new RecentDataSource {
                ConnectionString = _builder.ConnectionString,
                DisplayName = GetDisplayName(),
                ImportSessionType = this.GetType()
            });
        }
    }
}
