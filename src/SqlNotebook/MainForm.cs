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
using System.Windows.Forms;
using SqlNotebookCore;
using WeifenLuo.WinFormsUI.Docking;

namespace SqlNotebook {
    public partial class MainForm : Form {
        private readonly DockPanel _dockPanel;
        private Notebook _notebook;
        private readonly UserControlDockContent _notebookPane;
        private readonly Importer _importer;

        public MainForm(string tempFilePath) {
            InitializeComponent();

            _importer = new Importer(this);
            _notebook = new Notebook(tempFilePath);
            _dockPanel = new DockPanel {
                Dock = DockStyle.Fill,
                Theme = new VS2012LightTheme(),
                DocumentStyle = DocumentStyle.DockingWindow
            };
            _dockPanel.ActiveDocumentChanged += (sender, e) => {
                if (_dockPanel.ActiveDocument is UserControlDockContent) {
                    var doc = (UserControlDockContent)_dockPanel.ActiveDocument;
                    _executeBtn.Enabled = doc.Content is IExecutableDocument;
                }
            };
            _toolStripContainer.ContentPanel.Controls.Add(_dockPanel);

            _notebookPane = new UserControlDockContent("Notebook", new ExplorerControl());
            _notebookPane.CloseButtonVisible = false;
            _notebookPane.Show(_dockPanel, DockState.DockLeft);

            new UserControlDockContent("Hi", new ConsoleDocumentControl(_notebook)).Show(_dockPanel);
        }

        protected override void OnFormClosed(FormClosedEventArgs e) {
            base.OnFormClosed(e);
            _notebook.Dispose();
            _notebook = null;
        }

        private void AboutMnu_Click(object sender, EventArgs e) {
            using (var frm = new AboutForm()) {
                frm.ShowDialog(this);
            }
        }

        private void ImportFileMnu_Click(object sender, EventArgs e) {
            try {
                _importer.DoFileImport();
            } catch (Exception ex) {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExecuteBtn_Click(object sender, EventArgs e) {
            ((_dockPanel.ActiveDocument as UserControlDockContent)?.Content as IExecutableDocument)?.Execute();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if (keyData == Keys.F5 || keyData == (Keys.Control | Keys.Enter)) {
                ExecuteBtn_Click(this, EventArgs.Empty);
                return true;
            } else {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }
    }
}
