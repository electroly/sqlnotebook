﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SqlNotebook.Pages {
    public sealed class TextBlockControl : BlockControl {
        private SqlTextControl _textBox;

        public string BlockText { get; set; } = "";

        public TextBlockControl() {
            _hover.Click += Hover_Click;
        }

        private readonly StringFormat _stringFormat = new(StringFormatFlags.FitBlackBox);

        public override int CalculateHeight() {
            if (_editMode) {
                return this.Scaled(250);
            }

            var opt = UserOptions.Instance;
            using var g = CreateGraphics();
            var codeFont = opt.GetCodeFont();
            var textWidth = ClientSize.Width - 2 * HorizontalMargin;
            var size = g.MeasureString(
                BlockText.Replace("\r", "").Replace("\t", "    "),
                codeFont,
                textWidth,
                _stringFormat).ToSize();
            return Math.Max(this.Scaled(40), size.Height + 2 * VerticalMargin);
        }

        protected override void OnPaint(Graphics g, UserOptions opt, Color[] colors, Color backColor) {
            // User's text
            var codeFont = opt.GetCodeFont();
            var textWidth = ClientSize.Width - 2 * HorizontalMargin;
            using SolidBrush brush = new(colors[UserOptionsColor.GRID_PLAIN]);
            g.DrawString(
                BlockText.Replace("\r", "").Replace("\t", "    "),
                codeFont,
                brush,
                new RectangleF(
                    HorizontalMargin, VerticalMargin,
                    textWidth, ClientSize.Height
                ),
                _stringFormat);
        }

        private void Hover_Click(object sender, EventArgs e) {
            StartEditing();
        }

        public override void StartEditing() {
            if (_editMode) {
                return;
            }

            _editMode = true;
            Cursor = Cursors.Default;

            var (acceptButton, table, panel) = CreateStandardEditModeLayout();
            _textBox = new(readOnly: false, syntaxColoring: false, wrap: true, lineNumbers: true) {
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                BorderStyle = BorderStyle.FixedSingle,
                SqlText = BlockText,
            };
            panel.Controls.Add(_textBox);
            Controls.Add(table);
            _textBox.Focus();

            acceptButton.Click += delegate {
                StopEditing();
            };

            Height = CalculateHeight();
            Invalidate(true);
            RaiseBlockClicked();
        }

        private void StopEditing() {
            if (!_editMode) {
                return;
            }

            UpdatePropertiesFromEditMode();
            _editMode = false;
            Cursor = Cursors.Hand;
            for (var i = Controls.Count - 1; i >= 0; i--) {
                Controls.RemoveAt(i);
            }
            _textBox = null;
            Height = CalculateHeight();
            Invalidate(true);
            RaiseBlockClicked();
        }

        private void UpdatePropertiesFromEditMode() => BlockText = _textBox.SqlText;

        public override void Serialize(BinaryWriter writer) {
            if (_editMode) {
                UpdatePropertiesFromEditMode();
            }
            writer.Write(BlockText);
        }

        public override void Deserialize(BinaryReader reader) {
            BlockText = reader.ReadString();
            Height = CalculateHeight();
            Invalidate(true);
        }
    }
}
