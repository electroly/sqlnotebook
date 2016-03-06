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
    public partial class MainFrm : Form {
        private readonly DockPanel _dockPanel;
        private Notebook _notebook;
        private readonly UserControlDockContent _notebookPane;

        public MainFrm(string tempFilePath) {
            InitializeComponent();

            _notebook = new Notebook(tempFilePath);
            _dockPanel = new DockPanel {
                Dock = DockStyle.Fill,
                Theme = new VS2012LightTheme(),
                DocumentStyle = DocumentStyle.DockingWindow
            };
            _toolStripContainer.ContentPanel.Controls.Add(_dockPanel);

            _notebookPane = new UserControlDockContent("Notebook", new ExplorerControl());
            _notebookPane.CloseButtonVisible = false;
            _notebookPane.Show(_dockPanel, DockState.DockLeft);
        }

        protected override void OnFormClosed(FormClosedEventArgs e) {
            base.OnFormClosed(e);
            _notebook.Dispose();
            _notebook = null;
        }

        private void AboutMnu_Click(object sender, EventArgs e) {
            using (var frm = new AboutFrm()) {
                frm.ShowDialog(this);
            }
        }

        private void ImportBtn_Click(object sender, EventArgs e) {
        }
    }
}
