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
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using SqlNotebookCore;
using SqlNotebookScript.Utils;

namespace SqlNotebook {
    public partial class ConsoleDocumentControl : UserControl, IDocumentControl, IDocumentWithClosingEvent {
        private readonly IWin32Window _mainForm;
        private readonly NotebookManager _manager;
        private readonly Notebook _notebook;
        private readonly ConsoleServer _server;
        private readonly ChromiumWebBrowser _browser;
        private BlockingCollection<string> _consoleMessages = new BlockingCollection<string>();
        private bool _loaded;

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
            if (!_loaded) {
                return;
            }
            _consoleMessages.Drain();
            _browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("dumpToConsole();");
            while (!_consoleMessages.Any()) {
                Cef.DoMessageLoopWork();
                Thread.Sleep(0);
            }
            var text = _consoleMessages.Take();
            _manager.SetItemData(ItemName, text);
        }

        public void OnClosing() {
            Controls.Remove(_browser); // CEF will crash if we don't remove the control before closing the form
        }

        public ConsoleDocumentControl(string name, NotebookManager manager, IWin32Window mainForm, Slot<bool> operationInProgress) {
            InitializeComponent();
            ItemName = name;
            _manager = manager;
            _notebook = manager.Notebook;
            _mainForm = mainForm;
            _server = new ConsoleServer(manager, response => {
                _browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(
                    $"receiveCommandResponse(\"{HttpUtility.JavaScriptStringEncode(response)}\");"
                );
            }, name);
            var encodedName = Convert.ToBase64String(Encoding.UTF8.GetBytes(name));
            var url = $"http://127.0.0.1:{_server.Port}/console_{encodedName}";
            _browser = new ChromiumWebBrowser(url) {
                Dock = DockStyle.Fill
            };
            _browser.ProcessCefMessagesOnResize();
            _browser.ConsoleMessage += (sender, e) => {
                if (e.Message == "loaded") {
                    BeginInvoke(new MethodInvoker(() => {
                        _progressBar.Visible = false;
                        _loaded = true;
                        _browser.Focus();
                    }));
                } else {
                    _consoleMessages.Add(e.Message);
                    Console.WriteLine(e.Message);
                }
            };

            Controls.Add(_browser);
            Disposed += (sender, e) => _browser.Dispose();
        }
    }
}
