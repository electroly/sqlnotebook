using System.Windows.Forms;
using SqlNotebookScript;
using SqlNotebookScript.DataTables;

namespace SqlNotebook.Import;

public partial class ImportScriptPreviewForm : ZForm
{
    public ImportScriptPreviewForm(string sql, SimpleDataTable table)
    {
        InitializeComponent();

        SqlTextControl textbox = new(true) { Dock = DockStyle.Fill };
        textbox.SqlText = sql;
        _scriptPanel.Controls.Add(textbox);

        ImportPreviewControl grid = new(table.ToDataTable()) { Dock = DockStyle.Fill };
        _previewPanel.Controls.Add(grid);

        Ui ui = new(this, 150, 40);
        ui.Init(_table);
        ui.Init(_split, 0.5);
        ui.InitHeader(_scriptLabel);
        ui.InitHeader(_previewLabel);
        ui.Init(_buttonFlow);
        ui.MarginTop(_buttonFlow);
        ui.Init(_okButton);
    }
}
