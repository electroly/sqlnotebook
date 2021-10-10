using System;
using System.Windows.Forms;
using SqlNotebook.Properties;

namespace SqlNotebook {
    public partial class OptionsForm : Form {
        public OptionsForm() {
            InitializeComponent();

            Ui ui = new(this);
            ui.Init(_table);
            ui.Init(_helpExternalBrowserChk);
            ui.Init(_buttonFlow);
            ui.MarginTop(_buttonFlow);
            ui.Init(_okBtn);
            ui.Init(_cancelBtn);

            _helpExternalBrowserChk.Checked = Settings.Default.UseExternalHelpBrowser;

        }

        private void OkBtn_Click(object sender, EventArgs e) {
            Settings.Default.UseExternalHelpBrowser = _helpExternalBrowserChk.Checked;

            Settings.Default.Save();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
