using NPOI.SS.Util;
using SqlNotebook.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SqlNotebook.Import.Xls {
    public partial class ImportXlsForm : Form {
        private readonly Tuple<ImportTableExistsOption, string>[] _ifExistsOptions = new[] {
            Tuple.Create(ImportTableExistsOption.AppendNewRows, "Append new rows"),
            Tuple.Create(ImportTableExistsOption.DeleteExistingRows, "Delete existing rows"),
            Tuple.Create(ImportTableExistsOption.DropTable, "Drop table and re-create")
        };

        private readonly Tuple<ImportConversionFailOption, string>[] _conversionFailOptions = new[] {
            Tuple.Create(ImportConversionFailOption.ImportAsText, "Import the value as text"),
            Tuple.Create(ImportConversionFailOption.SkipRow, "Skip the row"),
            Tuple.Create(ImportConversionFailOption.Abort, "Stop import with error")
        };

        private readonly List<Tuple<int, string>> _sheets = new();

        private readonly XlsInput _input;
        private readonly DataGridView _grid;
        private readonly int _columnWidth;
        private readonly ImportColumnsControl _columnsControl;

        public ImportXlsForm(XlsInput input) {
            InitializeComponent();
            _input = input;
            _originalFilePanel.Controls.Add(_grid = DataGridViewUtil.NewDataGridView(
                rowHeadersVisible: true, autoGenerateColumns: false));
            _columnsPanel.Controls.Add(_columnsControl = new() {
                Dock = DockStyle.Fill
            });
            Ui ui = new(this, 170, 50);
            _columnWidth = ui.XWidth(25);

            Load += delegate {
                foreach (var sheet in input.Worksheets) {
                    _sheets.Add(Tuple.Create(sheet.Index, sheet.Name));
                }

                _grid.Dock = DockStyle.Fill;
                _grid.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                _grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                _grid.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
                DataGridViewUtil.ApplyCustomRowHeaderPaint(_grid);

                Text = $"{Path.GetFileName(input.FilePath)} - Import";
                Icon = Resources.file_extension_xls_ico;

                ui.Init(_table);
                ui.Init(_outerSplit, 0.53);
                ui.Init(_lowerSplit, 0.5);
                ui.Init(_originalFileTable);
                ui.InitHeader(_originalFileLabel);
                ui.Init(_sheetTable);
                ui.Pad(_sheetTable);
                ui.Init(_sheetLabel);
                ui.Init(_sheetCombo);
                ui.Init(_originalFilePanel);
                ui.Init(_optionsOuterTable);
                ui.Init(_optionsScrollPanel);
                ui.InitHeader(_optionsLabel);
                ui.Init(_optionsTable);
                ui.PadBig(_optionsTable);
                ui.Init(_sourceLabel);
                ui.MarginBottom(_sourceLabel);
                ui.Init(_specificColumnsCheck);
                ui.Init(_specificColumnsFlow);
                ui.Init(_columnStartText, 10);
                ui.Init(_columnToLabel);
                ui.Init(_columnEndText, 10);
                ui.Init(_columnRangeLabel);
                ui.Init(_specificRowsCheck);
                ui.Init(_specificRowsFlow);
                ui.Init(_rowStartText, 10);
                ui.Init(_rowToLabel);
                ui.Init(_rowEndText, 10);
                ui.Init(_rowRangeLabel);
                ui.Init(_useSelectionLink);
                ui.MarginBottom(_useSelectionLink);
                ui.Init(_columnNamesCheck);
                ui.Init(_targetLabel);
                ui.MarginBottom(_targetLabel);
                ui.MarginTop(_targetLabel);
                ui.Init(_tableNameLabel);
                ui.Init(_tableNameCombo, 40);
                ui.MarginBottom(_tableNameCombo);
                ui.MarginRight(_tableNameCombo);
                ui.Init(_ifTableExistsLabel);
                ui.Init(_ifExistsCombo, 30);
                ui.MarginRight(_ifExistsCombo);
                ui.Init(_ifConversionFailsLabel);
                ui.Init(_convertFailCombo, 30);
                ui.Init(_columnsTable);
                ui.InitHeader(_columnsLabel);
                ui.Init(_buttonFlow1);
                ui.MarginTop(_buttonFlow1);
                ui.Init(_previewButton);
                ui.Init(_buttonFlow2);
                ui.MarginTop(_buttonFlow2);
                ui.Init(_okButton);
                ui.Init(_cancelButton);

                _sheetCombo.DataSource = _sheets;
                _ifExistsCombo.DataSource = _ifExistsOptions;
                _convertFailCombo.DataSource = _conversionFailOptions;
                EnableDisableRowColumnTextboxes();
                LoadOriginalSheetPreview();
            };
        }

        private void EnableDisableRowColumnTextboxes() {
            _columnStartText.Enabled = _columnEndText.Enabled = _specificColumnsCheck.Checked;
            _rowStartText.Enabled = _rowEndText.Enabled = _specificRowsCheck.Checked;
        }

        private void SpecificColumnsCheck_CheckedChanged(object sender, EventArgs e) {
            EnableDisableRowColumnTextboxes();
        }

        private void SpecificRowsCheck_CheckedChanged(object sender, EventArgs e) {
            EnableDisableRowColumnTextboxes();
        }

        private void PreviewButton_Click(object sender, EventArgs e) {

        }

        private void UseSelectionLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            if (_grid.SelectedCells.Count == 0) {
                MessageForm.ShowError(this, "Import Error", "Please select a range of cells in the grid.");
                return;
            }

            var cells = _grid.SelectedCells.Cast<DataGridViewCell>();
            var minRowIndex = cells.Min(x => x.RowIndex);
            var maxRowIndex = cells.Max(x => x.RowIndex);
            var minColumnIndex = cells.Min(x => x.ColumnIndex);
            var maxColumnIndex = cells.Max(x => x.ColumnIndex);

            var numRows = _grid.RowCount;
            if (minRowIndex == 0 && maxRowIndex == numRows - 1) {
                // All rows selected.
                _specificRowsCheck.Checked = false;
                _rowStartText.Text = "";
                _rowEndText.Text = "";
            } else {
                // Subset of rows selected.
                _specificRowsCheck.Checked = true;
                _rowStartText.Text = $"{minRowIndex + 1}";
                _rowEndText.Text = $"{maxRowIndex + 1}";
            }

            var numColumns = _grid.ColumnCount;
            if (minColumnIndex == 0 && maxColumnIndex == numColumns - 1) {
                // All columns selected.
                _specificColumnsCheck.Checked = false;
                _columnStartText.Text = "";
                _columnEndText.Text = "";
            } else {
                // Subset of columns selected.
                _specificColumnsCheck.Checked = true;
                _columnStartText.Text = $"{CellReference.ConvertNumToColString(minColumnIndex)}";
                _columnEndText.Text = $"{CellReference.ConvertNumToColString(maxColumnIndex)}";
            }
        }

        private void SheetCombo_SelectedIndexChanged(object sender, EventArgs e) {
            LoadOriginalSheetPreview();
        }

        private void LoadOriginalSheetPreview() {
            var sheetIndex = (int)_sheetCombo.SelectedValue;
            var sheetInfo = _input.Worksheets[sheetIndex];
            _grid.DataSource = null;
            _grid.Columns.Clear();
            var dataTable = sheetInfo.DataTable;
            foreach (DataColumn column in dataTable.Columns) {
                _grid.Columns.Add(new DataGridViewTextBoxColumn {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    Width = _columnWidth,
                    HeaderText = column.ColumnName,
                    SortMode = DataGridViewColumnSortMode.NotSortable,
                    DataPropertyName = column.ColumnName,
                    Resizable = DataGridViewTriState.True,
                });
            }
            _grid.DataSource = dataTable;
            var rowNumber = 1;
            foreach (DataGridViewRow row in _grid.Rows) {
                row.HeaderCell.Value = $"{rowNumber}";
                rowNumber++;
            }
            _rowRangeLabel.Text = $"(1-{_grid.Rows.Count})";
            _columnRangeLabel.Text = $"(A-{CellReference.ConvertNumToColString(_grid.Columns.Count - 1)})";
        }
    }
}
