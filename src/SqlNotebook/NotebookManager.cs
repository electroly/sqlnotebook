using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using SqlNotebookScript;
using SqlNotebookScript.Core;
using SqlNotebookScript.Interpreter;
using SqlNotebookScript.Interpreter.Ast;
using SqlNotebookScript.Utils;

namespace SqlNotebook;

public enum NotebookItemType {
    Script,
    Table,
    View,
    Page
}

public struct NotebookItem {
    public NotebookItemType Type;
    public string Name;
    public bool IsVirtualTable;
    public NotebookItem(NotebookItemType type, string name, bool isVirtualTable = false) {
        Type = type;
        Name = name;
        IsVirtualTable = isVirtualTable;
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

public sealed class HotkeyEventArgs : EventArgs {
    public Keys KeyData { get; }
    public HotkeyEventArgs(Keys keyData) {
        KeyData = keyData;
    }
}

public sealed class NotebookManager {
    public Notebook Notebook { get; }
    public IReadOnlyList<NotebookItem> Items { get; private set; } = new NotebookItem[0];
    public event EventHandler<NotebookChangeEventArgs> NotebookChange; // informs the ExplorerControl about changes
    public event EventHandler<NotebookItemRequestEventArgs> NotebookItemOpenRequest;
    public event EventHandler<NotebookItemRequestEventArgs> NotebookItemCloseRequest;
    public event EventHandler NotebookItemsSaveRequest;
    public event EventHandler NotebookDirty;
    public event EventHandler<NotebookItemRenameEventArgs> NotebookItemRename;
    public event EventHandler<HotkeyEventArgs> HandleHotkeyRequest;
    private readonly Slot<bool> _isTransactionOpen;

    public NotebookManager(Notebook notebook, Slot<bool> isTransactionOpen) {
        Notebook = notebook;
        _isTransactionOpen = isTransactionOpen;
    }

    public void CommitOpenEditors() {
        NotebookItemsSaveRequest?.Invoke(this, EventArgs.Empty);
    }

    public void HandleAppHotkeys(Keys keyData) {
        HandleHotkeyRequest?.Invoke(this, new HotkeyEventArgs(keyData));
    }

    public void Save() {
        NotebookItemsSaveRequest?.Invoke(this, EventArgs.Empty);
        Notebook.Invoke(() => {
            Notebook.Execute("VACUUM");
            Notebook.Save();
        });
    }

    public void SaveAs(string filePath) {
        NotebookItemsSaveRequest?.Invoke(this, EventArgs.Empty);
        Notebook.Invoke(() => {
            Notebook.Execute("VACUUM");
            Notebook.SaveAs(filePath);
        });
    }

    public void Rescan(bool notebookItemsOnly = false) {
        var ignoredItems = notebookItemsOnly
            ? Items.Where(x => x.Type == NotebookItemType.Table || x.Type == NotebookItemType.View).ToList()
            : new List<NotebookItem>();
        var newItems = ReadItems(notebookItemsOnly);
        var addedItems = newItems.Except(Items).ToList();
        var removedItems = Items.Except(newItems).Except(ignoredItems).ToList();
        Items = newItems.Concat(ignoredItems).ToList();
        NotebookChange?.Invoke(this, new NotebookChangeEventArgs(addedItems, removedItems));

        if (!notebookItemsOnly) {
            // also check the transaction status
            Notebook.Invoke(() => {
                _isTransactionOpen.Value = Notebook.IsTransactionActive();
            });
        }
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

    private IReadOnlyList<NotebookItem> ReadItems(bool notebookItemsOnly) {
        var items = new List<NotebookItem>();

        if (!notebookItemsOnly) {
            // tables and views
            var dt = Notebook.SpecialReadOnlyQuery(
                "SELECT name, type, sql FROM sqlite_master WHERE type = 'table' OR type = 'view' ",
                new Dictionary<string, object>());
            for (int i = 0; i < dt.Rows.Count; i++) {
                var type = (string)dt.Get(i, "type") == "view" ? NotebookItemType.View : NotebookItemType.Table;
                var isVirtualTable = dt.Get(i, "sql").ToString().StartsWith("CREATE VIRTUAL TABLE");
                var name = (string)dt.Get(i, "name");
                if (name.StartsWith("_sqlnotebook_")) {
                    continue;
                }
                items.Add(new NotebookItem(type, name, isVirtualTable));
            }
        }

        // scripts
        items.AddRange(
            from x in Notebook.UserData.Items
            select new NotebookItem((NotebookItemType)Enum.Parse(typeof(NotebookItemType), x.Type), x.Name)
        );

        return items;
    }

    public string NewScript() {
        return NewItem(NotebookItemType.Script.ToString());
    }

    public string NewPage() {
        return NewItem(NotebookItemType.Page.ToString());
    }

    private string NewItem(string type, string name = null, string data = null) {
        if (name == null) {
            var existingNames = new HashSet<string>(Notebook.UserData.Items.Select(x => x.Name.ToLower()));
            int i;
            for (i = 1; existingNames.Contains($"{type.ToLower()}{i}"); i++) { }
            name = $"{type}{i}";
        }
        var itemRec = new NotebookItemRecord { Name = name, Data = data ?? "", Type = type };
        Notebook.UserData.Items.Add(itemRec);
        Rescan(notebookItemsOnly: true);
        SetDirty();
        return name;
    }

    public void SetItemData(string name, string data) {
        var ud = Notebook.UserData;
        var itemRec = ud.Items.FirstOrDefault(x => x.Name == name);
        if (itemRec == null) {
            throw new ArgumentException($"There is no item named \"{name}\".");
        }

        itemRec.Data = data;

        // if this is a Script, then we also need to update its list of parameters
        if (itemRec.Type == "Script") {
            ud.ScriptParameters.RemoveWhere(x => x.ScriptName == name);
            var paramRec = new ScriptParameterRecord { ScriptName = name };
            try {
                var parser = new ScriptParser(Notebook);
                var ast = parser.Parse(data);
                var paramNames =
                    ast.Traverse()
                    .OfType<DeclareStmt>()
                    .Where(x => x.IsParameter)
                    .Select(x => x.VariableName);
                foreach (var paramName in paramNames) {
                    paramRec.ParamNames.Add(paramName);
                }
            } catch (Exception) { }
            ud.ScriptParameters.Add(paramRec);
        }
    }

    public string GetItemData(string name) {
        var itemRec = Notebook.UserData.Items.FirstOrDefault(x => x.Name == name);
        if (itemRec == null) {
            return "";
        } else {
            return itemRec.Data;
        }
    }

    public ScriptOutput ExecuteScript(string code, IReadOnlyDictionary<string, object> args = null,
        TransactionType transactionType = TransactionType.NoTransaction, int maxRows = -1
        ) =>
        ExecuteScriptEx(code, args, transactionType, out _, maxRows);

    public enum TransactionType {
        Transaction,
        RollbackTransaction,
        NoTransaction
    }

    public ScriptOutput ExecuteScriptEx(string code, IReadOnlyDictionary<string, object> args,
        TransactionType transactionType, out Dictionary<string, object> vars, int maxRows = -1
        ) {
        var env = new ScriptEnv { MaxRows = maxRows };
        var output = Invoke(() => {
            var parser = new ScriptParser(Notebook);
            var script = parser.Parse(code);
            var runner = new ScriptRunner(Notebook, Notebook.GetScripts());
            if (transactionType == TransactionType.Transaction) {
                return SqlUtil.WithTransaction(Notebook, 
                    () => runner.Execute(script, env, args ?? new Dictionary<string, object>()));
            } else if (transactionType == TransactionType.RollbackTransaction) {
                return SqlUtil.WithRollbackTransaction(Notebook, 
                    () => runner.Execute(script, env, args ?? new Dictionary<string, object>()));
            } else {
                return runner.Execute(script, env, args ?? new Dictionary<string, object>());
            }
        });
        vars = env.Vars;
        return output;
    }

    public void DeleteItem(NotebookItem item) {
        switch (item.Type) {
            case NotebookItemType.Script:
                Notebook.UserData.Items.RemoveWhere(x => x.Name == item.Name);
                break;

            case NotebookItemType.Table:
                Notebook.Invoke(() => Notebook.Execute($"DROP TABLE {item.Name.DoubleQuote()}"));
                break;

            case NotebookItemType.View:
                Notebook.Invoke(() => Notebook.Execute($"DROP VIEW {item.Name.DoubleQuote()}"));
                break;
        }
        SetDirty();
    }

    public void RenameItem(NotebookItem item, string newName) {
        var lcNewName = newName.ToLower();
        var isCaseChange = item.Name.ToLower() == lcNewName;

        switch (item.Type) {
            case NotebookItemType.Script:
                if (!isCaseChange) {
                    // is the new name already in use?
                    if (Notebook.UserData.Items.Any(x => x.Name.ToLower() == lcNewName)) {
                        throw new Exception($"An item named \"{newName}\" already exists.");
                    }
                }

                var itemRec = Notebook.UserData.Items.Single(x => x.Name == item.Name);
                itemRec.Name = newName;

                if (item.Type == NotebookItemType.Script) {
                    var scriptParamRec = Notebook.UserData.ScriptParameters.SingleOrDefault(x => x.ScriptName == item.Name);
                    if (scriptParamRec != null) {
                        scriptParamRec.ScriptName = newName;
                    }
                }
                break;

            case NotebookItemType.Table:
                // sqlite won't let us rename a table with just a case change, so in that situation we'll rename
                // to something else and then rename back
                if (isCaseChange) {
                    var tempName = $"temp_{Guid.NewGuid()}";
                    Notebook.Invoke(() => {
                        Notebook.Execute($"ALTER TABLE {item.Name.DoubleQuote()} RENAME TO {tempName.DoubleQuote()};");
                        Notebook.Execute($"ALTER TABLE {tempName.DoubleQuote()} RENAME TO {newName.DoubleQuote()};");
                    });
                } else {
                    Notebook.Invoke(() => {
                        Notebook.Execute($"ALTER TABLE {item.Name.DoubleQuote()} RENAME TO {newName.DoubleQuote()}");
                    });
                }
                break;

            case NotebookItemType.View:
                Notebook.Invoke(() => {
                    var createViewSql = (string)Notebook.QueryValue("SELECT sql FROM sqlite_master WHERE name = @old_name",
                        new Dictionary<string, object> { ["@old_name"] = item.Name });
                    var tokens = Notebook.Tokenize(createViewSql);
                    if (tokens.Count < 3 || tokens[0].Type != TokenType.Create || tokens[1].Type != TokenType.View) {
                        throw new Exception($"Unable to parse the original CREATE VIEW statement for \"{item.Name}\".");
                    }
                    var suffix = string.Join(" ", tokens.Select(x => x.Text).Skip(3)); // everything after "CREATE VIEW viewname"
                    Notebook.Execute($"DROP VIEW {item.Name.DoubleQuote()}");
                    Notebook.Execute($"CREATE VIEW {newName.DoubleQuote()} {suffix}");
                });
                break;
        }

        NotebookItemRename?.Invoke(this, new NotebookItemRenameEventArgs(item, newName));
        SetDirty();
    }

    private T Invoke<T>(Func<T> func) {
        T value = default(T);
        Notebook.Invoke(() => {
            value = func();
        });
        return value;
    }
}
