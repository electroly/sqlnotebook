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
using System.Windows.Forms;
using SqlNotebook.Properties;
using SqlNotebookCore;
using WeifenLuo.WinFormsUI.Docking;

namespace SqlNotebook {
    public partial class MainForm : Form {
        private readonly DockPanel _dockPanel;
        private NotebookManager _manager;
        private Notebook _notebook;
        private readonly UserControlDockContent _notebookPane;
        private readonly Importer _importer;
        private readonly ExplorerControl _explorer;

        private readonly Dictionary<NotebookItem, UserControlDockContent> _openItems 
            = new Dictionary<NotebookItem, UserControlDockContent>();

        public MainForm(string tempFilePath, bool isNew) {
            InitializeComponent();

            _importer = new Importer(this);
            _notebook = new Notebook(tempFilePath);
            _manager = new NotebookManager(_notebook);
            _dockPanel = new DockPanel {
                Dock = DockStyle.Fill,
                Theme = new VS2012LightTheme(),
                DocumentStyle = DocumentStyle.DockingWindow
            };
            _dockPanel.Extender.FloatWindowFactory = new FloatWindowFactoryEx();
            _toolStripContainer.ContentPanel.Controls.Add(_dockPanel);

            _notebookPane = new UserControlDockContent("Notebook", _explorer = new ExplorerControl(_manager));
            _notebookPane.CloseButtonVisible = false;
            _notebookPane.Show(_dockPanel, DockState.DockLeft);

            _manager.NotebookItemOpenRequest += Manager_NotebookItemOpenRequest;

            if (isNew) {
                _manager.NewNote("Getting Started", Resources.GettingStartedRtf);
                Load += (sender, e) => OpenItem(new NotebookItem(NotebookItemType.Note, "Getting Started"));
            }
        }

        private void Manager_NotebookItemOpenRequest(object sender, NotebookItemOpenRequestEventArgs e) {
            OpenItem(e.Item);
        }

        private void OpenItem(NotebookItem item) {
            UserControlDockContent wnd;
            if (_openItems.TryGetValue(item, out wnd)) {
                wnd.Focus();
                return;
            }

            UserControlDockContent f = null;
            if (item.Type == NotebookItemType.Console) {
                var doc = new ConsoleDocumentControl(item.Name, _manager);
                f = new UserControlDockContent(item.Name, doc) {
                    Icon = Resources.ApplicationXpTerminalIco
                };
                f.FormClosing += (sender2, e2) => {
                    _manager.SetItemData(doc.ItemName, doc.RtfText);
                };
            } else if (item.Type == NotebookItemType.Script) {
                var doc = new QueryDocumentControl(item.Name, _manager);
                f = new UserControlDockContent(item.Name, doc) {
                    Icon = Resources.NoteIco
                };
                f.FormClosing += (sender2, e2) => {
                    _manager.SetItemData(doc.ItemName, doc.SqlText);
                };
            } else if (item.Type == NotebookItemType.Note) {
                var doc = new NoteDocumentControl(item.Name, _manager);
                f = new UserControlDockContent(item.Name, doc) {
                    Icon = Resources.NoteIco
                };
                f.FormClosing += (sender2, e2) => {
                    _manager.SetItemData(doc.ItemName, doc.RtfText);
                };
            } else {
                return;
            }

            f.FormClosed += (sender2, e2) => {
                _openItems.Remove(item);
            };
            f.Show(_dockPanel);
            _openItems[item] = f;
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

        private void NewConsoleBtn_Click(object sender, EventArgs e) {
            OpenItem(new NotebookItem(NotebookItemType.Console, _manager.NewConsole()));
        }

        private void NewScriptBtn_Click(object sender, EventArgs e) {
            OpenItem(new NotebookItem(NotebookItemType.Script, _manager.NewScript()));
        }

        private void NewNoteBtn_Click(object sender, EventArgs e) {
            OpenItem(new NotebookItem(NotebookItemType.Note, _manager.NewNote()));
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if (keyData == Keys.F5) {
                var doc = _dockPanel.ActiveDocument as UserControlDockContent;
                if (doc != null) {
                    var queryDoc = doc.Content as QueryDocumentControl;
                    if (queryDoc != null) {
                        queryDoc.Execute();
                        return true;
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
