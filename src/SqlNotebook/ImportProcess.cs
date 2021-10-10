using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlNotebook.ImportXls;
using SqlNotebookScript.Utils;

namespace SqlNotebook {
    public static class ImportProcess {
        public static string Filter => string.Join("|",
            "All data files|*.csv;*.txt;*.xls;*.xlsx",
            "Comma-separated values|*.csv;*.txt",
            "Excel workbooks|*.xls;*.xlsx");

        public static async Task<bool> Start(IWin32Window owner, string filePath, NotebookManager manager) {
            var schema = await ReadDatabaseSchema(owner, manager);
            if (schema == null)
                return false;

            var extension = Path.GetExtension(filePath).ToLower();
            switch (extension) {
                case ".csv":
                case ".txt":
                    return await ImportCsv(owner, filePath, manager, schema);

                case ".xls":
                case ".xlsx":
                case ".xlsm":
                case ".xlsb":
                    return await ImportXls(owner, filePath, manager, schema);

                default:
                    throw new InvalidOperationException($"The file type \"{extension}\" is not supported.");
            }
        }

        private static async Task<bool> ImportCsv(IWin32Window owner, string filePath, NotebookManager manager,
        DatabaseSchema schema) {
            string importSql;
            using (var f = new ImportCsvForm(filePath, schema, manager)) {
                if (f.ShowDialog(owner) != DialogResult.OK) {
                    return false;
                }
                importSql = f.GeneratedImportSql;
            }

            return await RunImportScript(importSql, owner, filePath, manager);
        }

        private static async Task<bool> ImportXls(IWin32Window owner, string filePath, NotebookManager manager,
        DatabaseSchema schema) {
            string importSql;
            using (var f = new ImportXlsBookForm(filePath, schema, manager)) {
                if (f.ShowDialog(owner) != DialogResult.OK) {
                    return false;
                }
                importSql = f.GeneratedImportSql;
            }

            return await RunImportScript(importSql, owner, filePath, manager);
        }

        private static async Task<DatabaseSchema> ReadDatabaseSchema(IWin32Window owner, NotebookManager manager) {
            DatabaseSchema schema = null;

            manager.PushStatus("Reading notebook...");
            try {
                schema = await Task.Run(() => DatabaseSchema.FromNotebook(manager.Notebook));
            } catch (Exception ex) {
                manager.PopStatus();
                MessageForm.ShowError(owner,
                    "Import Error",
                    "Failed to read the list of tables in the notebook.",
                    ex.Message);
                return null;
            }
            manager.PopStatus();
            return schema;
        }

        private static async Task<bool> RunImportScript(string importSql, IWin32Window owner, string filePath,
        NotebookManager manager) {
            manager.PushStatus($"Importing \"{Path.GetFileName(filePath)}\"...");
            try {
                await Task.Run(() => {
                    manager.ExecuteScript(importSql, transactionType: NotebookManager.TransactionType.Transaction);
                });
                manager.Rescan();
            } catch (Exception ex) {
                manager.PopStatus();
                MessageForm.ShowError(owner, "Import Error", "The import failed.", ex.GetErrorMessage());
                return false;
            }

            manager.SetDirty();
            manager.PopStatus();
            return true;
        }
    }
}
