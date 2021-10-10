using System;
using System.Diagnostics;
using System.Windows.Forms;
using SqlNotebook.Properties;

namespace SqlNotebook {
    public partial class AboutForm : Form {
        public AboutForm() {
            InitializeComponent();

            Ui ui = new(this, 100, 35);
            ui.Init(_table);
            ui.Init(_linkFlow);
            ui.Pad(_linkFlow);
            ui.MarginTop(_browserPanel);
            ui.Init(_buttonFlow);
            ui.MarginTop(_buttonFlow);
            ui.Init(_okBtn);

            _browser.DocumentText = Resources.ThirdPartyLicensesHtml;
            Text += $" {Application.ProductVersion}";
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
    }
}
