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
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using SqlNotebookCore;
using SqlNotebookScript;

namespace SqlNotebook {
    public partial class TableDocumentControl : UserControl, IDocumentControl {
        private readonly NotebookManager _manager;
        private readonly string _tableName;
        private readonly IWin32Window _mainForm;

        public TableDocumentControl(NotebookManager manager, string tableName, IWin32Window mainForm) {
            InitializeComponent();
            _manager = manager;
            _tableName = tableName;
            _mainForm = mainForm;

            Load += async (sender, e) => {
                Exception exception = null;
                SimpleDataTable sdt = null;
                
                manager.PushStatus("Reading table data...");
                await Task.Run(() => {
                    try {
                        var n = _manager.Notebook;
                        n.Invoke(() => {
                            sdt = n.Query($"SELECT * FROM {_tableName.DoubleQuote()} LIMIT 1000");
                        });
                    } catch (Exception ex) {
                        exception = ex;
                    }
                });
                manager.PopStatus();

                if (exception == null) {
                    var dt = new DataTable();
                    foreach (var col in sdt.Columns) {
                        dt.Columns.Add(col);
                    }
                    foreach (var row in sdt.Rows) {
                        var dtRow = dt.NewRow();
                        dtRow.ItemArray = row;
                        dt.Rows.Add(dtRow);
                    }
                    _grid.DataSource = dt;
                } else {
                    MessageDialog.ShowError(_mainForm, "Preview Table", "An error occurred.", exception.Message);
                }
            };
        }

        // IDocumentControl
        string IDocumentControl.ItemName { get; set; }
        string IDocumentControl.DocumentText { get; } = "";
        public void Save() { }

        private void ScriptBtn_Click(object sender, EventArgs e) {
            var name = _manager.NewScript();
            _manager.SetItemData(name, $"SELECT * FROM {_tableName.DoubleQuote()} LIMIT 1000");
            _manager.OpenItem(new NotebookItem(NotebookItemType.Script, name));
        }
    }
}
