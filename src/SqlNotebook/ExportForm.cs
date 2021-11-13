using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SqlNotebook;

public partial class ExportForm : ZForm {
    public NotebookItem NotebookItem { get; private set; }
    private ImageList _paddedImageList;

    public ExportForm(IEnumerable<string> scripts, IEnumerable<string> tables, IEnumerable<string> views) {
        InitializeComponent();
        using var g = CreateGraphics();
        _list.SmallImageList = _paddedImageList = _imageList.PadListViewIcons(g);

        Ui ui = new(this, 75, 25);
        ui.Init(_table);
        ui.Init(_helpLabel);
        ui.Init(_list);
        ui.MarginTop(_list);
        ui.Init(_buttonFlow);
        ui.MarginTop(_buttonFlow);
        ui.Init(_openBtn);
        ui.Init(_saveBtn);
        ui.Init(_cancelBtn);

        foreach (var name in scripts) {
            var lvi = _list.Items.Add(name);
            lvi.ImageIndex = 0;
            lvi.Group = _list.Groups[0];
        }
        foreach (var name in tables) {
            var lvi = _list.Items.Add(name);
            lvi.ImageIndex = 1;
            lvi.Group = _list.Groups[1];
        }
        foreach (var name in views) {
            var lvi = _list.Items.Add(name);
            lvi.ImageIndex = 2;
            lvi.Group = _list.Groups[2];
        }
        _list.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
    }

    private void OpenBtn_Click(object sender, EventArgs e) {
        if (_list.SelectedIndices.Count == 1) {
            SetNotebookItem();
            DialogResult = DialogResult.Yes;
            Close();
        }
    }

    private void SetNotebookItem() {
        var lvi = _list.SelectedItems[0];
        var type = (NotebookItemType)Enum.Parse(typeof(NotebookItemType), lvi.Group.Name);
        NotebookItem = new NotebookItem(type, lvi.Text);
    }

    private void SaveBtn_Click(object sender, EventArgs e) {
        if (_list.SelectedIndices.Count == 1) {
            SetNotebookItem();
            DialogResult = DialogResult.No;
            Close();
        }
    }

    private void List_SelectedIndexChanged(object sender, EventArgs e) {
        _openBtn.Enabled = _saveBtn.Enabled = _list.SelectedIndices.Count == 1;
    }
}
