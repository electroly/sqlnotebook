using System.Windows.Forms;

namespace SqlNotebook;

public partial class ViewFullTextForm : ZForm {
    private readonly SqlTextControl _textControl;

    public ViewFullTextForm(string text) {
        InitializeComponent();

        _textControl = new(true, syntaxColoring: false) {
            Dock = DockStyle.Fill,
            SqlText = text,
            BorderStyle = BorderStyle.FixedSingle,
        };
        _textPanel.Controls.Add(_textControl);

        Ui ui = new(this, 150, 30);
        ui.Init(_table);
        ui.Init(_textPanel);
        ui.Init(_buttonFlow1);
        ui.MarginTop(_buttonFlow1);
        ui.Init(_copyButton);
        ui.Init(_buttonFlow2);
        ui.MarginTop(_buttonFlow2);
        ui.Init(_closeButton);
    }

    private void CopyButton_Click(object sender, System.EventArgs e) {
        Clipboard.SetText(_textControl.SqlText);
    }
}
