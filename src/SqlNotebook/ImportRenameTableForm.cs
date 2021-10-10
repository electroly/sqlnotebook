using System;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class ImportRenameTableForm : Form {
        public string NewName { get; private set; }

        public ImportRenameTableForm(string oldName, string newName) {
            InitializeComponent();

            Ui ui = new(this);
            ui.Init(_topFlow);
            ui.Init(_oldNameLabel);
            ui.Init(_oldNameTxt, 60);
            ui.Init(_newNameLabel);
            ui.MarginTop(_newNameLabel);
            ui.Init(_newNameTxt, 60);
            ui.Init(_buttonFlow);
            ui.MarginTop(_buttonFlow);
            ui.Init(_okBtn);
            ui.Init(_cancelBtn);

            _oldNameTxt.Text = oldName;
            _newNameTxt.Text = newName;
            _newNameTxt.Select();
        }

        private void OkBtn_Click(object sender, EventArgs e) {
            if (_newNameTxt.Text == "") {
                MessageBox.Show(this, "Please enter a new name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (char ch in _newNameTxt.Text) {
                if (!char.IsLetterOrDigit(ch) && ch != '_') {
                    MessageBox.Show(this, 
                        "The new name must contain only letters, numbers, and underscores.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            NewName = _newNameTxt.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
