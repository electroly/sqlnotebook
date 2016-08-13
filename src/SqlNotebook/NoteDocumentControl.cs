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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using CefSharp.WinForms;
using System.Collections.Concurrent;
using CefSharp;
using System.Threading;

namespace SqlNotebook {
    public partial class NoteDocumentControl : UserControl, IDocumentControl, IDocumentWithClosingEvent {
        private readonly NotebookManager _manager;
        private readonly ChromiumWebBrowser _browser;
        private bool _mceLoaded;
        private BlockingCollection<string> _consoleMessages = new BlockingCollection<string>();

        public string ItemName { get; set; }

        private static object _saveLock = new object();
        public void Save() {
            if (!_mceLoaded) {
                return;
            }
            lock (_saveLock) {
                var browser = _browser.GetBrowser();
                var frame = browser.MainFrame;
                _consoleMessages.Drain();
                frame.ExecuteJavaScriptAsync("console.log(tinymce.activeEditor.getContent());");
                while (!_consoleMessages.Any()) {
                    Cef.DoMessageLoopWork();
                    Thread.Sleep(0);
                }
                var text = _consoleMessages.Take();
                _manager.SetItemData(ItemName, text);
            }
        }

        public void OnClosing() {
            Controls.Remove(_browser); // CEF will crash if we don't remove the control before closing the form
        }

        public NoteDocumentControl(string name, NotebookManager manager) {
            InitializeComponent();
            
            ItemName = name;
            _manager = manager;
            lock (_manager.NoteServerLock) {
                var encodedName = Convert.ToBase64String(Encoding.UTF8.GetBytes(name));
                var url = $"http://127.0.0.1:{_manager.NoteServer.Port}/note_{encodedName}";
                Debug.WriteLine(url);
                _browser = new ChromiumWebBrowser(url) {
                    Dock = DockStyle.Fill
                };
                _browser.ProcessCefMessagesOnResize();
                _browser.ConsoleMessage += (sender, e) => {
                    if (e.Message == "loaded") {
                        BeginInvoke(new MethodInvoker(() => {
                            _loadingPanel.Visible = false;
                            _mceLoaded = true;
                            _browser.Focus();
                        }));
                    } else {
                        _consoleMessages.Add(e.Message);
                    }
                };
                Controls.Add(_browser);
                Disposed += (sender, e) => _browser.Dispose();
            }
        }
    }
}
