// SQL Notebook
// Copyright (C) 2016 Brian Luft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class ExplorerControl : UserControl {
        private readonly NotebookManager _manager;
        private readonly ImageList _paddedImageList;

        public ExplorerControl(NotebookManager manager) {
            InitializeComponent();
            _manager = manager;
            _manager.NotebookChange += (sender, e) => HandleNotebookChange(e);

            _paddedImageList = new ImageList {
                ImageSize = new Size(25, 17),
                ColorDepth = ColorDepth.Depth32Bit
            };
            foreach (Image image in _imageList.Images) {
                var newImage = new Bitmap(25, 17, image.PixelFormat);
                using (var g = Graphics.FromImage(newImage)) {
                    g.DrawImage(image, 7, 1);
                }
                _paddedImageList.Images.Add(newImage);
            }
            _list.SmallImageList = _paddedImageList;
        }

        private void HandleNotebookChange(NotebookChangeEventArgs e) {
            BeginInvoke(new MethodInvoker(() => {
                _list.BeginUpdate();
                foreach (var item in e.RemovedItems) {
                    foreach (ListViewItem lvi in _list.Items) {
                        if (lvi.Text == item.Name) {
                            lvi.Remove();
                            break;
                        }
                    }
                }
                foreach (var item in e.AddedItems) {
                    var lvi = _list.Items.Add(item.Name);
                    lvi.Group = _list.Groups[item.Type.ToString()];
                    lvi.ImageIndex = (int)item.Type;
                }
                _list.Sort();
                _list.EndUpdate();
            }));
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            _list.Columns[0].Width = _list.Width - SystemInformation.VerticalScrollBarWidth - 5;
        }

        private void List_ItemActivate(object sender, EventArgs e) {
            if (_list.SelectedItems.Count != 1) {
                return;
            }
            var lvi = _list.SelectedItems[0];
            var type = (NotebookItemType)Enum.Parse(typeof(NotebookItemType), lvi.Group.Name);
            _manager.Open(new NotebookItem(type, lvi.Text));
        }
    }
}
