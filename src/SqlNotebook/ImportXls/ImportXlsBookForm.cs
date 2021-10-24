using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlNotebookScript.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace SqlNotebook.ImportXls {
    public partial class ImportXlsBookForm : ZForm {
        private readonly string _filePath;
        private readonly DatabaseSchema _databaseSchema;
        private readonly NotebookManager _manager;

        private readonly DockPanel _dockPanel;

        private readonly ImportXlsSheetsControl _sheetsControl;
        private readonly LoadingContainerControl _sheetsLoadControl;
        private readonly Slot<string> _sheetsError = new Slot<string>();

        private readonly DockContent _sheetInitialLoadDockContent;
        private readonly LoadingContainerControl _sheetInitialLoadControl;
        private readonly List<ImportXlsSheetControl> _sheetControls = new List<ImportXlsSheetControl>();

        private readonly ImportMultiTablePreviewControl _outputPreviewControl;
        private readonly LoadingContainerControl _outputPreviewLoadControl;
        private readonly UserControlDockContent _outputPreviewDockContent;

        private readonly SqlTextControl _sqlControl;
        private readonly LoadingContainerControl _sqlLoadControl;
        private readonly UserControlDockContent _sqlDockContent;

        private List<XlsSheetMeta> _xlsSheetMetas;

        public string GeneratedImportSql { get; private set; } // the ultimate result of this form

        public ImportXlsBookForm(string filePath, DatabaseSchema schema, NotebookManager manager) {
            InitializeComponent();
            _filePath = filePath;
            _databaseSchema = schema;
            _manager = manager;

            Ui ui = new(this, 160, 48);
            ui.Init(_table);
            ui.Init(_buttonFlow);
            ui.MarginTop(_buttonFlow);
            ui.Init(_okBtn);
            ui.Init(_cancelBtn);

            _dockPanel = new() {
                Dock = DockStyle.Fill,
                DocumentStyle = DocumentStyle.DockingWindow,
                Theme = new VS2012LightTheme {
                    ShowWindowListButton = true,
                    ForceActiveCaptionColor = true,
                },
                AllowEndUserDocking = false,
                AllowEndUserNestedDocking = false,
                DockTopPortion = ui.XHeight(8),
                ShowDocumentIcon = false
            };
            _dockPanelContainer.Controls.Add(_dockPanel);

            Text = $"Import {Path.GetFileName(filePath)}";

            // top pane - worksheets
            {
                _sheetsControl = new ImportXlsSheetsControl();
                _sheetsControl.ValueChanged += SheetsControl_ValueChanged;
                _sheetsLoadControl = new LoadingContainerControl { ContainedControl = _sheetsControl };
                var dc = new UserControlDockContent("Worksheets", _sheetsLoadControl, DockAreas.DockTop) {
                    CloseButtonVisible = false
                };
                dc.Show(_dockPanel, DockState.DockTop);
            }

            // import script panel
            {
                _sqlControl = new SqlTextControl(readOnly: true);
                _sqlLoadControl = new LoadingContainerControl { ContainedControl = _sqlControl };
                _sqlDockContent = new UserControlDockContent("Import Script", _sqlLoadControl) {
                    CloseButton = false,
                    CloseButtonVisible = false,
                    ControlBox = false,
                    Icon = Properties.Resources.script32Ico
                };
            }
            
            // import preview panel
            {
                _outputPreviewControl = new ImportMultiTablePreviewControl();
                _outputPreviewControl.GeneratePreview += OutputPreviewControl_GeneratePreview;
                _outputPreviewLoadControl = new LoadingContainerControl { ContainedControl = _outputPreviewControl };
                _outputPreviewDockContent = new UserControlDockContent("Preview", _outputPreviewLoadControl) {
                    CloseButton = false,
                    CloseButtonVisible = false,
                    ControlBox = false,
                    Icon = Properties.Resources.table32Ico
                };
            }

            // dummy panel shown while loading worksheet tabs
            {
                _sheetInitialLoadControl = new LoadingContainerControl { ContainedControl = new Panel() };
                var dc = new UserControlDockContent("Loading...", _sheetInitialLoadControl) {
                    CloseButton = false,
                    CloseButtonVisible = false,
                    ControlBox = false,
                    Icon = Properties.Resources.script32Ico
                };
                dc.Show(_dockPanel);
                _sheetInitialLoadDockContent = dc;
            }
        }

        private void OutputPreviewControl_GeneratePreview(object sender, GeneratePreviewEventArgs e) =>
            e.Task = GeneratePreview();

        private async Task<MultiTablePreview> GeneratePreview() {
            var metas = _xlsSheetMetas.Where(x => x.ToBeImported).ToList();
            var tableNames = metas.Select(m => m.NewName).ToList();

            // build the sql string
            var prefix = Guid.NewGuid().ToString().Replace("-", "") + "_";
            var sb = new StringBuilder(GetImportSql(prefix));
            sb.AppendLine();
            foreach (var m in metas) {
                sb.AppendLine($"SELECT * FROM {(prefix + m.NewName).DoubleQuote()} LIMIT 1000;");
                sb.AppendLine($"DROP TABLE {(prefix + m.NewName).DoubleQuote()};");
            }
            var sql = sb.ToString();

            // run it in a transaction and then roll it back so nothing is actually affected
            var output = await Task.Run(() =>
                _manager.ExecuteScriptEx(
                    code: sql,
                    args: new Dictionary<string, object>(),
                    transactionType: NotebookManager.TransactionType.RollbackTransaction,
                    vars: out _));

            if (output.DataTables.Count != metas.Count) {
                // this shouldn't happen, it should have thrown from ExecuteScriptEx
                throw new Exception("The import failed to generate all the tables.");
            }

            // read out the contents of each table, which we arranged to have selected in our script
            return new MultiTablePreview {
                Tables = metas.Select((x, i) => (TableName: x.NewName, DataTable: output.DataTables[i])).ToList()
            };
        }

        private void SheetsControl_ValueChanged(object sender, EventArgs e) => LoadImportScript();
        private void SheetControl_ValueChanged(object sender, EventArgs e) => LoadImportScript();

        private void LoadImportScript() {
            string sql;
            try {
                sql = GetImportSql();
                _sqlControl.SqlText = sql;
                _sqlControl.Refresh();
                _sqlLoadControl.ClearError();
            } catch (Exception ex) {
                _sqlLoadControl.SetError(ex.Message);
            }
        }

        private string GetImportSql(string temporaryTablePrefix = null) {
            var sb = new StringBuilder();
            
            if (_xlsSheetMetas == null) {
                return "";
            }

            var sheetControlsByIndex = _sheetControls.ToDictionary(x => x.SheetMeta.Index);

            foreach (var m in _xlsSheetMetas) {
                if (!m.ToBeImported) {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(m.NewName)) {
                    throw new Exception($"You must provide a table name for the worksheet \"{m.OriginalName}\".");
                }

                var sheetControl = sheetControlsByIndex.GetValueOrDefault(m.Index);
                if (sheetControl == null) {
                    continue;
                }

                var o = sheetControl.SheetOptions;

                if (m.ImportTableExists == ImportTableExistsOption.DropTable) {
                    sb.AppendLine($"DROP TABLE IF EXISTS {m.NewName.DoubleQuote()}; -- {m.ImportTableExists.GetDescription()}");
                    sb.AppendLine();
                }
                
                sb.AppendLine($"IMPORT XLS {_filePath.SingleQuote()}");
                sb.AppendLine($"WORKSHEET {m.Index + 1}");
                sb.Append($"INTO {((temporaryTablePrefix ?? "") + m.NewName).DoubleQuote()} ");
                var cols = sheetControl.SqlColumnList;
                if (cols.Trim().Any()) {
                    sb.AppendLine("(");
                    sb.AppendLine(cols);
                    sb.Append($") ");
                }
                sb.AppendLine($"OPTIONS (");
                if (temporaryTablePrefix != null) {
                    sb.AppendLine($"    TEMPORARY_TABLE: 1,");
                }
                if (o.FirstRowNumber.HasValue) {
                    sb.AppendLine($"    FIRST_ROW: {o.FirstRowNumber.Value},");
                }
                if (o.LastRowNumber.HasValue) {
                    sb.AppendLine($"    LAST_ROW: {o.LastRowNumber.Value},");
                }
                if (o.FirstColumnLetter != null) {
                    sb.AppendLine($"    FIRST_COLUMN: {o.FirstColumnLetter.SingleQuote()},");
                }
                if (o.LastColumnLetter != null) {
                    sb.AppendLine($"    LAST_COLUMN: {o.LastColumnLetter.SingleQuote()},");
                }
                sb.AppendLine($"    HEADER_ROW: {(o.ColumnHeaders == ColumnHeadersOption.Present ? 1 : 0)}, -- {o.ColumnHeaders.GetDescription()}");
                sb.AppendLine($"    TRUNCATE_EXISTING_TABLE: {(m.ImportTableExists == ImportTableExistsOption.DeleteExistingRows ? 1 : 0)}, -- {m.ImportTableExists.GetDescription()}");
                sb.AppendLine($"    IF_CONVERSION_FAILS: {(int)m.ImportConversionFail} -- {m.ImportConversionFail.GetDescription()}");
                sb.AppendLine($");");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private async void ImportXlsBookForm_Shown(object sender, EventArgs e) {
            try {
                _sheetInitialLoadControl.PushLoad();
                await Go();
            } catch (Exception ex) {
                MessageForm.ShowError(this, "Import Error", $"Error importing the workbook:\r\n{ex.GetErrorMessage()}");
                DialogResult = DialogResult.Cancel;
                Close();
            }

            async Task Go()
            {
                var sheetLoadTasks = new List<Task>();
                await _sheetsLoadControl.DoLoad(async () => {
                    _xlsSheetMetas = await Task.Run(() =>
                        XlsUtil.ReadWorksheetNames(_filePath)
                        .Select((x, i) =>
                            new XlsSheetMeta {
                                Index = i,
                                OriginalName = x,
                                NewName = x,
                                ToBeImported = true
                            })
                        .ToList()
                    );

                    // user might have closed the window while we were loading
                    if (IsDisposed) {
                        return;
                    }

                    _sheetsControl.SetWorksheetInfos(_xlsSheetMetas);

                    _sheetInitialLoadDockContent.Close();
                    _sheetInitialLoadDockContent.Dispose();

                    _sqlDockContent.Show(_dockPanel);
                    _outputPreviewDockContent.Show(_dockPanel);

                    DockContent firstDockContent = null;
                    ImportXlsSheetControl firstSheetControl = null;
                    foreach (var w in _xlsSheetMetas) {
                        var sheetControl = new ImportXlsSheetControl(_filePath, w);
                        sheetControl.ValueChanged += SheetControl_ValueChanged;
                        var dc = new UserControlDockContent($"📄 {w.OriginalName}", sheetControl) {
                            CloseButton = false,
                            CloseButtonVisible = false,
                            ControlBox = false,
                            Icon = Properties.Resources.table32Ico
                        };

                        dc.Show(_dockPanel);
                        _sheetControls.Add(sheetControl);
                        sheetLoadTasks.Add(sheetControl.LoadColumns());
                        firstDockContent = firstDockContent ?? dc;
                        firstSheetControl = firstSheetControl ?? sheetControl;

                        dc.Enter += async delegate {
                            await sheetControl.OnTabActivated();
                        };
                    }

                    firstDockContent?.Activate(); // select the first worksheet tab
                    sheetLoadTasks.Add(firstSheetControl?.OnTabActivated());
                });

                await Task.WhenAll(sheetLoadTasks.ToArray());

                LoadImportScript();

                _okBtn.Enabled = true;
                _progressBar.Visible = false;
            }
        }

        private void OkBtn_Click(object sender, EventArgs e) {
            try {
                GeneratedImportSql = GetImportSql();
                DialogResult = DialogResult.OK;
                Close();
            } catch (Exception ex) {
                MessageForm.ShowError(this, "Import Error", ex.GetCombinedMessage());
            }
        }
    }
}
