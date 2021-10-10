using System;
using System.IO;
using System.Windows.Forms;

namespace SqlNotebookUpdater {
    static class Program {
        [STAThread]
        static void Main() {
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UpdaterForm());
        }
    }
}
