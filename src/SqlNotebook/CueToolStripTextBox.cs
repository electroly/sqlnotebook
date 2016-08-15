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
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;



namespace SqlNotebook {
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public sealed class CueToolStripTextBox : ToolStripControlHost {
        public CueToolStripTextBox() : base(new StyledTextBox()) {
            InnerTextBox.Width = 150;
            Font = new Font("Segoe UI", 9);
        }

        public TextBox InnerTextBox {
            get {
                return (TextBox)Control;
            }
        }

        public override string Text {
            get { return InnerTextBox.Text; }
            set { InnerTextBox.Text = value; }
        }

        public string CueText {
            set { SetCueText(InnerTextBox, value); }
        }

        public void SelectAll() {
            InnerTextBox.SelectAll();
        }

        public static void SetCueText(TextBox self, string text) {
            if (self.IsHandleCreated) {
                NativeMethods.SendMessage(self.Handle, NativeMethods.EM_SETCUEBANNER, (IntPtr)1, text ?? "");
            } else {
                self.HandleCreated += (sender, e) => SetCueText(self, text);
            }
        }

        private static class NativeMethods {
            public const uint ECM_FIRST = 0x1500;
            public const uint EM_SETCUEBANNER = ECM_FIRST + 1;
            public const int WM_NCPAINT = 0x85;

            [DllImport("user32")]
            public static extern IntPtr GetWindowDC(IntPtr hwnd);

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wp, string lp);
        }

        private sealed class StyledTextBox : TextBox {
            private static readonly Pen _unfocusedPen = new Pen(Color.FromArgb(230, 230, 230));
            private static readonly Pen _focusedPen = new Pen(Color.FromArgb(0, 120, 215));
            protected override void WndProc(ref Message m) {
                base.WndProc(ref m);
                if (m.Msg == NativeMethods.WM_NCPAINT) {
                    var dc = NativeMethods.GetWindowDC(Handle);
                    using (Graphics g = Graphics.FromHdc(dc)) {
                        g.DrawRectangle(Focused ? _focusedPen : _unfocusedPen, 0, 0, Width - 1, Height - 1);
                    }
                }
            }
        }

    }
}
