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

        public SqlTextControl(bool readOnly) {
            InitializeComponent();

            _text = new() {
                IsReadOnly = readOnly,
                FontFamily = new("Consolas"),
                FontSize = 14,
                HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
                WordWrap = true,
                ShowLineNumbers = true,
                LineNumbersForeground = System.Windows.Media.Brushes.LightGray,
            };
            ElementHost host = new() {
                Dock = DockStyle.Fill,
                Child = _text,
            };

            IHighlightingDefinition def;
            using (MemoryStream stream = new(Resources.SQL_Mode_xshd)) {
                using XmlTextReader reader = new(stream);
                def = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }

            _text.SyntaxHighlighting = def;
            //SearchPanel.Install(_text);

            _text.TextChanged += (sender, e) => SqlTextChanged?.Invoke(this, EventArgs.Empty);

            Controls.Add(host);
        }
    }
}
