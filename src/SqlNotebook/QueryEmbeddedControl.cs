using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SqlNotebook.Properties;
using SqlNotebookScript.Interpreter;
using SqlNotebookScript.Utils;

namespace SqlNotebook;

public partial class QueryEmbeddedControl : UserControl {
    public const int MAX_GRID_ROWS = 10_000;
    private readonly NotebookManager _manager;
    private readonly Ui _ui;
    private ScriptOutput _output;

    // This includes tables for printed messages and scalar results. The indices match the tabs.
    private readonly List<(long Count, DataTable DataTable)> _dataTables = new();
    private TabControl _tabs;

    public SqlTextControl TextControl { get; private set; }
    public string SqlText {
        get => TextControl.SqlText;
        set => TextControl.SqlText = value;
    }
    public ScriptOutput Output {
        get => _output;
        set {
            _output = value;
            SetOutput();
        }
    }
    public bool ShowSql {
        get => _optionsShowSqlMenu.Checked;
        set => _optionsShowSqlMenu.Checked = value;
    }
    public bool ShowResults {
        get => _optionsShowResultsMenu.Checked;
        set => _optionsShowResultsMenu.Checked = value;
    }
    public int MaxRows {
        get {
            foreach (ToolStripMenuItem item in _limitRowsOnPageMenu.DropDownItems) {
                if (item.Checked) {
                    return int.Parse(item.Text);
                }
            }
            return 10;
        }
        set {
            foreach (ToolStripMenuItem item in _limitRowsOnPageMenu.DropDownItems) {
                item.Checked = int.Parse(item.Text) == value;
            }
        }
    }
    public event EventHandler SqlTextChanged;

    public QueryEmbeddedControl(NotebookManager manager, bool isPageContext) {
        InitializeComponent();
        _manager = manager;
        _toolStrip.SetMenuAppearance();

        BackColor = Color.FromArgb(250, 250, 250);

        TextControl = new SqlTextControl(false) {
            Dock = DockStyle.Fill,
            Padding = Padding.Empty,
        };
        TextControl.SqlTextChanged += (sender, e) => SqlTextChanged?.Invoke(sender, e);
        TextControl.TextBox.KeyDown += TextControl_KeyDown;
        _sqlPanel.Controls.Add(TextControl);

        Ui ui = new(this, false);
        ui.Init(_split, 0.3);
        ui.Init(_executeButton, Resources.ControlPlayBlue, Resources.control_play_blue32);
        ui.MarginRight(_executeButton);
        ui.Init(_sendMenu);
        ui.MarginRight(_sendMenu);
        ui.Init(_optionsMenu);
        _optionsMenu.Visible = isPageContext;
        ui.Init(_sendTableMenu, Resources.table, Resources.table32);
        ui.Init(_hideResultsButton, Resources.hide_detail, Resources.hide_detail32);
        _hideResultsButton.Visible = false;
        ui.Init(_showResultsButton, Resources.show_detail, Resources.show_detail32);
        _showResultsButton.Visible = false;
        _ui = ui;
    }

    private void TextControl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
        if (e.Key == System.Windows.Input.Key.F5 && !e.IsRepeat) {
            Execute();
            e.Handled = true;
        }
    }

    private void ExecuteButton_Click(object sender, EventArgs e) {
        Execute();
    }

    private void SetOutput() {
        // Remove the previous result controls, if any.
        for (var i = _split.Panel2.Controls.Count - 1; i >= 0; i--) {
            var control = _split.Panel2.Controls[i];
            _split.Panel2.Controls.RemoveAt(i);
            control.Dispose();
        }
        _dataTables.Clear();
        _tabs = null;

        // Now that the previous results are gone, we can bail early if there is no output.
        if (Output == null ||
            (!Output.DataTables.Any() && Output.ScalarResult == null && !Output.TextOutput.Any())
            ) {
            ShowHideResultsPane(show: false);
            return;
        }

        // Create a DataGridView for each DataTable
        var dataTables = ConvertOutputToDataTables();
        List<DataGridView> grids = new(dataTables.Count);
        foreach (var table in dataTables) {
            var grid = DataGridViewUtil.NewDataGridView();
            grid.AllowUserToResizeColumns = true;
            grid.AllowUserToOrderColumns = true;
            grid.Dock = DockStyle.Fill;
            grid.DataSource = table.DataTable;
            grid.Margin = Padding.Empty;
            grids.Add(grid);
        }

        // Show a tab control with one grid per tab.
        _tabs = new() {
            Dock = DockStyle.Fill
        };
        _ui.Init(_tabs);
        _split.Panel2.Controls.Add(_tabs);
        Debug.Assert(dataTables.Count == grids.Count);
        for (var i = 0; i < dataTables.Count; i++) {
            var (fullCount, dt) = dataTables[i];
            _dataTables.Add(dataTables[i]);
            TabPage page = new(
                dt.Rows.Count == fullCount
                ? $"Result ({fullCount:#,##0} row{(fullCount == 1 ? "" : "s")})"
                : $"Result ({fullCount:#,##0} row{(fullCount == 1 ? "" : "s")}, {dt.Rows.Count:#,##0} shown)"
                );
            _tabs.TabPages.Add(page);
            _ui.Init(page);
            page.Controls.Add(grids[i]);
            grids[i].AutoSizeColumns(this.Scaled(300));
        }
        ShowHideResultsPane(show: true);
    }

    private void ShowHideResultsPane(bool show) {
        if (_split.Panel2.Controls.Count == 0) {
            // There is nothing to show in the results panel.
            _split.Panel2Collapsed = true;
            _hideResultsButton.Visible = false;
            _showResultsButton.Visible = false;
        } else {
            _split.Panel2Collapsed = !show;
            _hideResultsButton.Visible = show;
            _showResultsButton.Visible = !show;
        }
    }

    private void Execute() {
        try {
            _manager.CommitOpenEditors();
            var sql = SqlText;
            var output = WaitForm.Go(TopLevelControl, "Script", "Running your script...", out var success, () => {
                return _manager.ExecuteScript(sql, maxRows: MAX_GRID_ROWS);
            });
            if (!success) {
                return;
            };
            Output = output;
            _manager.SetDirty();
            _manager.Rescan();
        } catch (Exception ex) {
            Ui.ShowError(TopLevelControl, "Script Error", ex);
        }
    }

    private List<(long Count, DataTable DataTable)> ConvertOutputToDataTables() {
        List<(long Count, DataTable DataTable)> resultSets = new();
        if (Output.ScalarResult != null) {
            DataTable dt = new("Scalar");
            dt.Columns.Add("value", Output.ScalarResult.GetType());
            var row = dt.NewRow();
            row.ItemArray = new object[] { Output.ScalarResult };
            dt.Rows.Add(row);
            resultSets.Add((1, dt));
        }
        if (Output.TextOutput.Any()) {
            DataTable dt = new("Output");
            dt.Columns.Add("message", typeof(string));
            foreach (var line in Output.TextOutput) {
                var row = dt.NewRow();
                row.ItemArray = new object[] { line };
                dt.Rows.Add(row);
            }
            resultSets.Add((dt.Rows.Count, dt));
        }
        foreach (var sdt in Output.DataTables) {
            resultSets.Add((sdt.FullCount, sdt.ToDataTable()));
        }
        return resultSets;
    }

    private void SendTableMenu_Click(object sender, EventArgs e) {
        if (_dataTables == null || !_dataTables.Any()) {
            Ui.ShowError(TopLevelControl, "Send to Table",
                "There are no data table results to send.", "Make sure to execute your query first.");
            return;
        }

        var (fullCount, dt) = _dataTables[_tabs.SelectedIndex];

        using SendToTableForm f = new(
            "results",
            _manager.Items
                .Where(x => x.Type == NotebookItemType.Table || x.Type == NotebookItemType.View)
                .Select(x => x.Name)
                .ToList()
        );
        if (f.ShowDialog(this) != DialogResult.OK) {
            return;
        }
        var name = f.SelectedName;

        var rerun = fullCount != dt.Rows.Count;
        if (rerun) {
            var choice = Ui.ShowTaskDialog(this, "Your query will be executed. Proceed?", "Send to Table",
                new[] { Ui.OK, Ui.CANCEL });
            if (choice != Ui.OK) {
                return;
            }
        }

        var columnDefs = dt.Columns.Cast<DataColumn>().Select(x =>
            $"{x.ColumnName.DoubleQuote()} {GetSqlNameForDbType(x.DataType)}");
        var createSql = $"CREATE TABLE {name.DoubleQuote()} ({string.Join(", ", columnDefs)})";

        var insertSql = SqlUtil.GetInsertSql(name, dt.Columns.Count);

        var sql = SqlText;
        var output = Output;
        var tabIndex = _tabs.SelectedIndex;
        WaitForm.GoWithCancel(TopLevelControl, "Send", $"Sending {fullCount:#,##0} rows to \"{name}\"...", out var success, cancel => {
            _manager.Notebook.Invoke(() => {
                SqlUtil.WithCancellableTransaction(_manager.Notebook, () => {
                    _manager.ExecuteScript(createSql);
                    if (!rerun) {
                        foreach (DataRow row in dt.Rows) {
                            cancel.ThrowIfCancellationRequested();
                            _manager.Notebook.Execute(insertSql, row.ItemArray);
                        }
                        return;
                    }

                    output = _manager.ExecuteScript(sql);
                    var scalarResultTabIndex =
                        output.ScalarResult != null ? 0
                        : -1;
                    var textTabIndex =
                        output.ScalarResult == null && output.TextOutput.Any() ? 0 :
                        output.ScalarResult != null && output.TextOutput.Any() ? 1 :
                        -1;
                    var tablesTabOffset =
                        (output.ScalarResult != null ? 1 : 0) +
                        (output.TextOutput.Any() ? 1 : 0);

                    if (tabIndex == scalarResultTabIndex) {
                        _manager.Notebook.Execute(insertSql, new object[] { output.ScalarResult });
                    } else if (tabIndex == textTabIndex) {
                        foreach (var line in output.TextOutput) {
                            cancel.ThrowIfCancellationRequested();
                            _manager.Notebook.Execute(insertSql, new object[] { line });
                        }
                    } else {
                        var table = output.DataTables[tabIndex - tablesTabOffset];
                        foreach (var row in table.Rows) {
                            cancel.ThrowIfCancellationRequested();
                            _manager.Notebook.Execute(insertSql, row);
                        }
                    }
                }, cancel);
            });
        });
        _manager.SetDirty();
        _manager.Rescan();
    }

    private static string GetSqlNameForDbType(Type type) {
        if (type == typeof(string)) {
            return "TEXT";
        } else if (type == typeof(int) || type == typeof(long)) {
            return "INTEGER";
        } else if (type == typeof(double)) {
            return "FLOAT";
        } else if (type == typeof(byte[])) {
            return "BLOB";
        } else {
            return "ANY";
        }
    }

    private void HideResultsButton_Click(object sender, EventArgs e) {
        ShowHideResultsPane(false);
    }

    private void ShowResultsButton_Click(object sender, EventArgs e) {
        ShowHideResultsPane(true);
    }

    private void LimitsRowsMenu_Click(object sender, EventArgs e) {
        foreach (ToolStripMenuItem item in _limitRowsOnPageMenu.DropDownItems) {
            item.Checked = ReferenceEquals(sender, item);
        }
    }
}
