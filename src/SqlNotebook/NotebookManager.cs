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

    public sealed class NotebookItemRenameEventArgs : EventArgs {
        public NotebookItem Item { get; }
        public string NewName { get; }
        public NotebookItemRenameEventArgs(NotebookItem item, string newName) {
            Item = item;
            NewName = newName;
        }
    }

    public sealed class StatusUpdateEventArgs : EventArgs {
        public string Status { get; }
        public StatusUpdateEventArgs(string status) {
            Status = status;
        }
    }

    public sealed class NotebookManager {
        private readonly Stack<string> _statusStack = new Stack<string>();
        public Notebook Notebook { get; }
        public IReadOnlyList<NotebookItem> Items { get; private set; } = new NotebookItem[0];
        public event EventHandler<NotebookChangeEventArgs> NotebookChange; // informs the ExplorerControl about changes
        public event EventHandler<NotebookItemRequestEventArgs> NotebookItemOpenRequest;
        public event EventHandler<NotebookItemRequestEventArgs> NotebookItemCloseRequest;
        public event EventHandler NotebookItemsSaveRequest;
        public event EventHandler NotebookDirty;
        public event EventHandler<NotebookItemRenameEventArgs> NotebookItemRename;
        public event EventHandler<StatusUpdateEventArgs> StatusUpdate;

        public NotebookManager(Notebook notebook) {
            Notebook = notebook;
        }

        public void Save() {
            NotebookItemsSaveRequest?.Invoke(this, EventArgs.Empty);
            Notebook.Invoke(() => Notebook.Save());
        }

        public void SaveAs(string filePath) {
            NotebookItemsSaveRequest?.Invoke(this, EventArgs.Empty);
            Notebook.Invoke(() => Notebook.SaveAs(filePath));
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
                var dt = Notebook.Query(
                    @"SELECT name, type 
                    FROM sqlite_master 
                    WHERE
                        (type = 'table' OR type = 'view') AND 
                        name != 'sqlnotebook_items' AND 
                        name != 'sqlnotebook_script_params' AND
                        name != 'sqlnotebook_last_error' ");
                for (int i = 0; i < dt.Rows.Count; i++) {
                    var type = (string)dt.Get(i, "type") == "view" ? NotebookItemType.View : NotebookItemType.Table;
                    items.Add(new NotebookItem(type, (string)dt.Get(i, "name")));
                }

                // consoles, queries, procedures, and notes
                dt = Notebook.Query("SELECT name, type FROM sqlnotebook_items");
                for (int i = 0; i < dt.Rows.Count; i++) {
                    string name = (string)dt.Get(i, "name");
                    string typeStr = (string)dt.Get(i, "type");
                    var type =
                        typeStr == "script" ? NotebookItemType.Script :
                        typeStr == "console" ? NotebookItemType.Console :
                        NotebookItemType.Note;
                    items.Add(new NotebookItem(type, name));
                }
            });
            return items;
        }

        public string NewConsole() {
            return NewItem("console");
        }

        public string NewScript() {
            return NewItem("script");
        }

        public string NewNote(string name = null, string data = null) {
            return NewItem("note", name, data);
        }

        private string NewItem(string type, string name = null, string data = null) {
            Notebook.Invoke(() => {
                Init();
                if (name == null) {
                    var dt = Notebook.Query($"SELECT name FROM sqlnotebook_items WHERE name LIKE '{type}%'");
                    var existingNames = new HashSet<string>(dt.Rows.Select(x => (string)x[0]));
                    int i;
                    for (i = 1; existingNames.Contains($"{type}{i}"); i++) { }
                    name = $"{type}{i}";
                }
                Notebook.Execute($"INSERT INTO sqlnotebook_items (name, type, data) VALUES (@name, @type, @data)",
                    new Dictionary<string, object> { ["@name"] = name, ["@type"] = type, ["@data"] = data ?? ""});
            });
            Rescan();
            SetDirty();
            return name;
        }

        public void SetItemData(string name, string data) {
            Notebook.Invoke(() => {
                Notebook.Execute("UPDATE sqlnotebook_items SET data = @data WHERE name = @name",
                    new Dictionary<string, object> { ["@name"] = name, ["@data"] = data });

                // if this is a Script, then we also need to update the list of parameters in sqlnotebook_script_params
                var type = Notebook.QueryValue("SELECT type FROM sqlnotebook_items WHERE name = @name",
                    new Dictionary<string, object> { ["@name"] = name }) as string;
                if (type != null && type == "script") {
                    try {
                        var parser = new ScriptParser(Notebook);
                        var ast = parser.Parse(data);
                        var paramNames =
                            ast.Traverse()
                            .OfType<SqlNotebookScript.Ast.DeclareStmt>()
                            .Where(x => x.IsParameter)
                            .Select(x => x.VariableName)
                            .ToList();
                        Notebook.Execute("DELETE FROM sqlnotebook_script_params WHERE script_name = @name",
                            new Dictionary<string, object> { ["@name"] = name });
                        foreach (var paramName in paramNames) {
                            Notebook.Execute("INSERT INTO sqlnotebook_script_params (script_name, param_name) VALUES (@script, @param)",
                                new Dictionary<string, object> { ["@script"] = name, ["@param"] = paramName });
                        }
                    } catch (Exception) { }
                }
            });
        }

        public string GetItemData(string name) {
            return Invoke(() => {
                var dt = Notebook.Query("SELECT data FROM sqlnotebook_items WHERE name = @name",
                    new Dictionary<string, object> { ["@name"] = name });
                if (dt.Rows.Count == 1) {
                    return (string)dt.Get(0, "data");
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

        public void DeleteItem(NotebookItem item) {
            Notebook.Invoke(() => {
                switch (item.Type) {
                    case NotebookItemType.Console:
                    case NotebookItemType.Script:
                    case NotebookItemType.Note:
                        Notebook.Execute("DELETE FROM sqlnotebook_items WHERE name = @name",
                            new Dictionary<string, object> { ["@name"] = item.Name });
                        break;

                    case NotebookItemType.Table:
                        Notebook.Execute($"DROP TABLE {item.Name.DoubleQuote()}");
                        break;

                    case NotebookItemType.View:
                        Notebook.Execute($"DROP VIEW {item.Name.DoubleQuote()}");
                        break;
                }
            });
            SetDirty();
        }

        public void RenameItem(NotebookItem item, string newName) {
            string lcNewName = newName.ToLower();
            bool isCaseChange = item.Name.ToLower() == lcNewName;

            Notebook.Invoke(() => {
                switch (item.Type) {
                    case NotebookItemType.Console:
                    case NotebookItemType.Note:
                    case NotebookItemType.Script:
                        if (!isCaseChange) {
                            // is the new name already in use?
                            var inUse = Notebook.Query("SELECT name FROM sqlnotebook_items")
                                .Rows.Any(x => ((string)x[0]).ToLower() == lcNewName);
                            if (inUse) {
                                throw new Exception($"An item named \"{newName}\" already exists.");
                            }
                        }

                        Notebook.Execute("UPDATE sqlnotebook_items SET name = @new_name WHERE name = @old_name",
                            new Dictionary<string, object> { ["@new_name"] = newName, ["@old_name"] = item.Name });

                        if (item.Type == NotebookItemType.Script) {
                            Notebook.Execute("UPDATE sqlnotebook_script_params SET script_name = @new_name WHERE script_name = @old_name",
                                new Dictionary<string, object> { ["@new_name"] = newName, ["@old_name"] = item.Name });
                        }
                        break;

                    case NotebookItemType.Table:
                        Notebook.Execute($"ALTER TABLE {item.Name.DoubleQuote()} RENAME TO \"{newName.Replace("\"", "\"\"")}\"");
                        break;

                    case NotebookItemType.View:
                        var createViewSql = (string)Notebook.QueryValue("SELECT sql FROM sqlite_master WHERE name = @old_name",
                            new Dictionary<string, object> { ["@old_name"] = item.Name });
                        var tokens = Notebook.Tokenize(createViewSql);
                        if (tokens.Count < 3 || tokens[0].Type != TokenType.Create || tokens[1].Type != TokenType.View) {
                            throw new Exception($"Unable to parse the original CREATE VIEW statement for \"{item.Name}\".");
                        }
                        var suffix = string.Join(" ", tokens.Select(x => x.Text).Skip(3)); // everything after "CREATE VIEW viewname"
                        Notebook.Execute($"DROP VIEW {item.Name.DoubleQuote()}");
                        Notebook.Execute($"CREATE VIEW {newName.DoubleQuote()} {suffix}");
                        break;
                }
            });

            NotebookItemRename?.Invoke(this, new NotebookItemRenameEventArgs(item, newName));
            SetDirty();
        }

        public void PushStatus(string status) {
            _statusStack.Push(status);
            StatusUpdate?.Invoke(this, new StatusUpdateEventArgs(status));
        }

        public void PopStatus() {
            _statusStack.Pop();
            StatusUpdate?.Invoke(this, new StatusUpdateEventArgs(_statusStack.Any() ? _statusStack.Peek() : ""));
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
            var dt = Notebook.Query("SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'sqlnotebook_items'");
            if (dt.Rows.Count == 0) {
                Notebook.Execute(@"CREATE TABLE sqlnotebook_items (name TEXT NOT NULL, type TEXT NOT NULL, " +
                    "data TEXT, PRIMARY KEY (name))");
            }

            // create the sqlnotebook_script_params table if it does not exist
            dt = Notebook.Query("SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'sqlnotebook_script_params'");
            if (dt.Rows.Count == 0) {
                Notebook.Execute(@"CREATE TABLE sqlnotebook_script_params (script_name TEXT, param_name TEXT, PRIMARY KEY (script_name, param_name))");
            }

            // create the sqlnotebook_last_error table if it does not exist
            dt = Notebook.Query("SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'sqlnotebook_last_error'");
            if (dt.Rows.Count == 0) {
                Notebook.Execute(@"CREATE TABLE sqlnotebook_last_error (error_number, error_message, error_state)");
            }
        }
    }
}
