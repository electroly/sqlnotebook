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
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlNotebookCore;
using SqlNotebookScript.Interpreter;
using ScintillaNET;
using SqlNotebookScript;

namespace SqlNotebook {
    public partial class QueryDocumentControl : UserControl, IDocumentControl {
        private readonly IWin32Window _mainForm;
        private readonly NotebookManager _manager;
        private readonly Notebook _notebook;
        private readonly Scintilla _scintilla;
        private readonly List<DataTable> _results = new List<DataTable>();
        private int _selectedResultIndex = 0;
        private readonly Slot<bool> _operationInProgress;

        public string DocumentText => _scintilla.Text;

        public string ItemName { get; set; }

        public void Save() {
            _manager.SetItemData(ItemName, DocumentText);
        }

        public QueryDocumentControl(string name, NotebookManager manager, IWin32Window mainForm, Slot<bool> operationInProgress) {
            InitializeComponent();
            ItemName = name;
            _manager = manager;
            _notebook = manager.Notebook;
            _mainForm = mainForm;
            _operationInProgress = operationInProgress;

            _grid.EnableDoubleBuffering();

            _scintilla = new Scintilla {
                Dock = DockStyle.Fill,
                Lexer = ScintillaNET.Lexer.Container,
                FontQuality = ScintillaNET.FontQuality.LcdOptimized,
                IndentWidth = 4,
                UseTabs = false,
                BufferedDraw = true,
                TabWidth = 4,
                ScrollWidthTracking = true,
                ScrollWidth = 1,
                BorderStyle = BorderStyle.None,
            };
            foreach (var style in _scintilla.Styles) {
                style.Font = "Consolas";
                style.Size = 10;
            }

            var operatorTokens = new HashSet<TokenType>(new[] {
                TokenType.Add, TokenType.Asterisk, TokenType.BitAnd, TokenType.Bitnot, TokenType.BitOr,
                TokenType.Comma, TokenType.Dot, TokenType.Eq, TokenType.Ge, TokenType.Gt, TokenType.Le, TokenType.Lp,
                TokenType.LShift, TokenType.Lt, TokenType.Ne, TokenType.Plus, TokenType.Rp, TokenType.RShift,
                TokenType.Semi, TokenType.Star, TokenType.UMinus, TokenType.UPlus
            });

            var sqlnbKeywords = new HashSet<string>(new[] {
                "declare", "parameter", "while", "break", "continue", "print", "execute", "exec", "return", "throw",
                "try", "catch"
            });

            var utf8Encoding = new UTF8Encoding(false);
            
            _scintilla.StyleNeeded += (sender, e) => {
                var text = _scintilla.Text;
                Task.Run(() => {
                    var tokens = _notebook.Tokenize(text);
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

            _sqlPanel.Controls.Add(_scintilla);

            Load += (sender, e) => {
                string initialText = _manager.GetItemData(ItemName);
                if (initialText != null) {
                    _scintilla.Text = initialText;
                }

                _scintilla.TextChanged += (sender2, e2) => _manager.SetDirty();

                ShowResult(0);
            };
        }

        public async Task Execute() {
            await Execute(_scintilla.Text);
        }

        private async void ExecuteBtn_Click(object sender, EventArgs e) {
            await Execute();
        }

        private async Task<bool> Execute(string sql) {
            try {
                if (_operationInProgress) {
                    throw new Exception("Another operation is already in progress.");
                }

                await ExecuteCore(sql);
                _manager.SetDirty();
                _manager.Rescan();
                return true;
            } catch (Exception ex) {
                MessageDialog.ShowError(ParentForm, "Script Error", "An error occurred.", ex.Message);
                return false;
            }
        }

        private async Task ExecuteCore(string sql) {
            ScriptOutput output = null;
            Exception exception = null;
            _manager.PushStatus("Running your script...");
            await Task.Run(() => {
                try {
                    output = _manager.ExecuteScript(sql);
                } catch (Exception ex) {
                    exception = ex;
                }
            });
            _manager.PopStatus();

            if (exception != null) {
                throw exception;
            } else {
                var resultSets = new List<DataTable>();

                foreach (var sdt in output.DataTables) {
                    resultSets.Add(sdt.ToDataTable());
                }

                if (output.TextOutput.Any()) {
                    var dt = new DataTable("Output");
                    dt.Columns.Add("printed_text", typeof(string));
                    foreach (var line in output.TextOutput) {
                        var row = dt.NewRow();
                        row.ItemArray = new object[] { line };
                        dt.Rows.Add(row);
                    }
                    resultSets.Insert(0, dt);
                }

                _grid.DataSource = null;
                _results.ForEach(x => x.Dispose());
                _results.Clear();
                _results.AddRange(resultSets);
                ShowResult(0);
            }
        }

        private void ShowResult(int index) {
            _selectedResultIndex = index = Math.Max(0, Math.Min(_results.Count - 1, index));
            if (_results.Count == 0) {
                _resultSetLbl.Text = "(no results)";
                _rowCountLbl.Text = "";
            } else {
                var source = index >= 0 && index < _results.Count ? _results[index] : null;
                _grid.DataSource = source;
                foreach (DataGridViewColumn col in _grid.Columns) {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    var width = col.Width;
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    col.Width = Math.Min(250, width);
                }
                _resultSetLbl.Text = $"{index + 1} of {_results.Count}";
                _rowCountLbl.Text = source == null ? "" : $"{source.Rows.Count} row{(source.Rows.Count == 1 ? "" : "s")}";
            }
            _prevBtn.Enabled = index > 0;
            _nextBtn.Enabled = index < _results.Count - 1;
        }

        private void PrevBtn_Click(object sender, EventArgs e) {
            ShowResult(_selectedResultIndex - 1);
        }

        private void NextBtn_Click(object sender, EventArgs e) {
            ShowResult(_selectedResultIndex + 1);
        }
    }
}
