using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SqlNotebook.ImportXls {
    public partial class ImportXlsSheetsControl : UserControl {
        private List<XlsSheetMeta> _list;

        public event EventHandler ValueChanged;

        public ImportXlsSheetsControl() {
            InitializeComponent();
            _grid.AutoGenerateColumns = false;
            _grid.ApplyOneClickComboBoxFix();
            _grid.EnableDoubleBuffering();
            _importTableExistsColumn.Items.AddRange(
                default(ImportTableExistsOption).GetDescriptions().Cast<object>().ToArray());
            _onErrorColumn.Items.AddRange(
                default(ImportConversionFailOption).GetDescriptions().Cast<object>().ToArray());

            Ui ui = new(this, false);
            ui.Init(_toBeImportedColumn, 10);
            ui.Init(_importTableExistsColumn, 35);
            ui.Init(_onErrorColumn, 35);
        }

        public void SetWorksheetInfos(IEnumerable<XlsSheetMeta> list) {
            _list = list.ToList();
            _grid.DataSource = _list;
        }

        private void Grid_CellValueChanged(object sender, DataGridViewCellEventArgs e) =>
            ValueChanged?.Invoke(this, EventArgs.Empty);
    }
}
