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
using SqlNotebookCore;
using Microsoft.WindowsAPICodePack.Dialogs;
using SqlNotebookScript;
using System.Runtime.InteropServices;

namespace SqlNotebook {
    public partial class ConsoleDocumentControl : UserControl, IDocumentControl {
        private readonly IWin32Window _mainForm;
        private readonly NotebookManager _manager;
        private readonly Notebook _notebook;
        private readonly ConsoleRichTextBox _consoleTxt;
        private readonly Font _promptFont = new Font("Consolas", 10, FontStyle.Bold);
        private readonly Font _font = new Font("Consolas", 10);
        private readonly Font _headerFont = new Font("Consolas", 10, FontStyle.Italic);
        private readonly Font _dividerFont = new Font("Arial", 10);
        private readonly Slot<bool> _operationInProgress;

        public string DocumentText {
            get {
                return _consoleTxt.Rtf;
            }
        }

        public string ItemName { get; set; }

        public void Save() {
            _manager.SetItemData(ItemName, DocumentText);
            _manager.SetConsoleHistory(ItemName, _consoleTxt.History);
        }

        public ConsoleDocumentControl(string name, NotebookManager manager, IWin32Window mainForm, Slot<bool> operationInProgress) {
            InitializeComponent();
            ItemName = name;
            _manager = manager;
            _notebook = manager.Notebook;
            _mainForm = mainForm;
            _operationInProgress = operationInProgress;
            _consoleTxt = new ConsoleRichTextBox {
                Dock = DockStyle.Fill,
                BackColor = SystemColors.Window,
                BorderStyle = BorderStyle.None,
                AutoWordSelection = true,
                Font = _font,
                Multiline = true,
                ScrollBars = RichTextBoxScrollBars.Both,
                WordWrap = false,
                PromptFont = _promptFont,
                PromptText = ">",
                PromptColor = Color.Red
            };
            Controls.Add(_consoleTxt);
            _consoleTxt.InstallCopyPasteHandling(allowRtfPaste: false);
            _consoleTxt.ConsoleCommand += ConsoleTxt_ConsoleCommand;

            Load += (sender, e) => {
                string initialRtf = _manager.GetItemData(ItemName);
                if (initialRtf != null) {
                    _consoleTxt.Rtf = initialRtf;
                    _consoleTxt.History = _manager.GetConsoleHistory(ItemName);
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

        private async void ConsoleTxt_ConsoleCommand(object sender, ConsoleCommandEventArgs e) {
            e.OnComplete(await Execute(e.Command));
        }

        private async Task<bool> Execute(string sql) {
            try {
                if (_operationInProgress) {
                    throw new Exception("Another operation is already in progress.");
                }
                _consoleTxt.ReadOnly = true;

                await ExecuteCore(sql);
                _manager.SetDirty();
                _manager.Rescan();
                return true;
            } catch (Exception ex) {
                MessageDialog.ShowError(ParentForm, "Console Error", "An error occurred.", ex.Message);
                return false;
            } finally {
                _consoleTxt.ReadOnly = false;
            }
        }

        private async Task ExecuteCore(string sql) {
            if (sql.Trim().ToLower() == "clear") {
                _consoleTxt.Text = "";
                _consoleTxt.Append(" ");
                return;
            }

            ScriptOutput output = null;
            Exception exception = null;
            _manager.PushStatus("Running your console command...");
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
                    _consoleTxt.EndUpdate();
                }
            }
        }

        private void PrintDataTable(SimpleDataTable dt) {
            const int MAX_ROWS = 100;
            if (dt != null && dt.Columns.Count > 0) {
                _consoleTxt.Append("\n");
                var columnWidths =
                    from colIndex in Enumerable.Range(0, dt.Columns.Count)
                    let maxLength =
                        dt.Rows
                        .Take(MAX_ROWS)
                        .Select(x => Math.Min(200, x[colIndex].ToString().Length))
                        .Concat(new[] { dt.Columns[colIndex].Length })
                        .Max()
                    select new { ColIndex = colIndex, MaxLength = maxLength };
                var paddedHeaders =
                    (from colIndex in Enumerable.Range(0, dt.Columns.Count)
                    join x in columnWidths on colIndex equals x.ColIndex
                    select dt.Columns[colIndex].PadRight(x.MaxLength))
                    .ToList();
                _consoleTxt.Append(" ", bg: Color.WhiteSmoke);
                for (int i = 0; i < dt.Columns.Count; i++) {
                    if (i > 0) {
                        _consoleTxt.Append(" │ ", bg: Color.WhiteSmoke);
                    }
                    _consoleTxt.Append(paddedHeaders[i], _headerFont, bg: Color.WhiteSmoke);
                }
                _consoleTxt.Append("\n");
                var sb = new StringBuilder();
                foreach (var row in dt.Rows.Take(MAX_ROWS)) {
                    var paddedValues =
                        (from colIndex in Enumerable.Range(0, dt.Columns.Count)
                        join x in columnWidths on colIndex equals x.ColIndex
                        select Truncate(row[colIndex].ToString().Replace("\r\n", "¶").Replace("\r", "¶").Replace("\n", "¶").PadRight(x.MaxLength), 200))
                        .ToList();
                    sb.Append(" ");
                    for (int i = 0; i < dt.Columns.Count; i++) {
                        if (i > 0) {
                            sb.Append(" │ ");
                        }
                        sb.Append(paddedValues[i]);
                    }
                    sb.Append(" \n");
                }
                _consoleTxt.AppendText(sb.ToString());
                _consoleTxt.Append($"({dt.Rows.Count} row{(dt.Rows.Count == 1 ? "" : "s")}{(dt.Rows.Count <= MAX_ROWS ? "" : $", {MAX_ROWS} shown")})", fg: Color.LightGray);
            }
        }

        private string Truncate(string str, int maxLen) {
            if (str.Length > maxLen) {
                return str.Substring(0, maxLen - 1) + "…";
            } else {
                return str;
            }
        }
    }
}
