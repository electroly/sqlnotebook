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
using SqlNotebook.Properties;

namespace SqlNotebook {
    public partial class OptionsForm : Form {
        public OptionsForm() {
            InitializeComponent();

            var autoCreate = Settings.Default.AutoCreateInNewNotebooks; // 0=nothing, 1=note, 2=console, 3=script
            if (autoCreate < 1 || autoCreate > 3) {
                _autoCreateChk.Checked = false;
                _autoCreateCmb.Enabled = false;
                _autoCreateCmb.SelectedIndex = 0;
            } else {
                _autoCreateChk.Checked = true;
                _autoCreateCmb.SelectedIndex = autoCreate - 1;
            }

            _helpExternalBrowserChk.Checked = Settings.Default.UseExternalHelpBrowser;
        }

        private void OkBtn_Click(object sender, EventArgs e) {
            if (_autoCreateChk.Checked) {
                Settings.Default.AutoCreateInNewNotebooks = _autoCreateCmb.SelectedIndex + 1;
            } else {
                Settings.Default.AutoCreateInNewNotebooks = 0;
            }
            Settings.Default.UseExternalHelpBrowser = _helpExternalBrowserChk.Checked;

            Settings.Default.Save();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void AutoCreateChk_CheckedChanged(object sender, EventArgs e) {
            _autoCreateCmb.Enabled = _autoCreateChk.Checked;
        }
    }
}
