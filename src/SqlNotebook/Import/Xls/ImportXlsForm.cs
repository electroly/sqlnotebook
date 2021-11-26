using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NPOI.SS.Util;
using SqlNotebook.Properties;
using SqlNotebookScript.Utils;

namespace SqlNotebook.Import.Xls;

public partial class ImportXlsForm : ZForm {
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
    private readonly NotebookManager _manager;
    private readonly DataGridView _grid;
    private readonly int _columnWidth;
    private readonly ImportColumnsControl _columnsControl;

    public ImportXlsForm(XlsInput input, NotebookManager manager, DatabaseSchema schema) {
        InitializeComponent();
        _input = input;
        _manager = manager;

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

            _tableNameCombo.Text = Path.GetFileNameWithoutExtension(input.FilePath);

            foreach (var tableName in schema.Tables.Keys) {
                _tableNameCombo.Items.Add(tableName);
            }

            Text = $"{Path.GetFileName(input.FilePath)} - Import";
            Icon = Resources.file_extension_xls_ico;

            ui.Init(_table);
            ui.Init(_outerSplit, 0.5);
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
            LoadColumns();

            _tableNameCombo.Select();
        };
    }

    private void EnableDisableRowColumnTextboxes() {
        _columnStartText.Enabled = _columnEndText.Enabled = _specificColumnsCheck.Checked;
        _rowStartText.Enabled = _rowEndText.Enabled = _specificRowsCheck.Checked;
    }

    private void SpecificColumnsCheck_CheckedChanged(object sender, EventArgs e) {
        EnableDisableRowColumnTextboxes();
        StartUpdateTimer();
    }

    private void SpecificRowsCheck_CheckedChanged(object sender, EventArgs e) {
        EnableDisableRowColumnTextboxes();
        StartUpdateTimer();
    }

    private void UseSelectionLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
        if (_grid.SelectedCells.Count == 0) {
            Ui.ShowError(this, "Import Error", "Please select a range of cells in the grid.");
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

        StartUpdateTimer();
    }

    private void SpecificRowColumnText_TextChanged(object sender, EventArgs e) {
        StartUpdateTimer();
    }

    private void SheetCombo_SelectedIndexChanged(object sender, EventArgs e) {
        LoadOriginalSheetPreview();
        StartUpdateTimer();
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
        _grid.AutoSizeColumns(this.Scaled(500));
        var rowNumber = 1;
        foreach (DataGridViewRow row in _grid.Rows) {
            row.HeaderCell.Value = $"{rowNumber}";
            rowNumber++;
        }
        _rowRangeLabel.Text = $"(1-{_grid.Rows.Count})";
        _columnRangeLabel.Text = $"(A-{CellReference.ConvertNumToColString(_grid.Columns.Count - 1)})";
    }

    private void LoadColumns() {
        var sheetIndex = (int)_sheetCombo.SelectedValue;
        var sheetInfo = _input.Worksheets[sheetIndex];
            
        var minRowIndex = 0;
        if (_specificRowsCheck.Checked) {
            try {
                minRowIndex = int.Parse(_rowStartText.Text) - 1;
            } catch {
                // Ignore for now. We will show this error when they hit OK.
            }
        }
            
        var minColumnIndex = 0;
        var maxColumnIndex = sheetInfo.DataTable.Columns.Count - 1;
        if (_specificColumnsCheck.Checked) {
            try {
                if (IsValidColumnString(_columnStartText.Text) && IsValidColumnString(_columnEndText.Text)) {
                    var (min, max) = (
                        CellReference.ConvertColStringToIndex(_columnStartText.Text),
                        CellReference.ConvertColStringToIndex(_columnEndText.Text));
                    minColumnIndex = min;
                    maxColumnIndex = max;
                }
            } catch {
                // Ignore for now. We will show this error when they hit OK.
            }
        }

        List<string> columnNames = new();
        for (var columnIndex = minColumnIndex; columnIndex <= maxColumnIndex; columnIndex++) {
            var columnName = CellReference.ConvertNumToColString(columnIndex);
            if (_columnNamesCheck.Checked) {
                try {
                    columnName = sheetInfo.DataTable.Rows[minRowIndex][columnIndex].ToString();
                } catch {
                    // Ignore for now. We will show this error when they hit OK.
                }
            }
            if (string.IsNullOrWhiteSpace(columnName)) {
                columnName = $"column{columnIndex + 1}";
            }

            columnNames.Add(columnName);
        }

        _columnsControl.SetSourceColumns(columnNames);
        _columnsControl.SetTargetToNewTable();
    }

    private static bool IsValidColumnString(string x) =>
        Regex.IsMatch(x, "^[A-Za-z]+$");

    private void StartUpdateTimer() {
        _updateTimer.Stop();
        _updateTimer.Start();
    }

    private void UpdateTimer_Tick(object sender, EventArgs e) {
        _updateTimer.Stop();
        LoadColumns();
    }

    private void ColumnNamesCheck_CheckedChanged(object sender, EventArgs e) {
        StartUpdateTimer();
    }

    private void PreviewButton_Click(object sender, EventArgs e) {
        string tempSql, realSql;
        try {
            var tableName = GetValidatedTableName();

            var tempPrefix = Guid.NewGuid().ToString().Replace("-", "") + "_";
            StringBuilder tempSqlSb = new(GenerateSql(tempPrefix));
            tempSqlSb.AppendLine($"SELECT * FROM {(tempPrefix + tableName).DoubleQuote()} LIMIT 1000;");
            tempSqlSb.AppendLine($"DROP TABLE {(tempPrefix + tableName).DoubleQuote()};");
            tempSql = tempSqlSb.ToString();

            realSql = GenerateSql();
        } catch (Exception ex) {
            Ui.ShowError(this, "Preview Error", ex);
            return;
        }

        var output = WaitForm.GoWithCancel(this, "Import Preview", "Generating preview...", out var success, cancel => {
            return SqlUtil.WithCancellableTransaction(_manager.Notebook, () => {
                return _manager.ExecuteScript(code: tempSql);
            }, rollback: true, cancel: cancel);
        });
        if (!success) {
            return;
        }

        if (output.DataTables.Count != 1) {
            Ui.ShowError(this, "Preview Error", "The import failed due to an unknown error.");
            return;
        }

        using ImportScriptPreviewForm previewForm = new(realSql, output.DataTables.Single());
        previewForm.ShowDialog(this);
    }

    private string GenerateSql(string temporaryTablePrefix = null) {
        var sheetIndex = (int)_sheetCombo.SelectedValue;
        var sheetInfo = _input.Worksheets[sheetIndex];

        var (minRowIndex, maxRowIndex) = GetValidatedMinMaxRowIndices(sheetInfo);
        var (minColumnIndex, maxColumnIndex) = GetValidatedMinMaxColumnIndices(sheetInfo);
        var columnHeaders = _columnNamesCheck.Checked ? ColumnHeadersOption.Present : ColumnHeadersOption.NotPresent;
        var tableName = GetValidatedTableName();
        var ifTableExists = (ImportTableExistsOption)_ifExistsCombo.SelectedValue;
        var ifConversionFails = (ImportConversionFailOption)_convertFailCombo.SelectedValue;
        var sqlColumnList = GetValidatedSqlColumnList();

        StringBuilder sb = new();

        if (ifTableExists == ImportTableExistsOption.DropTable) {
            sb.AppendLine($"DROP TABLE IF EXISTS {tableName.DoubleQuote()}; -- {ifTableExists.GetDescription()}");
            sb.AppendLine();
        }

        sb.AppendLine($"IMPORT XLS {_input.FilePath.SingleQuote()}");
        sb.AppendLine($"WORKSHEET {sheetIndex + 1}");
        sb.Append($"INTO {((temporaryTablePrefix ?? "") + tableName).DoubleQuote()} ");
        sb.AppendLine("(");
        sb.AppendLine(sqlColumnList);
        sb.Append(") ");
        sb.AppendLine("OPTIONS (");
        if (temporaryTablePrefix != null) {
            sb.AppendLine($"    TEMPORARY_TABLE: 1,");
        }
        if (minRowIndex.HasValue) {
            sb.AppendLine($"    FIRST_ROW: {minRowIndex.Value + 1},");
        }
        if (maxRowIndex.HasValue) {
            sb.AppendLine($"    LAST_ROW: {maxRowIndex.Value + 1},");
        }
        if (minColumnIndex.HasValue) {
            sb.AppendLine($"    FIRST_COLUMN: {CellReference.ConvertNumToColString(minColumnIndex.Value).SingleQuote()},");
        }
        if (maxColumnIndex.HasValue) {
            sb.AppendLine($"    LAST_COLUMN: {CellReference.ConvertNumToColString(maxColumnIndex.Value).SingleQuote()},");
        }
        sb.AppendLine($"    HEADER_ROW: {(columnHeaders == ColumnHeadersOption.Present ? 1 : 0)}, -- {columnHeaders.GetDescription()}");
        sb.AppendLine($"    TRUNCATE_EXISTING_TABLE: {(ifTableExists == ImportTableExistsOption.DeleteExistingRows ? 1 : 0)}, -- {ifTableExists.GetDescription()}");
        sb.AppendLine($"    IF_CONVERSION_FAILS: {(int)ifConversionFails} -- {ifConversionFails.GetDescription()}");
        sb.AppendLine($");");

        return sb.ToString();
    }

    private (int? Min, int? Max) GetValidatedMinMaxRowIndices(XlsWorksheetInfo sheetInfo) {
        if (_specificRowsCheck.Checked) {
            var numRows = sheetInfo.DataTable.Rows.Count;

            if (!int.TryParse(_rowStartText.Text, out var minRowNumber)) {
                throw new Exception($"\"{_rowStartText.Text}\" is not a valid starting row number. " +
                    $"Please enter a number from 1 to {numRows}.");
            }
            var minRowIndex = minRowNumber - 1;
            if (minRowIndex < 0 || minRowIndex >= numRows) {
                throw new Exception($"The starting row number \"{_rowStartText.Text}\" is out of range. " +
                    $"Please enter a number from 1 to {numRows}.");
            }

            if (!int.TryParse(_rowEndText.Text, out var maxRowNumber)) {
                throw new Exception($"\"{_rowEndText.Text}\" is not a valid ending row number. Please enter " +
                    $"a number from 1 to {numRows}.");
            }
            var maxRowIndex = maxRowNumber - 1;
            if (maxRowIndex < 0 || maxRowIndex >= numRows) {
                throw new Exception($"The ending row number \"{_rowEndText.Text}\" is out of range. " +
                    $"Please enter a number from 1 to {numRows}.");
            }

            if (minRowIndex > maxRowIndex) {
                throw new Exception(
                    "The ending row number must be greater than or equal to the starting row number.");
            }

            return (minRowIndex, maxRowIndex);
        }

        return (null, null);
    }

    private (int? Min, int? Max) GetValidatedMinMaxColumnIndices(XlsWorksheetInfo sheetInfo) {
        if (_specificColumnsCheck.Checked) {
            var numColumns = sheetInfo.DataTable.Columns.Count;

            if (!IsValidColumnString(_columnStartText.Text)) {
                throw new Exception($"\"{_columnStartText.Text}\" is not a valid starting column. " +
                    $"Please enter a column from A to {CellReference.ConvertNumToColString(numColumns - 1)}.");
            }
            var minColumnIndex = CellReference.ConvertColStringToIndex(_columnStartText.Text);
            if (minColumnIndex < 0 || minColumnIndex >= numColumns) {
                throw new Exception($"The starting column \"{_columnStartText.Text}\" is out of range. " +
                    $"Please enter a column from A to {CellReference.ConvertNumToColString(numColumns - 1)}.");
            }

            if (!IsValidColumnString(_columnEndText.Text)) {
                throw new Exception($"\"{_columnEndText.Text}\" is not a valid ending column. Please enter " +
                    $"a column from A to {CellReference.ConvertNumToColString(numColumns - 1)}.");
            }
            var maxColumnIndex = CellReference.ConvertColStringToIndex(_columnEndText.Text);
            if (maxColumnIndex < 0 || maxColumnIndex >= numColumns) {
                throw new Exception($"The ending column \"{_columnEndText.Text}\" is out of range. " +
                    $"Please enter a column from A to {CellReference.ConvertNumToColString(numColumns - 1)}.");
            }

            if (minColumnIndex > maxColumnIndex) {
                throw new Exception(
                    "The ending column must be greater than or equal to the starting column.");
            }

            return (minColumnIndex, maxColumnIndex);
        }

        return (null, null);
    }

    private string GetValidatedTableName() {
        if (string.IsNullOrWhiteSpace(_tableNameCombo.Text)) {
            throw new Exception("Please enter a target table name.");
        }
        return _tableNameCombo.Text;
    }

    private string GetValidatedSqlColumnList() {
        var x = _columnsControl.SqlColumnList;
        if (string.IsNullOrWhiteSpace(x)) {
            throw new Exception("Please select at least one column for import.");
        }
        return x;
    }

    private void OkButton_Click(object sender, EventArgs e) {
        string sql;
        try {
            sql = GenerateSql();
        } catch (Exception ex) {
            Ui.ShowError(this, "Import Error", ex);
            return;
        }

        WaitForm.GoWithCancel(this, "Import", $"Importing XLS file...", out var success, cancel => {
            SqlUtil.WithCancellableTransaction(_manager.Notebook, () => {
                _manager.ExecuteScriptNoOutput(sql);
            }, cancel);
        });
        if (!success) {
            return;
        }

        _manager.Rescan();
        _manager.SetDirty();
        DialogResult = DialogResult.OK;
        Close();
    }
}
