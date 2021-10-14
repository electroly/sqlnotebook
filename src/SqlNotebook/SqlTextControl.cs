using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using SqlNotebook.Properties;
using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Xml;

namespace SqlNotebook {
    public partial class SqlTextControl : UserControl {
        private readonly TextEditor _text;

        public TextEditor TextBox => _text;

        public string SqlText {
            get {
                return _text.Text;
            }
            set {
                _text.Text = value;
            }
        }

        public event EventHandler SqlTextChanged;

        public SqlTextControl(bool readOnly, bool syntaxColoring = true, bool wrap = true) {
            InitializeComponent();

            _text = new() {
                IsReadOnly = readOnly,
                FontFamily = new("Consolas"),
                FontSize = 14,
                HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
                WordWrap = wrap,
                ShowLineNumbers = true,
                LineNumbersForeground = System.Windows.Media.Brushes.LightGray,
            };
            ElementHost host = new() {
                Dock = DockStyle.Fill,
                Child = _text,
            };

            if (syntaxColoring) {
                IHighlightingDefinition def;
                using (MemoryStream stream = new(Resources.SQL_Mode_xshd)) {
                    using XmlTextReader reader = new(stream);
                    def = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }

                _text.SyntaxHighlighting = def;
            }

            _text.TextChanged += (sender, e) => SqlTextChanged?.Invoke(this, EventArgs.Empty);

            Controls.Add(host);
        }
    }
}
