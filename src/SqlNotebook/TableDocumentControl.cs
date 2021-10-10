using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlNotebook.Properties;
using SqlNotebookScript;
using SqlNotebookScript.Utils;

namespace SqlNotebook {
    public partial class TableDocumentControl : UserControl, IDocumentControl {
        private readonly NotebookManager _manager;
        private readonly string _tableName;
        private readonly IWin32Window _mainForm;

        public TableDocumentControl(NotebookManager manager, string tableName, IWin32Window mainForm) {
            InitializeComponent();
            _manager = manager;
            _tableName = tableName;
            _mainForm = mainForm;
            _toolStrip.SetMenuAppearance();

            _grid.EnableDoubleBuffering();

            Load += async (sender, e) => {
                Exception exception = null;
                SimpleDataTable sdt = null;
                
                manager.PushStatus("Reading table data...");
                await Task.Run(() => {
                    try {
                        var n = _manager.Notebook;
                        sdt = n.SpecialReadOnlyQuery(
                            $"SELECT * FROM {_tableName.DoubleQuote()} LIMIT 1000",
                            new Dictionary<string, object>());
                    } catch (Exception ex) {
                        exception = ex;
                    }
                });
                manager.PopStatus();

                if (exception == null) {
                    var dt = new DataTable();
                    foreach (var col in sdt.Columns) {
                        dt.Columns.Add(col);
                    }
                    foreach (var row in sdt.Rows) {
                        var dtRow = dt.NewRow();
                        dtRow.ItemArray = row;
                        dt.Rows.Add(dtRow);
                    }
                    _grid.DataSource = dt;
                    _grid.AutoSizeColumns();
                } else {
                    MessageForm.ShowError(_mainForm, "Preview Table", "An error occurred.", exception.Message);
                }

                Ui ui = new(this, false);
                ui.Init(_scriptBtn, Resources.script_go, Resources.script_go32);

            };
        }

        // IDocumentControl
        string IDocumentControl.ItemName { get; set; }
        public void Save() { }

        private void ScriptBtn_Click(object sender, EventArgs e) {
            var name = _manager.NewScript();
            _manager.SetItemData(name, $"SELECT * FROM {_tableName.DoubleQuote()} LIMIT 1000");
            _manager.OpenItem(new NotebookItem(NotebookItemType.Script, name));
        }
    }
}
