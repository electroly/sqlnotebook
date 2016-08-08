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
using System.IO;
using System.IO.Compression;
using System.Text;
using SqlNotebook.Properties;
using SqlNotebookCore;
using SqlNotebookScript;

namespace SqlNotebook {
    public sealed class NoteServer : IDisposable {
        private readonly HttpServer _server;
        private readonly ushort _port;
        private readonly Dictionary<string, byte[]> _files = new Dictionary<string, byte[]>();
        private readonly NotebookManager _manager;

        public int Port => _port;

        public NoteServer(NotebookManager manager) {
            using (var stream = new MemoryStream(Resources.TinymceZip))
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Read)) {
                foreach (var entry in archive.Entries) {
                    using (var entryStream = entry.Open())
                    using (var entryMemoryStream = new MemoryStream()) {
                        entryStream.CopyTo(entryMemoryStream);
                        _files[entry.FullName] = entryMemoryStream.ToArray();
                    }
                }
            }

            _manager = manager;
            _server = new HttpServer(0);
            _port = _server.Port;
            _server.Request += Server_Request;
        }

        private void Server_Request(object sender, HttpRequestEventArgs e) {
            var notePrefix = "/note_";
            if (e.Url.StartsWith(notePrefix)) {
                // the filename suffix is the base64-encoded bytes of the UTF-8 note name
                var encoded = e.Url.Substring(notePrefix.Length);
                var utf8bytes = Convert.FromBase64String(encoded);
                var noteName = Encoding.UTF8.GetString(utf8bytes);
                var noteContent = _manager.GetItemData(noteName);
                var html = Resources.NoteEditorHtml.Replace("<!--DOCUMENT_HTML-->", noteContent);
                var utf8 = new UTF8Encoding(false);
                e.Result = utf8.GetBytes(html);
                return;
            }

            var filePath = e.Url.TrimStart('/').Replace('/', '\\');
            var bytes = _files.GetValueOrDefault(filePath);
            e.Result = bytes ?? new byte[0];
            if (filePath.EndsWith(".css")) {
                e.ContentType = HttpContentType.Css;
            }
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
