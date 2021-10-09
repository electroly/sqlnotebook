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
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class SqlTextControl : UserControl {
        private readonly TextBox _text;

        public TextBox TextBox => _text;

        public string SqlText {
            get {
                return _text.Text;
            }
            set {
                _text.Text = value;
            }
        }

        public event EventHandler SqlTextChanged;

        public SqlTextControl(bool readOnly) {
            InitializeComponent();

            _text = new TextBox {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                ReadOnly = readOnly,
                Multiline = true,
            };
            _text.TextChanged += (sender, e) => SqlTextChanged?.Invoke(this, EventArgs.Empty);
            _contextMenuStrip.SetMenuAppearance();
            _text.ContextMenuStrip = _contextMenuStrip;
            _pasteMnu.Visible = _cutMnu.Visible = !readOnly;

            Controls.Add(_text);
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
            _copyMnu.Enabled = _text.SelectedText.Length > 0;
            _cutMnu.Enabled = !_text.ReadOnly && _text.SelectedText.Length > 0;
            _pasteMnu.Enabled = !_text.ReadOnly && Clipboard.ContainsText();
        }

        private void CutMnu_Click(object sender, EventArgs e) {
            _text.Cut();
        }

        private void CopyMnu_Click(object sender, EventArgs e) {
            _text.Copy();
        }

        private void PasteMnu_Click(object sender, EventArgs e) {
            _text.Paste();
        }

        private void SelectAllMnu_Click(object sender, EventArgs e) {
            _text.SelectAll();
        }
    }
}
