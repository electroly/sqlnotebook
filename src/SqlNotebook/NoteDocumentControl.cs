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
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using mshtml;

namespace SqlNotebook {
    public partial class NoteDocumentControl : UserControl, IDocumentControl {
        private readonly NotebookManager _manager;
        private readonly ChangeSink _sink = new ChangeSink();
        private uint _sinkCookie;

        public string ItemName { get; set; }

        public void Save() {
            var html = _browser.Document.Body.InnerHtml;
            _manager.SetItemData(ItemName, html);
        }

        public NoteDocumentControl(string name, NotebookManager manager) {
            InitializeComponent();
            ItemName = name;
            _manager = manager;
            _toolStrip.Renderer = new MenuRenderer();
            _contextMenuStrip.Renderer = new MenuRenderer();
            _browser.PreviewKeyDown += (sender, e) => {
                if (e.KeyData == (Keys.Control | Keys.C)) {
                    EnableDisableContextMenu();
                    _copyMnu.PerformClick();
                } else if (e.KeyData == (Keys.Control | Keys.X)) {
                    EnableDisableContextMenu();
                    _cutMnu.PerformClick();
                } else if (e.KeyData == (Keys.Control | Keys.V)) {
                    EnableDisableContextMenu();
                    _pasteMnu.PerformClick();
                } else if (e.KeyData == (Keys.Control | Keys.A)) {
                    _selectAllMnu.PerformClick();
                } else if (e.KeyData == (Keys.Control | Keys.B)) {
                    _boldBtn.PerformClick();
                } else if (e.KeyData == (Keys.Control | Keys.I)) {
                    _italicBtn.PerformClick();
                } else if (e.KeyData == (Keys.Control | Keys.U)) {
                    _underlineBtn.PerformClick();
                } else if (e.KeyData == (Keys.Control | Keys.L)) {
                    _alignLeftBtn.PerformClick();
                } else if (e.KeyData == (Keys.Control | Keys.E)) {
                    _alignCenterBtn.PerformClick();
                } else if (e.KeyData == (Keys.Control | Keys.R)) {
                    _alignRightBtn.PerformClick();
                } else if (e.KeyData == Keys.Delete) {
                    _browser.Document.ExecCommand("delete", false, null);
                } else {
                    manager.HandleAppHotkeys(e.KeyData);
                }
            };

            _browser.DocumentText = "<html><body></body></html>";
            ((IHTMLDocument2)_browser.Document.DomDocument).designMode = "On";

            var doc = (IHTMLDocument2)_browser.Document.DomDocument;
            doc.write(_manager.GetItemData(ItemName));

            var markupContainer = (IMarkupContainer2)doc;
            markupContainer.RegisterForDirtyRange(_sink, out _sinkCookie);

            _sink.Change += (sender2, e2) => _manager.SetDirty();
            _timer.Start();

            Load += (sender, e) => { _browser.Focus(); _browser.Select(); };
        }

        private sealed class ChangeSink : IHTMLChangeSink {
            public event EventHandler Change;
            public void Notify() => Change?.Invoke(this, EventArgs.Empty);
        }

        private void BoldBtn_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("bold", false, null);
        }

        private void ItalicBtn_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("italic", false, null);
        }

        private void UnderlineBtn_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("underline", false, null);
        }

        private void AlignLeftBtn_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("justifyLeft", false, null);
        }

        private void AlignCenterBtn_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("justifyCenter", false, null);
        }

        private void AlignRightBtn_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("justifyRight", false, null);
        }

        private void OutdentBtn_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("outdent", false, null);
        }

        private void IndentBtn_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("indent", false, null);
        }

        private void BulletListBtn_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("insertUnorderedList", false, null);
        }

        private void NumberListBtn_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("insertOrderedList", false, null);
        }

        private void TableBtn_Click(object sender, EventArgs e) {
            using (var f = new NoteNewTableForm()) {
                if (f.ShowDialog(TopLevelControl) != DialogResult.OK) {
                    return;
                }

                // inline the CSS so it gets included when the user copies tables to the clipboard
                var cellCss = "border: 1px solid rgb(229, 229, 229); padding: 3px; padding-left: 6px; " +
                    "padding-right: 6px; text-align: left; vertical-align: top;";
                var cellHtml = $"<td style=\"{cellCss}\"></td>";
                var rowHtml = "<tr>" + string.Join("", Enumerable.Range(0, f.SelectedColumns).Select(x => cellHtml)) + "</tr>";
                var tableHtml = string.Join("", Enumerable.Range(0, f.SelectedRows).Select(x => rowHtml));
                var html = $"<table style=\"border-collapse: collapse;\">{tableHtml}</table>";

                var doc = (IHTMLDocument2)_browser.Document.DomDocument;
                var range = doc.selection?.createRange() as IHTMLTxtRange;
                if (range != null) {
                    range.pasteHTML(html);
                }
            }
        }

        private void HrBtn_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("insertHorizontalRule", false, null);
        }

        private bool _ignoreChange = false; // true when Timer_Tick is updating the toolbar
        private void Timer_Tick(object sender, EventArgs e) {
            if (_fontNameCmb.Focused || _fontNameCmb.DroppedDown || _fontSizeCmb.Focused || _fontSizeCmb.DroppedDown) {
                return;
            }
            _ignoreChange = true;
            var doc = (IHTMLDocument2)_browser.Document.DomDocument;
            _fontNameCmb.Text = doc.queryCommandValue("fontName")?.ToString() ?? "";
            _fontSizeCmb.Text = HtmlFontSizeToPointSize(doc.queryCommandValue("fontSize")?.ToString());
            _boldBtn.Checked = doc.queryCommandState("bold");
            _italicBtn.Checked = doc.queryCommandState("italic");
            _underlineBtn.Checked = doc.queryCommandState("underline");
            _alignLeftBtn.Checked = doc.queryCommandState("justifyLeft");
            _alignCenterBtn.Checked = doc.queryCommandState("justifyCenter");
            _alignRightBtn.Checked = doc.queryCommandState("justifyRight");
            _bulletListBtn.Checked = doc.queryCommandState("insertUnorderedList");
            _numberListBtn.Checked = doc.queryCommandState("insertOrderedList");
            _ignoreChange = false;
        }

        private static string HtmlFontSizeToPointSize(string value) {
            int htmlValue;
            if (int.TryParse(value ?? "", out htmlValue)) {
                switch (htmlValue) {
                    case 1: return "8"; // 7.5pt
                    case 2: return "10"; // 9.75pt
                    case 3: return "12";
                    case 4: return "14"; // 13.5pt
                    case 5: return "18";
                    case 6: return "24";
                    case 7: return "36";
                }
            }
            return "";
        }

        private static string PointSizeToHtmlFontSize(int value) {
            if (value <= 8) {
                return "1";
            } else if (value <= 10) {
                return "2";
            } else if (value <= 12) {
                return "3";
            } else if (value <= 14) {
                return "4";
            } else if (value <= 18) {
                return "5";
            } else if (value <= 24) {
                return "6";
            } else {
                return "8";
            }
        }

        private void FontNameCmb_TextChanged(object sender, EventArgs e) {
            if (_ignoreChange) {
                return;
            }
            _browser.Document.ExecCommand("fontName", false, _fontNameCmb.Text);
        }

        private void FontSizeCmb_TextChanged(object sender, EventArgs e) {
            if (_ignoreChange) {
                return;
            }
            int value;
            if (int.TryParse(_fontSizeCmb.Text, out value)) {
                _browser.Document.ExecCommand("fontSize", false, PointSizeToHtmlFontSize(value));
            }
        }

        private void CutMnu_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("cut", false, null);
        }

        private void CopyMnu_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("copy", false, null);
        }

        private void PasteMnu_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("paste", false, null);
        }

        private void SelectAllMnu_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("selectAll", false, null);
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
            EnableDisableContextMenu();
        }

        private void EnableDisableContextMenu() {
            var doc = (IHTMLDocument2)_browser.Document.DomDocument;
            _cutMnu.Enabled = doc.queryCommandEnabled("cut");
            _copyMnu.Enabled = doc.queryCommandEnabled("copy");
            _pasteMnu.Enabled = doc.queryCommandEnabled("paste");
        }
    }
}
