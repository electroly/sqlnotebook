using SqlNotebook.Properties;
using SqlNotebookScript.Interpreter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class QueryEmbeddedControl : UserControl {
        private readonly NotebookManager _manager;
        private readonly bool _isPageContext;
        private readonly Ui _ui;

        public SqlTextControl TextControl { get; private set; }
        public string SqlText {
            get => TextControl.SqlText;
            set => TextControl.SqlText = value;
        }
        public ScriptOutput Output { get; private set; }
        public event EventHandler SqlTextChanged;

        public QueryEmbeddedControl(NotebookManager manager, bool isPageContext) {
            InitializeComponent();
            _manager = manager;
            _isPageContext = isPageContext;
            _toolStrip.SetMenuAppearance();

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
            _showResultsButton.Visible = true;
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

        public void SetOutput(ScriptOutput output) {
            Output = output;

            // Remove the previous result controls, if any.
            for (var i = _split.Panel2.Controls.Count - 1; i >= 0; i--) {
                var control = _split.Panel2.Controls[i];
                _split.Panel2.Controls.RemoveAt(i);
                control.Dispose();
            }

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
            TabControl tabs = new() {
                Dock = DockStyle.Fill
            };
            _ui.Init(tabs);
            _split.Panel2.Controls.Add(tabs);
            Debug.Assert(dataTables.Count == grids.Count);
            for (var i = 0; i < dataTables.Count; i++) {
                var s = dataTables[i].Count == 1 ? "" : "s";
                TabPage page = new($"Result ({dataTables[i].Count:#,##0} row{s})");
                tabs.TabPages.Add(page);
                _ui.Init(page);
                page.Controls.Add(grids[i]);
                grids[i].AutoSizeColumns(this.Scaled(300));
            }
            ShowHideResultsPane(show: true);
        }

        private void ShowHideResultsPane(bool show) {
            _split.Panel2Collapsed = !show;
            _hideResultsButton.Visible = show;
            _showResultsButton.Visible = !show;
        }

        private void Execute() {
            try {
                _manager.CommitOpenEditors();
                var sql = SqlText;
                var output = WaitForm.Go(TopLevelControl, "Script", "Running your script...", out var success, () =>
                    _manager.ExecuteScript(sql, maxRows: 100_000));
                if (!success) {
                    return;
                };
                SetOutput(output);
                _manager.SetDirty();
                _manager.Rescan();
            } catch (Exception ex) {
                Ui.ShowError(TopLevelControl, "Script Error", ex);
            }
        }

        private List<(long Count, DataTable DataTable)> ConvertOutputToDataTables() {
            List<(long Count, DataTable DataTable)> resultSets = new();
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

        }

        private void HideResultsButton_Click(object sender, EventArgs e) {
            ShowHideResultsPane(false);
        }

        private void ShowResultsButton_Click(object sender, EventArgs e) {
            ShowHideResultsPane(true);
        }
    }
}
