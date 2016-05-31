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

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using SqlNotebookScript;

namespace SqlNotebook {
    public partial class ImportColumnsControl : UserControl {
        private static class GridColumn {
            public static readonly string SourceName = "source_name";
            public static readonly string TargetName = "target_name";
            public static readonly string Conversion = "conversion";
        }

        private readonly DataTable _table;
        private TableSchema _targetTable; // null if the target is a new table

        public IEnumerable<ImportColumn> ImportColumns => GetImportColumns();
        public NotifySlot Change = new NotifySlot();
        public Slot<bool> Error = new Slot<bool>();

        public ImportColumnsControl() {
            InitializeComponent();

            _table = new DataTable();
            _table.Columns.Add(GridColumn.SourceName, typeof(string));
            _table.Columns.Add(GridColumn.TargetName, typeof(string));
            _table.Columns.Add(GridColumn.Conversion, typeof(string));
            _grid.AutoGenerateColumns = false;
            _grid.DataSource = _table;
            _grid.CellValueChanged += (sender, e) => Change.Notify();
            _grid.ApplyOneClickComboBoxFix();
            _grid.EnableDoubleBuffering();

            Bind.OnChange(new Slot[] { Change },
                (sender, e) => ValidateGridInput());
        }

        public void SetSourceColumns(IReadOnlyList<string> columnNames) {
            _table.BeginLoadData();
            _table.Clear();
            foreach (var columnName in columnNames) {
                var row = _table.NewRow();
                row.SetField(GridColumn.SourceName, columnName);
                // target_name will be set by ApplyTargetToTable() below
                row.SetField(GridColumn.Conversion, "TEXT");
                _table.Rows.Add(row);
            }
            _table.EndLoadData();

            ApplyTargetToTable();
            Change.Notify();
        }

        public void SetTargetToNewTable() {
            _targetTable = null;
            ApplyTargetToTable();
            Change.Notify();
        }

        public void SetTargetToExistingTable(TableSchema tableSchema) {
            _targetTable = tableSchema;
            ApplyTargetToTable();
            Change.Notify();
        }

        // reset target column names based on the target (new vs. existing table).
        private void ApplyTargetToTable() {
            var isNewTable = _targetTable == null;
            _table.BeginLoadData();
            foreach (DataRow row in _table.Rows) {
                var sourceName = row.Field<string>(GridColumn.SourceName);
                if (isNewTable) {
                    // for new tables, import every column with the name as-is
                    row.SetField(GridColumn.TargetName, sourceName);
                } else {
                    // for existing tables, try to match a column with the same name, otherwise don't import this
                    // column by default
                    if (_targetTable.Columns.Any(x => x.Name == sourceName)) {
                        row.SetField(GridColumn.TargetName, sourceName);
                    } else {
                        row.SetField(GridColumn.TargetName, "");
                    }
                }
            }
            _table.EndLoadData();
        }

        private IEnumerable<ImportColumn> GetImportColumns() {
            return
                from x in _table.Rows.Cast<DataRow>()
                select new ImportColumn {
                    SourceName = x.Field<string>(GridColumn.SourceName),
                    TargetName = TargetNameOrNull(x.Field<string>(GridColumn.TargetName)),
                    Conversion = x.Field<string>(GridColumn.Conversion)
                };
        }

        private static string TargetNameOrNull(string name) => string.IsNullOrWhiteSpace(name) ? null : name;

        private void ValidateGridInput() { // true = passed validation
            var seenLowercaseTargetColumnNames = new HashSet<string>();
            bool error = false;
            foreach (DataRow row in _table.Rows) {
                var targetName = row.Field<string>(GridColumn.TargetName);
                var lcTargetName = targetName?.ToLower();
                if (string.IsNullOrWhiteSpace(targetName)) {
                    // this is okay, the column will not be imported
                    row.SetColumnError(GridColumn.TargetName, null);
                } else if (seenLowercaseTargetColumnNames.Contains(lcTargetName)) {
                    error = true;
                    row.SetColumnError(GridColumn.TargetName, $"The column name \"{targetName}\" is already in use.");
                } else if (_targetTable != null && !_targetTable.Columns.Any(x => x.Name.ToLower() == lcTargetName)) {
                    error = true;
                    row.SetColumnError(GridColumn.TargetName, $"The column name \"{targetName}\" does not exist in the target table \"{_targetTable.Name}\".");
                } else {
                    row.SetColumnError(GridColumn.TargetName, null);
                    seenLowercaseTargetColumnNames.Add(lcTargetName);
                }
            }
            Error.Value = error;
        }
    }

    public sealed class ImportColumn {
        public string SourceName { get; set; }
        public string TargetName { get; set; }
        public string Conversion { get; set; }
    }
}
