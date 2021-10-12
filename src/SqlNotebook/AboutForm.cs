using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class AboutForm : Form {
        public AboutForm() {
            InitializeComponent();

            Ui ui = new(this);
            ui.Init(_table);
            ui.Init(_linkFlow);
            ui.Init(_titleLabel);
            ui.Init(_versionLabel);
            ui.Init(_copyrightLabel);
            ui.Init(_websiteLnk);
            ui.MarginTop(_websiteLnk);
            ui.Init(_githubLnk);
            ui.MarginTop(_githubLnk);
            ui.Init(_licenseLnk);
            ui.MarginTop(_licenseLnk);
            ui.MarginTop(_websiteLnk);
            ui.Init(_buttonFlow);
            ui.MarginTop(_buttonFlow, 1.5);
            ui.Init(_okBtn);

            _versionLabel.Text += Application.ProductVersion;

            _okBtn.Select();
        }

        private void OkBtn_Click(object sender, EventArgs e) {
            Close();
        }

        private void GithubLnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start(new ProcessStartInfo("https://github.com/electroly/sqlnotebook") { UseShellExecute = true });
        }

        private void WebsiteLnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start(new ProcessStartInfo("https://sqlnotebook.com/") { UseShellExecute = true });
        }

        private void LicenseLnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            var filePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Resources", "ThirdPartyLicenses.html");
            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }
    }
}
