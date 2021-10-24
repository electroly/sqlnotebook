using SqlNotebookScript;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class ImportScriptPreviewForm : ZForm {
        public ImportScriptPreviewForm(string sql, SimpleDataTable table) {
            InitializeComponent();

            SqlTextControl textbox = new(true) {
                Dock = DockStyle.Fill
            };
            textbox.SqlText = sql;
            _scriptPanel.Controls.Add(textbox);

            ImportPreviewControl grid = new() { Dock = DockStyle.Fill };
            grid.SetTable(table.ToDataTable(), true);
            _previewPanel.Controls.Add(grid);

            Ui ui = new(this, 150, 50);
            ui.Init(_table);
            ui.Init(_split, 0.6);
            ui.InitHeader(_scriptLabel);
            ui.InitHeader(_previewLabel);
            ui.Init(_buttonFlow);
            ui.MarginTop(_buttonFlow);
            ui.Init(_okButton);
        }
    }
}
