using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class SendToTableForm : ZForm {
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
            try {
                ValidateInput();
            } catch (Exception ex) {
                Ui.ShowError(this, "Send to Table", ex);
                return;
            }

            SelectedName = _nameTxt.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void NameTxt_TextChanged(object sender, EventArgs e) {
            ValidateInput();
        }

        private void ValidateInput() {
            if (string.IsNullOrWhiteSpace(_nameTxt.Text)) {
                throw new Exception("Please enter a table name.");
            }
            if (_existingNames.Contains(_nameTxt.Text.ToLowerInvariant())) {
                throw new Exception("This name is already in use.");
            }
        }
    }
}
