using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using SqlNotebookScript.Utils;

namespace SqlNotebook;

public static class Program
{
    public static HttpClient HttpClient {  get; } = new();

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main()
    {
        // .NET 5 moves the ANSI code pages into an external encoding provider. Bring it in.
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        NotebookTempFiles.Init();

        Application.SetHighDpiMode(HighDpiMode.SystemAware);
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
            MessageBox.Show(ex.GetExceptionMessage(), "SQL Notebook", MessageBoxButtons.OK, MessageBoxIcon.Error);
        } finally {
            NotebookTempFiles.DeleteFilesFromThisSession();
        }
    }
}
