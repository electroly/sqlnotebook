using SqlNotebook.Properties;
using SqlNotebookScript.Interpreter;
using SqlNotebookScript.Utils;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class ConsoleControl : UserControl {
        private const int MAX_GRID_ROWS = 10;
        private const int MAX_HISTORY = 50;

        private readonly IWin32Window _mainForm;
        private readonly NotebookManager _manager;
        private readonly Slot<bool> _operationInProgress;
        private readonly SqlTextControl _inputText;
        private readonly FlowLayoutPanel _outputFlow;
        private readonly Font _consolas = new("Consolas", 11f);
        private readonly Font _segoeBold = new("Segoe UI", 11f, FontStyle.Bold);
        private readonly Padding _outputSqlMargin;
        private readonly Padding _outputTableMargin;
        private readonly Padding _outputCountMargin;
        private readonly Size _spacerSize;

        public ConsoleControl(IWin32Window mainForm, NotebookManager manager, Slot<bool> operationInProgress) {
            InitializeComponent();
            _mainForm = mainForm;
            _manager = manager;
            _operationInProgress = operationInProgress;

            _inputText = new(false) {
                Dock = DockStyle.Fill
            };
            _inputText.TextBox.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
            _inputPanel.Controls.Add(_inputText);

            _outputFlow = new() {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown
            };
            _outputPanel.Controls.Add(_outputFlow);

            Ui ui = new(this, false);
            ui.Init(_table);
            ui.Init(_executeButton, Resources.ControlPlayBlue, Resources.control_play_blue32);
            _table.RowStyles[1].Height = ui.XHeight(4);
            _inputBorderPanel.Margin = new(0, ui.XHeight(0.2), 0, 0);
            _inputText.TextBox.KeyDown += InputText_KeyDown;

            _outputSqlMargin = new(ui.XWidth(1), ui.XHeight(0.5), ui.XWidth(1), ui.XHeight(0.75));
            _outputTableMargin = new(ui.XWidth(8), 0, ui.XWidth(1), ui.XHeight(0.75));
            _outputCountMargin = new(ui.XWidth(8), 0, 0, 0);
            _spacerSize = new(0, ui.XHeight(1));

            _operationInProgress.Change += OperationInProgress_Change;
            _contextMenuStrip.SetMenuAppearance();
            ui.Init(_clearHistoryMenu, Resources.Delete, Resources.delete32);
            _outputFlow.ContextMenuStrip = _contextMenuStrip;
            _outputPanel.ContextMenuStrip = _contextMenuStrip;
        }

        private void Log(string sql, ScriptOutput output) {
            BeginInvoke(new Action(() => {
                var maxColWidth = Ui.XWidth(50, this);
                _outputFlow.SuspendLayout();
                while (_outputFlow.Controls.Count > MAX_HISTORY) {
                    _outputFlow.Controls.RemoveAt(0);
                }

                if (!string.IsNullOrWhiteSpace(sql)) {
                    Label label = new() {
                        AutoSize = true,
                        Text = sql,
                        Font = _consolas,
                        Margin = _outputSqlMargin,
                        Cursor = Cursors.Hand,
                    };
                    label.MouseUp += (sender, e) => {
                        if (e.Button == MouseButtons.Left) {
                            _inputText.SqlText = sql;
                            TakeFocus();
                        }
                    };
                    _outputFlow.Controls.Add(label);
                }

                if ((output.TextOutput?.Count ?? 0) > 0) {
                    var text = string.Join(Environment.NewLine, output.TextOutput);
                    _outputFlow.Controls.Add(new Label {
                        AutoSize = true,
                        Text = text,
                        Margin = _outputTableMargin,
                        Font = _segoeBold,
                        ForeColor = Color.Red,
                        ContextMenuStrip = _contextMenuStrip
                    });
                }
                
                if (output.ScalarResult != null) {
                    _outputFlow.Controls.Add(new Label {
                        AutoSize = true,
                        Text = output.ScalarResult.ToString(),
                        Margin = _outputTableMargin,
                    });
                }

                foreach (var simpleDataTable in output.DataTables) {
                    var s = simpleDataTable.Rows.Count == 1 ? "" : "s";
                    _outputFlow.Controls.Add(new Label {
                        AutoSize = true,
                        Text = simpleDataTable.Rows.Count > MAX_GRID_ROWS
                            ? $"{simpleDataTable.Rows.Count:#,##0} row{s} ({MAX_GRID_ROWS} shown)"
                            : $"{simpleDataTable.Rows.Count:#,##0} row{s}",
                        Margin = _outputCountMargin,
                        ForeColor = Color.Gray
                    });

                    DataGridView grid = DataGridViewUtil.NewDataGridView();
                    grid.Margin = _outputTableMargin;
                    grid.ContextMenuStrip = _contextMenuStrip;
                    grid.ScrollBars = ScrollBars.None;
                    _outputFlow.Controls.Add(grid);
                    grid.DataSource = simpleDataTable.ToDataTable(MAX_GRID_ROWS);
                    grid.AutoSizeColumns(maxColWidth);
                    foreach (DataGridViewColumn col in grid.Columns) {
                        col.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    grid.Size = new(
                        grid.Columns.OfType<DataGridViewColumn>().Sum(x => x.Width),
                        grid.ColumnHeadersHeight + grid.Rows.OfType<DataGridViewRow>().Sum(x => x.Height));
                    grid.ClearSelection();
                }

                _outputFlow.Controls.Add(new Panel {
                    Size = _spacerSize,
                    AutoSize = false
                });

                _outputFlow.ResumeLayout(true);

                if (_outputPanel.AutoScrollPosition.X != 0) {
                    _outputPanel.AutoScrollPosition = new(0, _outputPanel.AutoScrollPosition.Y);
                }
                _outputPanel.ScrollControlIntoView(_outputFlow.Controls[_outputFlow.Controls.Count - 1]);
            }));
        }

        private void OperationInProgress_Change(bool oldValue, bool newValue) {
            _executeButton.Enabled = !newValue;
        }

        public void TakeFocus() {
            _inputText.TextBox.Focus();
            _inputText.TextBox.SelectAll();
        }

        private void ExecuteButton_Click(object sender, EventArgs e) {
            Execute();
        }

        private void Execute() {
            if (_operationInProgress) {
                // It shouldn't be possible for execution to get in here, because we disable the button when an
                // operation is in progress. But just in case.
                return;
            }

            var code = _inputText.SqlText.Trim();

            if (string.IsNullOrWhiteSpace(code)) {
                return;
            }

            ScriptOutput output = null;
            using WaitForm f = new("Delete", "Executing...", () => {
                output = SqlUtil.WithTransaction(_manager.Notebook, () =>
                    _manager.ExecuteScript(code));
            });

            if (f.ShowDialog(_mainForm) != DialogResult.OK) {
                MessageForm.ShowError(_mainForm, "Console Error", f.ResultException.Message);
                return;
            }

            _manager.Rescan();
            _inputText.SqlText = "";
            Log(code, output);
            TakeFocus();
        }

        private void ClearHistoryMenu_Click(object sender, EventArgs e) {
            _outputFlow.Controls.Clear();
        }

        private void InputText_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == System.Windows.Input.Key.Enter &&
                (e.KeyboardDevice.Modifiers & System.Windows.Input.ModifierKeys.Control) != 0) {
                Execute();
                e.Handled = true;
            }
        }
    }
}
