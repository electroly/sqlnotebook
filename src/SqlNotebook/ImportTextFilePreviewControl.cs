using System.Drawing;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class ImportTextFilePreviewControl : UserControl {
        private readonly TextBox _text;

        public string PreviewText {
            get {
                return _text.Text;
            }
            set {
                _text.ReadOnly = false;
                _text.Text = value;
                _text.ReadOnly = true;
            }
        }

        public ImportTextFilePreviewControl() {
            InitializeComponent();

            _text = new TextBox {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Multiline = true,
                BackColor = SystemColors.Window,
                Font = new("Consolas", 11f),
                ScrollBars = ScrollBars.Both
            };
            Controls.Add(_text);
        }
    }
}
