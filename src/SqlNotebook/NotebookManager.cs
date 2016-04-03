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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlNotebookCore;
using SqlNotebookScript;

namespace SqlNotebook {
    public enum NotebookItemType {
        Note,
        Console,
        Script,
        Table,
        View
    }

    public struct NotebookItem {
        public NotebookItemType Type;
        public string Name;
        public NotebookItem(NotebookItemType type, string name) {
            Type = type;
            Name = name;
        }
    }

    public sealed class NotebookChangeEventArgs : EventArgs {
        public IReadOnlyList<NotebookItem> AddedItems { get; }
        public IReadOnlyList<NotebookItem> RemovedItems { get; }
        public NotebookChangeEventArgs(IReadOnlyList<NotebookItem> addedItems, IReadOnlyList<NotebookItem> removedItems) {
            AddedItems = addedItems;
            RemovedItems = removedItems;
        }
    }

    public sealed class NotebookItemRequestEventArgs : EventArgs {
        public NotebookItem Item { get; }
        public NotebookItemRequestEventArgs(NotebookItem item) {
            Item = item;
        }
    }

    public sealed class NotebookManager {
        private readonly string _savepointName;
        public Notebook Notebook { get; }
        public IReadOnlyList<NotebookItem> Items { get; private set; } = new NotebookItem[0];
        public event EventHandler<NotebookChangeEventArgs> NotebookChange; // informs the ExplorerControl about changes
        public event EventHandler<NotebookItemRequestEventArgs> NotebookItemOpenRequest;
        public event EventHandler<NotebookItemRequestEventArgs> NotebookItemCloseRequest;
        public event EventHandler NotebookItemsSaveRequest;
        public event EventHandler NotebookDirty;

        public NotebookManager(Notebook notebook) {
            Notebook = notebook;

            _savepointName = Guid.NewGuid().ToString();
            Notebook.Invoke(() => {
                Notebook.Execute($"SAVEPOINT \"{_savepointName}\"");
            });
        }

        public void Save() {
            Notebook.Invoke(() => {
                Notebook.Execute($"RELEASE SAVEPOINT \"{_savepointName}\"");
                Notebook.Execute($"SAVEPOINT \"{_savepointName}\"");
            });
        }

        public void Revert() {
            Notebook.Invoke(() => {
                Notebook.Execute($"ROLLBACK TO SAVEPOINT \"{_savepointName}\"");
            });
        }

        public void Rescan() {
            var newItems = ReadItems();
            var addedItems = newItems.Except(Items).ToList();
            var removedItems = Items.Except(newItems).ToList();
            Items = newItems;
            NotebookChange?.Invoke(this, new NotebookChangeEventArgs(addedItems, removedItems));
        }

        public void OpenItem(NotebookItem item) {
            NotebookItemOpenRequest?.Invoke(this, new NotebookItemRequestEventArgs(item));
        }

        public void CloseItem(NotebookItem item) {
            NotebookItemCloseRequest?.Invoke(this, new NotebookItemRequestEventArgs(item));
        }

        public void SetDirty() {
            NotebookDirty?.Invoke(this, EventArgs.Empty);
        }

        private IReadOnlyList<NotebookItem> ReadItems() {
            var items = new List<NotebookItem>();
            Notebook.Invoke(() => {
                Init();

                // tables and views
                using (var dt = Notebook.Query(
                    @"SELECT name, type 
                    FROM sqlite_master 
                    WHERE
                        (type = 'table' OR type = 'view') AND 
                        name != 'sqlnotebook_items' AND 
                        name != 'sqlnotebook_proc_params'")) {
                    foreach (DataRow row in dt.Rows) {
                        var type = row.Field<string>("type") == "view" ? NotebookItemType.View : NotebookItemType.Table;
                        items.Add(new NotebookItem(type, row.Field<string>("name")));
                    }
                }

                // consoles, queries, procedures, and notes
                using (var dt = Notebook.Query("SELECT name, type FROM sqlnotebook_items")) {
                    foreach (DataRow row in dt.Rows) {
                        string name = row.Field<string>("name");
                        string typeStr = row.Field<string>("type");
                        var type =
                            typeStr == "script" ? NotebookItemType.Script :
                            typeStr == "console" ? NotebookItemType.Console :
                            NotebookItemType.Note;
                        items.Add(new NotebookItem(type, name));
                    }
                }
            });
            return items;
        }

        public string NewConsole() {
            var x = NewItem("console");
            SetDirty();
            return x;
        }

        public string NewScript() {
            var x = NewItem("script");
            SetDirty();
            return x;
        }

        public string NewNote(string name = null, string data = null) {
            var x = NewItem("note", name, data);
            SetDirty();
            return x;
        }

        private string NewItem(string type, string name = null, string data = null) {
            Notebook.Invoke(() => {
                Init();
                if (name == null) {
                    using (var dt = Notebook.Query($"SELECT name FROM sqlnotebook_items WHERE name LIKE '{type}%'")) {
                        var existingNames = new HashSet<string>(dt.Rows.Cast<DataRow>().Select(x => x.Field<string>("name")));
                        int i;
                        for (i = 1; existingNames.Contains($"{type}{i}"); i++) { }
                        name = $"{type}{i}";
                    }
                }
                Notebook.Execute($"INSERT INTO sqlnotebook_items (name, type, data) VALUES (@name, @type, @data)",
                    new Dictionary<string, object> { ["@name"] = name, ["@type"] = type, ["@data"] = data ?? ""});
            });
            Rescan();
            return name;
        }

        public void SetItemData(string name, string data) {
            Notebook.Invoke(() => {
                Notebook.Execute("UPDATE sqlnotebook_items SET data = @data WHERE name = @name",
                    new Dictionary<string, object> { ["@name"] = name, ["@data"] = data });
            });
        }

        public string GetItemData(string name) {
            return Invoke(() => {
                using (var dt = Notebook.Query("SELECT data FROM sqlnotebook_items WHERE name = @name",
                    new Dictionary<string, object> { ["@name"] = name })) {
                    if (dt.Rows.Count == 1) {
                        return dt.Rows.Cast<DataRow>().First().Field<string>("data");
                    }
                }
                return "";
            });
        }

        public ScriptOutput ExecuteScript(string code) {
            NotebookItemsSaveRequest?.Invoke(this, EventArgs.Empty);
            return Invoke(() => {
                var parser = new ScriptParser(Notebook);
                var script = parser.Parse(code);
                var runner = new ScriptRunner(Notebook);
                return runner.Execute(script, new Dictionary<string, object>());
            });
        }

        private T Invoke<T>(Func<T> func) {
            T value = default(T);
            Notebook.Invoke(() => {
                value = func();
            });
            return value;
        }

        private void Init() {
            // create the sqlnotebook_items table if it does not exist
            using (var dt = Notebook.Query("SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'sqlnotebook_items'")) {
                if (dt.Rows.Count == 0) {
                    Notebook.Execute(
                        @"CREATE TABLE sqlnotebook_items (
                            name TEXT NOT NULL, 
                            type TEXT NOT NULL, 
                            data TEXT, 
                            PRIMARY KEY (name)
                        )");
                }
            }

            // create the sqlnotebook_proc_params table if it does not exist
            using (var dt = Notebook.Query("SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'sqlnotebook_proc_params'")) {
                if (dt.Rows.Count == 0) {
                    Notebook.Execute(
                        @"CREATE TABLE sqlnotebook_proc_params (
                            proc TEXT, 
                            par_name TEXT, 
                            par_type TEXT, 
                            par_value, 
                            PRIMARY KEY (proc, par_name)
                        )");
                }
            }
        }
    }
}
