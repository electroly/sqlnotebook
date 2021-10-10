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
    public partial class ImportRenameTableForm : Form {
        public string NewName { get; private set; }

        public ImportRenameTableForm(string oldName, string newName) {
            InitializeComponent();

            Ui ui = new(this);
            ui.Init(_topFlow);
            ui.Init(_oldNameLabel);
            ui.Init(_oldNameTxt, 60);
            ui.Init(_newNameLabel);
            ui.MarginTop(_newNameLabel);
            ui.Init(_newNameTxt, 60);
            ui.Init(_buttonFlow);
            ui.MarginTop(_buttonFlow);
            ui.Init(_okBtn);
            ui.Init(_cancelBtn);

            _oldNameTxt.Text = oldName;
            _newNameTxt.Text = newName;
            _newNameTxt.Select();
        }

        private void OkBtn_Click(object sender, EventArgs e) {
            if (_newNameTxt.Text == "") {
                MessageBox.Show(this, "Please enter a new name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (char ch in _newNameTxt.Text) {
                if (!char.IsLetterOrDigit(ch) && ch != '_') {
                    MessageBox.Show(this, 
                        "The new name must contain only letters, numbers, and underscores.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            NewName = _newNameTxt.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
