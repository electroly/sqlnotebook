using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
    internal class VS2012LightSplitterControl : DockPane.SplitterControlBase
    {
        private static readonly SolidBrush _horizontalBrush = new SolidBrush(Color.FromArgb(0xFF, 204, 206, 219));
        private static readonly Color[] _verticalSurroundColors = new[] { SystemColors.Control };


        public VS2012LightSplitterControl(DockPane pane)
            : base(pane)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Rectangle rect = ClientRectangle;

            if (rect.Width <= 0 || rect.Height <= 0)
                return;

            switch (Alignment)
            {
                case DockAlignment.Right:
                case DockAlignment.Left:
                    {
                       e.Graphics.FillRectangle(SystemBrushes.Control, rect);
                       e.Graphics.DrawRectangle(SystemPens.ControlDark, rect.X, rect.Y - 1, rect.Width - 1, rect.Height + 2);
                    }
                    break;
                case DockAlignment.Bottom:
                case DockAlignment.Top:
                    {
                        e.Graphics.FillRectangle(_horizontalBrush, rect.X, rect.Y,
                                        rect.Width, Measures.SplitterSize);
                    }
                    break;
            }
        }
    }
}