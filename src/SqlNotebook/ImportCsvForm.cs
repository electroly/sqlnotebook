using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using SqlNotebookScript.Interpreter;
using SqlNotebookScript.Utils;
using System.IO;
using SqlNotebookScript;

namespace SqlNotebook {
    public partial class ImportCsvForm : ZForm {
        private readonly string _filePath;
        private readonly DatabaseSchema _databaseSchema;
        private readonly NotebookManager _manager;

        private readonly ImportCsvOptionsControl _optionsControl;
        private readonly Slot<string> _optionsError = new Slot<string>();

        private readonly LoadingContainerControl _columnsLoadControl;
        private readonly ImportColumnsControl _columnsControl;
        private readonly Slot<string> _columnsError = new Slot<string>();
        private Guid _columnsLoadId;

        private readonly LoadingContainerControl _inputPreviewLoadControl;
        private readonly ImportTextFilePreviewControl _inputPreviewControl;
        private readonly Slot<string> _inputPreviewError = new Slot<string>();
        private Guid _inputPreviewLoadId;

        public string GeneratedImportSql { get; private set; } // the ultimate result of this form

        public ImportCsvForm(string filePath, DatabaseSchema schema, NotebookManager manager) {
            InitializeComponent();
            _filePath = filePath;
            _databaseSchema = schema;
            _manager = manager;

            _optionsControl = new ImportCsvOptionsControl(schema) { Dock = DockStyle.Fill };
            _optionsPanel.Controls.Add(_optionsControl);

            _columnsControl = new ImportColumnsControl { Dock = DockStyle.Fill };
            _columnsLoadControl = new LoadingContainerControl { ContainedControl = _columnsControl, Dock = DockStyle.Fill };
            _columnsPanel.Controls.Add(_columnsLoadControl);

            _inputPreviewControl = new ImportTextFilePreviewControl { Dock = DockStyle.Fill };
            _inputPreviewLoadControl = new LoadingContainerControl { ContainedControl = _inputPreviewControl, Dock = DockStyle.Fill };
            _originalFilePanel.Controls.Add(_inputPreviewLoadControl);

            Ui ui = new(this, 170, 40);
            ui.Init(_table);
            ui.Init(_outerSplit, 0.5);
            ui.InitHeader(_originalFileLabel);
            ui.Init(_lowerSplit, 0.5);
            ui.InitHeader(_optionsLabel);
            ui.InitHeader(_columnsLabel);
            ui.Init(_buttonFlow1);
            ui.MarginTop(_buttonFlow1);
            ui.Init(_previewButton);
            ui.Init(_buttonFlow2);
            ui.MarginTop(_buttonFlow2);
            ui.Init(_okBtn);
            ui.Init(_cancelBtn);

            Load += async (sender, e) => {
                ValidateOptions();
                await UpdateControls(inputChange: true);
            };

            var o = _optionsControl;
            Bind.OnChange(new Slot[] { o.TargetTableName },
                async (sender, e) => {
                    ValidateOptions();
                    await UpdateControls(columnsChange: true);
                });
            Bind.OnChange(new Slot[] { o.FileEncoding },
                async (sender, e) => await UpdateControls(inputChange: true));
            Bind.OnChange(new Slot[] { o.IfTableExists, o.SkipLines, o.HasColumnHeaders },
                async (sender, e) => await UpdateControls(columnsChange: true));
            Bind.BindAny(new[] { _columnsLoadControl.IsOverlayVisible, _inputPreviewLoadControl.IsOverlayVisible },
                x => _okBtn.Enabled = !x);

            Text = $"Import {Path.GetFileName(_filePath)}";
            o.TargetTableName.Value = Path.GetFileNameWithoutExtension(_filePath);
        }

        private async Task UpdateControls(bool inputChange = false, bool columnsChange = false) {
            if (inputChange) {
                await UpdateInputPreview();
                columnsChange = true;
            }

            if (columnsChange) {
                await UpdateColumns();
            }
        }

        private void ValidateOptions() {
            if (string.IsNullOrWhiteSpace(_optionsControl.TargetTableName)) {
                _optionsError.Value = "You must enter a target table name.";
            } else {
                _optionsError.Value = null;
            }
        }

        private async Task UpdateInputPreview() {
            var loadId = Guid.NewGuid();
            _inputPreviewLoadId = loadId;
            _inputPreviewLoadControl.PushLoad();
            try {
                var tempTableName = Guid.NewGuid().ToString();
                var fileEncoding = _optionsControl.FileEncoding.Value;
                var text = await Task.Run(() => {
                    try {
                        var importSql =
                            @"IMPORT TXT @filePath INTO @tableName (number, line)
                            OPTIONS (TAKE_LINES: 1000, TEMPORARY_TABLE: 1, FILE_ENCODING: @encoding);";
                        _manager.ExecuteScript(importSql, new Dictionary<string, object> {
                            ["@filePath"] = _filePath,
                            ["@tableName"] = tempTableName,
                            ["@encoding"] = fileEncoding
                        });

                        var dt = _manager.ExecuteScript($"SELECT line FROM {tempTableName.DoubleQuote()} ORDER BY number")
                            .DataTables[0];

                        return string.Join(Environment.NewLine, dt.Rows.Select(x => x[0].ToString()));
                    } finally {
                        _manager.ExecuteScript($"DROP TABLE IF EXISTS {tempTableName.DoubleQuote()}");
                    }
                });

                if (_inputPreviewLoadId == loadId) {
                    _inputPreviewError.Value = null;
                    _inputPreviewControl.PreviewText = text;
                }
            } catch (UncaughtErrorScriptException ex) {
                if (_inputPreviewLoadId == loadId) {
                    _inputPreviewError.Value = $"Error loading the input file:\r\n{ex.ErrorMessage}";
                    _inputPreviewLoadControl.SetError(_inputPreviewError.Value);
                }
            } finally {
                _inputPreviewLoadControl.PopLoad();
            }
        }

        private async Task UpdateColumns() {
            var loadId = Guid.NewGuid();
            _columnsLoadId = loadId;
            _columnsLoadControl.PushLoad();

            try {
                var sourceColumns = await GetSourceColumns();
                if (_columnsLoadId == loadId) {
                    _columnsControl.SetSourceColumns(sourceColumns);
                    UpdateTargetColumns();
                    _columnsLoadControl.ClearError();
                    _columnsError.Value = null;
                }
            } catch (UncaughtErrorScriptException ex) {
                if (_columnsLoadId == loadId) {
                    _columnsError.Value = $"Error importing the CSV file:\r\n{ex.ErrorMessage}";
                    _columnsLoadControl.SetError(_columnsError.Value);
                }
            } catch (Exception ex) {
                if (_columnsLoadId == loadId) {
                    _columnsError.Value = $"Error importing the CSV file:\r\n{ex.Message}";
                    _columnsLoadControl.SetError(_columnsError.Value);
                }
            } finally {
                _columnsLoadControl.PopLoad();
            }
        }

        private async Task<IReadOnlyList<string>> GetSourceColumns() {
            var tempTableName = Guid.NewGuid().ToString();
            try {
                var headerRow = _optionsControl.HasColumnHeaders.Value;
                var fileEncoding = _optionsControl.FileEncoding.Value;
                var skipLines = _optionsControl.SkipLines.Value;
                return await Task.Run(() => {
                    var importSql =
                        @"IMPORT CSV @filePath INTO @tableName 
                        OPTIONS (SKIP_LINES: @skipLines, TAKE_LINES: 0, HEADER_ROW: @headerRow, TEMPORARY_TABLE: 1, 
                            FILE_ENCODING: @encoding);";
                    _manager.ExecuteScript(importSql, new Dictionary<string, object> {
                        ["@filePath"] = _filePath,
                        ["@tableName"] = tempTableName,
                        ["@headerRow"] = headerRow ? 1 : 0,
                        ["@encoding"] = fileEncoding,
                        ["@skipLines"] = skipLines
                    });

                    var dt = _manager.ExecuteScript($"PRAGMA TABLE_INFO ({tempTableName.DoubleQuote()})")
                        .DataTables[0];
                    var nameCol = dt.GetIndex("name");
                    return dt.Rows.Select(x => x[nameCol].ToString()).ToList();
                });
            } finally {
                await Task.Run(() => {
                    _manager.ExecuteScript($"DROP TABLE IF EXISTS {tempTableName.DoubleQuote()}");
                });
            }
        }

        private void UpdateTargetColumns() {
            var targetTable = _optionsControl.TargetTableName.Value;
            TableSchema schema;

            if (_databaseSchema.NonTables.Contains(targetTable.ToLower())) {
                throw new Exception($"\"{targetTable}\" already exists, but is not a table.");
            }

            if (_optionsControl.IfTableExists.Value == ImportTableExistsOption.DropTable) {
                _columnsControl.SetTargetToNewTable();
            } else if (_databaseSchema.Tables.TryGetValue(targetTable.ToLower(), out schema)) {
                _columnsControl.SetTargetToExistingTable(schema);
            } else {
                _columnsControl.SetTargetToNewTable();
            }
        }

        private string GetImportSql(int takeRows = -1, string temporaryTableName = null) {
            var truncate = _optionsControl.IfTableExists.Value != ImportTableExistsOption.AppendNewRows;
            var drop = _optionsControl.IfTableExists.Value == ImportTableExistsOption.DropTable;
            var tableName = temporaryTableName ?? _optionsControl.TargetTableName.Value;

            return
                (drop ? $"DROP TABLE IF EXISTS {_optionsControl.TargetTableName.Value.DoubleQuote()};\r\n\r\n" : "") +
                $"IMPORT CSV {_filePath.SingleQuote()}\r\n" +
                $"INTO {tableName.DoubleQuote()} (\r\n" +
                _columnsControl.SqlColumnList + "\r\n" +
                $")\r\n" +
                $"OPTIONS (\r\n" +
                $"    SKIP_LINES: {_optionsControl.SkipLines.Value},\r\n" +
                (takeRows >= 0 ? $"TAKE_LINES: {takeRows}," : "") +
                (temporaryTableName != null ? "TEMPORARY_TABLE: 1," : "") +
                $"    HEADER_ROW: {(_optionsControl.HasColumnHeaders.Value ? 1 : 0)},\r\n" +
                $"    TRUNCATE_EXISTING_TABLE: {(truncate ? 1 : 0)},\r\n" +
                $"    FILE_ENCODING: {_optionsControl.FileEncoding.Value},\r\n" +
                $"    IF_CONVERSION_FAILS: {(int)_optionsControl.IfConversionFails.Value}\r\n" +
                $");\r\n";
        }

        private string GetErrorMessage() { // or null
            if (_columnsError.Value != null) {
                // this error is shown in the columns pane
                return "Please correct the error in the \"Columns\" pane.";
            } else if (_optionsError.Value != null) {
                // this error is not shown
                return _optionsError.Value;
            } else if (_inputPreviewError.Value != null) {
                // this error is shown in the original file pane
                return "Please correct the error in the \"Original File\" pane.";
            } else {
                return null;
            }
        }

        private void OkBtn_Click(object sender, EventArgs e) {
            var errorMessage = GetErrorMessage();
            if (errorMessage == null) {
                DialogResult = DialogResult.OK;
                GeneratedImportSql = GetImportSql();
            } else {
                MessageForm.ShowError(this, "Import Error", errorMessage);
            }
        }

        private void PreviewButton_Click(object sender, EventArgs e) {
            var errorMessage = GetErrorMessage();
            if (errorMessage != null) {
                MessageForm.ShowError(this, "Error", errorMessage);
                return;
            }

            string script = null;
            SimpleDataTable table = null;
            using WaitForm waitForm = new("Import", "Generating preview...", () => {
                var tableName = Guid.NewGuid().ToString();
                script = GetImportSql();
                try {
                    var sql = GetImportSql(100, tableName);
                    _manager.ExecuteScript(sql);
                    table = _manager.ExecuteScript($"SELECT * FROM {tableName.DoubleQuote()}").DataTables[0];
                } finally {
                    _manager.ExecuteScript($"DROP TABLE IF EXISTS {tableName.DoubleQuote()}");
                }
            });
            if (waitForm.ShowDialog(this) != DialogResult.OK) {
                MessageForm.ShowError(this, "Error", waitForm.ResultException.Message);
                return;
            }

            using ImportScriptPreviewForm previewForm = new(script, table);
            previewForm.ShowDialog(this);
        }
    }
}
