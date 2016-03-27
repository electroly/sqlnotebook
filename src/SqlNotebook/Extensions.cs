using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlNotebook {
    public static class Extensions {
        private const int WM_SETREDRAW = 0x0B;

        public static void BeginUpdate(this RichTextBox self) {
            NativeMethods.SendMessage(self.Handle, WM_SETREDRAW, (IntPtr)0, IntPtr.Zero);
        }

        public static void EndUpdate(this RichTextBox self) {
            NativeMethods.SendMessage(self.Handle, WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
            self.Invalidate();
        }

        public static void SetInnerMargins(this TextBoxBase textBox, int left, int top, int right, int bottom) {
            var rect = textBox.GetFormattingRect();

            var newRect = new Rectangle(left, top, rect.Width - left - right, rect.Height - top - bottom);
            textBox.SetFormattingRect(newRect);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT {
            public readonly int Left;
            public readonly int Top;
            public readonly int Right;
            public readonly int Bottom;

            private RECT(int left, int top, int right, int bottom) {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }
        }

        private const int EM_GETRECT = 0xB2;
        private const int EM_SETRECT = 0xB3;

        private static void SetFormattingRect(this TextBoxBase textbox, Rectangle rect) {
            var rc = new RECT(rect);
            NativeMethods.SendMessageRefRect(textbox.Handle, EM_SETRECT, 0, ref rc);
        }

        private static Rectangle GetFormattingRect(this TextBoxBase textbox) {
            var rect = new Rectangle();
            NativeMethods.SendMessage(textbox.Handle, EM_GETRECT, (IntPtr)0, ref rect);
            return rect;
        }

        private static class NativeMethods {
            [DllImport("user32.dll")]
            public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

            [DllImport("user32.dll", EntryPoint = @"SendMessage", CharSet = CharSet.Auto)]
            public static extern int SendMessageRefRect(IntPtr hWnd, uint msg, int wParam, ref RECT rect);

            [DllImport("user32.dll", EntryPoint = @"SendMessage", CharSet = CharSet.Auto)]
            public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, ref Rectangle lParam);
        }


    }
}
