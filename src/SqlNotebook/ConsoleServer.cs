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
        private readonly Action<string> _responseFunc;
        private Dictionary<string, object> _variables = new Dictionary<string, object>();

        public string ConsoleName { get; set; }

        public int Port => _port;

        public ConsoleServer(NotebookManager manager, Action<string> responseFunc, string consoleName) {
            _manager = manager;
            _responseFunc = responseFunc;
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
                Task.Run(() => Execute(sql));
                e.Result = new byte[] { 79, 75 }; // 'OK'
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

        private void Execute(string sql) {
            _manager.PushStatus("Running console command...");
            _manager.CommitOpenEditors();
            string response;
            try {
               Dictionary<string, object> newVars;
                var result = _manager.ExecuteScriptEx(sql, _variables, true, out newVars);
                _variables = newVars;
                UpdateConsoleState();
                _manager.SetDirty();
                var parts = new List<string>();
                if (result.ScalarResult != null) {
                    parts.Add($"Returned: {WebUtility.HtmlEncode(result.ScalarResult.ToString())}");
                }
                if (result.TextOutput.Any()) {
                    parts.Add(string.Join("<br>", result.TextOutput.Select(WebUtility.HtmlEncode)));
                }
                var sb = new StringBuilder();
                foreach (var dt in result.DataTables) {
                    sb.Append("<div style=\"overflow-x: auto;\">");
                    sb.Append("<table><thead><tr>");
                    foreach (var col in dt.Columns) {
                        sb.Append($"<td><b>{WebUtility.HtmlEncode(col)}</b></td>");
                    }
                    sb.Append("</tr></thead><tbody>");
                    int count = 0;
                    foreach (var row in dt.Rows) {
                        if (count >= 100) {
                            break;
                        }
                        sb.Append("<tr>");
                        foreach (var cell in row) {
                            sb.Append($"<td>{WebUtility.HtmlEncode(cell.ToString())}</td>");
                        }
                        sb.Append("</tr>");
                        count++;
                    }
                    sb.Append("</tbody></table></div>");
                    if (dt.Rows.Count > 100) {
                        sb.Append($"Table has {dt.Rows.Count} rows, showing 100.");
                    }
                }
                parts.Add(sb.ToString());
                var html = string.Join("<br><br>", parts);
                response = html;
            } catch (Exception ex) {
                response = $"<span style=\"color: red\">{WebUtility.HtmlEncode(ex.Message)}</span>";
            }

            _responseFunc($"<div class=\"response\">{response}</div>");
            _manager.PopStatus();
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
