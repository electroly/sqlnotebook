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
using System.Linq;
using System.Runtime.InteropServices;
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
        private const int WM_SETREDRAW = 0x0B;
        private int _inputStart = 0;

        public string PromptText { get; set; } = ">>";
        public Font PromptFont { get; set; }
        public Color PromptColor { get; set; } = Color.Black;
        public event EventHandler<ConsoleCommandEventArgs> ConsoleCommand;

        public ConsoleRichTextBox() {
            SelectionProtected = true;
        }

        public void BeginUpdate() {
            NativeMethods.SendMessage(Handle, WM_SETREDRAW, (IntPtr)0, IntPtr.Zero);
        }

        public void EndUpdate() {
            NativeMethods.SendMessage(Handle, WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
            Invalidate();
        }

        private static class NativeMethods {
            [DllImport("user32.dll")]
            public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        }

        public void ShowPrompt() {
            BeginUpdate();
            Append("\n");
            Append(PromptText, PromptFont, PromptColor, BackColor);
            Append(" ");
            _inputStart = Text.Length;
            SelectAll();
            SelectionProtected = true;
            SelectionStart = Text.Length;
            SelectionProtected = false;
            ScrollToCaret();
            EndUpdate();
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
                        BeginUpdate();
                        SelectAll();
                        SelectionProtected = true;
                        Append("\n");
                        SelectionStart = Text.Length;
                        ShowPrompt();
                        EndUpdate();
                    }
                    ReadOnly = false;
                }));
            }
            base.OnKeyDown(e);
        }
    }
}
