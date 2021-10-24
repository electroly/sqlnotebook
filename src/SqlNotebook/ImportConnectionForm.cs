using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class ImportConnectionForm : ZForm {
        private Func<DbConnectionStringBuilder, BasicOptions> _getBasicOptions;
        private Action<DbConnectionStringBuilder, BasicOptions> _setBasicOptions;
        private DbConnectionStringBuilder _builder;

        public sealed class BasicOptions {
            public string Server { get; set; } = "";
            public string Database { get; set; } = "";
            public string Username { get; set; } = "";
            public string Password { get; set; } = "";
            public bool UseWindowsAuth { get; set; }
        }

        public ImportConnectionForm(string title, DbConnectionStringBuilder builder,
            Func<DbConnectionStringBuilder, BasicOptions> getBasicOptions,
            Action<DbConnectionStringBuilder, BasicOptions> setBasicOptions) {
            InitializeComponent();

            _getBasicOptions = getBasicOptions;
            _setBasicOptions = setBasicOptions;
            _builder = builder;

            Ui ui = new(this, 75, 30);
            ui.Init(_table);
            ui.Init(_tabs);
            ui.Init(_basicTable);
            ui.Pad(_basicTable);
            ui.Init(_addressLabel);
            ui.Init(_serverTxt);
            ui.Init(_databaseLabel);
            ui.MarginTop(_databaseLabel);
            ui.Init(_databaseTxt);
            ui.Init(_usernameLabel);
            ui.MarginTop(_usernameLabel);
            ui.Init(_usernameTxt);
            ui.Init(_passwordLabel);
            ui.MarginTop(_passwordLabel);
            ui.Init(_passwordTxt);
            ui.Init(_windowsAuthChk);
            ui.MarginTop(_windowsAuthChk);
            ui.Init(_propertyGrid);
            ui.Init(_buttonFlow);
            ui.MarginTop(_buttonFlow);
            ui.Init(_okBtn);
            ui.Init(_cancelBtn);

            if (!(builder is SqlConnectionStringBuilder)) {
                _windowsAuthChk.Visible = false;
            }
            _propertyGrid.SelectedObject = builder;
            Text = title;

            UpdateBasicOptionsUi(_getBasicOptions(builder)); // populate basic options with info from builder

            Load += delegate { _serverTxt.Select(); };
        }

        private void UpdateBasicOptionsUi(BasicOptions opt) {
            _serverTxt.Text = opt.Server;
            _databaseTxt.Text = opt.Database;
            _usernameTxt.Text = opt.Username;
            _passwordTxt.Text = opt.Password;
            _windowsAuthChk.Checked = opt.UseWindowsAuth;
        }

        private BasicOptions ReadBasicOptionsUi() {
            return new BasicOptions {
                Server = _serverTxt.Text,
                Database = _databaseTxt.Text,
                Username = _usernameTxt.Text,
                Password = _passwordTxt.Text,
                UseWindowsAuth = _windowsAuthChk.Checked
            };
        }

        private void OkBtn_Click(object sender, EventArgs e) {
            if (_tabs.SelectedIndex == 0) {
                // "Basic" tab is selected.
                _setBasicOptions(_builder, ReadBasicOptionsUi());
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void WindowsAuthChk_CheckedChanged(object sender, EventArgs e) {
            _usernameTxt.Enabled = _passwordTxt.Enabled = !_windowsAuthChk.Checked;
        }

        private void Tabs_TabIndexChanged(object sender, EventArgs e) {
            if (_tabs.SelectedIndex == 0) {
                // "Basic" tab is now selected.
                UpdateBasicOptionsUi(_getBasicOptions(_builder));
            } else {
                // "Advanced" tab is now selected.
                _setBasicOptions(_builder, ReadBasicOptionsUi());
                _propertyGrid.SelectedObject = _builder; // Refresh the property grid.
            }
        }
    }
}
