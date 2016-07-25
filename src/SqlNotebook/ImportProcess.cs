// SQL Notebook
// Copyright (C) 2016 Brian Luft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlNotebookScript.Utils;

namespace SqlNotebook {
    public static class CsvImportProcess {
        public static async Task<bool> Start(IWin32Window owner, string filePath, NotebookManager manager) {
            DatabaseSchema schema = null;

            manager.PushStatus("Reading notebook...");
            try {
                schema = await Task.Run(() => DatabaseSchema.FromNotebook(manager.Notebook));
            } catch (Exception ex) {
                MessageDialog.ShowError(owner,
                    "Import Error",
                    "Failed to read the list of tables in the notebook.",
                    ex.Message);
                return false;
            }
            manager.PopStatus();

            string importSql;
            using (var f = new ImportCsvForm(filePath, schema, manager)) {
                if (f.ShowDialog(owner) != DialogResult.OK) {
                    return false;
                }
                importSql = f.GeneratedImportSql;
            }

            manager.PushStatus($"Importing \"{Path.GetFileName(filePath)}\"...");
            try {
                await Task.Run(() => manager.ExecuteScript(importSql));
                manager.Rescan();
            } catch (Exception ex) {
                manager.PopStatus();
                await Task.Run(() => manager.ExecuteScript("ROLLBACK"));
                MessageDialog.ShowError(owner, "Import Error", "The import failed.", ex.GetErrorMessage());
                return false;
            }

            manager.SetDirty();
            manager.PopStatus();
            return true;
        }
    }
}
