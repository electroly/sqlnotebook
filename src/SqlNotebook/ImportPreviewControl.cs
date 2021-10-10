using System.Data;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class ImportPreviewControl : UserControl {
        private DataTable _table;
        private bool _disposeTable;

        public ImportPreviewControl() {
            InitializeComponent();
            _grid.EnableDoubleBuffering();
            _grid.Disposed += (sender, e) => {
                if (_disposeTable) {
                    _table?.Dispose();
                }
            };
        }

        public void SetTable(DataTable table, bool disposeTable = false) {
            if (_disposeTable) {
                _table?.Dispose();
            }

            _disposeTable = disposeTable;
            _grid.DataSource = _table = table;
        }
    }
}
