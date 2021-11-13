using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using SqlNotebook.Properties;
using SqlNotebookScript.Utils;

namespace SqlNotebook;

public partial class TableDocumentControl : UserControl, IDocumentControl {
    private readonly NotebookManager _manager;
    private readonly string _tableName;
    private readonly DataGridView _grid;

    public TableDocumentControl(NotebookManager manager, string tableName) {
        InitializeComponent();
        _manager = manager;
        _tableName = tableName;
        _toolStrip.SetMenuAppearance();

        _grid = DataGridViewUtil.NewDataGridView();
        _grid.Dock = DockStyle.Fill;
        _tablePanel.Controls.Add(_grid);

        Load += delegate {
            var simpleDataTable = WaitForm.Go(TopLevelControl, "Table", "Reading table data...", out var success, () =>
                _manager.Notebook.SpecialReadOnlyQuery(
                    $"SELECT * FROM {_tableName.DoubleQuote()} LIMIT 1000",
                    new Dictionary<string, object>()));
            if (!success) {
                return;
            }

            var dt = new DataTable();
            foreach (var col in simpleDataTable.Columns) {
                dt.Columns.Add(col);
            }
            foreach (var row in simpleDataTable.Rows) {
                var dtRow = dt.NewRow();
                dtRow.ItemArray = row;
                dt.Rows.Add(dtRow);
            }
            _grid.DataSource = dt;
            _grid.AutoSizeColumns(this.Scaled(300));

            Ui ui = new(this, false);
            ui.Init(_scriptBtn, Resources.script_go, Resources.script_go32);
        };
    }

    // IDocumentControl
    string IDocumentControl.ItemName { get; set; }
    public void Save() { }

    private void ScriptBtn_Click(object sender, EventArgs e) {
        var name = _manager.NewScript();
        _manager.SetItemData(name, $"SELECT * FROM {_tableName.DoubleQuote()} LIMIT 1000");
        _manager.OpenItem(new NotebookItem(NotebookItemType.Script, name));
    }
}
