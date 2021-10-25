using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class WaitForm : ZForm {
        private readonly Stopwatch _stopwatch;
        private readonly Brush _brush;
        private readonly Pen _pen;
        private readonly float _penWidth;

        public Task WaitTask;
        public Exception ResultException { get; private set; }

        public WaitForm(string title, string text, Action action) {
            InitializeComponent();
            Text = title;
            _infoTxt.Text = text;
            _stopwatch = Stopwatch.StartNew();

            Ui ui = new(this, 65, 8);
            ui.Init(_table);
            ui.Init(_infoTxt);
            ui.Init(_spinner);
            ui.MarginRight(_spinner, 2);
            _spinner.Size = new(ui.XWidth(4), ui.XWidth(4));
            _spinner.EnableDoubleBuffering();

            _penWidth = Math.Max(1, ui.XWidth(0.4));
            _brush = new SolidBrush(Color.FromArgb(0, 122, 204));
            _pen = new(_brush, _penWidth);
            Disposed += delegate {
                _brush.Dispose();
                _pen.Dispose();
            };

            WaitTask = Task.Run(() => {
                try {
                    action();
                } catch (Exception ex) {
                    ResultException = ex;
                }
                while (!IsHandleCreated) {
                    Thread.Sleep(1);
                }
                BeginInvoke(new MethodInvoker(() => {
                    DialogResult = ResultException == null ? DialogResult.OK : DialogResult.Abort;
                    Close();
                }));
            });
        }

        private void Spinner_Paint(object sender, PaintEventArgs e) {
            var fraction = _stopwatch.Elapsed.TotalSeconds;
            fraction -= Math.Floor(fraction);
            var degrees = fraction * 360;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            e.Graphics.FillRectangle(Brushes.White, e.ClipRectangle);

            var size = _spinner.ClientSize;
            RectangleF rect = new(_penWidth, _penWidth, size.Width - 2 * _penWidth, size.Height - 2 * _penWidth);

            e.Graphics.DrawArc(_pen, rect, (float)degrees, 100);

            degrees += 180;
            degrees %= 360;

            e.Graphics.DrawArc(_pen, rect, (float)degrees, 100);
        }

        private void SpinnerTimer_Tick(object sender, EventArgs e) {
            _spinner.Invalidate();
        }
    }
}
