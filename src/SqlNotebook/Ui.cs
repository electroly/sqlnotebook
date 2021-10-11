﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SqlNotebook {
    public sealed class Ui {
        private readonly float _dpi;
        private readonly SizeF _x;
        private readonly Padding _buttonPadding;
        private int _tabIndex = 1;

        private static readonly Font _bigFont = new("Consolas", 11f);

        public Ui(Control control, bool padded = true) {
            using var graphics = control.CreateGraphics();
            _x = graphics.MeasureString("x", control.Font, PointF.Empty, StringFormat.GenericTypographic);
            _dpi = graphics.DpiX;
            var buttonPaddingX = (int)(_x.Width * 3);
            var buttonPaddingY = (int)(_x.Height * 0.25);
            _buttonPadding = new(buttonPaddingX, buttonPaddingY, buttonPaddingX, buttonPaddingY);

            if (padded) {
                var formPaddingX = (int)(_x.Width * 2);
                var formPaddingY = (int)(_x.Height * 0.75);
                control.Padding = new(formPaddingX, formPaddingY, formPaddingX, formPaddingY);
            }

            if (control is Form form) {
                form.AutoSize = true;
                form.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            }
        }

        public Ui(Form form, double xWidth, double xHeight, bool padded = true) : this(form, padded) {
            Size size = new(XWidth(xWidth), XHeight(xHeight));
            Size maxSize = new(size.Width * 10, size.Height * 10);

            form.AutoSize = false;
            form.Size = size;
            form.MinimumSize = size;
            form.MaximumSize = maxSize;
        }

        public Size XSize(double x, double y) => new(XWidth(x), XHeight(y));
        public int XWidth(double x) => (int)(_x.Width * x);
        public int XHeight(double y) => (int)(_x.Height * y);

        public void Init(Label label) {
            label.TabIndex = _tabIndex++;
        }

        public void Init(Button button) {
            button.TabIndex = _tabIndex++;
            button.AutoSize = true;
            button.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button.Padding = _buttonPadding;
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

        public void Init(CheckBox check) {
            check.TabIndex = _tabIndex++;
            check.AutoSize = true;
        }

        public void Init(RadioButton radio) {
            radio.TabIndex = _tabIndex++;
            radio.AutoSize = true;
        }

        public void Init(TextBox textbox, int width = 25) {
            textbox.AutoSize = false;
            textbox.TabIndex = _tabIndex++;
            textbox.Width = XWidth(width);
            textbox.Font = _bigFont;
        }

        public void Init(ListView listView) {
            listView.TabIndex = _tabIndex++;
            
        }

        public void Init(TabControl tabs) {
            tabs.TabIndex = _tabIndex++;
            tabs.Padding = new(XWidth(2), XHeight(0.25));
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
        }

        public void Init(ToolStripProgressBar bar) {
            bar.AutoSize = false;
            bar.Width = XWidth(15);
        }

        public void Init(ProgressBar bar) {
            bar.AutoSize = false;
            bar.Width = XWidth(15);
        }

        public void Init(NumericUpDown upd, int width = 10) {
            upd.Width = XWidth(width);
        }

        public void Init(ComboBox cmb, int width = 50) {
            cmb.Width = XWidth(width);
        }

        public void Init(DataGridViewColumn col, int width) {
            col.Width = XWidth(width);
        }

        public void MarginTop(Control control) {
            var m = control.Margin;
            m.Top = XHeight(0.75);
            control.Margin = m;
        }

        public void Pad(Control control) {
            var x = XWidth(1);
            var y = XHeight(0.375);
            control.Padding = new(x, y, x, y);
        }
    }
}