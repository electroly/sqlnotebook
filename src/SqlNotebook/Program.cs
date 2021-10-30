using SqlNotebookCore;
using System;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;

namespace SqlNotebook {
    public static class Program
    {
        public static HttpClient HttpClient {  get; } = new();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            NotebookTempFiles.Init();

            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string filePath;
            bool isNew;
            if (Environment.GetCommandLineArgs().Length == 2) {
                filePath = Environment.GetCommandLineArgs()[1];
                isNew = false;
            } else {
                filePath = NotebookTempFiles.GetTempFilePath(".sqlnb");
                isNew = true;
            }

            if (!File.Exists(filePath)) {
                MessageBox.Show("File does not exist: " + filePath, "SQL Notebook", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try {
                Application.Run(new MainForm(filePath, isNew));
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "SQL Notebook", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                NotebookTempFiles.DeleteFiles();
            }
        }
    }
}
