using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;

namespace SqlNotebook.Pages {
    public sealed class PageControl : UserControl, IDocumentControl {
        private readonly NotebookManager _manager;
        private readonly IWin32Window _mainForm;

        public readonly Panel _scrollPanel;
        public readonly FlowLayoutPanel _flow;

        public PageControl(string name, NotebookManager manager, IWin32Window mainForm) {
            ItemName = name;
            _manager = manager;
            _mainForm = mainForm;

            Controls.Add(_scrollPanel = new() {
                Dock = DockStyle.Fill,
                AutoScroll = true,
            });
            _scrollPanel.Controls.Add(_flow = new() {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Margin = Padding.Empty,
            });

            DividerBlockControl divider = new();
            divider.AddBlock += Divider_AddPart;
            InsertBlock(divider, 0);

            Ui ui = new(this, padded: false);
            ui.Init(_scrollPanel);
            ui.Init(_flow);

            UserOptions.OnUpdateAndNow(this, () => {
                var opt = UserOptions.Instance;
                var colors = opt.GetColors();
                BackColor = colors[UserOptionsColor.GRID_BACKGROUND];
            });

            UserOptions.OnUpdatePost(this, () => {
                OnSizeChanged(EventArgs.Empty);
            });

            OnSizeChanged(EventArgs.Empty);

            LoadPage();
        }

        private void Divider_AddPart(object sender, AddBlockEventArgs e) {
            BlockControl block = e.Type switch {
                BlockType.Query => new QueryBlockControl(_manager),
                BlockType.Text => new TextBlockControl(),
                _ => throw new NotImplementedException(),
            };
            var dividerPartIndex = GetPartIndex(e.Divider);
            InsertBlock(block, dividerPartIndex + 1);

            DividerBlockControl bottomDivider = new();
            bottomDivider.AddBlock += Divider_AddPart;
            InsertBlock(bottomDivider, dividerPartIndex + 2);

            block.StartEditing();
        }

        private int GetPartIndex(BlockControl part) => _flow.Controls.GetChildIndex(part);

        public string ItemName { get; set; }

        private void LoadPage() {
            try {
                var serializedData = _manager.GetItemData(ItemName);
                if (string.IsNullOrEmpty(serializedData)) {
                    return; // New page; nothing to load
                }

                using MemoryStream memoryStream = new(Convert.FromBase64String(serializedData));
                using GZipStream gzipStream = new(memoryStream, CompressionMode.Decompress);
                using BinaryReader reader = new(gzipStream);

                var numBlocks = reader.ReadInt32();

                var index = 1; // Start at 1 because the top divider is already in place.
                for (var i = 0; i < numBlocks; i++) {
                    var blockType = reader.ReadByte();
                    BlockControl blockControl = blockType switch {
                        1 => new TextBlockControl(),
                        2 => new QueryBlockControl(_manager),
                        _ => throw new Exception($"The file is corrupt.")
                    };

                    blockControl.Deserialize(reader);
                    InsertBlock(blockControl, index++);

                    DividerBlockControl bottomDivider = new();
                    bottomDivider.AddBlock += Divider_AddPart;
                    InsertBlock(bottomDivider, index++);
                }

                OnSizeChanged(EventArgs.Empty);
            } catch (Exception ex) {
                Ui.ShowError(TopLevelControl, "Page Error", ex);
            }
        }

        public void Save() {
            using MemoryStream memoryStream = new();
            using (GZipStream gzipStream = new(memoryStream, CompressionLevel.Fastest)) {
                using BinaryWriter writer = new(gzipStream);

                var blockControls =
                    _flow.Controls.Cast<BlockControl>()
                    .Where(x => x is TextBlockControl || x is QueryBlockControl)
                    .ToList();

                writer.Write(blockControls.Count);

                foreach (var control in blockControls) {
                    writer.Write((byte)(
                        control switch {
                            TextBlockControl => 1,
                            QueryBlockControl => 2,
                            _ => throw new NotImplementedException()
                        }));
                    control.Serialize(writer);
                }
            }

            _manager.SetItemData(ItemName, Convert.ToBase64String(memoryStream.ToArray()));
        }

        protected override void OnSizeChanged(EventArgs e) {
            base.OnSizeChanged(e);
            _scrollPanel.HorizontalScroll.Visible = false;
            _scrollPanel.HorizontalScroll.Enabled = false;
            var w = _scrollPanel.ClientSize.Width;
            foreach (BlockControl control in _flow.Controls) {
                control.Width = w;
                control.Height = control.CalculateHeight();
            }
        }

        public void InsertBlock(BlockControl block, int index) {
            block.AutoSize = false;
            block.Margin = Padding.Empty;
            block.Size = new(_scrollPanel.ClientSize.Width, block.CalculateHeight());
            block.BlockDeleted += DeletePart;
            _flow.Controls.Add(block);
            _flow.Controls.SetChildIndex(block, index);
            OnSizeChanged(EventArgs.Empty);

            block.BlockClicked += Block_BlockClicked;
            block.BlockMoved += Block_BlockMoved;
        }

        private void Block_BlockClicked(object sender, EventArgs e) => OnSizeChanged(EventArgs.Empty);

        private void Block_BlockMoved(BlockControl block, bool up) {
            var controlIndex = _flow.Controls.IndexOf(block);
            if (up) {
                // Topmost is index 1, second is index 3, etc. because of the dividers in between each one..
                if (controlIndex >= 3) {
                    var partner = _flow.Controls[controlIndex - 2];
                    _flow.Controls.SetChildIndex(block, controlIndex - 2);
                    _flow.Controls.SetChildIndex(partner, controlIndex);
                }
            } else {
                var lastControlIndex = _flow.Controls.Count - 2; // skip the last divider
                if (controlIndex < lastControlIndex) {
                    var partner = _flow.Controls[controlIndex + 2];
                    _flow.Controls.SetChildIndex(partner, controlIndex);
                    _flow.Controls.SetChildIndex(block, controlIndex + 2);
                }
            }
        }

        public void DeletePart(BlockControl part) {
            var index = _flow.Controls.IndexOf(part);
            _flow.Controls.RemoveAt(index); // the part itself
            _flow.Controls.RemoveAt(index); // the divider after the part
            OnSizeChanged(EventArgs.Empty);
        }
    }
}
