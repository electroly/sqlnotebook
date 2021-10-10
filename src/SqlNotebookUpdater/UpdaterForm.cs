using System;
using System.Diagnostics;
using System.Drawing;
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

            using var g = CreateGraphics();
            var x = g.MeasureString("x", Font, PointF.Empty, StringFormat.GenericTypographic);
            Size = new((int)(x.Width * 80), (int)(x.Height * 12));
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
            _progressBar.Style = ProgressBarStyle.Marquee;
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
                    _progressBar.Visible = false;
                    _label.Text = "Download failed.";
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
            _progressBar.Style = ProgressBarStyle.Continuous;
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
                        _progressBar.Visible = false;
                        _label.Text = "Download failed.";
                        MessageBox.Show(this, $"There was a problem downloading the update.\r\n{e.Error.Message}",
                            "SQL Notebook Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else if (!e.Cancelled) {
                    _progressBar.Visible = false;
                    _label.Text = "Running installer...";
                    Process.Start(new ProcessStartInfo(msiFilePath) { UseShellExecute = true });
                }
                if (!IsDisposed) {
                    Close();
                }
            };
            await _client.DownloadFileTaskAsync(new Uri(_msiUrl), msiFilePath);

            await Task.Delay(2000);
        }
    }
}
