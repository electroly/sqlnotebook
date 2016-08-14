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
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using mshtml;
using SqlNotebookCore;
using SqlNotebookScript.Utils;

namespace SqlNotebook {
    public partial class ConsoleDocumentControl : UserControl, IDocumentControl { //, IDocumentWithClosingEvent {
        private readonly IWin32Window _mainForm;
        private readonly NotebookManager _manager;
        private readonly Notebook _notebook;
        private readonly ConsoleServer _server;

        private string _itemName;
        public string ItemName {
            get {
                return _itemName;
            }
            set {
                _itemName = value;
                if (_server != null) {
                    _server.ConsoleName = value;
                }
            }
        }

        public void Save() {
            try {
                var html = _browser.Document.GetElementById("simple-console-output").InnerHtml;
                _manager.SetItemData(ItemName, html);
            } catch (Exception) { }
        }

        public ConsoleDocumentControl(string name, NotebookManager manager, IWin32Window mainForm) {
            InitializeComponent();
            ItemName = name;
            _manager = manager;
            _notebook = manager.Notebook;
            _mainForm = mainForm;
            _contextMenuStrip.Renderer = new MenuRenderer();
            
            _server = new ConsoleServer(manager, name);
            var url = $"http://127.0.0.1:{_server.Port}/console";
            Debug.WriteLine(url);
            _browser.Navigate(url);
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
            var doc = (IHTMLDocument2)_browser.Document.DomDocument;
            _cutMnu.Enabled = doc.queryCommandEnabled("cut");
            _copyMnu.Enabled = doc.queryCommandEnabled("copy");
            _pasteMnu.Enabled = doc.queryCommandEnabled("paste");
        }

        private void CutMnu_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("cut", false, null);
        }

        private void CopyMnu_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("copy", false, null);
        }

        private void PasteMnu_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("paste", false, null);
        }

        private void SelectAllMnu_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("selectAll", false, null);
        }
    }
}
