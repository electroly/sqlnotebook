// SQL Notebook
// Copyright (C) 2017 Brian Luft
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlNotebookScript.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace SqlNotebook {
    public partial class ImportXlsBookForm : Form {
        private readonly string _filePath;
        private readonly DatabaseSchema _databaseSchema;
        private readonly NotebookManager _manager;

        private readonly DockPanel _dockPanel;

        private readonly ImportXlsSheetsControl _sheetsControl;
        private readonly Slot<string> _sheetsError = new Slot<string>();

        private readonly ImportXlsSheetControl _optionsControl;
        private readonly Slot<string> _optionsError = new Slot<string>();

        private readonly ImportPreviewControl _outputPreviewControl;
        private readonly LoadingContainerControl _outputPreviewLoadControl;
        private Guid _outputPreviewLoadId;

        private readonly SqlTextControl _sqlControl;
        private readonly LoadingContainerControl _sqlLoadControl;

        public ImportXlsBookForm(string filePath, DatabaseSchema schema, NotebookManager manager) {
            InitializeComponent();
            _filePath = filePath;
            _databaseSchema = schema;
            _manager = manager;

            _dockPanel = DockPanelUtil.CreateDockPanel();
            _dockPanel.DockLeftPortion = 300;
            _dockPanelContainer.Controls.Add(_dockPanel);

            _sheetsControl = new ImportXlsSheetsControl();
            {
                var dc = new UserControlDockContent("Workbook", _sheetsControl, DockAreas.DockLeft) {
                    CloseButtonVisible = false
                };
                dc.Show(_dockPanel, DockState.DockLeft);
            }

            _optionsControl = new ImportXlsSheetControl();
            {
                var dc = new UserControlDockContent("Worksheet", _optionsControl) {
                    CloseButtonVisible = false,
                    Icon = Properties.Resources.TableSheetIco
                };
                dc.Show(_dockPanel);
            }

            _sqlControl = new SqlTextControl(readOnly: true);
            _sqlLoadControl = new LoadingContainerControl { ContainedControl = _sqlControl };
            {
                var dc = new UserControlDockContent("Import Script", _sqlLoadControl, DockAreas.DockBottom) {
                    CloseButtonVisible = false
                };
                dc.Show(_dockPanel, DockState.DockBottomAutoHide);
            }

            _outputPreviewControl = new ImportPreviewControl();
            _outputPreviewLoadControl = new LoadingContainerControl { ContainedControl = _outputPreviewControl };
            {
                var dc = new UserControlDockContent("Preview", _outputPreviewLoadControl, DockAreas.DockBottom) {
                    CloseButtonVisible = false,
                    Icon = Properties.Resources.TableImportIco
                };
                dc.Show(_dockPanel, DockState.DockBottomAutoHide);
            }
        }
    }
}
