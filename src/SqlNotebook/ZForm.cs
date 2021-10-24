using System;
using System.Drawing;
using System.Windows.Forms;

namespace SqlNotebook {
    /// <summary>
    /// Various bug fixes in the framework's Form class. We need to draw our own size gripper because the framework's
    /// gripper is not DPI aware. We also need the actual hit box for the gripper to resize based on DPI.
    /// </summary>
    public abstract class ZForm : Form {
        private const int WM_NCHITTEST = 0x0084;
        private const int HTBOTTOMRIGHT = 17;

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            
            // This must happen in OnPaint, not in the Paint event handler, because we need to paint over the
            // framework's gripper which it draws after firing the Paint event.
            if (FormBorderStyle == FormBorderStyle.Sizable && SizeGripStyle != SizeGripStyle.Hide) {
                DrawSizeGripper(e.Graphics);
            }
        }

        private void DrawSizeGripper(Graphics g) {
            var z = GetSizeGripDotLength();
            g.FillRectangle(SystemBrushes.Control, ClientSize.Width - z * 8, ClientSize.Height - z * 8, z * 8, z * 8);
            DrawOneColorOfGripper(g, SystemBrushes.Window, false);
            DrawOneColorOfGripper(g, SystemBrushes.ControlDark, true);
        }

        private int GetSizeGripDotLength() => (int)(2 * (float)DeviceDpi / 96);

        private void DrawOneColorOfGripper(Graphics g, Brush brush, bool offset) {
            var z = GetSizeGripDotLength();
            var w = ClientSize.Width;
            var h = ClientSize.Height;
            var extra = offset ? z / 2 : 0;

            // Draw something like this:
            //       .
            //     . .
            //   . . .

            // Top row
            g.FillRectangle(brush, w - z * 2 - extra, h - z * 6 - extra, z, z);

            // Middle row
            g.FillRectangle(brush, w - z * 2 - extra, h - z * 4 - extra, z, z);
            g.FillRectangle(brush, w - z * 4 - extra, h - z * 4 - extra, z, z);

            // Bottom row
            g.FillRectangle(brush, w - z * 2 - extra, h - z * 2 - extra, z, z);
            g.FillRectangle(brush, w - z * 4 - extra, h - z * 2 - extra, z, z);
            g.FillRectangle(brush, w - z * 6 - extra, h - z * 2 - extra, z, z);
        }

        protected override void WndProc(ref Message m) {
            // Intercept WM_NCHITTEST so we can enlarge the size grip area based on DPI.
            if (m.Msg == WM_NCHITTEST &&
                FormBorderStyle == FormBorderStyle.Sizable &&
                SizeGripStyle != SizeGripStyle.Hide
                ) {
                var point = PointToClient(new(LeastSignificantWord(m.LParam), MostSignificantWord(m.LParam)));
                var formSize = ClientSize;
                var gripSize = 8 * GetSizeGripDotLength();

                if (point.X >= formSize.Width - gripSize && point.Y >= formSize.Height - gripSize && formSize.Height >= gripSize) {
                    m.Result = (IntPtr)HTBOTTOMRIGHT;
                    return;
                }
            }

            base.WndProc(ref m);
        }

        private static int MostSignificantWord(IntPtr n) => (n.ToInt32() >> 16) & 0xFFFF;
        private static int LeastSignificantWord(IntPtr n) => n.ToInt32() & 0xFFFF;
    }
}
