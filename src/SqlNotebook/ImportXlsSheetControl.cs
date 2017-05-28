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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using unvell.ReoGrid;
using SqlNotebookScript.Utils;
using unvell.ReoGrid.Graphics;

namespace SqlNotebook {
    public partial class ImportXlsSheetControl : UserControl {
        private const int OPTIONS_WIDTH = 425;
        private const int BOTTOM_PANE_HEIGHT = 250;

        private ReoGridControl _grid;

        private readonly LoadingContainerControl _columnsLoadControl;
        private readonly ImportColumnsControl _columnsControl;
        private readonly Slot<string> _columnsError = new Slot<string>();
        private Guid _columnsLoadId;

        public ImportXlsSheetControl() {
            InitializeComponent();

            _grid = new ReoGridControl();
            ReoGridUtil.InitGrid(_grid, read_only: true);
            _previewPanel.Controls.Add(_grid);

            _columnsControl = new ImportColumnsControl { Dock = DockStyle.Fill };
            _columnsLoadControl = new LoadingContainerControl { ContainedControl = _columnsControl, Dock = DockStyle.Fill };
            _columnsPanel.Controls.Add(_columnsLoadControl);

            _propGrid.SelectedObject = new SheetOptions();

            Load += (sender, e) => {
                _outerSplitContainer.SplitterDistance = _outerSplitContainer.Height - BOTTOM_PANE_HEIGHT;
                _outerSplitContainer.SplitterWidth = 11;
                
                _bottomSplitContainer.SplitterDistance = OPTIONS_WIDTH;
                _bottomSplitContainer.SplitterWidth = 11;
            };

        }

        private sealed class SheetOptions {
            [DisplayName("Cell range"), Category("Source")]
            public string CellRange { get; set; }

            [DisplayName("Column headers"), DefaultValue(ColumnHeadersOption.Present), Category("Source"),
                TypeConverter(typeof(DescriptionEnumConverter))]
            public ColumnHeadersOption ColumnHeaders { get; set; } = ColumnHeadersOption.Present;

            [DisplayName("Table name"), Category("Target")]
            public string TableName { get; set; }

            [DisplayName("If the table exists, then..."), DefaultValue(ImportTableExistsOption.DropTable),
                Category("Target"), TypeConverter(typeof(DescriptionEnumConverter))]
            public ImportTableExistsOption ImportTableExists { get; set; } = ImportTableExistsOption.DropTable;

            [DisplayName("If data conversion fails, then..."), DefaultValue(ImportConversionFailOption.Abort),
                Category("Target"), TypeConverter(typeof(DescriptionEnumConverter))]
            public ImportConversionFailOption ImportConversionFail { get; set; } = ImportConversionFailOption.Abort;

            public void Hello() { }
        }
    }
}
