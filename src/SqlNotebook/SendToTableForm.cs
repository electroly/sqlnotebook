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
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class SendToTableForm : Form {
        private readonly HashSet<string> _existingNames; // lowercase

        public string SelectedName { get; private set; }

        public SendToTableForm(string defaultName, IReadOnlyList<string> existingNames) {
            InitializeComponent();
            _existingNames = new HashSet<string>(existingNames.Select(x => x.ToLowerInvariant()));
            _existingNames.Add("sqlite_master");

            var prefix = defaultName;
            var suffix = 2;
            while (existingNames.Contains(defaultName.ToLowerInvariant())) {
                defaultName = $"{prefix}{suffix}";
                suffix++;
            }

            _nameTxt.Text = defaultName;
            ValidateInput();

            Ui ui = new(this);
            ui.Init(_table);
            ui.Pad(_picturePanel);
            ui.Init(_nameLabel);
            ui.Init(_nameTxt, 35);
            ui.Init(_buttonFlow);
            ui.MarginTop(_buttonFlow);
            ui.Init(_okBtn);
            ui.Init(_cancelBtn);
        }

        private void OkBtn_Click(object sender, EventArgs e) {
            SelectedName = _nameTxt.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void NameTxt_TextChanged(object sender, EventArgs e) {
            ValidateInput();
        }

        private void ValidateInput() {
            bool error = true;
            if (string.IsNullOrWhiteSpace(_nameTxt.Text)) {
                _errorProvider.SetError(_nameTxt, "Please enter a table name.");
            } else if (_existingNames.Contains(_nameTxt.Text.ToLowerInvariant())) {
                _errorProvider.SetError(_nameTxt, "This name is already in use.");
            } else {
                _errorProvider.SetError(_nameTxt, null);
                error = false;
            }

            _okBtn.Enabled = !error;
        }
    }
}
