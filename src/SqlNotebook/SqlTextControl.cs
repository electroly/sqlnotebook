using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using SqlNotebook.Properties;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Xml;

namespace SqlNotebook {
    public partial class SqlTextControl : UserControl {
        private readonly TextEditor _text;
        private readonly bool _syntaxColoring;

        public TextEditor TextBox => _text;

        public string SqlText {
            get {
                return _text.Text;
            }
            set {
                _text.Text = value;
            }
        }

        public void SetFont(Font font) {
            _text.FontFamily = new(font.FontFamily.Name);
            // I have no idea how to actually convert this, it's just clear that the WPF font size doesn't match our
            // GDI font size. 96/72 does visually seem to be the right ratio, but why?
            _text.FontSize = font.Size * (96f / 72f);
            _text.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(font.Bold ? 700 : 400);
        }

        public event EventHandler SqlTextChanged;

        public SqlTextControl(bool readOnly, bool syntaxColoring = true, bool wrap = true) {
            InitializeComponent();
            _syntaxColoring = syntaxColoring;

            _text = new() {
                IsReadOnly = readOnly,
                HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
                WordWrap = wrap,
                ShowLineNumbers = true,
                LineNumbersForeground = System.Windows.Media.Brushes.LightGray,
            };
            var opt = UserOptions.Instance;
            SetFont(opt.GetCodeFont());
            ElementHost host = new() {
                Dock = DockStyle.Fill,
                Child = _text,
            };
            SetupSyntaxColoring();

            _text.TextChanged += (sender, e) => SqlTextChanged?.Invoke(this, EventArgs.Empty);

            Controls.Add(host);

            UserOptions.Updated += UserOptions_Updated;
            Disposed += delegate { UserOptions.Updated -= UserOptions_Updated; };
        }

        private void SetupSyntaxColoring() {
            var opt = UserOptions.Instance;
            if (_syntaxColoring) {
                var xml =
                    Encoding.UTF8.GetString(Resources.SQL_Mode_xshd)
                    .Replace("[COMMENT_COLOR]", opt.GetHexColor(UserOptionsColor.CODE_COMMENT))
                    .Replace("[STRING_COLOR]", opt.GetHexColor(UserOptionsColor.CODE_STRING))
                    .Replace("[KEYWORD_COLOR]", opt.GetHexColor(UserOptionsColor.CODE_KEYWORD))
                    ;
                IHighlightingDefinition def;
                using (MemoryStream stream = new(Encoding.UTF8.GetBytes(xml))) {
                    using XmlTextReader reader = new(stream);
                    def = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }

                _text.SyntaxHighlighting = def;
            }

            var colors = opt.GetColors();
            var bg = colors[UserOptionsColor.CODE_BACKGROUND];
            _text.Background = new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromRgb(bg.R, bg.G, bg.B));
            var fg = colors[UserOptionsColor.CODE_PLAIN];
            _text.Foreground = new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromRgb(fg.R, fg.G, fg.B));
            var line = colors[UserOptionsColor.CODE_LINENUMS];
            _text.LineNumbersForeground = new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromRgb(line.R, line.G, line.B));
        }

        private void UserOptions_Updated(object sender, EventArgs e) {
            SetFont(UserOptions.Instance.GetCodeFont());
            SetupSyntaxColoring();
        }
    }
}
