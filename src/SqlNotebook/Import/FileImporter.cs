using System;
using System.IO;
using System.Windows.Forms;
using SqlNotebook.Import.Csv;
using SqlNotebook.Import.Xls;
using SqlNotebookScript.Utils;

namespace SqlNotebook.Import;

public static class FileImporter {
    public static string Filter => string.Join("|",
        "All data files|*.csv;*.txt;*.tsv;*.xls;*.xlsx",
        "Comma-separated values|*.csv;*.tsv;*.txt",
        "Excel workbooks|*.xls;*.xlsx");

    public static void Start(IWin32Window owner, string filePath, NotebookManager manager) {
        var schema = ReadDatabaseSchema(owner, manager);
        if (schema == null)
            return;

        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        switch (extension) {
            case ".csv":
            case ".tsv":
            case ".txt":
                ImportCsv(owner, filePath, manager, schema);
                break;

            case ".xls":
            case ".xlsx":
            case ".xlsm":
            case ".xlsb":
                ImportXls(owner, filePath, manager, schema);
                break;

            default:
                throw new InvalidOperationException($"The file type \"{extension}\" is not supported.");
        }
    }

    private static void ImportCsv(IWin32Window owner, string filePath, NotebookManager manager,
        DatabaseSchema schema
        ) {
        string importSql;
        using ImportCsvForm f = new(filePath, schema, manager);
        if (f.ShowDialog(owner) != DialogResult.OK) {
            return;
        }
        importSql = f.GeneratedImportSql;

        RunImportScript(importSql, owner, filePath, manager);
    }

    private static void ImportXls(
        IWin32Window owner, string filePath, NotebookManager manager, DatabaseSchema schema
        ) {
        var input = WaitForm.Go(owner, "Import", "Reading workbook...", out var success, () =>
            XlsInput.Load(filePath));
        if (!success) {
            return;
        }

        using ImportXlsForm f = new(input, manager, schema);
        f.ShowDialog(owner);
    }

    private static DatabaseSchema ReadDatabaseSchema(IWin32Window owner, NotebookManager manager) {
        var schema = WaitForm.Go(owner, "Import", "Reading notebook...", out var success, () =>
            DatabaseSchema.FromNotebook(manager.Notebook));
        if (!success) {
            return null;
        }
        return schema;
    }

    private static bool RunImportScript(string importSql, IWin32Window owner, string filePath,
    NotebookManager manager) {
        WaitForm.GoWithCancel(owner, "Import", $"Importing {Path.GetExtension(filePath).ToUpperInvariant().TrimStart('.')} file...", out var success, cancel => {
            SqlUtil.WithCancellableTransaction(manager.Notebook, () => {
                manager.ExecuteScriptNoOutput(importSql, transactionType: NotebookManager.TransactionType.Transaction);
            }, cancel);
        });

        manager.Rescan();
        manager.SetDirty();
        return success;
    }
}
