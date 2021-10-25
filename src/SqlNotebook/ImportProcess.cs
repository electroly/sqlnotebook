using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlNotebook.Import.Xls;
using SqlNotebookScript.Utils;

namespace SqlNotebook {
    public static class ImportProcess {
        public static string Filter => string.Join("|",
            "All data files|*.csv;*.txt;*.xls;*.xlsx",
            "Comma-separated values|*.csv;*.txt",
            "Excel workbooks|*.xls;*.xlsx");

        public static async Task Start(IWin32Window owner, string filePath, NotebookManager manager) {
            var schema = await ReadDatabaseSchema(owner, manager);
            if (schema == null)
                return;

            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            switch (extension) {
                case ".csv":
                case ".txt":
                    await ImportCsv(owner, filePath, manager, schema);
                    break;

                case ".xls":
                case ".xlsx":
                case ".xlsm":
                case ".xlsb":
                    await ImportXls(owner, filePath, manager, schema);
                    break;

                default:
                    throw new InvalidOperationException($"The file type \"{extension}\" is not supported.");
            }
        }

        private static async Task ImportCsv(IWin32Window owner, string filePath, NotebookManager manager,
            DatabaseSchema schema
            ) {
            string importSql;
            using (var f = new ImportCsvForm(filePath, schema, manager)) {
                if (f.ShowDialog(owner) != DialogResult.OK) {
                    return;
                }
                importSql = f.GeneratedImportSql;
            }

            await RunImportScript(importSql, owner, filePath, manager);
        }

        private static async Task ImportXls(
            IWin32Window owner, string filePath, NotebookManager manager, DatabaseSchema schema
            ) {
            XlsInput input = null;
            using WaitForm waitForm = new("Import", "Reading workbook...", () => {
                input = XlsInput.Load(filePath);
            });
            if (waitForm.ShowDialog(owner) != DialogResult.OK) {
                MessageForm.ShowError(owner, "Import Error", waitForm.ResultException.Message);
                return;
            }

            using ImportXlsForm f = new(input, manager, schema);
            f.ShowDialog(owner);
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
