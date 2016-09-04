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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SqlNotebook.Properties;
using SqlNotebookScript.Utils;

namespace SqlNotebook {
    public partial class ExplorerControl : UserControl {
        private readonly NotebookManager _manager;
        private readonly ImageList _paddedImageList;
        private readonly IWin32Window _mainForm;
        private readonly Slot<bool> _operationInProgress;

        public ExplorerControl(NotebookManager manager, IWin32Window mainForm, Slot<bool> operationInProgress) {
            InitializeComponent();
            _list.EnableDoubleBuffer();
            _detailsLst.EnableDoubleBuffer();

            _mainForm = mainForm;
            _manager = manager;
            _operationInProgress = operationInProgress;
            _manager.NotebookChange += (sender, e) => HandleNotebookChange(e);
            _contextMenuStrip.Renderer = new MenuRenderer();

            _list.SmallImageList = _detailsLst.SmallImageList = _paddedImageList = _imageList.PadListViewIcons();

            _operationInProgress.Change += (a, b) => {
                if (a && !b) {
                    List_SelectedIndexChanged(null, EventArgs.Empty);
                }
            };

            _list.GotFocus += (sender, e) => {
                _manager.CommitOpenEditors();
                _manager.Rescan();
            };
        }

        private void HandleNotebookChange(NotebookChangeEventArgs e) {
            if (e.RemovedItems.Any() || e.AddedItems.Any()) {
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
                        if (item.IsVirtualTable) {
                            lvi.ImageIndex = 7;
                        } else {
                            lvi.ImageIndex = (int)item.Type;
                        }
                    }
                    _list.Sort();
                    _list.EndUpdate();
                }));
            }
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            _list.Columns[0].Width = _detailsLst.Columns[0].Width =
                _list.Width - SystemInformation.VerticalScrollBarWidth - 5;
        }

        private void List_ItemActivate(object sender, EventArgs e) {
            if (_list.SelectedItems.Count != 1) {
                return;
            }
            var lvi = _list.SelectedItems[0];
            var type = (NotebookItemType)Enum.Parse(typeof(NotebookItemType), lvi.Group.Name);
            _manager.OpenItem(new NotebookItem(type, lvi.Text));
        }

        private void DeleteMnu_Click(object sender, EventArgs e) {
            DoDelete();
        }

        private void DoDelete() {
            if (_list.SelectedItems.Count != 1) {
                return;
            }
            var lvi = _list.SelectedItems[0];
            var name = lvi.Text;
            var type = (NotebookItemType)Enum.Parse(typeof(NotebookItemType), lvi.Group.Name);

            // can't delete tables or views if an operation is in progress
            bool isTableOrView = type == NotebookItemType.Table || type == NotebookItemType.View;
            if (isTableOrView && _operationInProgress) {
                MessageForm.ShowError(_mainForm,
                    "Delete Item",
                    "Cannot delete tables or views while an operation is in progress.",
                    "Please wait until the current operation finishes, and then try again.");
                return;
            }

            var deleteBtn = "&Delete";
            var d = new MessageForm {
                Title = "Delete Item",
                Message = $"Are you sure you want to delete \"{name}\"?",
                Buttons = new[] { deleteBtn, "Cancel" },
                Icon = Resources.Warning32
            };
            if (d.ShowDialog(this) != deleteBtn) {
                return;
            }

            var item = new NotebookItem(type, name);
            _manager.CloseItem(item);

            new WaitForm("Delete", "Deleting the selected item...", () => {
                _manager.DeleteItem(item);
            }).ShowDialogAndDispose(this);

            _manager.Rescan(notebookItemsOnly: !isTableOrView);
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
            var isSelection = _list.SelectedItems.Count == 1;
            _openMnu.Enabled = _deleteMnu.Enabled = _renameMnu.Enabled = isSelection;
        }

        private void List_SelectedIndexChanged(object sender, EventArgs e) {
            _detailsLst.BeginUpdate();
            _detailsLst.Items.Clear();
            if (_list.SelectedItems.Count != 1) {
                _detailsLst.EndUpdate();
                return;
            }
            var lvi = _list.SelectedItems[0];
            var notebookItemName = lvi.Text;
            var n = _manager.Notebook;
            var details = new List<Tuple<string, string>>();

            try {
                if (lvi.Group.Name == "Table" || lvi.Group.Name == "View") {
                    var dt = n.SpecialReadOnlyQuery($"PRAGMA table_info ({lvi.Text.DoubleQuote()})",
                        new Dictionary<string, object>());
                    for (int i = 0; i < dt.Rows.Count; i++) {
                        var name = (string)dt.Get(i, "name");
                        var info = (string)dt.Get(i, "type");
                        info += Convert.ToInt32(dt.Get(i, "notnull")) != 0 ? " NOT NULL" : "";
                        info += Convert.ToInt32(dt.Get(i, "pk")) != 0 ? " PRIMARY KEY" : "";
                        details.Add(Tuple.Create(name, info));
                    }
                } else if (lvi.Group.Name == "Script") {
                    var paramRec = n.UserData.ScriptParameters.FirstOrDefault(x => x.ScriptName == notebookItemName);
                    if (paramRec != null) {
                        foreach (var paramName in paramRec.ParamNames) {
                            details.Add(Tuple.Create(paramName, "parameter"));
                        }
                    }
                }
            } catch (Exception) { }

            foreach (var detail in details) {
                var detailLvi = new ListViewItem(_detailsLst.Groups[0]);
                var text = detail.Item1;
                var extra = detail.Item2.Replace(" PRIMARY KEY", "");
                if (extra.Any()) {
                    text += $" ({extra})";
                }
                detailLvi.Text = text;
                if (detail.Item2.Contains(" PRIMARY KEY")) {
                    detailLvi.ImageIndex = 6;
                }
                _detailsLst.Items.Add(detailLvi);
            }
            _detailsLst.Groups[0].Header = notebookItemName;
            _detailsLst.EndUpdate();
        }

        private void RenameMnu_Click(object sender, EventArgs e) {
            if (_list.SelectedItems.Count == 1) {
                _list.SelectedItems[0].BeginEdit();
            }
        }

        private void List_AfterLabelEdit(object sender, LabelEditEventArgs e) {
            e.CancelEdit = true; // if this is a successful edit, we will update the label ourselves
            var lvi = _list.Items[e.Item];
            var type = (NotebookItemType)Enum.Parse(typeof(NotebookItemType), lvi.Group.Name);

            if (e.Label == null) {
                return; // user did not change the name
            }

            // can't delete tables or views if an operation is in progress
            bool isTableOrView = type == NotebookItemType.Table || type == NotebookItemType.View;
            if (isTableOrView && _operationInProgress) {
                MessageForm.ShowError(_mainForm,
                    "Delete Item",
                    "Cannot rename tables or views while an operation is in progress.",
                    "Please wait until the current operation finishes, and then try again.");
                return;
            }

            try {
                _manager.RenameItem(new NotebookItem(type, lvi.Text), e.Label);
            } catch (Exception ex) {
                MessageForm.ShowError(TopLevelControl, "Rename Error", ex.Message);
            }
        }

        private void List_KeyDown(object sender, KeyEventArgs e) {
            if (_list.SelectedItems.Count == 1) {
                if (e.KeyData == Keys.F2) {
                    _list.SelectedItems[0].BeginEdit();
                } else if (e.KeyCode == Keys.Delete) {
                    DoDelete();
                }
            }
        }
    }
}
