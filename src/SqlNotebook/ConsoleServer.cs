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
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using SqlNotebook.Properties;
using SqlNotebookCore;
using System.Net;
using System.Web;
using SqlNotebookScript;
using SqlNotebookScript.Utils;

namespace SqlNotebook {
    public sealed class ConsoleServer : IDisposable {
        private readonly HttpServer _server;
        private readonly ushort _port;
        private readonly NotebookManager _manager;
        private Dictionary<string, object> _variables = new Dictionary<string, object>();

        public string ConsoleName { get; set; }

        public int Port => _port;

        public ConsoleServer(NotebookManager manager, string consoleName) {
            _manager = manager;
            ConsoleName = consoleName;
            _server = new HttpServer(0);
            _port = _server.Port;
            _server.Request += Server_Request;

            // read previous variable data if present
            var state = manager.Notebook.UserData.ConsoleStates.FirstOrDefault(x => x.ConsoleName == consoleName);
            if (state != null) {
                var keys = state.VarNames;
                var values = ArrayUtil.GetArrayElements(Convert.FromBase64String(state.VarDataB64));
                _variables = keys.Zip(values, (k, v) => Tuple.Create(k, v)).ToDictionary(x => x.Item1, x => x.Item2);
            }
        }

        private void Server_Request(object sender, HttpRequestEventArgs e) {
            var consolePrefix = "/console";
            var executePrefix = "/execute_";
            if (e.Url.StartsWith(consolePrefix)) {
                var html = Resources.ConsoleHtml.Replace("SAVED_HISTORY_GOES_HERE",
                    HttpUtility.JavaScriptStringEncode(_manager.GetItemData(ConsoleName)));
                e.Result = Encoding.UTF8.GetBytes(html);
            } else if (e.Url.StartsWith(executePrefix)) {
                var sql = e.Url.Substring(executePrefix.Length);
                var html = Execute(sql);
                e.Result = Encoding.UTF8.GetBytes(html);
                return;
            } else if (e.Url == "/simple-console.js") {
                e.ContentType = HttpContentType.JavaScript;
                e.Result = Resources.SimpleConsoleJs;
            } else if (e.Url == "/simple-console.css") {
                e.ContentType = HttpContentType.Css;
                e.Result = Resources.SimpleConsoleCss;
            } else {
                e.ResultCode = 404;
                e.Result = new byte[] { 78, 79 }; // 'NO'
            }
        }

        private void UpdateConsoleState() {
            var list = _manager.Notebook.UserData.ConsoleStates;
            list.RemoveWhere(x => x.ConsoleName == ConsoleName);
            var data = ArrayUtil.ConvertToSqlArray(_variables.Values.ToList());
            var dataB64 = Convert.ToBase64String(data);
            list.Add(new ConsoleStateRecord {
                ConsoleName = ConsoleName,
                VarNames = _variables.Keys.ToList(),
                VarDataB64 = dataB64
            });
        }

        private string Execute(string sql) {
            _manager.PushStatus("Running console command...");
            _manager.CommitOpenEditors();
            string response;
            try {
                var result = _manager.ExecuteScriptEx(sql, _variables, NotebookManager.TransactionType.Transaction, out var newVars);
                _variables = newVars;
                UpdateConsoleState();
                _manager.SetDirty();
                _manager.Rescan();
                var parts = new List<string>();
                if (result.ScalarResult == null && !result.TextOutput.Any() && !result.DataTables.Any()) {
                    parts.Add("<div style=\"overflow: hidden;\">&nbsp;</div>");
                }
                if (result.ScalarResult != null) {
                    parts.Add(
                        "<div style=\"overflow: hidden;\">" +
                        $"Returned: {WebUtility.HtmlEncode(result.ScalarResult.ToString())}" +
                        "</div>"
                    );
                }
                if (result.TextOutput.Any()) {
                    parts.Add(
                        "<div style=\"overflow: hidden; overflow-x: auto; padding-bottom: 18px;\"><pre style=\"margin: 0; padding: 0;\"><font face=\"Segoe UI\" size=\"2\">" +
                        string.Join("<br>", result.TextOutput.Select(WebUtility.HtmlEncode)) +
                        "</font></pre></div>"
                    );
                }
                var sb = new StringBuilder();
                // inline the CSS so it gets included when the user copies tables to the clipboard
                var cellCss = "border: 1px solid rgb(229, 229, 229); padding: 3px; padding-left: 6px; " +
                    "padding-right: 6px; text-align: left; vertical-align: top; font: 'Segoe UI' 9pt; " +
                    "max-height: 100px; overflow-y: auto;";
                foreach (var dt in result.DataTables) {
                    sb.Append("<div style=\"overflow-x: auto; overflow-y: hidden; padding-bottom: 18px;\">");
                    sb.Append("<table style=\"border-collapse: collapse;\"><thead><tr>");
                    foreach (var col in dt.Columns) {
                        sb.Append($"<td style=\"{cellCss}\"><pre style=\"margin: 0; padding: 0;\">" +
                            $"<font face=\"Segoe UI\" size=\"2\"><b>{WebUtility.HtmlEncode(col)}</b></font></pre></td>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    int count = 0;
                    foreach (var row in dt.Rows) {
                        if (count >= 100) {
                            break;
                        }
                        sb.Append("<tr>");
                        foreach (var cell in row) {
                            string cellText = cell.ToString();
                            var cellBytes = cell as byte[];
                            if (cellBytes != null) {
                                if (ArrayUtil.IsSqlArray(cellBytes)) {
                                    cellText = "[" + string.Join(", ", ArrayUtil.GetArrayElements(cellBytes)) + "]";
                                }
                            }
                            sb.Append($"<td style=\"{cellCss}\"><pre style=\"margin:0; padding:0;\">" +
                                $"<font face=\"Segoe UI\" size=\"2\">{WebUtility.HtmlEncode(cellText)}</font></pre></td>");
                        }
                        sb.Append("</tr>");
                        count++;
                    }
                    sb.Append("</tbody></table></div>");
                    if (dt.Rows.Count > 100) {
                        sb.Append($"<div>Table has {dt.Rows.Count} rows, showing 100.</div>");
                    }
                }
                parts.Add(sb.ToString());
                var html = string.Join("", parts);
                response = html;
            } catch (Exception ex) {
                response = $"<div style=\"overflow: hidden; color: red;\">{WebUtility.HtmlEncode(ex.Message)}</div>";
            }

            _manager.PopStatus();
            return $"<div class=\"response\">{response}</div>";
        }

        #region IDisposable Support
        private bool _disposed = false; // To detect redundant calls

        void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    // dispose managed state (managed objects).
                    if (_server != null) {
                        _server.Dispose();
                    }
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
