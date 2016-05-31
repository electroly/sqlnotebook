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

namespace SqlNotebook {
    public partial class ImportTextFilePreviewControl : UserControl {
        private readonly Scintilla _scintilla;

        public string PreviewText {
            get {
                return _scintilla.Text;
            }
            set {
                _scintilla.ReadOnly = false;
                _scintilla.Text = value;
                _scintilla.ReadOnly = true;
            }
        }

        public ImportTextFilePreviewControl() {
            InitializeComponent();

            _scintilla = new Scintilla {
                Dock = DockStyle.Fill,
                Lexer = Lexer.Null,
                FontQuality = FontQuality.LcdOptimized,
                IndentWidth = 4,
                BufferedDraw = true,
                TabWidth = 4,
                ScrollWidthTracking = true,
                ScrollWidth = 1,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
            };
            foreach (var style in _scintilla.Styles) {
                style.Font = "Consolas";
                style.Size = 10;
            }
            _scintilla.Margins[1].Width = 0;
            Controls.Add(_scintilla);
        }
    }
}
