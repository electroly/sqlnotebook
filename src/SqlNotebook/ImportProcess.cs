using SqlNotebook.Import.Csv;
using SqlNotebook.Import.Xls;
using System;
using System.IO;
using System.Windows.Forms;

namespace SqlNotebook {
    public static class ImportProcess {
        public static string Filter => string.Join("|",
            "All data files|*.csv;*.txt;*.xls;*.xlsx",
            "Comma-separated values|*.csv;*.txt",
            "Excel workbooks|*.xls;*.xlsx");

        public static void Start(IWin32Window owner, string filePath, NotebookManager manager) {
            var schema = ReadDatabaseSchema(owner, manager);
            if (schema == null)
                return;

            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            switch (extension) {
                case ".csv":
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
            using (var f = new ImportCsvForm(filePath, schema, manager)) {
                if (f.ShowDialog(owner) != DialogResult.OK) {
                    return;
                }
                importSql = f.GeneratedImportSql;
            }

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
            WaitForm.Go(owner, "Import", $"Importing \"{Path.GetFileName(filePath)}\"...", out var success, () => {
                manager.ExecuteScript(importSql, transactionType: NotebookManager.TransactionType.Transaction);
            });

            manager.Rescan();
            manager.SetDirty();
            return success;
        }
    }
}
