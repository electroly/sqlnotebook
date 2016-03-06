using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace SqlNotebook {
    public partial class UserControlDockContent : DockContent {
        public UserControlDockContent(string title, UserControl control) {
            InitializeComponent();
            Text = title;
            Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }
    }
}
