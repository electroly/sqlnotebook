using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using SqlNotebookScript.Core;
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
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        try {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            NotebookTempFiles.Init();

            Notebook notebook;
            bool isNew;
            if (Environment.GetCommandLineArgs().Length == 2) {
                var filePath = Environment.GetCommandLineArgs()[1];
                if (!File.Exists(filePath)) {
                    Ui.ShowError(null, "SQL Notebook", "File does not exist: " + filePath);
                    return;
                }
                notebook = WaitForm.GoWithCancel(
                    null, "SQL Notebook", $"Opening \"{Truncate(Path.GetFileName(filePath))}\"...", out var success,
                    cancel => Notebook.Open(filePath,
                        c => WaitForm.ProgressText = $"{c}% complete",
                        cancel));
                if (!success) {
                    return;
                }
                isNew = false;
            } else {
                notebook = Notebook.New();
                isNew = true;
            }

            Application.Run(new MainForm(notebook, isNew));
        } catch (Exception ex) {
            Ui.ShowError(null, "SQL Notebook", ex);
        } finally {
            NotebookTempFiles.DeleteFilesFromThisSession();
        }
    }

    private static string Truncate(string filename) => filename.Length > 50 ? $"{filename[..50]}..." : filename;
}
