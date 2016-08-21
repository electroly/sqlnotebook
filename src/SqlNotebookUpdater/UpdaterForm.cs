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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlNotebookUpdater {
    public partial class UpdaterForm : Form {
        private WebClient _client = new WebClient();
        private string _version = "";
        private string _msiUrl = "";
        private string _tempPath = Path.Combine(Path.GetTempPath(), "SqlNotebookTemp");

        public UpdaterForm() {
            InitializeComponent();
        }

        private void CancelBtn_Click(object sender, EventArgs e) {
            _client.CancelAsync();
            Close();
        }

        private async void Timer_Tick(object sender, EventArgs e) {
            if (!Process.GetProcessesByName("SqlNotebook").Any()) {
                // all processes have ended, so begin the update
                _timer.Enabled = false;
                await DoUpdate();
            }
        }

        private async Task DoUpdate() {
            _label.Text = "Downloading version information...";
            try {
                var appversion = await _client.DownloadStringTaskAsync(
                    "https://sqlnotebook.com/appversion.txt?" + DateTime.Now.Date);
                var data = appversion.Split('\n').Select(x => x.Trim()).ToList();
                if (data.Count < 2) {
                    throw new Exception("The appversion.txt data is malformed.");
                }
                _version = data[0];
                _msiUrl = data[1];
            } catch (Exception ex) {
                if (!IsDisposed) {
                    MessageBox.Show(this, $"There was a problem retrieving the update information.\r\n{ex.Message}",
                        "SQL Notebook Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
                return;
            }

            _label.Text = $"Downloading version {_version}...";
            var deleteLstFilePath = Path.Combine(_tempPath, "delete.lst");
            var msiFilename = _msiUrl.Substring(_msiUrl.LastIndexOf('/') + 1);
            var msiFilePath = Path.Combine(_tempPath, msiFilename);
            File.Delete(msiFilePath);
            File.AppendAllText(deleteLstFilePath, $"{msiFilePath}\r\n");
            int lastPercent = 0;
            _client.DownloadProgressChanged += (sender, e) => {
                var percent = e.ProgressPercentage;
                if (lastPercent != percent) {
                    _progressBar.Value = Math.Max(_progressBar.Minimum, Math.Min(_progressBar.Maximum, percent));
                    lastPercent = percent;
                }
            };
            _client.DownloadFileCompleted += (sender, e) => {
                if (e.Error != null) {
                    if (!IsDisposed) {
                        MessageBox.Show(this, $"There was a problem downloading the update.\r\n{e.Error.Message}",
                            "SQL Notebook Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else if (!e.Cancelled) {
                    Process.Start(msiFilePath);
                }
                if (!IsDisposed) {
                    Close();
                }
            };
            _client.DownloadFileAsync(new Uri(_msiUrl), msiFilePath);
        }
    }
}
