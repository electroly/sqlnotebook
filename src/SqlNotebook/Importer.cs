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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlNotebook {
    public sealed class RecentDataSource {
        public Type ImportSessionType;
        public string DisplayName;
        public string ConnectionString;
    }

    public sealed class Importer {
        private IWin32Window _owner;

        private readonly IReadOnlyList<Type> _fileSessionTypes = new[] {
            typeof(CsvImportSession)
        };

        public List<RecentDataSource> RecentlyUsed = new List<RecentDataSource>();

        public Importer(IWin32Window owner) {
            _owner = owner;
        }

        public void DoFileImport() {
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
            DoCommonImport(session);
        }

        public void DoDatabaseImport<T>() where T : IDatabaseImportSession, new() {
            var session = new T();
            if (session.ShowConnectForm(_owner)) {
               DoCommonImport(session);
            }
        }

        public void DoRecentImport(RecentDataSource recent) {
            var session = (IImportSession)Activator.CreateInstance(recent.ImportSessionType);
            session.FromConnectionString(recent.ConnectionString);
            DoCommonImport(session);
        }

        private void DoCommonImport(IImportSession session) {
            using (var frm = new ImportPreviewForm(session)) {
                if (frm.ShowDialog(_owner) != DialogResult.OK) {
                    return;
                }

            }
        }
    }

    public interface IImportSession {
        void FromConnectionString(string connectionString);
        IReadOnlyList<string> TableNames { get; }
        bool HasTableOptions { get; }
        void ShowTableOptionsForm(IWin32Window owner, string tableName);
        string GetCreateVirtualTableStatement(string sourceTableName, string notebookTableName);
        void AddToRecentlyUsed(Importer importer);
    }

    public interface IFileImportSession : IImportSession {
        IReadOnlyList<string> SupportedFileExtensions { get; }
        void FromFilePath(string filePath);
    }

    public interface IDatabaseImportSession : IImportSession {
        bool ShowConnectForm(IWin32Window owner);
    }

    public sealed class CsvImportSession : IFileImportSession {
        private string _filePath;
        private string _tableName;
        private CsvImportTableOptions _tableOptions = new CsvImportTableOptions { HasHeaderRow = true };

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

        public IReadOnlyList<string> TableNames {
            get {
                return new[] { _tableName };
            }
        }

        public bool HasTableOptions {
            get {
                return true;
            }
        }

        public void ShowTableOptionsForm(IWin32Window owner, string tableName) {
            using (var f = new ImportCsvOptionsForm()) {
                f.ShowDialog(owner);
            }
        }

        public string GetCreateVirtualTableStatement(string sourceTableName, string notebookTableName) {
            return $"CREATE VIRTUAL TABLE \"{notebookTableName}\" USING csv ('{_filePath}', {(_tableOptions.HasHeaderRow ? "HEADER" : "NO_HEADER")})";
        }

        public void FromConnectionString(string connectionString) {
            // the "connection string" is just the file path
            FromFilePath(connectionString);
        }

        public void AddToRecentlyUsed(Importer importer) {
            importer.RecentlyUsed.Add(new RecentDataSource {
                ConnectionString = _filePath,
                DisplayName = Path.GetFileName(_filePath),
                ImportSessionType = typeof(CsvImportSession)
            });
        }
    }

    public sealed class CsvImportTableOptions {
        public bool HasHeaderRow;
    }
}
