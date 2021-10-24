using System.Drawing;
using System.Windows.Forms;

namespace SqlNotebook {
    public static class DataGridViewUtil {
        public static DataGridView NewDataGridView(
            bool rowHeadersVisible = false,
            bool autoGenerateColumns = true
            ) {
            DataGridView grid = new() {
                AutoSize = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                AutoGenerateColumns = autoGenerateColumns,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToOrderColumns = false,
                AllowUserToResizeColumns = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                BackgroundColor = Color.White,
                RowHeadersVisible = rowHeadersVisible,
                ColumnHeadersVisible = true,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single,
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing,
                ShowCellToolTips = false,
            };
            grid.EnableDoubleBuffering();
            return grid;
        }

        // This will hide the arrow on the row header of the current row.
        public static void ApplyCustomRowHeaderPaint(DataGridView grid) {
            SolidBrush brush = new(Color.Black);
            grid.CellPainting += (_, e) => {
                if (e.ColumnIndex < 0 && e.RowIndex >= 0) {
                    e.PaintBackground(e.CellBounds, true);
                    StringFormat stringFormat = new();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString($"{e.Value}", e.CellStyle.Font, brush, e.CellBounds, stringFormat);
                    e.Handled = true;
                }
            };
        }
    }
}
