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
using System.Drawing;
using System.Windows.Forms;

namespace SqlNotebook {
    public sealed class ConsoleCommandEventArgs : EventArgs {
        public string Command { get; }
        public bool WasSuccessful { get; set; }
        public ConsoleCommandEventArgs(string command) {
            Command = command;
        }
    }

    public sealed class ConsoleRichTextBox : RichTextBox {
        private int _inputStart = 0;

        public string PromptText { get; set; } = ">>";
        public Font PromptFont { get; set; }
        public Color PromptColor { get; set; } = Color.Black;
        public event EventHandler<ConsoleCommandEventArgs> ConsoleCommand;

        public ConsoleRichTextBox() {
            SelectionProtected = true;
        }

        public void ShowPrompt() {
            this.BeginUpdate();
            Append("\n");
            Append(PromptText, PromptFont, PromptColor, BackColor);
            Append(" ");
            _inputStart = Text.Length;
            SelectAll();
            SelectionProtected = true;
            SelectionStart = Text.Length;
            SelectionProtected = false;
            ScrollToCaret();
            this.EndUpdate();
        }

        public void Append(string text, Font font = null, Color? fg = null, Color? bg = null) {
            SelectionStart = Text.Length;
            SelectionFont = font ?? Font;
            SelectionColor = fg ?? ForeColor;
            SelectionBackColor = bg ?? BackColor;
            AppendText(text);
            SelectionStart = Text.Length;
            SelectionFont = Font;
            SelectionColor = ForeColor;
            SelectionBackColor = BackColor;
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                e.Handled = true;
                string command = Text.Substring(_inputStart);
                ReadOnly = true;
                BeginInvoke(new MethodInvoker(() => {
                    var e2 = new ConsoleCommandEventArgs(command);
                    ConsoleCommand?.Invoke(this, e2);
                    if (e2.WasSuccessful) {
                        this.BeginUpdate();
                        SelectAll();
                        SelectionProtected = true;
                        Append("\n");
                        SelectionStart = Text.Length;
                        ShowPrompt();
                        this.EndUpdate();
                    }
                    ReadOnly = false;
                }));
            } else if (e.KeyCode == Keys.Home && SelectionStart >= _inputStart) {
                this.BeginUpdate();
                if (e.Shift) {
                    int loc = SelectionStart;
                    SelectionStart = _inputStart;
                    SelectionLength = loc - SelectionStart;
                } else {
                    SelectionStart = _inputStart;
                    SelectionLength = 0;
                }
                this.EndUpdate();
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }
    }
}
