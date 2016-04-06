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
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class ExportForm : Form {
        public string ScriptName { get; private set; }

        public ExportForm(IEnumerable<string> scripts) {
            InitializeComponent();
            foreach (var name in scripts) {
                _list.Items.Add(name).ImageIndex = 0;
            }
            _list.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void OpenBtn_Click(object sender, EventArgs e) {
            if (_list.SelectedIndices.Count == 1) {
                ScriptName = _list.SelectedItems[0].Text;
                DialogResult = DialogResult.Yes;
                Close();
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e) {
            if (_list.SelectedIndices.Count == 1) {
                ScriptName = _list.SelectedItems[0].Text;
                DialogResult = DialogResult.No;
                Close();
            }
        }

        private void List_SelectedIndexChanged(object sender, EventArgs e) {
            _openBtn.Enabled = _saveBtn.Enabled = _list.SelectedIndices.Count == 1;
        }
    }
}
