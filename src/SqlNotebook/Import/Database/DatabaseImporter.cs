using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using SqlNotebookScript.Core;
using SqlNotebookScript.Utils;

namespace SqlNotebook.Import.Database;

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

        string text = $"Importing {selectedTables.Count} table{(selectedTables.Count == 1 ? "" : "s")}...";
        WaitForm.GoWithCancel(_owner, "Import", text, out _, cancel => {
            _notebook.Invoke(() => {
                _notebook.Execute("BEGIN");

                try {
                    cancel.Register(() => _manager.Notebook.BeginUserCancel());
                    DoDatabaseImport(selectedTables, copyData, session, cancel);
                    _notebook.Execute("COMMIT");
                } catch {
                    _manager.Notebook.EndUserCancel();
                    try { _notebook.Execute("ROLLBACK"); } catch { }
                    throw;
                } finally {
                    _manager.Notebook.EndUserCancel();
                }
            });
        });
        _manager.Rescan();
        _manager.SetDirty();
    }

    private void DoDatabaseImport(IReadOnlyList<DatabaseImportTablesForm.SelectedTable> selectedTables, bool copyData,
        IImportSession dbSession, CancellationToken cancel
        ) {
        foreach (var selectedTable in selectedTables) {
            cancel.ThrowIfCancellationRequested();
            if (copyData) {
                dbSession.ImportTableByCopyingData(
                    selectedTable.SourceName.DoubleQuote(),
                    selectedTable.TargetName,
                    _notebook,
                    cancel);
            } else {
                // just create the virtual table
                _notebook.Execute(dbSession.GetCreateVirtualTableStatement(selectedTable.SourceName, selectedTable.TargetName));
            }
        }
    }
}
