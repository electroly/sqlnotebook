using SqlNotebookScript.Utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SqlNotebook {
    public sealed class Ui {
        private readonly float _dpi;
        private readonly SizeF _x;
        private readonly Padding _buttonPadding;
        private int _tabIndex = 1;

        private static readonly Font _consolas = new("Consolas", 11f);

        private Ui(Control control, bool padded, bool autoSize) {
            using var graphics = control.CreateGraphics();
            _x = graphics.MeasureString("x", control.Font, PointF.Empty, StringFormat.GenericTypographic);
            _dpi = control.DeviceDpi;
            var buttonPaddingX = (int)(_x.Width * 3);
            var buttonPaddingY = (int)(_x.Height * 0.25);
            _buttonPadding = new(buttonPaddingX, buttonPaddingY, buttonPaddingX, buttonPaddingY);

            if (padded) {
                var formPaddingX = (int)(_x.Width * 2);
                var formPaddingY = (int)(_x.Height * 0.75);
                control.Padding = new(formPaddingX, formPaddingY, formPaddingX, formPaddingY);
            }

            if (autoSize && control is Form form) {
                form.AutoSize = true;
                form.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            }
        }

        public Ui(Control control, bool padded = true) : this(control, padded, true) {
        }

        public Ui(Form form, double xWidth, double xHeight, bool padded = true) : this(form, padded, false) {
            Size size = new(XWidth(xWidth), XHeight(xHeight));
            Size maxSize = new(size.Width * 10, size.Height * 10);

            form.AutoSize = false;
            form.Size = size;
            form.MinimumSize = size;
            form.MaximumSize = maxSize;
            form.SizeGripStyle = SizeGripStyle.Show;
        }

        public Size XSize(double x, double y) => new(XWidth(x), XHeight(y));
        public int XWidth(double x) => (int)(_x.Width * x);
        public int XHeight(double y) => (int)(_x.Height * y);

        public static int XWidth(double x, Control control) {
            using var g = control.CreateGraphics();
            var size = g.MeasureString("x", control.Font, PointF.Empty, StringFormat.GenericTypographic);
            return (int)(size.Width * x);
        }

        public static int XHeight(double x, Control control) {
            using var g = control.CreateGraphics();
            var size = g.MeasureString("x", control.Font, PointF.Empty, StringFormat.GenericTypographic);
            return (int)(size.Height * x);
        }

        public int SizeGripHeight {
            get => (int)(2 * _dpi / 96) * 8;
        }

        public void Init(Label label) {
            label.TabIndex = _tabIndex++;
            var margin = label.Margin;
            label.Margin = new(0, margin.Top, margin.Right, margin.Bottom);
        }

        public void InitHeader(Label label) {
            Init(label);
            label.BackColor = Color.FromArgb(0, 122, 204);
            label.ForeColor = Color.White;
            label.Padding = new(XWidth(0.6), XHeight(0.2), XWidth(0.5), XHeight(0.2));
            label.Margin = Padding.Empty;
            label.Dock = DockStyle.Fill;
        }

        public void Init(LinkLabel label) {
            Init((Label)label);
        }

        public void Init(Button button) {
            button.TabIndex = _tabIndex++;
            button.AutoSize = true;
            button.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button.Padding = _buttonPadding;
        }

        public void Init(Button button, Image lodpi, Image hidpi) {
            Init(button);
            button.Image = GetScaledIcon(button, lodpi, hidpi, _dpi);
        }

        public void Init(TableLayoutPanel table) {
            table.TabIndex = _tabIndex++;
            table.AutoSize = true;
            table.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            table.Margin = Padding.Empty;
        }

        public void Init(FlowLayoutPanel flow) {
            flow.TabIndex = _tabIndex++;
            flow.AutoSize = true;
            flow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flow.Margin = Padding.Empty;
        }

        public void Init(Panel panel) {
            panel.Margin = Padding.Empty;
            panel.TabIndex = _tabIndex++;
        }

        public void Init(CheckBox check) {
            check.TabIndex = _tabIndex++;
            check.AutoSize = true;
        }

        public void Init(RadioButton radio) {
            radio.TabIndex = _tabIndex++;
            radio.AutoSize = true;
        }

        public void Init(TextBox textbox, int width = 25) {
            textbox.AutoSize = true;
            textbox.TabIndex = _tabIndex++;
            textbox.Width = XWidth(width);
            textbox.Font = _consolas;
        }

        public void Init(ListView listView) {
            listView.TabIndex = _tabIndex++;
            
        }

        public void Init(TabControl tabs) {
            tabs.TabIndex = _tabIndex++;
            tabs.Padding = new(XWidth(2), XHeight(0.25));
            tabs.Margin = Padding.Empty;
        }

        public void Init(TabPage tab) {
            tab.TabIndex = _tabIndex++;
        }

        public void Init(PropertyGrid grid) {
            grid.TabIndex = _tabIndex++;
        }

        public void Init(ToolStripButton button, Image lodpi, Image hidpi) {
            button.Image = GetScaledIcon(button, lodpi, hidpi, _dpi);
            button.ImageScaling = ToolStripItemImageScaling.None;
            button.AutoSize = true;
        }

        public void Init(ToolStripMenuItem item, Image lodpi, Image hidpi) {
            item.Image = GetScaledIcon(item, lodpi, hidpi, _dpi);
            item.ImageScaling = ToolStripItemImageScaling.None;
        }

        private static Image GetScaledIcon(Component button, Image lodpi, Image hidpi, float dpi) {
            var scale = dpi / 96;
            Image image;
            if (scale <= 1) {
                image = lodpi;
            } else {
                var scaledWidth = 16 * scale;
                Bitmap bmp = new((int)Math.Ceiling(scaledWidth), (int)Math.Ceiling(scaledWidth));
                button.Disposed += delegate { bmp.Dispose(); };
                using (var bmpG = Graphics.FromImage(bmp)) {
                    bmpG.DrawImage(hidpi, new RectangleF(0, 0, scaledWidth, scaledWidth));
                }
                image = bmp;
            }

            return image;
        }

        public void Init(CueToolStripTextBox textbox, int width = 25) {
            textbox.Width = XWidth(width);
        }

        public void Init(StatusStrip strip) {
            strip.TabIndex = _tabIndex++;
        }

        public void Init(ToolStripProgressBar bar) {
            bar.AutoSize = false;
            bar.Width = XWidth(15);
        }

        public void Init(ProgressBar bar) {
            bar.AutoSize = false;
            bar.Width = XWidth(15);
            bar.TabIndex = _tabIndex++;
        }

        public void Init(NumericUpDown upd, int width = 10) {
            upd.Width = XWidth(width);
            upd.TabIndex = _tabIndex++;
        }

        public void Init(ComboBox cmb, int width = 50) {
            cmb.Width = XWidth(width);
            cmb.TabIndex = _tabIndex++;
            if (cmb.DropDownStyle != ComboBoxStyle.DropDownList) {
                cmb.Font = _consolas;
            }
        }

        public void Init(DataGridViewColumn col, int width) {
            col.Width = XWidth(width);
        }

        public void Init(SplitContainer split, double dividerFraction) {
            split.Cursor = Cursors.Default;
            split.SplitterWidth = XWidth(1);
            if (split.Orientation == Orientation.Horizontal) {
                split.SplitterDistance = (int)(split.Height * dividerFraction);
            } else {
                split.SplitterDistance = (int)(split.Width * dividerFraction);
            }
            split.TabIndex = _tabIndex++;
        }

        public void MarginTop(Control control, double height = 0.75) {
            var m = control.Margin;
            m.Top = XHeight(height);
            control.Margin = m;
        }

        public void MarginBottom(Control control, double height = 0.75) {
            var m = control.Margin;
            m.Bottom = XHeight(height);
            control.Margin = m;
        }

        public void MarginRight(Control control, double width = 1) {
            var m = control.Margin;
            m.Right = XWidth(width);
            control.Margin = m;
        }

        public void Pad(Control control) {
            var x = XWidth(1);
            var y = XHeight(0.375);
            control.Padding = new(x, y, x, y);
        }

        public void PadBig(Control control) {
            // same as form padding
            var x = XWidth(2);
            var y = XHeight(0.75);
            control.Padding = new(x, y, x, y);
        }

        public const string OK = "OK";
        public const string CANCEL = "Cancel";
        public const string SAVE = "Save";
        public const string DONT_SAVE = "Don't save";
        public const string DELETE = "Delete";
        public const string INSTALL = "Install";
        public const string RELEASE_NOTES = "View release notes";

        public static string ShowTaskDialog(IWin32Window owner, string heading, string caption,
            string[] buttons, TaskDialogIcon icon = null, bool defaultIsFirst = true
            ) {
            TaskDialogPage taskDialogPage = new() {
                Heading = heading,
                Caption = caption,
                Icon = icon,
            };
            foreach (var button in buttons) {
                taskDialogPage.Buttons.Add(button);
            }
            taskDialogPage.DefaultButton = taskDialogPage.Buttons[defaultIsFirst ? 0 : buttons.Length - 1];
            var result = TaskDialog.ShowDialog(owner, taskDialogPage);
            return result.Text;
        }

        public static void ShowError(IWin32Window owner, string title, string message, string details = null) {
            TaskDialogPage taskDialogPage = new() {
                Heading = message,
                Caption = title,
                Icon = TaskDialogIcon.Error,
            };
            if (details != null) {
                taskDialogPage.Text = details;
            }
            taskDialogPage.Buttons.Add("OK");
            taskDialogPage.DefaultButton = taskDialogPage.Buttons[0];
            TaskDialog.ShowDialog(owner, taskDialogPage);
        }

        public static void ShowError(IWin32Window owner, string title, Exception exception) {
            var extended = exception as ExceptionEx;
            var message = exception.GetExceptionMessage();
            string details = null;
            if (message.Contains("\r\n\r\n")) {
                var parts = message.Split("\r\n\r\n", 2);
                message = parts[0];
                details = parts[1];
            }
            if (extended != null) {
                details = extended.Details;
            }

            TaskDialogPage taskDialogPage = new() {
                Heading = extended?.Heading ?? message,
                Caption = title,
                Icon = TaskDialogIcon.Error,
            };
            if (details != null) {
                taskDialogPage.Text = details;
            }

            taskDialogPage.Buttons.Add("OK");
            taskDialogPage.DefaultButton = taskDialogPage.Buttons[0];
            TaskDialog.ShowDialog(owner, taskDialogPage);
        }
    }
}
