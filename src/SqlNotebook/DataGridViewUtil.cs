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
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None,
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
            AttachFontColorEventHandler(grid);
            return grid;
        }

        public static void AttachFontColorEventHandler(DataGridView grid) {
            void OptionsUpdated() {
                using var g = grid.CreateGraphics();
                var oldX = g.MeasureString("x", grid.Font, PointF.Empty, StringFormat.GenericTypographic);
                var opt = UserOptions.Instance;
                grid.Font =
                    grid.DefaultCellStyle.Font =
                    grid.ColumnHeadersDefaultCellStyle.Font =
                    grid.RowHeadersDefaultCellStyle.Font =
                    grid.RowsDefaultCellStyle.Font =
                    grid.DefaultCellStyle.Font =
                    opt.GetDataTableFont();
                var newX = g.MeasureString("x", grid.Font, PointF.Empty, StringFormat.GenericTypographic);
                var widthRatio = newX.Width / oldX.Width;
                var rowHeight = (int)(newX.Height * 1.5);
                grid.RowTemplate.Height = rowHeight;
                grid.ColumnHeadersHeight = rowHeight;

                var colors = opt.GetColors();
                grid.BackgroundColor = colors[UserOptionsColor.GRID_BACKGROUND];
                grid.ForeColor = colors[UserOptionsColor.GRID_PLAIN];
                grid.GridColor = colors[UserOptionsColor.GRID_LINES];
                grid.ColumnHeadersDefaultCellStyle.BackColor = colors[UserOptionsColor.GRID_HEADER];
                grid.ColumnHeadersDefaultCellStyle.ForeColor = colors[UserOptionsColor.GRID_PLAIN];
                grid.RowHeadersDefaultCellStyle.BackColor = colors[UserOptionsColor.GRID_HEADER];
                grid.RowHeadersDefaultCellStyle.ForeColor = colors[UserOptionsColor.GRID_PLAIN];
                grid.RowTemplate.DefaultCellStyle.BackColor = colors[UserOptionsColor.GRID_BACKGROUND];
                grid.DefaultCellStyle.ForeColor = colors[UserOptionsColor.GRID_PLAIN];
                grid.DefaultCellStyle.BackColor = colors[UserOptionsColor.GRID_BACKGROUND];

                foreach (DataGridViewColumn column in grid.Columns) {
                    column.Width = (int)(column.Width * widthRatio);
                }

                DataGridViewCellStyle cellStyle = null;
                foreach (DataGridViewRow row in grid.Rows) {
                    row.Height = rowHeight;
                    foreach (DataGridViewCell cell in row.Cells) {
                        if (cellStyle == null) {
                            cellStyle = cell.Style;
                            cellStyle.ForeColor = colors[UserOptionsColor.GRID_PLAIN];
                            cellStyle.BackColor = colors[UserOptionsColor.GRID_BACKGROUND];
                        }
                        cell.Style = cellStyle;
                    }
                }

                // This seems to be necessary, otherwise a black bar sometimes appears between the column header and
                // the rows. Refresh() and Invalidate() don't seem to work.
                grid.Width++;
                grid.Width--;
            };
            OptionsUpdated();
            UserOptions.OnUpdate(grid, OptionsUpdated);
        }

        // This will hide the arrow on the row header of the current row.
        public static void ApplyCustomRowHeaderPaint(DataGridView grid) {
            SolidBrush brush = new(grid.ForeColor);
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
