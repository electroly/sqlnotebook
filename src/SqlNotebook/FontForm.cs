using System;
using System.Drawing;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class FontForm : Form {
        private readonly SqlTextControl _previewText;

        public FontForm(Font initialFont) {
            InitializeComponent();
            _previewPanel.Controls.Add(_previewText = new(true) { Dock = DockStyle.Fill });
            _previewText.SqlText = 
                "-- Example comment.\n" +
                "SELECT type, name, tbl_name, rootpage, sql\n" +
                "FROM sqlite_master\n" +
                "LIMIT 100;";

            Ui ui = new(this);
            ui.Init(_table);
            ui.Init(_rowFlow1);
            ui.Init(_columnFlow1);
            ui.MarginRight(_columnFlow1);
            ui.Init(_fontLabel);
            ui.Init(_fontList, 45, 16);
            ui.Init(_columnFlow2);
            ui.MarginRight(_columnFlow2);
            ui.Init(_sizeLabel);
            ui.Init(_sizeList, 10, 16);
            ui.Init(_columnFlow3);
            ui.Init(_styleLabel);
            ui.Init(_boldCheck);
            ui.Init(_rowFlow2);
            ui.MarginTop(_rowFlow2);
            ui.Init(_previewLabel);
            ui.Init(_previewPanel, 80, 6, margin: true);
            ui.Init(_buttonFlow);
            ui.MarginTop(_buttonFlow);
            ui.Init(_okButton);
            ui.Init(_cancelButton);

            foreach (var family in FontFamily.Families) {
                _fontList.Items.Add(family.Name);
                if (initialFont.Name == family.Name) {
                    _fontList.SelectedIndex = _fontList.Items.Count - 1;
                }
            }

            _sizeList.SelectedIndex = 0;
            for (var i = 0; i < _sizeList.Items.Count; i++) {
                var size = int.Parse((string)_sizeList.Items[i]);
                if (size <= initialFont.SizeInPoints) {
                    _sizeList.SelectedIndex = i;
                }
            }

            _boldCheck.Checked = initialFont.Bold;
            _previewText.TextBox.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Hidden;
            _previewText.TextBox.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Hidden;

            UpdatePreview();
        }

        private void OkButton_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FontList_SelectedIndexChanged(object sender, EventArgs e) {
            UpdatePreview();
        }

        private void SizeList_SelectedIndexChanged(object sender, EventArgs e) {
            UpdatePreview();
        }

        private void BoldCheck_CheckedChanged(object sender, EventArgs e) {
            UpdatePreview();
        }

        private string GetFamily() {
            var index = _fontList.SelectedIndex;
            return
                index >= 0 && index < _fontList.Items.Count
                ? (string)_fontList.Items[index]
                : "Segoe UI";
        }

        private int GetSize() {
            try {
                return int.Parse((string)_sizeList.Items[_sizeList.SelectedIndex]);
            } catch {
                return 9;
            }
        }

        private FontStyle GetStyle() {
            return _boldCheck.Checked ? FontStyle.Bold : FontStyle.Regular;
        }

        public Font GetFont() {
            try {
                return new(GetFamily(), GetSize(), GetStyle());
            } catch {
                return new("Segoe UI", 9f);
            }
        }

        private void UpdatePreview() {
            var font = GetFont();
            _previewText.SetFont(font);
        }
    }
}
