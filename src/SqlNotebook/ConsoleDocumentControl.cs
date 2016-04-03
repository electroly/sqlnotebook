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
using SqlNotebookCore;
using Microsoft.WindowsAPICodePack.Dialogs;
using SqlNotebookScript;

namespace SqlNotebook {
    public partial class ConsoleDocumentControl : UserControl {
        private readonly NotebookManager _manager;
        private readonly Notebook _notebook;
        private readonly ConsoleRichTextBox _consoleTxt;
        private readonly Font _promptFont = new Font("Consolas", 10, FontStyle.Bold);
        private readonly Font _font = new Font("Consolas", 10);
        private readonly Font _headerFont = new Font("Consolas", 10, FontStyle.Italic);
        private readonly Font _dividerFont = new Font("Arial", 10);

        public string RtfText {
            get {
                return _consoleTxt.Rtf;
            }
        }

        public string ItemName { get; set; }

        public ConsoleDocumentControl(string name, NotebookManager manager) {
            InitializeComponent();
            ItemName = name;
            _manager = manager;
            _notebook = manager.Notebook;
            _consoleTxt = new ConsoleRichTextBox {
                Dock = DockStyle.Fill,
                BackColor = SystemColors.Window,
                BorderStyle = BorderStyle.None,
                AutoWordSelection = true,
                Font = _font,
                EnableAutoDragDrop = false,
                Multiline = true,
                ScrollBars = RichTextBoxScrollBars.Both,
                ShortcutsEnabled = true,
                WordWrap = false,
                PromptFont = _promptFont,
                PromptText = ">",
                PromptColor = Color.Red
            };
            Controls.Add(_consoleTxt);
            _consoleTxt.ConsoleCommand += (sender, e) => {
                e.WasSuccessful = Execute(e.Command);
            };

            Load += (sender, e) => {
                string initialRtf = _manager.GetItemData(ItemName);
                if (initialRtf != null) {
                    _consoleTxt.Rtf = initialRtf;
                }
                if (_consoleTxt.Text.EndsWith($"\n{_consoleTxt.PromptText} ")) {
                    var len = _consoleTxt.PromptText.Length + 2;
                    _consoleTxt.Select(_consoleTxt.Text.Length - len, len);
                    _consoleTxt.SelectionProtected = false;
                    _consoleTxt.SelectedText = "";
                }
                _consoleTxt.ShowPrompt();
            };
        }

        private bool Execute(string sql) {
            try {
                ExecuteCore(sql);
                _manager.SetDirty();
                _manager.Rescan();
                return true;
            } catch (Exception ex) {
                var td = new TaskDialog {
                    Cancelable = false,
                    Caption = "Console Error",
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
            ScriptOutput output = null;
            Exception exception = null;
            var f = new WaitForm("SQL Query", "Running your SQL query...", () => {
                try {
                    output = _manager.ExecuteScript(sql);
                } catch (Exception ex) {
                    exception = ex;
                }
            });
            using (f) {
                if (f.ShowDialog(ParentForm, 25) != DialogResult.OK) {
                    return;
                }
                if (exception != null) {
                    throw exception;
                } else {
                    using (output) {
                        _consoleTxt.BeginUpdate();
                        try {
                            foreach (var line in output.TextOutput) {
                                _consoleTxt.Append($"\n{line}");
                            }

                            foreach (var dt in output.DataTables) {
                                PrintDataTable(dt);
                            }

                            if (output.ScalarResult != null) {
                                _consoleTxt.Append($"\nReturned: {output.ScalarResult}");
                            }

                            _consoleTxt.Append("\n");
                        } finally {
                            _consoleTxt.SelectionStart = _consoleTxt.Text.Length;
                            _consoleTxt.ScrollToCaret();
                            _consoleTxt.EndUpdate();
                        }
                    }
                }
            }
        }

        private void PrintDataTable(DataTable dt) {
            if (dt == null || dt.Columns.Count == 0) {
                if (dt != null) {
                    dt.Dispose();
                }
            } else {
                _consoleTxt.Append("\n");
                var columnWidths =
                    from colIndex in Enumerable.Range(0, dt.Columns.Count)
                    let maxLength =
                        dt.Rows.Cast<DataRow>()
                        .Select(x => x[colIndex].ToString().Length)
                        .Concat(new[] { dt.Columns[colIndex].ColumnName.Length })
                        .Max()
                    select new { ColIndex = colIndex, MaxLength = maxLength };
                var paddedHeaders =
                    (from colIndex in Enumerable.Range(0, dt.Columns.Count)
                        join x in columnWidths on colIndex equals x.ColIndex
                        select dt.Columns[colIndex].ColumnName.PadRight(x.MaxLength))
                    .ToList();
                _consoleTxt.AppendText(" ");
                for (int i = 0; i < dt.Columns.Count; i++) {
                    if (i > 0) {
                        _consoleTxt.Append(" | ");
                    }
                    _consoleTxt.Append(paddedHeaders[i], _headerFont);
                }
                _consoleTxt.Append("\n");
                var sb = new StringBuilder();
                foreach (var row in dt.Rows.Cast<DataRow>().Take(1000)) {
                    var paddedValues =
                        (from colIndex in Enumerable.Range(0, dt.Columns.Count)
                            join x in columnWidths on colIndex equals x.ColIndex
                            select row.ItemArray[colIndex].ToString().Replace("\r", "\\r").Replace("\n", "\\n").PadRight(x.MaxLength))
                        .ToList();
                    sb.Append(" ");
                    for (int i = 0; i < dt.Columns.Count; i++) {
                        if (i > 0) {
                            sb.Append(" | ");
                        }
                        sb.Append(paddedValues[i]);
                    }
                    sb.Append(" \n");
                }
                _consoleTxt.AppendText(sb.ToString());
                _consoleTxt.Append($"({dt.Rows.Count} row{(dt.Rows.Count == 1 ? "" : "s")}{(dt.Rows.Count <= 1000 ? "" : ", 1000 shown")})", fg: Color.LightGray);
            }
        }
    }
}
