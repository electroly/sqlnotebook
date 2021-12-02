using System.Windows.Forms;
using SqlNotebookScript;

namespace SqlNotebook;

public partial class QueryDocumentControl : UserControl, IDocumentControl {
    private readonly NotebookManager _manager;
    private readonly QueryEmbeddedControl _queryControl;

    public string ItemName { get; set; }

    public void Save() {
        _manager.SetItemData(ItemName, new ScriptNotebookItemRecord { Name = ItemName, Sql = _queryControl.SqlText });
    }

    public QueryDocumentControl(string name, NotebookManager manager, IWin32Window mainForm) {
        InitializeComponent();
        ItemName = name;
        _manager = manager;

        var record = _manager.GetItemData(ItemName) as ScriptNotebookItemRecord;
        _queryControl = new(manager, isPageContext: false, initialText: record?.Sql ?? "");
        _queryControl.Dock = DockStyle.Fill;
        _queryControl.TextControl.SqlTextChanged += (sender2, e2) => _manager.SetDirty();
        Controls.Add(_queryControl);
    }
}
