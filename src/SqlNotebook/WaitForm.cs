﻿using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class WaitForm : ZForm {
        private readonly Stopwatch _stopwatch;
        private readonly Brush _brush;
        private readonly Pen _pen;
        private readonly float _penWidth;
        private readonly System.Windows.Forms.Timer _timer;
        
        // Set this at startup.
        public static Form MainAppForm { private get; set; }

        public Task WaitTask;
        public Exception ResultException { get; private set; }

        public static void Go(IWin32Window owner, string title, string text, out bool success, Action action) {
            success = false;

            // Try running the action briefly before pulling up the wait form, to avoid a flicker of the wait form for
            // very fast tasks.
            var task = Task.Run(action);
            task.Wait(millisecondsTimeout: 20);

            if (task.IsCompleted) {
                try {
                    task.GetAwaiter().GetResult();
                } catch (Exception ex) {
                    MessageForm.ShowError(owner, title, ex.Message);
                    return;
                }
            } else {
                using WaitForm f = new(title, text, () => task.GetAwaiter().GetResult());
                var result = f.ShowDialog(owner);
                if (result != DialogResult.OK) {
                    MessageForm.ShowError(owner, title, f.ResultException.Message);
                    return;
                }
            }

            success = true;
            return;
        }

        public static T Go<T>(IWin32Window owner, string title, string text, out bool success, Func<T> func) {
            T result = default;
            Go(owner, title, text, out success, action: () => {
                result = func();
            });
            return result;
        }

        private WaitForm(string title, string text, Action action) {
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

            if (TaskbarManager.IsPlatformSupported) {
                // It seems like the indeterminate progressbar state isn't shown on Windows 10. Let's fake a progress
                // by asymptotically approaching 90% with a hand-tweaked curve that seems to be OK most of the time.
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal, MainAppForm.Handle);
                var stopwatch = Stopwatch.StartNew();
                _timer = new() {
                    Interval = 16,
                    Enabled = true
                };
                var lastValue = 0;
                _timer.Tick += delegate {
                    try {
                        var value = (int)(90 * (1 - Math.Exp(0.01 * stopwatch.ElapsedMilliseconds * Math.Log(0.9))));
                        if (value != lastValue) {
                            TaskbarManager.Instance.SetProgressValue(value, 100);
                            lastValue = value;
                        }
                    } catch { }
                };
                FormClosed += delegate {
                    try {
                        _timer.Stop();
                        TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress, MainAppForm.Handle);
                    } catch { }
                };
                Disposed += delegate { _timer.Dispose(); };
            }
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
