using System.Windows.Forms;

namespace SqlNotebook.Import.Database {
    public partial class DatabaseImportScriptForm : ZForm {
        public DatabaseImportScriptForm(string sql) {
            InitializeComponent();

            SqlTextControl textbox = new(true) {
                Dock = DockStyle.Fill
            };
            textbox.SqlText = sql;
            _scriptPanel.BorderStyle = BorderStyle.FixedSingle;
            _scriptPanel.Controls.Add(textbox);

            Ui ui = new(this, 150, 40);
            ui.Init(_table);
            ui.Init(_scriptPanel);
            ui.Init(_buttonFlow);
            ui.MarginTop(_buttonFlow);
            ui.Init(_okButton);
        }
    }
}
