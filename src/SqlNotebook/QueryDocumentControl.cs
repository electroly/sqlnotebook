using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlNotebook.Properties;
using SqlNotebookCore;
using SqlNotebookScript.Interpreter;
using SqlNotebookScript.Utils;

namespace SqlNotebook {
    public partial class QueryDocumentControl : UserControl, IDocumentControl {
        private readonly IWin32Window _mainForm;
        private readonly NotebookManager _manager;
        private readonly Notebook _notebook;
        private readonly SqlTextControl _textCtl;
        private readonly List<(long Count, DataTable DataTable)> _results = new();
        private int _selectedResultIndex = 0;

        public string ItemName { get; set; }

        public void Save() {
            _manager.SetItemData(ItemName, _textCtl.SqlText);
        }

        public QueryDocumentControl(string name, NotebookManager manager, IWin32Window mainForm) {
            InitializeComponent();
            ItemName = name;
            _manager = manager;
            _notebook = manager.Notebook;
            _mainForm = mainForm;
            _resultToolStrip.SetMenuAppearance();

            _grid.EnableDoubleBuffering();

            _textCtl = new SqlTextControl(false) {
                Dock = DockStyle.Fill,
                Padding = new Padding(4, 0, 0, 0),
            };
            _sqlPanel.Controls.Add(_textCtl);

            Ui ui = new(this, false);
            ui.Init(_executeBtn, Resources.ControlPlayBlue, Resources.control_play_blue32);
            ui.Init(_prevBtn, Resources.resultset_previous, Resources.resultset_previous32);
            ui.Init(_nextBtn, Resources.resultset_next, Resources.resultset_next32);
            ui.Init(_sendTableMnu, Resources.table, Resources.table32);
            ui.Init(_sendCsvMnu, Resources.page_white_text, Resources.page_white_text32);
            ui.Init(_sendExcelMnu, Resources.page_white_excel, Resources.page_white_excel32);

            // if this tool window has been pulled off into a floating window, then the MainForm's key handler won't
            // trigger on F5, so catch it here.
            _textCtl.TextBox.KeyDown += async (sender, e) => {
                if (e.SystemKey == System.Windows.Input.Key.F5) {
                    Execute();
                }
            };

            string initialText = _manager.GetItemData(ItemName);
            if (initialText != null) {
                _textCtl.SqlText = initialText;
            }

            _textCtl.SqlTextChanged += (sender2, e2) => _manager.SetDirty();

            ShowResult(0);
        }

        public void Execute() {
            Execute(_textCtl.SqlText);
        }

        private void ExecuteBtn_Click(object sender, EventArgs e) {
            Execute();
        }

        private bool Execute(string sql) {
            try {
                _manager.CommitOpenEditors();
                if (!ExecuteCore(sql)) {
                    return false; // Error box already shown.
                }
                _manager.SetDirty();
                _manager.Rescan();
                return true;
            } catch (Exception ex) {
                Ui.ShowError(TopLevelControl, "Script Error", ex);
                return false;
            }
        }

        private bool GetResultSets(string sql, int maxRows, out List<(long Count, DataTable DataTable)> resultSets) {
            var output = WaitForm.Go(FindForm(), "Script", "Running your script...", out var success, () =>
                _manager.ExecuteScript(sql, maxRows: maxRows));
            if (!success) {
                resultSets = null;
                return false;
            };

            resultSets = new List<(long Count, DataTable DataTable)>();
            foreach (var sdt in output.DataTables) {
                resultSets.Add((sdt.FullCount, sdt.ToDataTable()));
            }

            if (output.TextOutput.Any()) {
                var dt = new DataTable("Output");
                dt.Columns.Add("printed_text", typeof(string));
                foreach (var line in output.TextOutput) {
                    var row = dt.NewRow();
                    row.ItemArray = new object[] { line };
                    dt.Rows.Add(row);
                }
                resultSets.Insert(0, (dt.Rows.Count, dt));
            }
            return true;
        }

        private bool ExecuteCore(string sql) {
            if (!GetResultSets(sql, 500_000, out var resultSets)) {
                return false;
            }
            _grid.DataSource = null;
            _results.ForEach(x => x.DataTable.Dispose());
            _results.Clear();
            _results.AddRange(resultSets);
            ShowResult(0);
            return true;
        }

        private void ShowResult(int index) {
            _selectedResultIndex = index = Math.Max(0, Math.Min(_results.Count - 1, index));
            if (_results.Count == 0) {
                _resultSetLbl.Text = "(no results)";
                _rowCountLbl.Text = "";
                foreach (ToolStripItem btn in _resultToolStrip.Items) {
                    btn.Visible = btn == _executeBtn;
                }
            } else {
                foreach (ToolStripItem btn in _resultToolStrip.Items) {
                    btn.Visible = true;
                }
                var source = index >= 0 && index < _results.Count ? _results[index].DataTable : null;
                var fullCount = index >= 0 && index < _results.Count ? _results[index].Count : 0;
                _grid.DataSource = source;
                _grid.AutoSizeColumns(Ui.XWidth(50, this));
                _resultSetLbl.Text = $"{index + 1:#,##0} of {_results.Count:#,##0}";
                var countText = source == null ? "" : $"{fullCount:#,##0} row{(fullCount == 1 ? "" : "s")}";
                if (fullCount > source.Rows.Count) {
                    countText += $" ({source.Rows.Count:#,##0} shown)";
                }
                _rowCountLbl.Text = countText;
            }
            _prevBtn.Enabled = index > 0;
            _nextBtn.Enabled = index < _results.Count - 1;
        }

        private void PrevBtn_Click(object sender, EventArgs e) {
            ShowResult(_selectedResultIndex - 1);
        }

        private void NextBtn_Click(object sender, EventArgs e) {
            ShowResult(_selectedResultIndex + 1);
        }

        private void SendTableMnu_Click(object sender, EventArgs e) {
            var f = new SendToTableForm(
                $"{ItemName}_result",
                _manager.Items
                    .Where(x => x.Type == NotebookItemType.Table || x.Type == NotebookItemType.View)
                    .Select(x => x.Name)
                    .ToList()
            );
            string name;
            if (f.ShowDialogAndDispose(this) == DialogResult.OK) {
                name = f.SelectedName;
            } else {
                return;
            }

            var result = _results[_selectedResultIndex];
            if (result.Count > result.DataTable.Rows.Count) {
                var choice = Ui.ShowTaskDialog(
                    _mainForm,
                    "The results will be truncated. Do you want to continue?",
                    "Send to Table",
                    new[] { Ui.OK, Ui.CANCEL },
                    TaskDialogIcon.Warning,
                    details: $"The query returned {result.Count:#,##0} rows, but only {result.DataTable.Rows.Count:#,##0} will be sent to the table. Consider using an \"INSERT INTO\" statement for large tables.");
                if (choice != Ui.OK) {
                    return;
                }
            }
            var dt = result.DataTable;
            var columnDefs = dt.Columns.Cast<DataColumn>().Select(x => 
                $"{x.ColumnName.DoubleQuote()} {GetSqlNameForDbType(x.DataType)}");
            var createSql = $"CREATE TABLE {name.DoubleQuote()} ({string.Join(", ", columnDefs)})";

            var insertSql = SqlUtil.GetInsertSql(name, dt.Columns.Count);

            WaitForm.Go(FindForm(), "Send Data", $"Sending {result.DataTable.Rows.Count:#,##0} rows to \"{name}\"...", out var success, () => {
                _notebook.Invoke(() => {
                    SqlUtil.WithTransaction(_notebook, () => {
                        _manager.ExecuteScript(createSql);
                        foreach (DataRow row in dt.Rows) {
                            _notebook.Execute(insertSql, row.ItemArray);
                        }
                    });
                });
            });
            _manager.SetDirty();
            _manager.Rescan();

            if (!success) {
                return;
            }
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
    }
}
