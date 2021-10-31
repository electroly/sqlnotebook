using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SqlNotebook.Import.Database {
    public partial class DatabaseImportTablesForm : ZForm {
        public sealed class SelectedTable {
            public string SourceName;
            public string TargetName;
        }

        public IReadOnlyList<SelectedTable> SelectedTables { get; private set; }
        public bool CopyData { get; private set; }

        private readonly IReadOnlyList<string> _sourceTableNames;
        private readonly List<string> _targetTableNames;

        public DatabaseImportTablesForm(IImportSession session) {
            InitializeComponent();

            _sourceTableNames = session.TableNames;
            _targetTableNames = new List<string>(session.TableNames);
            foreach (var tableName in _sourceTableNames) {
                int index = _listBox.Items.Add(tableName);

                // if there's only one table, then check it.  if there are multiple, then uncheck by default.
                _listBox.SetItemChecked(index, session.TableNames.Count == 1);
            }

            EnableDisableButtons();

            Ui ui = new(this, 70, 30);
            ui.InitHeader(_importTablesLabel);
            ui.Init(_importTable);
            ui.Init(_opsFlow);
            ui.Pad(_opsFlow);
            ui.Init(_renameTableBtn);
            ui.MarginRight(_renameTableBtn);
            ui.Init(_selectAllBtn);
            ui.Init(_selectNoneBtn);
            ui.InitHeader(_methodLabel);
            ui.MarginTop(_methodLabel);
            ui.Init(_methodFlow);
            ui.Pad(_methodFlow);
            ui.Init(_methodCopyRad);
            ui.Init(_methodLinkRad);
            ui.Init(_buttonFlow);
            ui.MarginTop(_buttonFlow);
            ui.Init(_okBtn);
            ui.Init(_cancelBtn);
        }

        private void SelectNoneBtn_Click(object sender, EventArgs e) {
            for (int i = 0; i < _listBox.Items.Count; i++) {
                _listBox.SetItemChecked(i, false);
            }
        }
        
        private void SelectAllBtn_Click(object sender, EventArgs e) {
            for (int i = 0; i < _listBox.Items.Count; i++) {
                _listBox.SetItemChecked(i, true);
            }
        }

        private void ListBox_SelectedIndexChanged(object sender, EventArgs e) {
            EnableDisableButtons();
        }

        private void EnableDisableButtons() {
            bool hasSelection = _listBox.SelectedIndex >= 0;
            _renameTableBtn.Enabled = hasSelection;
        }

        private void RenameTableBtn_Click(object sender, EventArgs e) {
            int i = _listBox.SelectedIndex;
            var oldName = _sourceTableNames[i];
            using (var f = new DatabaseImportRenameTableForm(oldName, _targetTableNames[i])) {
                if (f.ShowDialog(this) == DialogResult.OK) {
                    _targetTableNames[i] = f.NewName;
                    if (oldName != f.NewName) {
                        _listBox.Items[i] = $"{oldName} → {f.NewName}";
                    } else {
                        _listBox.Items[i] = $"{oldName}";
                    }
                }
            }
        }

        private void OkBtn_Click(object sender, EventArgs e) {
            var tables = new List<SelectedTable>();
            for (int i = 0; i < _sourceTableNames.Count; i++) {
                var srcName = _sourceTableNames[i];
                var dstName = _targetTableNames[i];
                var isSelected = _listBox.GetItemChecked(i);
                if (isSelected) {
                    tables.Add(new SelectedTable { SourceName = srcName, TargetName = dstName });
                }
            }
            SelectedTables = tables;
            CopyData = _methodCopyRad.Checked;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
