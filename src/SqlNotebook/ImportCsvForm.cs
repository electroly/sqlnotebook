// SQL Notebook
// Copyright (C) 2016 Brian Luft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using SqlNotebookScript.Interpreter;
using System.IO;

namespace SqlNotebook {
    public partial class ImportCsvForm : Form {
        private readonly string _filePath;
        private readonly DatabaseSchema _databaseSchema;
        private readonly NotebookManager _manager;

        private readonly DockPanel _dockPanel;

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

        private readonly SqlTextControl _sqlControl;
        private readonly LoadingContainerControl _sqlLoadControl;

        private readonly ImportPreviewControl _outputPreviewControl;
        private readonly LoadingContainerControl _outputPreviewLoadControl;
        private Guid _outputPreviewLoadId;

        public string GeneratedImportSql { get; private set; } // the ultimate result of this forum

        public ImportCsvForm(string filePath, DatabaseSchema schema, NotebookManager manager) {
            InitializeComponent();
            _filePath = filePath;
            _databaseSchema = schema;
            _manager = manager;

            _dockPanel = new DockPanel {
                Dock = DockStyle.Fill,
                DocumentStyle = DocumentStyle.DockingWindow,
                Theme = new VS2012LightTheme {
                    ShowWindowListButton = false,
                    ShowAutoHideButton = false,
                    ForceActiveCaptionColor = true
                },
                DockTopPortion = 0.5,
                AllowEndUserDocking = false,
                AllowEndUserNestedDocking = false,
                ShowDocumentIcon = true
            };
            _dockPanelContainer.Controls.Add(_dockPanel);

            _optionsControl = new ImportCsvOptionsControl(schema) { Dock = DockStyle.Fill };
            {
                var dc = new UserControlDockContent("Import Options", _optionsControl, DockAreas.DockTop);
                dc.CloseButtonVisible = false;
                dc.Show(_dockPanel, DockState.DockTop);
            }

            _columnsControl = new ImportColumnsControl { Dock = DockStyle.Fill };
            _columnsLoadControl = new LoadingContainerControl { ContainedControl = _columnsControl };
            {
                var dc = new UserControlDockContent("Columns", _columnsLoadControl, DockAreas.Float | DockAreas.DockTop);
                dc.CloseButtonVisible = false;
                dc.Show(_dockPanel, new Rectangle(-50000, -50000, 100, 100)); // hide brief flash of this floating window
                dc.DockHandler.FloatPane.DockTo(_dockPanel.DockWindows[DockState.DockTop]);
                dc.DockAreas = DockAreas.DockTop;
            }

            DockContent inputPreviewDc;
            _inputPreviewControl = new ImportTextFilePreviewControl { Dock = DockStyle.Fill };
            _inputPreviewLoadControl = new LoadingContainerControl { ContainedControl = _inputPreviewControl };
            {
                var dc = new UserControlDockContent("Original File", _inputPreviewLoadControl);
                dc.CloseButton = false;
                dc.CloseButtonVisible = false;
                dc.ControlBox = false;
                dc.Icon = Properties.Resources.PageWhiteTextIco;
                dc.Show(_dockPanel);
                inputPreviewDc = dc;
            }

            _sqlControl = new SqlTextControl(readOnly: true) { Dock = DockStyle.Fill };
            _sqlLoadControl = new LoadingContainerControl { ContainedControl = _sqlControl };
            {
                var dc = new UserControlDockContent("Import Script", _sqlLoadControl);
                dc.CloseButton = false;
                dc.CloseButtonVisible = false;
                dc.ControlBox = false;
                dc.Icon = Properties.Resources.ScriptIco;
                dc.Show(_dockPanel);
            }

            _outputPreviewControl = new ImportPreviewControl { Dock = DockStyle.Fill };
            _outputPreviewLoadControl = new LoadingContainerControl { ContainedControl = _outputPreviewControl };
            {
                var dc = new UserControlDockContent("Preview", _outputPreviewLoadControl);
                dc.CloseButton = false;
                dc.CloseButtonVisible = false;
                dc.ControlBox = false;
                dc.Icon = Properties.Resources.TableImportIco;
                dc.Show(_dockPanel);
            }

            inputPreviewDc.Activate(); // select "Original File" tab initially

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
            Bind.OnChange(new Slot[] { o.IfConversionFails, _columnsControl.Change, _optionsError, _columnsError,
                _inputPreviewError },
                async (sender, e) => await UpdateScriptAndOutputPreview());
            Bind.BindAny(new[] { _columnsLoadControl.IsOverlayVisible, _inputPreviewLoadControl.IsOverlayVisible,
                _outputPreviewLoadControl.IsOverlayVisible, _sqlLoadControl.IsOverlayVisible },
                x => _okBtn.Enabled = !x);

            Text = $"Import {Path.GetFileName(_filePath)}";
            o.TargetTableName.Value = Path.GetFileNameWithoutExtension(_filePath);
        }

        private async Task UpdateControls(bool inputChange = false, bool columnsChange = false, bool scriptChange = false) {
            if (inputChange) {
                await UpdateInputPreview();
                columnsChange = true;
            }

            if (columnsChange) {
                await UpdateColumns();
                scriptChange = true;
            }

            if (scriptChange) {
                await UpdateScriptAndOutputPreview();
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

        private string GetImportSql(int takeRows = -1, string temporaryTableName = null, bool transaction = true) {
            var columnList =
                from col in _columnsControl.ImportColumns
                let renamed = col.SourceName != col.TargetName && col.TargetName != null
                select $"    {col.SourceName.DoubleQuote()}{(renamed ? " AS " + col.TargetName.DoubleQuote() : "")} {col.Conversion}";

            var truncate = _optionsControl.IfTableExists.Value != ImportTableExistsOption.AppendNewRows;
            var drop = _optionsControl.IfTableExists.Value == ImportTableExistsOption.DropTable;
            var tableName = temporaryTableName ?? _optionsControl.TargetTableName.Value;

            return
                (transaction ? "BEGIN;\r\n\r\n" : "") +
                (drop ? $"DROP TABLE IF EXISTS {_optionsControl.TargetTableName.Value.DoubleQuote()};\r\n\r\n" : "") +
                $"IMPORT CSV\r\n" +
                $"    {_filePath.SingleQuote()}\r\n" +
                $"INTO {tableName.DoubleQuote()} (\r\n" +
                string.Join(",\r\n", columnList) + "\r\n" +
                $")\r\n" +
                $"OPTIONS (\r\n" +
                $"    SKIP_LINES: {_optionsControl.SkipLines.Value},\r\n" +
                (takeRows >= 0 ? $"TAKE_LINES: {takeRows}," : "") +
                (temporaryTableName != null ? "TEMPORARY_TABLE: 1," : "") +
                $"    HEADER_ROW: {(_optionsControl.HasColumnHeaders.Value ? 1 : 0)},\r\n" +
                $"    TRUNCATE_EXISTING_TABLE: {(truncate ? 1 : 0)},\r\n" +
                $"    FILE_ENCODING: {_optionsControl.FileEncoding.Value},\r\n" +
                $"    IF_CONVERSION_FAILS: {(int)_optionsControl.IfConversionFails.Value}\r\n" +
                $");\r\n" +
                (transaction ? "\r\nCOMMIT;\r\n" : "");
        }

        private async Task UpdateScriptAndOutputPreview() {
            var errorMessage = GetErrorMessage();
            // Import Script pane
            if (errorMessage == null) {
                _sqlLoadControl.ClearError();
                _sqlControl.SqlText = GetImportSql();
            } else {
                _sqlLoadControl.SetError(errorMessage);
            }

            // Preview pane
            if (errorMessage == null) {
                _outputPreviewLoadControl.ClearError();
                var loadId = Guid.NewGuid();
                _outputPreviewLoadId = loadId;
                _outputPreviewLoadControl.PushLoad();
                try {
                    var dt = await Task.Run(() => {
                        var tableName = Guid.NewGuid().ToString();
                        try {
                            _manager.ExecuteScript(GetImportSql(100, tableName, false));
                            return _manager.ExecuteScript($"SELECT * FROM {tableName.DoubleQuote()}").DataTables[0];
                        } finally {
                            _manager.ExecuteScript($"DROP TABLE IF EXISTS {tableName.DoubleQuote()}");
                        }
                    });
                    if (_outputPreviewLoadId == loadId) {
                        _outputPreviewControl.SetTable(dt.ToDataTable(), disposeTable: true);
                    }
                } catch (UncaughtErrorScriptException ex) {
                    if (_outputPreviewLoadId == loadId) {
                        _outputPreviewLoadControl.SetError($"Error importing the CSV file:\r\n{ex.ErrorMessage}");
                    }
                } catch (Exception ex) {
                    if (_outputPreviewLoadId == loadId) {
                        _outputPreviewLoadControl.SetError($"Error importing the CSV file:\r\n{ex.Message}");
                    }
                } finally {
                    _outputPreviewLoadControl.PopLoad();
                }
            } else {
                _outputPreviewLoadControl.SetError(errorMessage);
            }
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
                MessageDialog.ShowError(this, "Import Error", errorMessage);
            }
        }
    }
}
