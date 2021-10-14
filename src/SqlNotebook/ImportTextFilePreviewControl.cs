using System.Windows.Forms;

namespace SqlNotebook {
    public partial class ImportTextFilePreviewControl : UserControl {
        private readonly SqlTextControl _text;

        public string PreviewText {
            get {
                return _text.Text;
            }
            set {
                _text.SqlText = value;
            }
        }

        public ImportTextFilePreviewControl() {
            InitializeComponent();

            _text = new(true, false, false) {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
            };
            Controls.Add(_text);
        }
    }
}
