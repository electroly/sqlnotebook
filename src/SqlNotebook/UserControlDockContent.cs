using System.Windows.Forms;
using SqlNotebookScript.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace SqlNotebook;

public partial class UserControlDockContent : DockContent
{
    public IDocumentControl Content { get; private set; }

    public UserControlDockContent(string title, UserControl control, DockAreas dockAreas = DockAreas.Document)
    {
        InitializeComponent();
        Text = title.EscapeAmpersand();
        control.Dock = DockStyle.Fill;
        Controls.Add(control);
        Content = control as IDocumentControl;

        // disable floating windows
        DockAreas = dockAreas;
    }
}
