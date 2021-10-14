using HtmlAgilityPack;
using SqlNotebookCore;
using SqlNotebookScript.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace SqlNotebook {
    public static class HelpSearcher {
        private static byte[] _cachedNotebookBytes;

        public static List<Result> Search(string keyword) {
            var tempFilePath = Path.GetTempFileName();
            try {
                var hasCache = _cachedNotebookBytes != null;
                if (hasCache) {
                    File.WriteAllBytes(tempFilePath, _cachedNotebookBytes);
                }
                List<Result> results = null;
                using (Notebook notebook = new(tempFilePath, isNew: !hasCache)) {
                    if (!hasCache) {
                        InitHelpNotebook(notebook);
                    }
                    notebook.Invoke(() => {
                        results = SearchQuery(notebook, keyword);
                    });
                }
                if (!hasCache) {
                    _cachedNotebookBytes = File.ReadAllBytes(tempFilePath);
                }
                return results;
            } finally {
                File.Delete(tempFilePath);
            }
        }

        private static void InitHelpNotebook(Notebook notebook) {
            var exeDir = Path.GetDirectoryName(Application.ExecutablePath);
            var docDir = Path.Combine(exeDir, "doc");
            var htmlFiles = (
                from htmlFilePath in Directory.GetFiles(docDir, "*.html", SearchOption.AllDirectories)
                let content = File.ReadAllText(htmlFilePath)
                select (FilePath: htmlFilePath, Content: content)
                ).ToList();

            notebook.Invoke(() => {
                notebook.Execute("BEGIN");

                notebook.Execute(
                    @"CREATE TABLE docs (
                        id INTEGER PRIMARY KEY,
                        path TEXT NOT NULL,
                        book TEXT NOT NULL, 
                        title TEXT NOT NULL,
                        html TEXT NOT NULL
                    )");
                notebook.Execute(
                    @"CREATE TABLE books_txt (number INTEGER PRIMARY KEY, line TEXT NOT NULL)");
                notebook.Execute(
                    @"CREATE TABLE art (file_path TEXT PRIMARY KEY, content BLOB)");
                notebook.Execute("CREATE VIRTUAL TABLE docs_fts USING fts5 (id, title, text)");
                for (var i = 0; i < htmlFiles.Count; i++) {
                    var (filePath, content) = htmlFiles[i];
                    ParseHtml(content, out var text, out var title);

                    notebook.Execute("INSERT INTO docs VALUES (@id, @path, @book, @title, @html)", new Dictionary<string, object> {
                        ["@id"] = i,
                        ["@path"] = filePath,
                        ["@book"] = "SQLite Documentation",
                        ["@title"] = title,
                        ["@html"] = content
                    });
                    notebook.Execute("INSERT INTO docs_fts VALUES (@id, @title, @text)", new Dictionary<string, object> {
                        ["@id"] = i,
                        ["@title"] = title,
                        ["@text"] = text
                    });
                }

                notebook.Execute("COMMIT");
                notebook.Execute("ANALYZE");
                notebook.Save();
            });
        }

        private static void ParseHtml(string html, out string text, out string title) {
            HtmlAgilityPack.HtmlDocument htmlDoc = new() {
                OptionDefaultStreamEncoding = Encoding.UTF8
            };
            htmlDoc.LoadHtml(html);
            var stack = new Stack<HtmlNode>();
            stack.Push(htmlDoc.DocumentNode);
            while (stack.Any()) {
                var node = stack.Pop();
                if (node.Name == "style" || node.Name == "script") {
                    node.InnerHtml = "";
                    continue;
                }

                if (node.Name == "li" || node.Name == "p" || node.Name == "td" || node.Name == "br") {
                    node.InnerHtml = " " + node.InnerHtml + " ";
                }

                foreach (var child in node.ChildNodes) {
                    stack.Push(child);
                }
            }
            text = WebUtility.HtmlDecode(htmlDoc.DocumentNode.InnerText
                .Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Replace("&nbsp;", " "));
            while (text.Contains("  ")) {
                text = text.Replace("  ", " ");
            }

            title = htmlDoc.DocumentNode.Descendants("title").FirstOrDefault()?.InnerText ?? "(no title)";
        }

        private static List<Result> SearchQuery(Notebook notebook, string keyword) { // run from notebook thread
            var dt = notebook.Query(
                @"SELECT
                    f.id, d.path, d.book,
                    HIGHLIGHT(docs_fts, 1, '<b>', '</b>'),
                    SNIPPET(docs_fts, 2, '<b>', '</b>', '...', 25)
                FROM docs_fts f
                INNER JOIN docs d ON f.id = d.id
                WHERE f.docs_fts MATCH @keyword
                ORDER BY rank",
                new Dictionary<string, object> {
                    ["@keyword"] = string.Join(" ", keyword.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.DoubleQuote()))
                });
            return (
                from row in dt.Rows
                let path = (string)row[1]
                let title = (string)row[3]
                let snippet = (string)row[4]
                select new Result { Path = path, Title = title, Snippet = snippet }
                ).ToList();
        }

        public sealed class Result {
            public string Path;
            public string Title;
            public string Snippet;
        }
    }
}
