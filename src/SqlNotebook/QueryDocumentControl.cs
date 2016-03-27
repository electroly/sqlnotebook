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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using Microsoft.WindowsAPICodePack.Dialogs;
using SqlNotebookCore;

namespace SqlNotebook {
    public partial class QueryDocumentControl : UserControl {
        private readonly NotebookManager _manager;
        private readonly Notebook _notebook;
        private readonly TextEditorControlEx _textEditor;
        private readonly List<DataTable> _results = new List<DataTable>();
        private int _selectedResultIndex = 0;

        public string SqlText {
            get {
                return _textEditor.Text;
            }
        }

        public string ItemName { get; set; }

        public QueryDocumentControl(string name, NotebookManager manager) {
            InitializeComponent();
            ItemName = name;
            _manager = manager;
            _notebook = manager.Notebook;

            _textEditor = new TextEditorControlEx {
                Dock = DockStyle.Fill,
                SyntaxHighlighting = "SQL",
                BorderStyle = BorderStyle.None,
                ConvertTabsToSpaces = true,
                EnableFolding = true,
                Font = new Font("Consolas", 10),
                IsIconBarVisible = false,
                ShowLineNumbers = false,
                ShowHRuler = false,
                ShowVRuler = false,
                TabIndent = 4
            };
            _sqlPanel.Controls.Add(_textEditor);

            Load += (sender, e) => {
                string initialText = _manager.GetItemData(ItemName);
                if (initialText != null) {
                    _textEditor.Text = initialText;
                }
                ShowResult(0);
            };
        }

        public void Execute() {
            Execute(_textEditor.Text);
        }

        private void _executeBtn_Click(object sender, EventArgs e) {
            Execute();
        }

        private bool Execute(string sql) {
            try {
                ExecuteCore(sql);
                _manager.Rescan();
                return true;
            } catch (Exception ex) {
                var td = new TaskDialog {
                    Cancelable = false,
                    Caption = "Script Error",
                    Icon = TaskDialogStandardIcon.Error,
                    InstructionText = "An error occurred.",
                    StandardButtons = TaskDialogStandardButtons.Ok,
                    OwnerWindowHandle = ParentForm.Handle,
                    StartupLocation = TaskDialogStartupLocation.CenterOwner,
                    Text = ex.Message
                };
                td.Show();
                return false;
            }
        }

        private void ExecuteCore(string sql) {
            var dts = new List<DataTable>();
            Exception exception = null;
            var f = new WaitForm("SQL Query", "Running your SQL query...", () => {
                _notebook.Invoke(() => {
                    try {
                        foreach (var statement in NotebookManager.SplitStatements(sql)) {
                            dts.Add(_notebook.Query(statement));
                        }
                    } catch (Exception ex) {
                        foreach (var dt in dts) {
                            if (dt != null) {
                                dt.Dispose();
                            }
                        }
                        dts.Clear();
                        exception = ex;
                    }
                });
            });
            using (f) {
                f.ShowDialog(ParentForm, 25);
                if (exception != null) {
                    throw exception;
                } else {
                    var resultSets = dts.Where(x => x != null && x.Columns.Count > 0).ToList();
                    dts.Except(resultSets).ToList().ForEach(x => x?.Dispose());
                    dts.Clear();

                    _grid.DataSource = null;
                    _results.ForEach(x => x.Dispose());
                    _results.Clear();
                    _results.AddRange(resultSets);
                    ShowResult(0);
                }
            }
        }

        private void ShowResult(int index) {
            _selectedResultIndex = index = Math.Max(0, Math.Min(_results.Count - 1, index));
            if (_results.Count == 0) {
                _resultSetLbl.Text = "(no results)";
                _rowCountLbl.Text = "";
            } else {
                var source = index >= 0 && index < _results.Count ? _results[index] : null;
                _grid.DataSource = source;
                foreach (DataGridViewColumn col in _grid.Columns) {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    var width = col.Width;
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    col.Width = Math.Min(250, width);
                }
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
    }
}
