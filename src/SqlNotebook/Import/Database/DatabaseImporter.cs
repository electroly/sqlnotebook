using SqlNotebookCore;
using SqlNotebookScript.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SqlNotebook.Import.Database {
    public sealed class DatabaseImporter {
        private readonly NotebookManager _manager;
        private readonly Notebook _notebook;
        private readonly IWin32Window _owner;

        public DatabaseImporter(NotebookManager manager, IWin32Window owner) {
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

        public void DoDatabaseImport<T>() where T : IImportSession, new() {
            var session = new T();
            if (session.FromConnectForm(_owner)) {
               DoCommonImport(session);
            }
        }

        private void DoCommonImport(IImportSession session) {
            IReadOnlyList<DatabaseImportTablesForm.SelectedTable> selectedTables = null;
            var copyData = false;

            using (var frm = new DatabaseImportTablesForm(session)) {
                if (frm.ShowDialog(_owner) != DialogResult.OK) {
                    return;
                }
                selectedTables = frm.SelectedTables;
                copyData = frm.CopyData;
            }

            WaitForm.Go(_owner, "Import", "Importing tables...", out _, () => {
                _notebook.Invoke(() => {
                    _notebook.Execute("BEGIN");

                    try {
                        DoDatabaseImport(selectedTables, copyData, session);
                        _notebook.Execute("COMMIT");
                    } catch {
                        try { _notebook.Execute("ROLLBACK"); } catch { }
                        throw;
                    }
                });
            });
            _manager.Rescan();
            _manager.SetDirty();
        }

        private void DoDatabaseImport(IReadOnlyList<DatabaseImportTablesForm.SelectedTable> selectedTables, bool copyData, IImportSession dbSession) {
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
}
