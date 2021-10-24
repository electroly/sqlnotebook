using System;
using System.Drawing;
using System.Windows.Forms;

namespace SqlNotebook {
    public sealed class SizeGripControl : Control {
        // These are set when drag starts.
        private Point _startingScreenLocation;
        private Size _startingParentSize;

        public SizeGripControl() {
            Cursor = Cursors.SizeNWSE;
        }

        protected override void OnPaint(PaintEventArgs e) {
            DrawSizeGripper(e.Graphics);
        }

        private void DrawSizeGripper(Graphics g) {
            g.FillRectangle(Brushes.Red, 0, 0, ClientSize.Width, ClientSize.Height);
            DrawOneColorOfGripper(g, SystemBrushes.Window, false);
            DrawOneColorOfGripper(g, SystemBrushes.ControlDark, true);
        }

        private void DrawOneColorOfGripper(Graphics g, Brush brush, bool offset) {
            var z = 2 * (float)DeviceDpi / 96;
            float w = ClientSize.Width;
            float h = ClientSize.Height;
            float extra = offset ? z / 2 : 0;

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

        protected override void OnDpiChangedAfterParent(EventArgs e) {
            base.OnDpiChangedAfterParent(e);
            SetSize();
        }

        protected override void OnParentChanged(EventArgs e) {
            base.OnParentChanged(e);
            SetSize();
        }

        //protected override void OnMouseDown(MouseEventArgs e) {
        //    base.OnMouseDown(e);
        //    Capture = true;
        //    _startingScreenLocation = PointToScreen(e.Location);
        //    _startingParentSize = Parent.Size;
        //}

        //protected override void OnMouseMove(MouseEventArgs e) {
        //    base.OnMouseMove(e);
        //    if (Capture) {
        //        var point = PointToScreen(e.Location);
        //        var deltaX = point.X - _startingScreenLocation.X;
        //        var deltaY = point.Y - _startingScreenLocation.Y;
        //        Parent.Size = new(_startingParentSize.Width + deltaX, _startingParentSize.Height + deltaY);
        //        Parent.Invalidate();
        //        Parent.Update();
        //    }
        //}

        //protected override void OnMouseUp(MouseEventArgs e) {
        //    base.OnMouseUp(e);
        //    Capture = false;
        //}

        private void SetSize() {
            Anchor = AnchorStyles.None;
            var z = 2 * (float)DeviceDpi / 96;
            Size = new((int)(z * 8), (int)(z * 8));
            Location = new(
                Parent.ClientSize.Width - Size.Width,
                Parent.ClientSize.Height - Size.Height);
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        }
    }
}
