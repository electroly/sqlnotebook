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
        private readonly SqlTextControl _inputText;
        private readonly FlowLayoutPanel _outputFlow;
        private readonly Padding _outputSqlMargin;
        private readonly Padding _outputTableMargin;
        private readonly Padding _outputCountMargin;
        private readonly Size _spacerSize;

        public ConsoleControl(IWin32Window mainForm, NotebookManager manager) {
            InitializeComponent();
            _mainForm = mainForm;
            _manager = manager;

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

            _contextMenuStrip.SetMenuAppearance();
            ui.Init(_clearHistoryMenu, Resources.Delete, Resources.delete32);
            _outputFlow.ContextMenuStrip = _contextMenuStrip;
            _outputPanel.ContextMenuStrip = _contextMenuStrip;

            EventHandler optUpdated = delegate {
                var opt = UserOptions.Instance;
                _outputPanel.BackColor = opt.GetColors()[UserOptionsColor.GRID_BACKGROUND];
            };
            optUpdated(null, null);
            UserOptions.Updated += optUpdated;
            Disposed += delegate { UserOptions.Updated -= optUpdated; };
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
                        Margin = _outputSqlMargin,
                        Cursor = Cursors.Hand,
                    };
                    
                    EventHandler optUpdated = delegate {
                        var opt = UserOptions.Instance;
                        label.Font = opt.GetCodeFont();
                        var colors = opt.GetColors();
                        label.BackColor = colors[UserOptionsColor.GRID_BACKGROUND];
                        label.ForeColor = colors[UserOptionsColor.GRID_PLAIN];
                    };
                    optUpdated(null, null);
                    UserOptions.Updated += optUpdated;
                    label.Disposed += delegate { UserOptions.Updated -= optUpdated; };

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
                    Label label = new() {
                        AutoSize = true,
                        Text = text,
                        Margin = _outputTableMargin,
                        ContextMenuStrip = _contextMenuStrip
                    };

                    EventHandler optUpdated = delegate {
                        var opt = UserOptions.Instance;
                        label.Font = opt.GetDataTableFont();
                        var colors = opt.GetColors();
                        label.BackColor = colors[UserOptionsColor.GRID_BACKGROUND];
                        label.ForeColor = colors[UserOptionsColor.GRID_PLAIN];
                    };
                    optUpdated(null, null);
                    UserOptions.Updated += optUpdated;
                    label.Disposed += delegate { UserOptions.Updated -= optUpdated; };

                    _outputFlow.Controls.Add(label);
                }
                
                if (output.ScalarResult != null) {
                    Label label = new() {
                        AutoSize = true,
                        Text = output.ScalarResult.ToString(),
                        Margin = _outputTableMargin,
                    };

                    EventHandler optUpdated = delegate {
                        var opt = UserOptions.Instance;
                        label.Font = opt.GetDataTableFont();
                        var colors = opt.GetColors();
                        label.BackColor = colors[UserOptionsColor.GRID_BACKGROUND];
                        label.ForeColor = colors[UserOptionsColor.GRID_PLAIN];
                    };
                    optUpdated(null, null);
                    UserOptions.Updated += optUpdated;
                    label.Disposed += delegate { UserOptions.Updated -= optUpdated; };

                    _outputFlow.Controls.Add(label);
                }

                foreach (var simpleDataTable in output.DataTables) {
                    var s = simpleDataTable.Rows.Count == 1 ? "" : "s";

                    Label label = new() {
                        AutoSize = true,
                        Text = simpleDataTable.FullCount > MAX_GRID_ROWS
                            ? $"{simpleDataTable.FullCount:#,##0} row{s} ({MAX_GRID_ROWS} shown)"
                            : $"{simpleDataTable.FullCount:#,##0} row{s}",
                        Margin = _outputCountMargin,
                    };

                    EventHandler optUpdated = delegate {
                        var opt = UserOptions.Instance;
                        label.Font = opt.GetDataTableFont();
                        var colors = opt.GetColors();
                        label.BackColor = colors[UserOptionsColor.GRID_BACKGROUND];
                        label.ForeColor = colors[UserOptionsColor.GRID_PLAIN];
                    };
                    optUpdated(null, null);
                    UserOptions.Updated += optUpdated;
                    label.Disposed += delegate { UserOptions.Updated -= optUpdated; };

                    _outputFlow.Controls.Add(label);

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

        public void TakeFocus() {
            _inputText.TextBox.Focus();
            _inputText.TextBox.SelectAll();
        }

        private void ExecuteButton_Click(object sender, EventArgs e) {
            Execute();
        }

        private void Execute() {
            var code = _inputText.SqlText.Trim();

            if (string.IsNullOrWhiteSpace(code)) {
                return;
            }

            var output = WaitForm.Go(FindForm(), "Delete", "Executing...", out var success, () =>
                SqlUtil.WithTransaction(_manager.Notebook, () => _manager.ExecuteScript(code, maxRows: MAX_GRID_ROWS)));
            if (!success) {
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
