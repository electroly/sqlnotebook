using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace SqlNotebook {
    public static class DockPanelUtil {
        public static DockPanel CreateDockPanel(bool showWindowListButton = false) =>
            new DockPanel {
                Dock = DockStyle.Fill,
                DocumentStyle = DocumentStyle.DockingWindow,
                Theme = new VS2012LightTheme {
                    ShowWindowListButton = showWindowListButton,
                    ShowAutoHideButton = false,
                    ForceActiveCaptionColor = true
                },
                AllowEndUserDocking = false,
                AllowEndUserNestedDocking = false,
                ShowDocumentIcon = true
            };
    }
}
