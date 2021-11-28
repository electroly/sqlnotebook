using System;
using System.Windows.Forms;

namespace SqlNotebook.Import.Database;

public partial class DatabaseImportRenameTableForm : ZForm {
    public string NewName { get; private set; }

    public DatabaseImportRenameTableForm(string oldName, string newName) {
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
            Ui.ShowError(this, "Error", "Please enter a new name.");
            return;
        }
        NewName = _newNameTxt.Text;
        DialogResult = DialogResult.OK;
        Close();
    }
}
