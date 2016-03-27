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

    public sealed class NotebookItemOpenRequestEventArgs : EventArgs {
        public NotebookItem Item { get; }
        public NotebookItemOpenRequestEventArgs(NotebookItem item) {
            Item = item;
        }
    }

    public sealed class NotebookManager {
        public Notebook Notebook { get; }
        public IReadOnlyList<NotebookItem> Items { get; private set; } = new NotebookItem[0];
        public event EventHandler<NotebookChangeEventArgs> NotebookChange;
        public event EventHandler<NotebookItemOpenRequestEventArgs> NotebookItemOpenRequest;

        public NotebookManager(Notebook notebook) {
            Notebook = notebook;
        }

        public void Rescan() {
            var newItems = ReadItems();
            var addedItems = newItems.Except(Items).ToList();
            var removedItems = Items.Except(newItems).ToList();
            Items = newItems;
            NotebookChange?.Invoke(this, new NotebookChangeEventArgs(addedItems, removedItems));
        }

        public void Open(NotebookItem item) {
            NotebookItemOpenRequest?.Invoke(this, new NotebookItemOpenRequestEventArgs(item));
        }

        private IReadOnlyList<NotebookItem> ReadItems() {
            var items = new List<NotebookItem>();
            Notebook.Invoke(() => {
                Init();

                // tables and views
                using (var dt = Notebook.Query("SELECT name, type FROM sqlite_master WHERE name != 'sqlnotebook_items'")) {
                    foreach (DataRow row in dt.Rows) {
                        var type = row.Field<string>("type") == "view" ? NotebookItemType.View : NotebookItemType.Table;
                        items.Add(new NotebookItem(type, row.Field<string>("name")));
                    }
                }

                // queries, consoles, and notes
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
            return NewItem("console");
        }

        public string NewScript() {
            return NewItem("script");
        }

        public string NewNote() {
            return NewItem("note");
        }

        private string NewItem(string type) {
            string name = null;
            Notebook.Invoke(() => {
                Init();
                using (var dt = Notebook.Query($"SELECT name FROM sqlnotebook_items WHERE name LIKE '{type}%'")) {
                    var existingNames = new HashSet<string>(dt.Rows.Cast<DataRow>().Select(x => x.Field<string>("name")));
                    int i;
                    for (i = 1; existingNames.Contains($"{type}{i}"); i++) { }
                    name = $"{type}{i}";
                }
                Notebook.Execute($"INSERT INTO sqlnotebook_items (name, type, data) VALUES (@name, '{type}', '')",
                    new Dictionary<string, object> { ["@name"] = name });
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
            string data = null;
            Notebook.Invoke(() => {
                using (var dt = Notebook.Query("SELECT data FROM sqlnotebook_items WHERE name = @name",
                    new Dictionary<string, object> { ["@name"] = name })) {
                    if (dt.Rows.Count == 1) {
                        data = dt.Rows.Cast<DataRow>().First().Field<string>("data");
                    }
                }
            });
            return data;
        }

        private void Init() {
            // create the sqlnotebook_items table if it does not exist
            using (var dt = Notebook.Query("SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'sqlnotebook_items'")) {
                if (dt.Rows.Count == 0) {
                    Notebook.Execute("CREATE TABLE sqlnotebook_items (name TEXT, type TEXT, data TEXT)");
                }
            }
        }

        /*
            State 1: Normal
                if [-] [-] then 2
                if [/] [*] then 3
                if ['] then 4
                if ["] then 5
                if [;] then 1 (end of statement)
                else 1
            State 2: Inside single-line comment
                if [\n] then 1
                else 2
            State 3: Inside multi-line comment
                if [*] [/] then 1
                else 3
            State 4: Inside single-quoted string
                if ['] ['] then 4
                if ['] then 1
                else 4
            State 5: Inside double-quoted string
                if ["] ["] then 5
                if ["] then 1
                else 5
            State 6: Inside bracketed string
                if []] then 1
                else 6
        */
        public static IEnumerable<string> SplitStatements(string multiSql) {
            multiSql = multiSql + " ";
            var statements = new List<string>();
            var sb = new StringBuilder();
            const int NORMAL = 1;
            const int SINGLE_LINE_COMMENT = 2;
            const int MULTI_LINE_COMMENT = 3;
            const int SINGLE_QUOTED_STRING = 4;
            const int DOUBLE_QUOTED_STRING = 5;
            const int BRACKET_STRING = 6;
            int state = NORMAL;
            for (int i = 0; i < multiSql.Length - 1; i++) {
                char ch = multiSql[i];
                char ch2 = multiSql[i + 1];
                sb.Append(ch);
                switch (state) {
                    case NORMAL:
                        if (ch == '-' && ch2 == '-') {
                            state = SINGLE_LINE_COMMENT;
                            sb.Append(ch2); i++;
                        } else if (ch == '/' && ch2 == '*') {
                            state = MULTI_LINE_COMMENT;
                            sb.Append(ch2); i++;
                        } else if (ch == '\'') {
                            state = SINGLE_QUOTED_STRING;
                        } else if (ch == '"') {
                            state = DOUBLE_QUOTED_STRING;
                        } else if (ch == '[') {
                            state = BRACKET_STRING;
                        } else if (ch == ';') {
                            statements.Add(sb.ToString());
                            sb.Clear();
                        }
                        break;
                    case SINGLE_LINE_COMMENT:
                        if (ch == '\n') {
                            state = NORMAL;
                        }
                        break;
                    case MULTI_LINE_COMMENT:
                        if (ch == '*' && ch2 == '/') {
                            state = NORMAL;
                            sb.Append(ch2); i++;
                        }
                        break;
                    case SINGLE_QUOTED_STRING:
                        if (ch == '\'' && ch2 == '\'') {
                            state = SINGLE_QUOTED_STRING;
                            sb.Append(ch2); i++;
                        } else if (ch == '\'') {
                            state = NORMAL;
                        }
                        break;
                    case DOUBLE_QUOTED_STRING:
                        if (ch == '"' && ch2 == '"') {
                            state = DOUBLE_QUOTED_STRING;
                            sb.Append(ch2); i++;
                        } else if (ch == '"') {
                            state = NORMAL;
                        }
                        break;
                    case BRACKET_STRING:
                        if (ch == ']') {
                            state = NORMAL;
                        }
                        break;
                }
            }
            statements.Add(sb.ToString());
            return statements.Where(x => x.Trim().Any()).ToList();
        }
    }
}
