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
using SqlNotebookScript;
using System.Runtime.InteropServices;

namespace SqlNotebook {
    public partial class QueryDocumentControl : UserControl, IDocumentControl {
        private readonly IWin32Window _mainForm;
        private readonly NotebookManager _manager;
        private readonly Notebook _notebook;
        private readonly TextEditorControlEx _textEditor;
        private readonly List<DataTable> _results = new List<DataTable>();
        private int _selectedResultIndex = 0;

        public string DocumentText {
            get {
                return _textEditor.Text;
            }
        }

        public string ItemName { get; set; }

        public QueryDocumentControl(string name, NotebookManager manager, IWin32Window mainForm, bool runImmediately = false) {
            InitializeComponent();
            ItemName = name;
            _manager = manager;
            _notebook = manager.Notebook;
            _mainForm = mainForm;

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

            Load += async (sender, e) => {
                string initialText = _manager.GetItemData(ItemName);
                if (initialText != null) {
                    _textEditor.Text = initialText;
                }

                _textEditor.TextChanged += (sender2, e2) => _manager.SetDirty();

                ShowResult(0);

                if (runImmediately) {
                    await Execute();
                }
            };
        }

        public async Task Execute() {
            await Execute(_textEditor.Text);
        }

        private async void ExecuteBtn_Click(object sender, EventArgs e) {
            await Execute();
        }

        private async Task<bool> Execute(string sql) {
            try {
                await ExecuteCore(sql);
                _manager.SetDirty();
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

        private async Task ExecuteCore(string sql) {
            ScriptOutput output = null;
            Exception exception = null;
            _manager.PushStatus("Running your script. Press ESC to cancel.");
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
                    var dt = new DataTable();
                    foreach (var col in sdt.Columns) {
                        dt.Columns.Add(col);
                    }
                    foreach (var row in sdt.Rows) {
                        var dtRow = dt.NewRow();
                        dtRow.ItemArray = row;
                        dt.Rows.Add(dtRow);
                    }
                    resultSets.Add(dt);
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
