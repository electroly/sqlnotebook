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
using System.Drawing.Text;
using System.Diagnostics;

namespace SqlNotebook {
    public partial class NoteDocumentControl : UserControl {
        private readonly NotebookManager _manager;

        public string RtfText {
            get {
                return _text.Rtf;
            }
        }

        public string ItemName { get; set; }

        public NoteDocumentControl(string name, NotebookManager manager) {
            InitializeComponent();
            ItemName = name;
            _manager = manager;
            _text.SelectionFont = Font;
            _text.SetInnerMargins(10, 10, 10, 0);

            Load += (sender, e) => {
                string initialRtf = _manager.GetItemData(ItemName);
                if (initialRtf != null) {
                    _text.Rtf = initialRtf;
                }
                Text_SelectionChanged(this, EventArgs.Empty);
            };
        }

        private void FontMnu_Click(object sender, EventArgs e) {
            var f = new FontDialog {
                Font = _text.SelectionFont,
                Color = _text.SelectionColor,
                FontMustExist = true,
                ShowApply = true,
                ShowColor = true
            };
            f.Apply += (sender2, e2) => {
                _text.SelectionFont = f.Font ?? _text.SelectionFont ?? _text.Font;
                _text.SelectionColor = f.Color;
            };
            using (f) {
                if (f.ShowDialog(ParentForm) == DialogResult.OK) {
                    _text.SelectionFont = f.Font ?? _text.SelectionFont ?? _text.Font;
                    _text.SelectionColor = f.Color;
                }
            }
        }

        private void ResetFontMnu_Click(object sender, EventArgs e) {
            _text.SelectionFont = Font;
            _text.SelectionColor = ForeColor;
        }

        private void Text_TextChanged(object sender, EventArgs e) {
            _text.BeginUpdate();
            int oldStart = _text.SelectionStart;
            int oldLength = _text.SelectionLength;
            _text.SelectAll();
            _text.SelectionProtected = false;
            _text.Select(oldStart, oldLength);
            _text.EndUpdate();
        }

        private void Text_SelectionChanged(object sender, EventArgs e) {
            UpdateToolbar();
        }

        private void UpdateToolbar() {
            var f = _text.SelectionFont ?? _text.Font;
            _boldBtn.Checked = f.Bold;
            _italicBtn.Checked = f.Italic;
            _underlineBtn.Checked = f.Underline;
            _strikeBtn.Checked = f.Strikeout;
            _alignLeftBtn.Checked = _text.SelectionAlignment == HorizontalAlignment.Left;
            _alignCenterBtn.Checked = _text.SelectionAlignment == HorizontalAlignment.Center;
            _alignRightBtn.Checked = _text.SelectionAlignment == HorizontalAlignment.Right;
        }

        private void FontSizeUpBtn_Click(object sender, EventArgs e) {
            var f = _text.SelectionFont;
            _text.SelectionFont = new Font(f.FontFamily, f.Size + 1, f.Style, f.Unit, f.GdiCharSet, f.GdiVerticalFont);
        }

        private void FontSizeDownBtn_Click(object sender, EventArgs e) {
            var f = _text.SelectionFont;
            _text.SelectionFont = new Font(f.FontFamily, Math.Max(8, f.Size - 1), f.Style, f.Unit, f.GdiCharSet, f.GdiVerticalFont);
        }

        private void ToggleFontStyle(FontStyle flag) {
            var f = _text.SelectionFont;
            var s = f.Style & ~flag;
            if ((f.Style & flag) == 0) {
                s |= flag;
            }
            _text.SelectionFont = new Font(f.FontFamily, f.Size, s, f.Unit, f.GdiCharSet, f.GdiVerticalFont);
            UpdateToolbar();
        }

        private void BoldBtn_Click(object sender, EventArgs e) {
            ToggleFontStyle(FontStyle.Bold);
        }

        private void ItalicBtn_Click(object sender, EventArgs e) {
            ToggleFontStyle(FontStyle.Italic);
        }

        private void UnderlineBtn_Click(object sender, EventArgs e) {
            ToggleFontStyle(FontStyle.Underline);
        }

        private void StrikeBtn_Click(object sender, EventArgs e) {
            ToggleFontStyle(FontStyle.Strikeout);
        }

        private void AlignLeftBtn_Click(object sender, EventArgs e) {
            _text.SelectionAlignment = HorizontalAlignment.Left;
            UpdateToolbar();
        }

        private void AlignCenterBtn_Click(object sender, EventArgs e) {
            _text.SelectionAlignment = HorizontalAlignment.Center;
            UpdateToolbar();
        }

        private void AlignRightBtn_Click(object sender, EventArgs e) {
            _text.SelectionAlignment = HorizontalAlignment.Right;
            UpdateToolbar();
        }

        private void Text_LinkClicked(object sender, LinkClickedEventArgs e) {
            if (e.LinkText.StartsWith("http://") || e.LinkText.StartsWith("https://")) {
                Process.Start(e.LinkText);
            }
        }
    }
}
