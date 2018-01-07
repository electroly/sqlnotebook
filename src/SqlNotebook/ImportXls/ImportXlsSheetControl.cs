// SQL Notebook
// Copyright (C) 2018 Brian Luft
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlNotebookScript.Utils;
using unvell.ReoGrid;
using unvell.ReoGrid.IO;

namespace SqlNotebook.ImportXls {
    public partial class ImportXlsSheetControl : UserControl {
        private const int OPTIONS_WIDTH = 425;
        private const int BOTTOM_PANE_HEIGHT = 150;
        private const int MAX_SHEET_COLUMN_WIDTH = 300;
        private const int MAX_SHEET_ROWS = 1000;
        private const int STREAM_BUFFER_SIZE = 4096;

        private readonly string _filePath;
        private readonly XlsSheetMeta _sheetMeta;
        private readonly LoadingContainerControl _gridLoadControl;
        private readonly ReoGridControl _grid;
        private readonly XlsSheetOptions _sheetOptions;
        private bool _gridHasLoaded;

        private readonly LoadingContainerControl _columnsLoadControl;
        private readonly ImportColumnsControl _columnsControl;
        private readonly Slot<string> _columnsError = new Slot<string>();
        private ColumnHeaderRange _columnHeaderRange;

        public XlsSheetMeta SheetMeta => _sheetMeta;
        public XlsSheetOptions SheetOptions => _sheetOptions;

        public event EventHandler ValueChanged;

        public ImportXlsSheetControl(string filePath, XlsSheetMeta xlsSheetMeta) {
            InitializeComponent();
            _filePath = filePath;
            _sheetMeta = xlsSheetMeta;

            _grid = new ReoGridControl();
            ReoGridUtil.InitGrid(_grid, readOnly: true);
            _gridLoadControl = new LoadingContainerControl { ContainedControl = _grid, Dock = DockStyle.Fill };
            _previewPanel.Controls.Add(_gridLoadControl);

            _columnsControl = new ImportColumnsControl { Dock = DockStyle.Fill };
            _columnsControl.SetFixedColumnWidths();
            _columnsLoadControl = new LoadingContainerControl { ContainedControl = _columnsControl, Dock = DockStyle.Fill };
            _columnsPanel.Controls.Add(_columnsLoadControl);
            Bind.OnChange(new Slot[] { _columnsControl.Change }, (sender, e) => ValueChanged?.Invoke(this, EventArgs.Empty));

            _sheetOptions = new XlsSheetOptions();
            _propGrid.SelectedObject = _sheetOptions;
        }

        public async Task OnTabActivated() {
            if (!_gridHasLoaded) {
                _gridHasLoaded = true;
                await LoadGrid();
            }
        }

        private void ImportXlsSheetControl_Load(object sender, EventArgs e) {
            if (IsDisposed || _outerSplitContainer.IsDisposed || _bottomSplitContainer.IsDisposed) {
                return;
            }

            _outerSplitContainer.SplitterDistance = _outerSplitContainer.Height - BOTTOM_PANE_HEIGHT;
            _outerSplitContainer.SplitterWidth = 11;

            _bottomSplitContainer.SplitterDistance = OPTIONS_WIDTH;
            _bottomSplitContainer.SplitterWidth = 11;
        }

        public string SqlColumnList => _columnsControl.SqlColumnList;

        public async Task LoadGrid() {
            await _gridLoadControl.DoLoad(async () => {
                var data = await Task.Run(() =>
                    XlsUtil.ReadWorksheet(_filePath, _sheetMeta.Index, lastRowIndex: MAX_SHEET_ROWS - 1));

                // user might have closed the window while we were loading
                if (_grid.IsDisposed) {
                    return;
                }

                // reogrid doesn't seem to properly support newlines in quoted CSV strings but we're just using it for
                // preview purposes, so we'll just remove newlines.  it's fine here.  it's important that reogrid's
                // row numbers match what we think the row numbers are
                foreach (var row in data) {
                    for (var j = 0; j < row.Length; j++) {
                        if (row[j] is string s && s.Contains('\n')) {
                            row[j] = s.Replace("\r", "").Replace("\n", " ");
                        }
                    }
                }

                var columnCount = Math.Max(1, data.Max(x => x.Length));

                // the fastest way to get the data into the grid control seems to be via CSV
                using (var memoryStream = new MemoryStream()) {
                    await Task.Run(() => {
                        var streamWriter = new StreamWriter(
                            memoryStream, Encoding.Default, STREAM_BUFFER_SIZE, leaveOpen: true);
                        using (streamWriter) {
                            CsvUtil.WriteCsv(data, streamWriter);
                        }
                        memoryStream.Position = 0;
                    });

                    // user might have closed the window while we were loading
                    if (_grid.IsDisposed) {
                        return;
                    }

                    _grid.Load(memoryStream, FileFormat.CSV);
                }

                var w = _grid.CurrentWorksheet;
                w.SetRows(data.Count);
                w.SetCols(columnCount);

                for (var i = 0; i < columnCount; i++) {
                    w.AutoFitColumnWidth(i);
                    var width = Math.Min(MAX_SHEET_COLUMN_WIDTH, w.GetColumnWidth(i) + 5);
                    w.SetColumnsWidth(i, 1, (ushort)width);
                }

                w.SetSettings(WorksheetSettings.Edit_Readonly, true);

                _setCellRangeLnk.Enabled = true;
            });
        }

        public async Task LoadColumns() {
            await _columnsLoadControl.DoLoad(async () => {
                var sheetIndex = _sheetMeta.Index;
                var range = new ColumnHeaderRange(_sheetOptions);

                var columnNames = await Task.Run(() => XlsUtil.ReadColumnNames(
                    _filePath, sheetIndex, range.FirstRowNumber, range.FirstColumnLetter, range.LastColumnLetter,
                    range.HeaderRow));

                _columnsControl.SetSourceColumns(columnNames);
                _columnsControl.SetTargetToNewTable();
            });
        }

        private async void PropGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e) {
            var newColumnHeaderRange = new ColumnHeaderRange(_sheetOptions);
            if (!newColumnHeaderRange.Equals(_columnHeaderRange)) {
                await LoadColumns();
                _columnHeaderRange = newColumnHeaderRange;
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private struct ColumnHeaderRange {
            public int? FirstRowNumber;
            public string FirstColumnLetter;
            public string LastColumnLetter;
            public bool HeaderRow;

            public ColumnHeaderRange(XlsSheetOptions o) {
                FirstRowNumber = o.FirstRowNumber;
                FirstColumnLetter = o.FirstColumnLetter;
                LastColumnLetter = o.LastColumnLetter;
                HeaderRow = o.ColumnHeaders == ColumnHeadersOption.Present;
            }
        }

        private async void SetCellRangeLnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            var range = _grid.CurrentWorksheet.SelectionRange;
            if (range.IsEmpty) {
                return;
            }
            _setCellRangeLnk.Enabled = false;
            _sheetOptions.FirstRowNumber = range.Row + 1;
            _sheetOptions.LastRowNumber = range.EndRow + 1;
            if (_sheetOptions.LastRowNumber == MAX_SHEET_ROWS) {
                _sheetOptions.LastRowNumber = null;
            }
            _sheetOptions.FirstColumnLetter = XlsUtil.GetColumnRefString(range.Col + 1);
            _sheetOptions.LastColumnLetter = XlsUtil.GetColumnRefString(range.EndCol + 1);
            _propGrid.Refresh();
            await LoadColumns();
            _setCellRangeLnk.Enabled = true;
        }
    }
}
