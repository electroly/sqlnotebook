using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlNotebook.Properties;

namespace SqlNotebook {
    public sealed class MessageForm {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public IReadOnlyList<string> Buttons { get; set; }
        public Image Icon { get; set; }

        private const int MAX_WIDTH = 350;

        public static void ShowError(IWin32Window owner, string title, string message, string details = null) {
            var d = new MessageForm {
                Title = title,
                Message = message,
                Details = details,
                Icon = Resources.Exclamation32,
                Buttons = new[] { "OK" }
            };
            d.ShowDialog(owner);
        }

        public string ShowDialog(IWin32Window owner) {
            using (var f = new Form()) {
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.MinimizeBox = false;
                f.MaximizeBox = false;
                f.ShowInTaskbar = false;
                f.BackColor = SystemColors.Window;
                f.StartPosition = FormStartPosition.CenterParent;
                f.Text = Title;

                int leftMargin = 11;
                if (Icon != 
                    null) {
                    var box = new PictureBox {
                        Left = 7,
                        Top = 7,
                        Width = 32,
                        Height = 32,
                        Image = Icon
                    };
                    f.Controls.Add(box);
                    leftMargin = box.Right + 7;
                }

                int y = 10;

                var mainInstructionFont = new Font("Segoe UI", 12);
                var mainInstructionColor = Color.FromArgb(0x003399);
                var regularFont = new Font("Segoe UI", 9);

                AddLabels(f, leftMargin, ref y, Message, mainInstructionFont, mainInstructionColor, 0);

                if (Details != null && Details.Any()) {
                    y += 10;
                    AddLabels(f, leftMargin, ref y, Details, regularFont, Color.Black, 1);
                }

                y += 71;

                var controls = f.Controls.Cast<Control>();
                f.ClientSize = new Size(
                    Math.Max(MAX_WIDTH, controls.Select(x => x.Right).Max()) + leftMargin,
                    y
                );

                var buttonPanelHeight = 42;

                var separator = new Panel {
                    Left = 0, Top = f.ClientSize.Height - buttonPanelHeight - 1, Width = f.Width, Height = 1,
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                    BackColor = Color.FromArgb(unchecked((int)0xFFDFDFDF))
                };
                f.Controls.Add(separator);

                var buttonPanel = new Panel {
                    Left = 0, Top = f.ClientSize.Height - buttonPanelHeight, Width = f.Width, Height = buttonPanelHeight, 
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                    BackColor = Color.FromArgb(unchecked((int)0xFFF0F0F0))
                };
                f.Controls.Add(buttonPanel);

                var buttonCtls = new List<Button>();
                string clickedButton = null;
                foreach (var btnText in Buttons) {
                    var button = new Button {
                        Top = 7, Height = 26, Anchor = AnchorStyles.Bottom | AnchorStyles.Right, Text = btnText,
                        Font = regularFont, UseVisualStyleBackColor = true
                    };
                    button.Width = Math.Max(88, TextRenderer.MeasureText(btnText, regularFont).Width + 25);
                    button.Click += (sender, e) => {
                        clickedButton = button.Text;
                        f.Close();
                    };
                    buttonCtls.Add(button);
                }

                var buttonSpacing = 7;
                var buttonsWidth = buttonCtls.Sum(x => x.Width) + 7 * (buttonCtls.Count - 1);
                var buttonsLeft = f.ClientSize.Width - 11 - buttonsWidth;
                foreach (var ctl in buttonCtls) {
                    ctl.Left = buttonsLeft;
                    buttonsLeft += ctl.Width + buttonSpacing;
                    buttonPanel.Controls.Add(ctl);
                }
                f.AcceptButton = buttonCtls.First();
                f.CancelButton = buttonCtls.Last();

                f.ShowDialog(owner);
                return clickedButton;
            }
        }

        private static void AddLabels(Form f, int leftMargin, ref int y, string message, Font font, Color color,
        int lineGap) {
            var messageWords = new Stack<string>(message.Split(' ').Reverse());
            var currentSize = new Size(0, 0);
            string currentLine = "";
            while (messageWords.Any()) {
                var word = messageWords.Pop();
                string appended = !currentLine.Any() ? word : (currentLine + " " + word);
                var appendedSize = TextRenderer.MeasureText(appended, font);
                if (appendedSize.Width > MAX_WIDTH) {
                    if (!currentLine.Any()) {
                        // this word by itself is longer than the max, so allow it to exceed the max
                        currentLine = appended;
                    } else {
                        // we don't have enough room on this line to add this word
                        messageWords.Push(word);
                    }

                    AddLabelCore(f, leftMargin, ref y, font, color, ref currentSize, ref currentLine, lineGap);
                } else {
                    // there is room for this word
                    currentLine = appended;
                    currentSize = appendedSize;
                }
            }

            AddLabelCore(f, leftMargin, ref y, font, color, ref currentSize, ref currentLine, lineGap);
        }

        private static void AddLabelCore(Form f, int leftMargin, ref int y, Font font, Color color,
        ref Size currentSize, ref string currentLine, int lineGap) {
            var label = new Label {
                Font = font,
                ForeColor = color,
                Text = currentLine,
                Left = leftMargin,
                Top = y,
                AutoSize = true
            };
            f.Controls.Add(label);
            y += currentSize.Height + lineGap;
            currentLine = "";
            currentSize = new Size(0, 0);
        }
    }
}
