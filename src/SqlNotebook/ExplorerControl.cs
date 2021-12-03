using SqlNotebook.Properties;
using SqlNotebookScript.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SqlNotebook;

public partial class ExplorerControl : UserControl {
    private readonly NotebookManager _manager;
    private readonly IWin32Window _mainForm;

    private const int ICON_PAGE = 0;
    private const int ICON_SCRIPT = 1;
    private const int ICON_TABLE = 2;
    private const int ICON_VIEW = 3;
    private const int ICON_LINKED_TABLE = 4;

    public event EventHandler NewPageButtonClicked;
    public event EventHandler NewScriptButtonClicked;

    public ExplorerControl(NotebookManager manager, IWin32Window mainForm) {
        InitializeComponent();
        _list.EnableDoubleBuffer();
        _detailsGrid.EnableDoubleBuffering();
        _detailsGrid.AutoGenerateColumns = false;

        _mainForm = mainForm;
        _manager = manager;
        _manager.NotebookChange += (sender, e) => HandleNotebookChange(e);
        _contextMenuStrip.SetMenuAppearance();
        _toolStrip.SetMenuAppearance();
        _toolStrip.Padding = new(this.Scaled(1), 0, 0, 0);
        _toolStrip.BackColor = SystemColors.Window;

        ImageList imageList = new() {
            ColorDepth = ColorDepth.Depth32Bit,
            ImageSize = new(this.Scaled(16), this.Scaled(16)),
        };
            
        // Same order as the ICON_* constants.
        // ICON_PAGE
        imageList.Images.Add(Ui.GetScaledIcon(this, Resources.note, Resources.note32, dispose: false));
        // ICON_SCRIPT
        imageList.Images.Add(Ui.GetScaledIcon(this, Resources.script, Ui.ShiftImage(Resources.script32, 0, 1), dispose: false));
        // ICON_TABLE
        imageList.Images.Add(Ui.GetScaledIcon(this, Resources.table, Resources.table32, dispose: false));
        // ICON_VIEW
        imageList.Images.Add(Ui.GetScaledIcon(this, Resources.filter, Resources.filter32, dispose: false));
        // ICON_LINKED_TABLE
        imageList.Images.Add(Ui.GetScaledIcon(this, Resources.link, Resources.link32, dispose: false));
        _list.SmallImageList = imageList;

        Ui ui = new(this, false);
        Padding buttonPadding = new(this.Scaled(2));
        var newPageImage16 = Ui.SuperimposePlusSymbol(Resources.note);
        var newPageImage32 = Ui.SuperimposePlusSymbol(Resources.note32);
        ui.Init(_toolbarNewPageButton, newPageImage16, newPageImage32);
        var newScriptImage16 = Ui.SuperimposePlusSymbol(Resources.script);
        var newScriptImage32 = Ui.SuperimposePlusSymbol(Ui.ShiftImage(Resources.script32, 0, 1));
        ui.Init(_toolbarNewScriptButton, newScriptImage16, newScriptImage32);

        _list.GotFocus += (sender, e) => {
            _manager.CommitOpenEditors();
            _manager.Rescan(notebookItemsOnly: true);
        };

        _splitContainer.Panel2Collapsed = true;

        _splitContainer.SplitterDistance = Math.Max(
            _splitContainer.Height / 2, _splitContainer.Height - ui.XHeight(10));
    }

    public void TakeFocus() {
        _list.Select();
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
                        lvi.ImageIndex = ICON_LINKED_TABLE;
                    } else {
                        lvi.ImageIndex =
                            item.Type switch {
                                NotebookItemType.Page => ICON_PAGE,
                                NotebookItemType.Script => ICON_SCRIPT,
                                NotebookItemType.Table => ICON_TABLE,
                                NotebookItemType.View => ICON_VIEW,
                                _ => throw new NotImplementedException()
                            };
                    }
                }
                _list.Sort();
                _list.EndUpdate();
            }));
        }
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

        var choice = Ui.ShowTaskDialog(
            _mainForm,
            $"Do you want to delete \"{name}\"?",
            "Delete Item",
            new[] { Ui.DELETE, Ui.CANCEL },
            TaskDialogIcon.Warning,
            defaultIsFirst: false);
        if (choice != Ui.DELETE) {
            return;
        }

        var item = new NotebookItem(type, name);
        _manager.CloseItem(item);

        WaitForm.Go(TopLevelControl, "Delete", "Deleting the selected item...", out _, () => {
            _manager.DeleteItem(item);
        });

        var isTableOrView = type == NotebookItemType.Table || type == NotebookItemType.View;
        _manager.Rescan(notebookItemsOnly: !isTableOrView);
    }

    private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
        var isSelection = _list.SelectedItems.Count == 1;
        _openMnu.Enabled = _deleteMnu.Enabled = _renameMnu.Enabled = isSelection;
    }

    private void List_SelectedIndexChanged(object sender, EventArgs e) {
        _detailsGrid.DataSource = null;
        _selectionLabel.Text = "Selected item";
        if (_list.SelectedItems.Count != 1) {
            _splitContainer.Panel2Collapsed = true;;
            return;
        }
        var lvi = _list.SelectedItems[0];
        var notebookItemName = lvi.Text;
        var n = _manager.Notebook;
        var details = new List<Tuple<string, string>>();

        try {
            if (lvi.Group.Name == "Table" || lvi.Group.Name == "View") {
                using var dt = n.Query($"PRAGMA table_info ({lvi.Text.DoubleQuote()})",
                    new Dictionary<string, object>());
                for (int i = 0; i < dt.Rows.Count; i++) {
                    var name = (string)dt.Get(i, "name");
                    var info = (string)dt.Get(i, "type");
                    info += Convert.ToInt32(dt.Get(i, "notnull")) != 0 ? " NOT NULL" : "";
                    info += Convert.ToInt32(dt.Get(i, "pk")) != 0 ? " PRIMARY KEY" : "";
                    details.Add(Tuple.Create(name, info));
                }
            } else if (lvi.Group.Name == "Script") {
                var paramNames = n.UserData.GetScriptParameters(notebookItemName);
                if (paramNames != null) {
                    foreach (var paramName in paramNames) {
                        details.Add(Tuple.Create(paramName, "parameter"));
                    }
                }
            } else if (lvi.Group.Name == "Note") {
                // Nothing to show.
            }
        } catch (Exception) { }

        List<Row> list = new();
        foreach (var detail in details) {
            var text = detail.Item1;
            var extra = detail.Item2.Replace(" PRIMARY KEY", "");
            if (extra.Any()) {
                text += $" ({extra})";
            }
            var detailRow = new Row { Name = detail.Item1, Type = extra };
            list.Add(detailRow);
        }
        _detailsGrid.DataSource = list;
        _selectionLabel.Text = notebookItemName;

        _splitContainer.Panel2Collapsed = list.Count == 0;
    }

    public sealed class Row {
        public string Name { get; set; }
        public string Type { get; set; }
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

        try {
            _manager.RenameItem(new NotebookItem(type, lvi.Text), e.Label);
        } catch (Exception ex) {
            Ui.ShowError(TopLevelControl, "Rename Error", ex);
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

    private void ToolbarNewPageButton_Click(object sender, EventArgs e) {
        NewPageButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    private void ToolbarNewScriptButton_Click(object sender, EventArgs e) {
        NewScriptButtonClicked?.Invoke(this, EventArgs.Empty);
    }
}
