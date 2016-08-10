// SQL Notebook
// Copyright (C) 2016 Brian Luft
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
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScintillaNET;
using SqlNotebookCore;
using SqlNotebookScript;

namespace SqlNotebook {
    public partial class SqlTextControl : UserControl {
        private readonly Scintilla _scintilla;

        public Scintilla Scintilla => _scintilla;

        public string SqlText {
            get {
                return _scintilla.Text;
            }
            set {
                // scintilla won't let us update the text programmatically if it is set to read-only
                var oldReadOnly = _scintilla.ReadOnly;
                _scintilla.ReadOnly = false;
                _scintilla.Text = value;
                _scintilla.ReadOnly = oldReadOnly;
            }
        }

        public event EventHandler SqlTextChanged;

        public SqlTextControl(bool readOnly) {
            InitializeComponent();

            _scintilla = new Scintilla {
                Dock = DockStyle.Fill,
                Lexer = Lexer.Container,
                FontQuality = FontQuality.LcdOptimized,
                IndentWidth = 4,
                UseTabs = false,
                BufferedDraw = true,
                TabWidth = 4,
                ScrollWidthTracking = true,
                ScrollWidth = 1,
                BorderStyle = BorderStyle.None,
                ReadOnly = readOnly
            };
            _scintilla.TextChanged += (sender, e) => SqlTextChanged?.Invoke(this, EventArgs.Empty);

            foreach (var style in _scintilla.Styles) {
                style.Font = "Consolas";
                style.Size = 10;
            }

            var operatorTokens = new HashSet<TokenType>(new[] {
                TokenType.Add, TokenType.Asterisk, TokenType.Bitand, TokenType.Bitnot, TokenType.Bitor,
                TokenType.Comma, TokenType.Dot, TokenType.Eq, TokenType.Ge, TokenType.Gt, TokenType.Le, TokenType.Lp,
                TokenType.Lshift, TokenType.Lt, TokenType.Ne, TokenType.Plus, TokenType.Rp, TokenType.Rshift,
                TokenType.Semi, TokenType.Star, TokenType.Uminus, TokenType.Uplus
            });

            var sqlnbKeywords = new HashSet<string>(new[] {
                "declare", "parameter", "while", "break", "continue", "print", "execute", "exec", "return", "throw",
                "try", "catch", "import", "csv", "options", "text", "integer", "real", "date", "datetime", "datetimeoffset",
                "table", "txt"
            });

            var utf8Encoding = new UTF8Encoding(false);

            _scintilla.StyleNeeded += (sender, e) => {
                var text = _scintilla.Text;
                Task.Run(() => {
                    var tokens = Notebook.StaticTokenize(text);
                    ulong i = 0;
                    var utf8 = utf8Encoding.GetBytes(text);
                    var list = new List<Tuple<int, int>>(); // length, type; for successive calls to SetStyling()

                    foreach (var token in tokens) {
                        // treat characters between i and token.Utf8Start as a comment
                        var numCommentBytes = token.Utf8Start - i;
                        var strComment = Encoding.UTF8.GetString(utf8, (int)i, (int)numCommentBytes);
                        list.Add(Tuple.Create(strComment.Length, ScintillaNET.Style.Sql.Comment));
                        i += numCommentBytes;

                        var strSpace = Encoding.UTF8.GetString(utf8, (int)i, (int)token.Utf8Length);
                        int type = 0;
                        if (operatorTokens.Contains(token.Type)) {
                            type = ScintillaNET.Style.Sql.Operator;
                        } else {
                            switch (token.Type) {
                                case TokenType.Space:
                                case TokenType.Span:
                                case TokenType.Illegal:
                                case TokenType.Column:
                                case TokenType.Table:
                                    type = ScintillaNET.Style.Sql.Default;
                                    break;

                                case TokenType.String:
                                case TokenType.UnclosedString:
                                    type = ScintillaNET.Style.Sql.String;
                                    break;

                                case TokenType.Id:
                                    if (sqlnbKeywords.Contains(token.Text.ToLower())) {
                                        type = ScintillaNET.Style.Sql.Word;
                                    } else {
                                        type = ScintillaNET.Style.Sql.Identifier;
                                    }
                                    break;

                                case TokenType.Variable:
                                    type = ScintillaNET.Style.Sql.User1;
                                    break;

                                case TokenType.Integer:
                                case TokenType.Float:
                                    type = ScintillaNET.Style.Sql.Number;
                                    break;

                                default:
                                    type = ScintillaNET.Style.Sql.Word;
                                    break;
                            }
                        }

                        list.Add(Tuple.Create(strSpace.Length, type));
                        i += token.Utf8Length;
                    }

                    // everything from the last token to the end of the string is a comment
                    var numEndCommentBytes = utf8.Length - (int)i;
                    if (numEndCommentBytes > 0) {
                        var strEndComment = Encoding.UTF8.GetString(utf8, (int)i, numEndCommentBytes);
                        list.Add(Tuple.Create(strEndComment.Length, ScintillaNET.Style.Sql.Comment));
                    }

                    BeginInvoke(new MethodInvoker(() => {
                        if (_scintilla.Text != text) {
                            // the text changed while we were working, so discard these results because
                            // another tokenization will soon deliver more up-to-date results.
                            return;
                        }

                        _scintilla.StartStyling(0);
                        foreach (var item in list) {
                            _scintilla.SetStyling(item.Item1, item.Item2);
                        }
                    }));
                });
            };

            _scintilla.Margins[1].Width = 0;
            _scintilla.Styles[ScintillaNET.Style.Sql.String].ForeColor = Color.Red;
            _scintilla.Styles[ScintillaNET.Style.Sql.Comment].ForeColor = Color.Green;
            _scintilla.Styles[ScintillaNET.Style.Sql.Number].ForeColor = Color.Gray;
            _scintilla.Styles[ScintillaNET.Style.Sql.Operator].ForeColor = Color.Gray;
            _scintilla.Styles[ScintillaNET.Style.Sql.Word].ForeColor = Color.Blue;
            _scintilla.Styles[ScintillaNET.Style.Sql.User1].Italic = true; // variables
            _scintilla.Styles[ScintillaNET.Style.Sql.Number].ForeColor = Color.Gray;

            Controls.Add(_scintilla);
        }
    }
}
