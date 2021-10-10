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
            using var g = CreateGraphics();
            _tablesLst.SmallImageList = _imageList.PadListViewIcons(g);
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
