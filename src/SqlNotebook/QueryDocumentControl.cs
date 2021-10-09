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
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlNotebookCore;
using SqlNotebookScript.Interpreter;
using SqlNotebookScript.Utils;

namespace SqlNotebook {
    public partial class QueryDocumentControl : UserControl, IDocumentControl {
        private readonly IWin32Window _mainForm;
        private readonly NotebookManager _manager;
        private readonly Notebook _notebook;
        private readonly SqlTextControl _textCtl;
        private readonly List<DataTable> _results = new List<DataTable>();
        private int _selectedResultIndex = 0;
        private readonly Slot<bool> _operationInProgress;

        public string ItemName { get; set; }

        public void Save() {
            _manager.SetItemData(ItemName, _textCtl.SqlText);
        }

        public QueryDocumentControl(string name, NotebookManager manager, IWin32Window mainForm, Slot<bool> operationInProgress) {
            InitializeComponent();
            ItemName = name;
            _manager = manager;
            _notebook = manager.Notebook;
            _mainForm = mainForm;
            _operationInProgress = operationInProgress;
            _resultToolStrip.SetMenuAppearance();

            _grid.EnableDoubleBuffering();

            _textCtl = new SqlTextControl(false) {
                Dock = DockStyle.Fill,
                Padding = new Padding(4, 0, 0, 0)
            };
            _sqlPanel.Controls.Add(_textCtl);

            // if this tool window has been pulled off into a floating window, then the MainForm's key handler won't
            // trigger on F5, so catch it here.
            _textCtl.TextBox.KeyDown += async (sender, e) => {
                if (e.KeyCode == Keys.F5) {
                    await Execute();
                }
            };

            Load += (sender, e) => {
                string initialText = _manager.GetItemData(ItemName);
                if (initialText != null) {
                    _textCtl.SqlText = initialText;
                }

                _textCtl.SqlTextChanged += (sender2, e2) => _manager.SetDirty();

                ShowResult(0);
            };
        }

        public async Task Execute() {
            await Execute(_textCtl.SqlText);
        }

        private async void ExecuteBtn_Click(object sender, EventArgs e) {
            await Execute();
        }

        private async Task<bool> Execute(string sql) {
            try {
                if (_operationInProgress) {
                    throw new Exception("Another operation is already in progress.");
                }

                _manager.CommitOpenEditors();
                await ExecuteCore(sql);
                _manager.SetDirty();
                _manager.Rescan();
                return true;
            } catch (Exception ex) {
                MessageForm.ShowError(TopLevelControl, "Script Error", "An error occurred.", ex.Message);
                return false;
            }
        }

        private async Task ExecuteCore(string sql) {
            ScriptOutput output = null;
            Exception exception = null;
            _manager.PushStatus("Running your script...");
            await Task.Run(() => {
                try {
                    output = _manager.ExecuteScript(sql);
                } catch (Exception ex) {
                    exception = ex;
                }
            });
            _manager.PopStatus();

            if (exception != null) {
                throw exception;
            } else {
                var resultSets = new List<DataTable>();

                foreach (var sdt in output.DataTables) {
                    resultSets.Add(sdt.ToDataTable());
                }

                if (output.TextOutput.Any()) {
                    var dt = new DataTable("Output");
                    dt.Columns.Add("printed_text", typeof(string));
                    foreach (var line in output.TextOutput) {
                        var row = dt.NewRow();
                        row.ItemArray = new object[] { line };
                        dt.Rows.Add(row);
                    }
                    resultSets.Insert(0, dt);
                }

                _grid.DataSource = null;
                _results.ForEach(x => x.Dispose());
                _results.Clear();
                _results.AddRange(resultSets);
                ShowResult(0);
            }
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
                var source = index >= 0 && index < _results.Count ? _results[index] : null;
                _grid.DataSource = source;
                _grid.AutoSizeColumns();
                _resultSetLbl.Text = $"{index + 1} of {_results.Count}";
                _rowCountLbl.Text = source == null ? "" : $"{source.Rows.Count} row{(source.Rows.Count == 1 ? "" : "s")}";
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

        private async void SendTableMnu_Click(object sender, EventArgs e) {
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

            var dt = _results[_selectedResultIndex];
            var columnDefs = dt.Columns.Cast<DataColumn>().Select(x => 
                $"{x.ColumnName.DoubleQuote()} {GetSqlNameForDbType(x.DataType)}");
            var createSql = $"CREATE TABLE {name.DoubleQuote()} ({string.Join(", ", columnDefs)})";

            var insertSql = SqlUtil.GetInsertSql(name, dt.Columns.Count);

            Exception exception = null;
            _manager.PushStatus($"Sending data to \"{name}\"...");
            await Task.Run(() => {
                try {
                    _notebook.Invoke(() => {
                        SqlUtil.WithTransaction(_notebook, () => {
                            _manager.ExecuteScript(createSql);
                            foreach (DataRow row in dt.Rows) {
                                _notebook.Execute(insertSql, row.ItemArray);
                            }
                        });
                    });
                    _manager.Rescan();
                } catch (Exception ex) {
                    exception = ex;
                }
            });
            _manager.PopStatus();
            
            if (exception != null) {
                MessageForm.ShowError(this, "Send to Table", exception.Message);
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
