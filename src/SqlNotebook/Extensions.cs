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

        public static void BeginTaskbarProgress(this IWin32Window window) {
            try {
                SetTaskbarProgressState(window.Handle, TaskbarStates.Indeterminate);
            } catch (Exception) { }
        }

        public static void EndTaskbarProgress(this IWin32Window window) {
            try {
                SetTaskbarProgressState(window.Handle, TaskbarStates.NoProgress);
            } catch (Exception) { }
        }

        // https://github.com/SlavaRa/flashdevelop-plugins/blob/5eb08b6b9afda8450135224c28bd0457e69504d2/External/Tools/AppMan/Utilities/TaskbarProgress.cs
        private enum TaskbarStates {
            NoProgress = 0,
            Indeterminate = 0x1,
            Normal = 0x2,
            Error = 0x4,
            Paused = 0x8
        }

        [ComImport, Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface ITaskbarList3 {
            // ITaskbarList
            [PreserveSig]
            void HrInit();
            [PreserveSig]
            void AddTab(IntPtr hwnd);
            [PreserveSig]
            void DeleteTab(IntPtr hwnd);
            [PreserveSig]
            void ActivateTab(IntPtr hwnd);
            [PreserveSig]
            void SetActiveAlt(IntPtr hwnd);
            // ITaskbarList2
            [PreserveSig]
            void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);
            // ITaskbarList3
            [PreserveSig]
            void SetProgressValue(IntPtr hwnd, UInt64 ullCompleted, UInt64 ullTotal);
            [PreserveSig]
            void SetProgressState(IntPtr hwnd, TaskbarStates state);
        }

        [ComImport, Guid("56FDF344-FD6D-11d0-958A-006097C9A090"), ClassInterface(ClassInterfaceType.None)]
        private class TaskbarInstance { }

        private static ITaskbarList3 _taskbar = (ITaskbarList3)new TaskbarInstance();
        private static bool _taskbarSupported = Environment.OSVersion.Version >= new Version(6, 1);

        private static void SetTaskbarProgressState(IntPtr hwnd, TaskbarStates state) {
            if (_taskbarSupported)
                _taskbar.SetProgressState(hwnd, state);
        }

        private static void SetTaskbarProgressValue(IntPtr hwnd, double value) {
            if (_taskbarSupported)
                _taskbar.SetProgressValue(hwnd, (ulong)(value * 1000), 1000);
        }

        public static string DoubleQuote(this string str) {
            return $"\"{str.Replace("\"", "\"\"")}\"";
        }
    }
}
