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
