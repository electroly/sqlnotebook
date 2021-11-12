using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlNotebook.Properties;
using SqlNotebookScript.Core;
using SqlNotebookScript.Interpreter;
using SqlNotebookScript.Utils;

namespace SqlNotebook {
    public partial class QueryDocumentControl : UserControl, IDocumentControl {
        private readonly NotebookManager _manager;
        private readonly QueryEmbeddedControl _queryControl;

        public string ItemName { get; set; }

        public void Save() {
            _manager.SetItemData(ItemName, _queryControl.SqlText);
        }

        public QueryDocumentControl(string name, NotebookManager manager, IWin32Window mainForm) {
            InitializeComponent();
            ItemName = name;
            _manager = manager;

            _queryControl = new(manager, isPageContext: false);
            _queryControl.Dock = DockStyle.Fill;
            _queryControl.TextControl.SqlTextChanged += (sender2, e2) => _manager.SetDirty();
            Controls.Add(_queryControl);

            var initialText = _manager.GetItemData(ItemName);
            if (initialText != null) {
                _queryControl.SqlText = initialText;
            }
        }
    }
}
