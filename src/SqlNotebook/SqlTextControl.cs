using System;
using System.Drawing;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class SqlTextControl : UserControl {
        private readonly TextBox _text;

        public TextBox TextBox => _text;

        public string SqlText {
            get {
                return _text.Text;
            }
            set {
                _text.Text = value;
            }
        }

        public event EventHandler SqlTextChanged;

        public SqlTextControl(bool readOnly) {
            InitializeComponent();

            _text = new TextBox {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                ReadOnly = readOnly,
                Multiline = true,
                BackColor = SystemColors.Window,
                Font = new("Consolas", 11f),
                ScrollBars = ScrollBars.Both
            };
            _text.TextChanged += (sender, e) => SqlTextChanged?.Invoke(this, EventArgs.Empty);

            Controls.Add(_text);
        }
    }
}
