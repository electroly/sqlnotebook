﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using SqlNotebook.Properties;
using SqlNotebookScript.Core;
using SqlNotebookScript.Utils;

namespace SqlNotebook.Import.Database;

public partial class DatabaseImportTablesForm : ZForm {
    private readonly List<SourceTable> _tables;
    private readonly NotebookManager _manager;
    private readonly IImportSession _session;

    public DatabaseImportTablesForm(NotebookManager manager, IImportSession session) {
        InitializeComponent();
        _manager = manager;
        _session = session;
        _tables = session.TableNames.Select(x => SourceTable.FromTable(x.Schema, x.Table)).ToList();
        foreach (var table in _tables) {
            int index = _listBox.Items.Add(table.DisplayText);

            // if there's only one table, then check it.  if there are multiple, then uncheck by default.
            _listBox.SetItemChecked(index, session.TableNames.Count == 1);
        }

        EnableDisableButtons();

        Ui ui = new(this, 85, 35);
        ui.InitHeader(_importTablesLabel);
        ui.Init(_toolStrip);
        var addTableIcon16 = Ui.SuperimposePlusSymbol(Resources.table);
        var addTableIcon32 = Ui.SuperimposePlusSymbol(Resources.table32);
        ui.Init(_addQueryButton, addTableIcon16, addTableIcon32);
        ui.MarginRight(_addQueryButton);
        ui.Init(_editTableButton, Resources.table_edit, Resources.table_edit32);
        ui.MarginRight(_editTableButton);
        ui.Init(_selectAllButton, Resources.check_box, Resources.check_box32);
        ui.Init(_selectNoneButton, Resources.check_box_uncheck, Resources.check_box_uncheck32);
        ui.Init(_listPanel);
        ui.PadLeft(_listPanel, 1);
        ui.Init(_listBox);
        ui.InitHeader(_methodLabel);
        ui.MarginTop(_methodLabel);
        ui.Init(_methodFlow);
        ui.Pad(_methodFlow);
        ui.Init(_methodCopyRad);
        ui.Init(_methodLinkRad);
        ui.Init(_buttonFlow2);
        ui.MarginTop(_buttonFlow2);
        ui.Init(_viewSqlButton);
        ui.Init(_buttonFlow);
        ui.MarginTop(_buttonFlow);
        ui.Init(_okBtn);
        ui.Init(_cancelBtn);
    }

    private void SelectNoneBtn_Click(object sender, EventArgs e) {
        for (int i = 0; i < _listBox.Items.Count; i++) {
            _listBox.SetItemChecked(i, false);
        }
    }
        
    private void SelectAllBtn_Click(object sender, EventArgs e) {
        for (int i = 0; i < _listBox.Items.Count; i++) {
            _listBox.SetItemChecked(i, true);
        }
    }

    private void ListBox_SelectedIndexChanged(object sender, EventArgs e) {
        EnableDisableButtons();
    }

    private void EnableDisableButtons() {
        bool hasSelection = _listBox.SelectedIndex >= 0;
        _editTableButton.Enabled = hasSelection;
    }

    private void EditTableBtn_Click(object sender, EventArgs e) {
        int i = _listBox.SelectedIndex;
        var sourceTable = _tables[i];
        if (sourceTable.SourceIsTable) {
            using DatabaseImportRenameTableForm f = new(sourceTable.SourceTableName, sourceTable.TargetTableName);
            if (f.ShowDialog(this) == DialogResult.OK) {
                sourceTable.TargetTableName = f.NewName;
                _listBox.Items[i] = sourceTable.DisplayText;
            }
        } else if (sourceTable.SourceIsSql) {
            using DatabaseImportCustomQueryForm f = new(_session, sourceTable.TargetTableName, sourceTable.SourceSql);
            if (f.ShowDialog(this) == DialogResult.OK) {
                sourceTable.SourceSql = f.Sql;
                sourceTable.TargetTableName = f.TargetName;
                _listBox.Items[i] = sourceTable.DisplayText;
            }
        } else {
            Debug.Fail("No source table type.");
        }
        Focus();
    }

    private void OkBtn_Click(object sender, EventArgs e) {
        string sql;
        try {
            sql = _session.GenerateSql(GetSelectedTables(), _methodLinkRad.Checked);
        } catch (Exception ex) {
            Ui.ShowError(this, "Error", ex);
            return;
        }
        
        var text = $"Importing from database...";
        WaitForm.GoWithCancel(this, "Import", text, out var success, cancel => {
            SqlUtil.WithCancellableTransaction(_manager.Notebook, () => {
                _manager.ExecuteScript(sql);
            }, cancel);
        });
        _manager.Rescan();
        _manager.SetDirty();
        if (!success) {
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private List<SourceTable> GetSelectedTables() {
        List<SourceTable> tables = new();
        for (int i = 0; i < _tables.Count; i++) {
            var sourceTable = _tables[i];
            var isSelected = _listBox.GetItemChecked(i);
            if (isSelected) {
                tables.Add(sourceTable);
            }
        }
        return tables;
    }

    private void ViewSqlButton_Click(object sender, EventArgs e) {
        try {
            var sql = _session.GenerateSql(GetSelectedTables(), _methodLinkRad.Checked);
            using DatabaseImportScriptForm f = new(sql);
            f.ShowDialog(this);
        } catch (Exception ex) {
            Ui.ShowError(this, "Error", ex);
        }
        Focus();
    }

    private void AddQueryButton_Click(object sender, EventArgs e) {
        // Find a name like "query1", "query2", etc. that doesn't exist in the notebook and isn't to be imported.
        // First make a list of all the existing names that start with "query" so we can avoid them.
        var notebook = _manager.Notebook;
        List<string> existingNames = new();
        using (var sdt = notebook.Query("SELECT name FROM sqlite_master")) {
            foreach (var row in sdt.Rows) {
                var name = (string)row[0];
                if (name.StartsWith("query", StringComparison.OrdinalIgnoreCase)) {
                    existingNames.Add((string)row[0]);
                }
            }
        }
        foreach (var t in _tables) {
            existingNames.Add(t.TargetTableName);
        }

        // Count up until we find a unique name.
        string targetName;
        for (var i = 1; ; i++) {
            var proposedTargetName = $"query{i}";
            if (!existingNames.Any(x => x.Equals(proposedTargetName, StringComparison.OrdinalIgnoreCase))) {
                targetName = proposedTargetName;
                break;
            }
        }

        // Ask the user for a query.
        DatabaseImportCustomQueryForm f = new(_session, targetName, "SELECT * FROM ");
        var result = f.ShowDialog(this);
        Focus();
        if (result != DialogResult.OK) {
            return;
        }

        // Add a SourceTable to the list for this query. Check it by default.
        var sourceTable = SourceTable.FromSql(f.Sql, f.TargetName);
        var index = _tables.Count;
        _tables.Add(sourceTable);
        _listBox.Items.Add(sourceTable.DisplayText);
        _listBox.SetItemChecked(index, true);
        _listBox.TopIndex = index;
    }
}
