using System;
using System.Drawing;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class OptionsForm : ZForm {
        private Font _dataTableFont;
        private Font _codeFont;
        private Color[] _colors;
        private readonly ColorDialog _colorDialog;

        public OptionsForm() {
            InitializeComponent();

            Ui ui = new(this);
            ui.Init(_table);
            ui.Init(_tabs);
            _tabs.Size = new(ui.XWidth(80), ui.XHeight(24));
            ui.Init(_fontsColorsTab);
            ui.Init(_fontsColorsFlow);
            ui.Pad(_fontsColorsFlow);

            ui.Init(_rowFlow1);
            ui.Init(_codeFontFlow);
            ui.MarginRight(_codeFontFlow);
            ui.Init(_codeFontLabel);
            ui.Init(_codeFontButton, 35);
            ui.PadLeft(_codeFontButton, 1);

            ui.Init(_rowFlow2);
            ui.MarginTop(_rowFlow2);
            ui.Init(_editorColorsFlow);
            ui.MarginRight(_editorColorsFlow);
            ui.Init(_editorPlainColorButton, 35);
            ui.PadLeft(_editorPlainColorButton, 6);
            ui.Init(_editorKeywordColorButton, 35);
            ui.PadLeft(_editorKeywordColorButton, 6);
            ui.Init(_editorCommentColorButton, 35);
            ui.PadLeft(_editorCommentColorButton, 6);
            ui.Init(_editorStringColorButton, 35);
            ui.PadLeft(_editorStringColorButton, 6);
            ui.Init(_editorLineNumbersColorButton, 35);
            ui.PadLeft(_editorLineNumbersColorButton, 6);
            ui.Init(_editorBackgroundColorButton, 35);
            ui.PadLeft(_editorBackgroundColorButton, 6);

            ui.Init(_gridFontFlow);
            ui.Init(_gridFontLabel);
            ui.Init(_gridFontButton, 35);
            ui.PadLeft(_gridFontButton, 1);

            ui.Init(_gridColorsFlow);
            ui.Init(_gridPlainColorButton, 35);
            ui.PadLeft(_gridPlainColorButton, 6);
            ui.Init(_gridHeaderColorButton, 35);
            ui.PadLeft(_gridHeaderColorButton, 6);
            ui.Init(_gridLineColorButton, 35);
            ui.PadLeft(_gridLineColorButton, 6);
            ui.Init(_gridBackgroundColorButton, 35);
            ui.PadLeft(_gridBackgroundColorButton, 6);

            ui.Init(_resetFlow);
            ui.MarginTop(_resetFlow);
            ui.Init(_resetButton);

            ui.Init(_buttonFlow);
            ui.MarginTop(_buttonFlow);
            ui.Init(_okButton);
            ui.Init(_cancelButton);

            var x = UserOptions.Instance;
            _colors = x.GetColors();
            _dataTableFont = x.GetDataTableFont();
            _codeFont = x.GetCodeFont();
            UpdateDataTableFontButton();
            UpdateCodeFontButton();

            _colorDialog = new() {
                AllowFullOpen = true,
                AnyColor = true,
                FullOpen = true,
                ShowHelp = false,
            };
            Disposed += delegate { _colorDialog.Dispose(); };
        }

        private void OkButton_Click(object sender, EventArgs e) {
            var x = UserOptions.Instance;
            x.SetDataTableFont(_dataTableFont);
            x.SetCodeFont(_codeFont);
            x.SetColors(_colors);
            x.Save();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void DataTableFontButton_Click(object sender, EventArgs e) {
            var newFont = ChooseFont(_dataTableFont);
            if (newFont == null) {
                return;
            }

            _dataTableFont = newFont;
            UpdateDataTableFontButton();
        }

        private void UpdateDataTableFontButton() {
            _gridFontButton.Text = GetFontDisplayName(_dataTableFont);
        }

        private void CodeFontButton_Click(object sender, EventArgs e) {
            var newFont = ChooseFont(_codeFont);
            if (newFont == null) {
                return;
            }

            _codeFont = newFont;
            UpdateCodeFontButton();
        }

        private void UpdateCodeFontButton() {
            _codeFontButton.Text = GetFontDisplayName(_codeFont);
        }

        private static string GetFontDisplayName(Font font) =>
            $"{font.Name}{(font.Style == FontStyle.Regular ? "" : $" {font.Style}")} {font.SizeInPoints:0}pt";

        private Font ChooseFont(Font font) {
            using FontForm f = new(font);
            if (f.ShowDialog(this) != DialogResult.OK) {
                return null;
            }

            return f.GetFont();
        }

        private void GridPlainColorButton_Click(object sender, EventArgs e) {
            ClickColorButton(_gridPlainColorButton, UserOptionsColor.GRID_PLAIN);
        }

        private void GridHeaderColorButton_Click(object sender, EventArgs e) {
            ClickColorButton(_gridHeaderColorButton, UserOptionsColor.GRID_HEADER);
        }

        private void GridLineColorButton_Click(object sender, EventArgs e) {
            ClickColorButton(_gridLineColorButton, UserOptionsColor.GRID_LINES);
        }

        private void GridBackgroundColorButton_Click(object sender, EventArgs e) {
            ClickColorButton(_gridBackgroundColorButton, UserOptionsColor.GRID_BACKGROUND);
        }

        private void EditorPlainColorButton_Click(object sender, EventArgs e) {
            ClickColorButton(_editorPlainColorButton, UserOptionsColor.CODE_PLAIN);
        }

        private void EditorKeywordColorButton_Click(object sender, EventArgs e) {
            ClickColorButton(_editorKeywordColorButton, UserOptionsColor.CODE_KEYWORD);
        }

        private void EditorCommentColorButton_Click(object sender, EventArgs e) {
            ClickColorButton(_editorCommentColorButton, UserOptionsColor.CODE_COMMENT);
        }

        private void EditorStringColorButton_Click(object sender, EventArgs e) {
            ClickColorButton(_editorStringColorButton, UserOptionsColor.CODE_STRING);
        }

        private void EditorLineNumbersColorButton_Click(object sender, EventArgs e) {
            ClickColorButton(_editorLineNumbersColorButton, UserOptionsColor.CODE_LINENUMS);
        }

        private void EditorBackgroundColorButton_Click(object sender, EventArgs e) {
            ClickColorButton(_editorBackgroundColorButton, UserOptionsColor.CODE_BACKGROUND);
        }

        private void ClickColorButton(Button button, int colorIndex) {
            _colorDialog.Color = _colors[colorIndex];
            if (_colorDialog.ShowDialog(this) != DialogResult.OK) {
                return;
            }

            _colors[colorIndex] = _colorDialog.Color;
            button.Invalidate();
        }

        private void GridPlainColorButton_Paint(object sender, PaintEventArgs e) {
            PaintColorButton(_gridPlainColorButton, _colors[UserOptionsColor.GRID_PLAIN], e.Graphics);
        }

        private void GridHeaderColorButton_Paint(object sender, PaintEventArgs e) {
            PaintColorButton(_gridHeaderColorButton, _colors[UserOptionsColor.GRID_HEADER], e.Graphics);
        }

        private void GridLineColorButton_Paint(object sender, PaintEventArgs e) {
            PaintColorButton(_gridLineColorButton, _colors[UserOptionsColor.GRID_LINES], e.Graphics);
        }

        private void GridBackgroundColorButton_Paint(object sender, PaintEventArgs e) {
            PaintColorButton(_gridBackgroundColorButton, _colors[UserOptionsColor.GRID_BACKGROUND], e.Graphics);
        }

        private void EditorPlainColorButton_Paint(object sender, PaintEventArgs e) {
            PaintColorButton(_editorPlainColorButton, _colors[UserOptionsColor.CODE_PLAIN], e.Graphics);
        }

        private void EditorKeywordColorButton_Paint(object sender, PaintEventArgs e) {
            PaintColorButton(_editorKeywordColorButton, _colors[UserOptionsColor.CODE_KEYWORD], e.Graphics);
        }

        private void EditorCommentColorButton_Paint(object sender, PaintEventArgs e) {
            PaintColorButton(_editorCommentColorButton, _colors[UserOptionsColor.CODE_COMMENT], e.Graphics);
        }

        private void EditorStringColorButton_Paint(object sender, PaintEventArgs e) {
            PaintColorButton(_editorStringColorButton, _colors[UserOptionsColor.CODE_STRING], e.Graphics);
        }

        private void EditorLineNumbersColorButton_Paint(object sender, PaintEventArgs e) {
            PaintColorButton(_editorLineNumbersColorButton, _colors[UserOptionsColor.CODE_LINENUMS], e.Graphics);
        }

        private void EditorBackgroundColorButton_Paint(object sender, PaintEventArgs e) {
            PaintColorButton(_editorBackgroundColorButton, _colors[UserOptionsColor.CODE_BACKGROUND], e.Graphics);
        }

        private static void PaintColorButton(Button button, Color color, Graphics g) {
            var squareSize = 6 * button.Height / 10;
            var margin = 2 * button.Height / 10;
            using var brush = new SolidBrush(color);
            g.FillRectangle(brush,
                margin * 2,
                margin,
                squareSize,
                squareSize);
            g.DrawRectangle(Pens.Black,
                margin * 2,
                margin,
                squareSize,
                squareSize);
        }

        private void ResetButton_Click(object sender, EventArgs e) {
            _colors = UserOptions.GetDefaultColors();
            _codeFont = UserOptions.GetDefaultCodeFont();
            _dataTableFont = UserOptions.GetDefaultDataTableFont();
            UpdateCodeFontButton();
            UpdateDataTableFontButton();

            foreach (Control c in _editorColorsFlow.Controls) {
                c.Invalidate();
            }
            foreach (Control c in _gridColorsFlow.Controls) {
                c.Invalidate();
            }
        }
    }
}
