using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class ImportConnectionForm : Form {
        private int _collapsedHeight;
        private int _expandedHeight;
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
            _getBasicOptions = getBasicOptions;
            _setBasicOptions = setBasicOptions;
            _builder = builder;

            InitializeComponent();
            if (!(builder is SqlConnectionStringBuilder)) {
                _windowsAuthChk.Visible = false;
                _basicPnl.Height -= _windowsAuthChk.Height + 5;
                Height -= _windowsAuthChk.Height + 5;
            }
            _collapsedHeight = Height;
            _expandedHeight = Height + 200;
            _propertyGrid.SelectedObject = builder;
            Text = title;

            UpdateBasicOptionsUi(_getBasicOptions(builder)); // populate basic options with info from builder
        }

        private void ToggleBtn_Click(object sender, EventArgs e) {
            ToggleAdvanced();
        }

        private void ToggleAdvanced() {
            if (_propertyGrid.Visible) {
                // advanced -> basic
                UpdateBasicOptionsUi(_getBasicOptions(_builder));
                _propertyGrid.Visible = false;
                Size = new Size(Width, _collapsedHeight);
                _basicPnl.Visible = true;
                _toggleBtn.Text = "Show advanced options";
            } else {
                // basic -> advanced
                _setBasicOptions(_builder, ReadBasicOptionsUi());
                _basicPnl.Visible = false;
                Size = new Size(Width, _expandedHeight);
                _propertyGrid.Visible = true;
                _toggleBtn.Text = "Show basic options";
                _propertyGrid.Refresh();
            }
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
            if (!_propertyGrid.Visible) {
                _setBasicOptions(_builder, ReadBasicOptionsUi());
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void WindowsAuthChk_CheckedChanged(object sender, EventArgs e) {
            _usernameTxt.Enabled = _passwordTxt.Enabled = !_windowsAuthChk.Checked;
        }
    }
}
