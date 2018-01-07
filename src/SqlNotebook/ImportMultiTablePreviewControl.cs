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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlNotebookScript;
using SqlNotebookScript.Utils;

namespace SqlNotebook {
    public partial class ImportMultiTablePreviewControl : UserControl {
        public event EventHandler<GeneratePreviewEventArgs> GeneratePreview;
        private MultiTablePreview _preview;

        public ImportMultiTablePreviewControl() {
            InitializeComponent();
            _toolStrip.SetMenuAppearance();
            _tablesLst.SmallImageList = _imageList.PadListViewIcons();
        }

        private void GeneratePreviewBtn_Click(object sender, EventArgs e) {
            try {
                var gpea = new GeneratePreviewEventArgs();
                GeneratePreview?.Invoke(this, gpea);

                new WaitForm("Import Preview", "Generating the import preview...", () => {
                    gpea.Task.Wait();
                }).ShowDialogAndDispose(this);

                _preview = gpea.Task.Result;

                _tablesLst.BeginUpdate();
                try {
                    _tablesLst.Items.Clear();
                    foreach (var (tableName, _) in _preview.Tables) {
                        var lvi = _tablesLst.Items.Add(tableName);
                        lvi.ImageIndex = 0;
                    }
                } finally {
                    _tablesLst.EndUpdate();
                }

                if (_preview.Tables.Any()) {
                    _tablesLst.Items[0].Selected = true;
                }

            } catch (Exception ex) {
                MessageForm.ShowError(this, "Import Preview",
                    "There was an error while generating the import preview: " + ex.GetCombinedMessage());
            }
        }

        private void TablesLst_SelectedIndexChanged(object sender, EventArgs e) {
            var lvi = _tablesLst.SelectedItems.Cast<ListViewItem>().FirstOrDefault();
            if (lvi != null) {
                var simpleDataTable = _preview.Tables.Single(x => x.TableName == lvi.Text).DataTable;
                _previewGrid.DataSource = simpleDataTable.ToDataTable();
                _previewGrid.AutoSizeColumns();
                foreach (DataGridViewColumn c in _previewGrid.Columns) {
                    c.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
        }
    }

    public sealed class MultiTablePreview {
        public List<(string TableName, SimpleDataTable DataTable)> Tables { get; set; }
    }

    public sealed class GeneratePreviewEventArgs : EventArgs {
        public Task<MultiTablePreview> Task { get; set; }
    }
}
