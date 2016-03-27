using System;
using System.Collections.Generic;
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

        private static class NativeMethods {
            [DllImport("user32.dll")]
            public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        }
    }
}
