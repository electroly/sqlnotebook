using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HtmlAgilityPack;
using SqlNotebook.Properties;
using SqlNotebookCore;
using SqlNotebookScript.Utils;

namespace SqlNotebook {
    public sealed class HelpServer : IDisposable {
        private readonly Notebook _notebook;
        private readonly HttpServer _httpServer;
        private string _indexHtml = "";

        public ushort PortNumber { get; private set; }

        public HelpServer() {
            var filePath = GetHelpNotebookPath();
            if (File.Exists(filePath) && File.GetLastWriteTime(filePath) < File.GetLastWriteTime(Application.ExecutablePath)) {
                // help file is outdated; regenerate it
                File.Delete(filePath);
            }

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            var isNew = !File.Exists(filePath);
            _notebook = new Notebook(filePath, isNew);
            if (isNew) {
                InitNewNotebook(filePath);
            }
            _notebook.Invoke(() => {
                try {
                    _indexHtml = BuildIndexHtml();
                } catch (Exception ex) {
                    _indexHtml = ex.Message;
                }
            });
            _httpServer = StartHttpServer();
        }

        private void InitNewNotebook(string filePath) {
            var sqliteDocAmalgamation = ReadSqliteDocAmalgamationFromResources();

            var sqliteFileSeparator = $"{Environment.NewLine}--[file separator]--{Environment.NewLine}";
            var sqliteFiles =
                (from file in sqliteDocAmalgamation.SqliteDoc.Split(new[] { sqliteFileSeparator }, StringSplitOptions.None)
                 let fileParts = file.Split(new[] { '\n' }, 2)
                 select new { RelativeFilePath = fileParts[0].Trim(), Content = fileParts[1] })
                .ToList();

            _notebook.Invoke(() => {
                _notebook.Execute("BEGIN");

                _notebook.Execute(
                    @"CREATE TABLE docs (
                        id INTEGER PRIMARY KEY,
                        path TEXT NOT NULL,
                        book TEXT NOT NULL, 
                        title TEXT NOT NULL,
                        html TEXT NOT NULL
                    )");
                _notebook.Execute(
                    @"CREATE TABLE books_txt (number INTEGER PRIMARY KEY, line TEXT NOT NULL)");
                _notebook.Execute(
                    @"CREATE TABLE art (file_path TEXT PRIMARY KEY, content BLOB)");
                _notebook.Execute("CREATE VIRTUAL TABLE docs_fts USING fts5 (id, title, text)");
                int i = 0;
                for (; i < sqliteFiles.Count; i++) {
                    var file = sqliteFiles[i];
                    string text, title;
                    ParseHtml(file.Content, out text, out title);

                    _notebook.Execute("INSERT INTO docs VALUES (@id, @path, @book, @title, @html)", new Dictionary<string, object> {
                        ["@id"] = i, ["@path"] = file.RelativeFilePath, ["@book"] = "SQLite Documentation", ["@title"] = title, ["@html"] = file.Content
                    });
                    _notebook.Execute("INSERT INTO docs_fts VALUES (@id, @title, @text)", new Dictionary<string, object> {
                        ["@id"] = i, ["@title"] = title, ["@text"] = text
                    });
                }

                // SQL Notebook help files
                foreach (var pair in sqliteDocAmalgamation.SqlNotebookDoc) {
                    var filename = pair.Key;
                    var content = pair.Value;
                    if (filename.EndsWith("books.txt")) {
                        int lineNum = 0;
                        foreach (var line in content.Split('\n')) {
                            _notebook.Execute("INSERT INTO books_txt VALUES (@number, @line)", new Dictionary<string, object> {
                                ["@number"] = lineNum++,
                                ["@line"] = line.Trim()
                            });
                        }
                        continue;
                    }

                    string text, title;
                    ParseHtml(content, out text, out title);

                    _notebook.Execute("INSERT INTO docs VALUES (@id, @path, @book, @title, @html)", new Dictionary<string, object> {
                        ["@id"] = i, ["@path"] = $".\\{filename}", ["@book"] = "SQL Notebook Help", ["@title"] = title, ["@html"] = content
                    });
                    _notebook.Execute("INSERT INTO docs_fts VALUES (@id, @title, @text)", new Dictionary<string, object> {
                        ["@id"] = i, ["@title"] = title, ["@text"] = text
                    });
                    i++;
                }

                // SQL Notebook art files
                foreach (var pair in sqliteDocAmalgamation.SqlNotebookArt) {
                    var filename = pair.Key;
                    var bytes = pair.Value;
                    _notebook.Execute("INSERT INTO art VALUES (@file_path, @bytes)", new Dictionary<string, object> {
                        ["@file_path"] = filename,
                        ["@bytes"] = bytes
                    });
                }

                _notebook.Execute("COMMIT");
                _notebook.Execute("ANALYZE");
                _notebook.Save();
            });
        }

        private sealed class DocAmalgamation {
            public string SqliteDoc { get; set; }
            public Dictionary<string, string> SqlNotebookDoc { get; set; } = new Dictionary<string, string>();
            public Dictionary<string, byte[]> SqlNotebookArt { get; set; } = new Dictionary<string, byte[]>();
        }

        private static DocAmalgamation ReadSqliteDocAmalgamationFromResources() {
            var doc = new DocAmalgamation();

            using (var zipStream = new MemoryStream(Resources.SqliteDocZip))
            using (var archive = new ZipArchive(zipStream)) {
                foreach (var entry in archive.Entries) {
                    if (entry.Name.EndsWith(".png")) {
                        using (var entryStream = entry.Open())
                        using (var memoryStream = new MemoryStream()) {
                            entryStream.CopyTo(memoryStream);
                            doc.SqlNotebookArt["/art/" + entry.Name] = memoryStream.ToArray();
                        }
                    } else {
                        string content;
                        using (var txtStream = entry.Open())
                        using (var reader = new StreamReader(txtStream, Encoding.UTF8)) {
                            content = reader.ReadToEnd();
                        }

                        if (entry.Name == "sqlite-doc.txt") {
                            doc.SqliteDoc = content;
                        } else {
                            doc.SqlNotebookDoc[entry.Name] = content;
                        }
                    }
                }
            }

            return doc;
        }

        private static void ParseHtml(string html, out string text, out string title) {
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.OptionDefaultStreamEncoding = Encoding.UTF8;
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

        private static string GetHelpNotebookPath() {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SQL Notebook", "Help.sqlnb");
        }

        private HttpServer StartHttpServer() {
            var server = new HttpServer(0);
            PortNumber = server.Port;
            server.Request += (sender, e) => {
                var rawUrl = e.Url;
                byte[] bytes;
                var header = "<!DOCTYPE html><style>" + Resources.HelpCss + "</style><meta http-equiv=\"Pragma\" content=\"no-cache\">" +
                    @"<script>
                        function hideorshow(btn, obj) {
                            var x = document.getElementById(obj);
                            var b = document.getElementById(btn);
                            if (x.style.display != 'none') {
                                x.style.display = 'none';
                                b.innerHTML = 'show';
                            } else {
                                x.style.display = '';
                                b.innerHTML = 'hide';
                            }
                            return false;
                        }
                    </script>";

                if (rawUrl.StartsWith("/sqlite-doc/") && (rawUrl.EndsWith(".jpg") || rawUrl.EndsWith(".gif") || rawUrl.EndsWith(".png"))) {
                    // we don't store images in our resources, instead we download them on-the-fly
                    var sqliteUrl = rawUrl.Replace("/sqlite-doc", "http://www.sqlite.org");
                    try {
                        using (var webClient = new WebClient()) {
                            bytes = webClient.DownloadData(sqliteUrl);
                        }
                    } catch {
                        bytes = Resources.SqliteSiteErrorPlaceholderPng;
                    }
                } else if (rawUrl == "/index.html") {
                    bytes = Encoding.UTF8.GetBytes(header + _indexHtml);
                } else if (rawUrl == "/book.png") {
                    bytes = Resources.BookPicture32Png;
                } else if (rawUrl == "/page.png") {
                    bytes = Resources.PageWhiteTextPng;
                } else if (rawUrl == "/link.png") {
                    bytes = Resources.LinkGo32Png;
                } else if (rawUrl == "/favicon.ico") {
                    bytes = Resources.FaviconIco;
                } else if (rawUrl.StartsWith("/search?q=")) {
                    var keyword = rawUrl.Substring("/search?q=".Length);
                    if (keyword.Trim().Any()) {
                        bytes = Encoding.UTF8.GetBytes(header + BuildSearchHtml(keyword));
                    } else {
                        bytes = Encoding.UTF8.GetBytes(header + _indexHtml);
                    }
                } else if (rawUrl.StartsWith("/art/")) {
                    bytes = null;
                    try {
                        _notebook.Invoke(() => {
                            bytes = (byte[])_notebook.QueryValue(
                                "SELECT content FROM art WHERE file_path = @file_path",
                                new Dictionary<string, object> { ["@file_path"] = rawUrl });
                        });
                    } catch { }
                } else {
                    var path = "." + rawUrl.Replace("/", "\\");
                    string html = null;
                    _notebook.Invoke(() => {
                        html = _notebook.QueryValue("SELECT html FROM docs WHERE path = @path", new Dictionary<string, object> {
                            ["@path"] = path
                        }) as string;
                    });
                    if (html == null) {
                        html = "<title>Not Found</title><body>The requested document is not available in the SQL Notebook help collection.</body>";
                    }
                    bytes = Encoding.UTF8.GetBytes(header + html);
                }
                e.Result = bytes ?? new byte[0];
            };
            return server;
        }

        private static ushort FindUnusedPort() {
            // this function is inherently racy; another application could bind to this port before the caller has a
            // chance to do so.  it at least limits the chance of collision to that race situation.
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return (ushort)port;
        }

        private sealed class GroupDef {
            public string Book;
            public string Group;
            public Regex Pattern;
            public string Html = "";
        }

        private List<GroupDef> ReadGroupDefs() {
            var dt = _notebook.Query("SELECT line FROM books_txt ORDER BY number");
            var list = new List<GroupDef>();
            foreach (var line in dt.Rows.Select(x => x[0].ToString())) {
                if (line.StartsWith("Group#")) {
                    var cells = line.Split(new[] { '#' }, 4);
                    if (cells.Length != 4) {
                        throw new Exception("Internal error. Invalid group line: " + line);
                    }
                    list.Add(new GroupDef {
                        Book = cells[1],
                        Group = cells[2],
                        Pattern = new Regex(cells[3].Trim())
                    });
                }
            }
            return list;
        }

        private string BuildIndexHtml() { // run from notebook thread
            const string PAGE_TMPL =
                @"<title>SQL Notebook Help</title>
                <style>
                    img.header-icon {
                        padding-top: 0px;
                        vertical-align: middle;
                        margin-right: 10px;
                    }
                    h1 {
                        margin-bottom: -12px;
                    }
                    ul {
                        list-style-type: square;
                    }
                    ul.group {
                        padding-top: 5px;
                        padding-bottom: 10px;
                    }
                    ul.group li {
                        font-size: 12px;
                    }
                </style>
                ";
            const string BOOK_TMPL = "<h2><img src=\"/book.png\" class=\"header-icon\"><span class=\"book-title\">{0}</span></h2><ul>{1}</ul>";
            const string DOC_TMPL = "<li><a href=\"{0}\">{1}</a></li>";

            var groupDefs = ReadGroupDefs();

            var docs =
                from row in _notebook.Query("SELECT path, book, title FROM docs").Rows
                let doc = new { Path = row[0].ToString(), Book = row[1].ToString(), Title = row[2].ToString() }
                orderby doc.Title.StartsWith("The ") ? doc.Title.Substring(4) : doc.Title
                select doc;

            foreach (var doc in docs) {
                foreach (var groupDef in groupDefs) {
                    if (groupDef.Book == doc.Book && groupDef.Pattern.IsMatch(doc.Title)) {
                        groupDef.Html += string.Format(DOC_TMPL,
                            doc.Path.Substring(1).Replace('\\', '/'),
                            WebUtility.HtmlEncode(doc.Title));
                        break;
                    }
                }
            }

            return PAGE_TMPL + string.Join("",
                from def in groupDefs
                group def by def.Book into grp
                orderby grp.Key
                let bookHtml = string.Join("",
                    from def in grp
                    select $"<li><b style=\"font-size: 14px;\">{def.Group}</b><ul class=\"group\">{def.Html}</ul></li>")
                select string.Format(BOOK_TMPL, WebUtility.HtmlEncode(grp.Key), bookHtml)
            );
        }

        private string BuildSearchHtml(string keyword) { // run from notebook thread
            try {
                string PAGE_TMPL =
                    @"<meta http-equiv=""content-type"" content=""text/html; charset=UTF-8"">
                    <title>Search Results: " + WebUtility.HtmlEncode(keyword) + @"</title>
                    <style>a {{ font-size: 14pt; }}</style>
                    <h2>Search for """ + WebUtility.HtmlEncode(keyword) + @"""</h2>
                    {0}";
                const string RESULT_TMPL = @"<a href=""{0}"">{1}</a><br>{2}<br><br>";
                var dt = _notebook.Query(
                    @"SELECT
                        f.id, d.path, d.book,
                        HIGHLIGHT(docs_fts, 1, '<b>', '</b>'),
                        SNIPPET(docs_fts, 2, '<b>', '</b>', '...', 25)
                    FROM docs_fts f
                    INNER JOIN docs d ON f.id = d.id
                    WHERE f.docs_fts MATCH @keyword
                    ORDER BY rank",
                    new Dictionary<string, object> { ["@keyword"] = string.Join(" ", keyword.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.DoubleQuote())) });
                var results =
                    from row in dt.Rows
                    let path = ((string)row[1]).Substring(1).Replace('\\', '/')
                    let title = (string)row[3]
                    let snippet = (string)row[4]
                    select string.Format(RESULT_TMPL, path, title, snippet);
                return string.Format(PAGE_TMPL, string.Join("", results));
            } catch (Exception ex) {
                return ex.Message;
            }
        }

        #region IDisposable
        private bool _disposed = false; // To detect redundant calls
        void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    _httpServer.Dispose();
                    _notebook.Dispose();
                }
                _disposed = true;
            }
        }
        public void Dispose() {
            Dispose(true);
        }
        #endregion
    }
}
