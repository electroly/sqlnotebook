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
using System.Linq;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class ImportPreviewForm : Form {
        public sealed class SelectedTable {
            public string SourceName;
            public string TargetName;
        }

        public IReadOnlyList<SelectedTable> SelectedTables { get; private set; }
        public bool CsvHasHeaderRow { get; private set; }

        private readonly IReadOnlyList<string> _sourceTableNames;
        private readonly List<string> _targetTableNames;

        public ImportPreviewForm(IImportSession session) {
            InitializeComponent();

            // hide the "This file has a header row" checkbox for non-CSV sources
            if (!(session is CsvImportSession)) {
                _csvHeaderChk.Visible = false;
                _tablesGrp.Height += (_tablesGrp.Top - _csvHeaderChk.Top);
                _tablesGrp.Top = _csvHeaderChk.Top;
            }

            _sourceTableNames = session.TableNames;
            _targetTableNames = new List<string>(session.TableNames);
            foreach (var tableName in _sourceTableNames) {
                int index = _listBox.Items.Add(tableName);
                _listBox.SetItemChecked(index, true);
            }

            EnableDisableButtons();
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
            _renameTableBtn.Enabled = _previewBtn.Enabled = hasSelection;
        }

        private void RenameTableBtn_Click(object sender, EventArgs e) {
            int i = _listBox.SelectedIndex;
            var oldName = _sourceTableNames[i];
            using (var f = new ImportRenameTableForm(oldName, _targetTableNames[i])) {
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

        private void PreviewBtn_Click(object sender, EventArgs e) {

        }
    }
}
