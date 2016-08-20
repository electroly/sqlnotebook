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
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using mshtml;

namespace SqlNotebook {
    public partial class HelpDocumentControl : UserControl, IDocumentControl {
        private readonly string _homeUrl;

        string IDocumentControl.ItemName { get; set; }
        public void Save() { }

        public Action<string> SetTitleProc { get; set; } = x => { };

        public HelpDocumentControl(NotebookManager manager, string homeUrl, string initialUrl = "about:blank") {
            InitializeComponent();
            _homeUrl = homeUrl;
            _toolStrip.Renderer = new MenuRenderer();
            _contextMenuStrip.Renderer = new MenuRenderer();
            _browser.DocumentTitleChanged += (sender, e) => UpdateToolbar();
            _browser.CanGoBackChanged += (sender, e) => UpdateToolbar();
            _browser.CanGoForwardChanged += (sender, e) => UpdateToolbar();
            _browser.Navigated += (sender, e) => UpdateToolbar();
            _browser.DocumentTitleChanged += (sender, e) => UpdateToolbar();
            _browser.Navigate(initialUrl);
            _browser.PreviewKeyDown += (sender, e) => {
                if (e.KeyData == (Keys.Control | Keys.C)) {
                    EnableDisableContextMenu();
                    _copyMnu.PerformClick();
                } else if (e.KeyData == (Keys.Control | Keys.A)) {
                    _selectAllMnu.PerformClick();
                } else if (e.KeyData == (Keys.Alt | Keys.Left)) {
                    EnableDisableContextMenu();
                    _backMnu.PerformClick();
                } else if (e.KeyData == (Keys.Alt | Keys.Right)) {
                    EnableDisableContextMenu();
                    _forwardMnu.PerformClick();
                } else {
                    manager.HandleAppHotkeys(e.KeyData);
                }
            };
        }

        public void Navigate(string url) {
            _browser.Navigate(url);
        }
        
        private void UpdateToolbar() {
            SetTitleProc(_browser.DocumentTitle);
            _backBtn.Enabled = _browser.CanGoBack;
            _forwardBtn.Enabled = _browser.CanGoForward;
        }

        private void BackBtn_Click(object sender, EventArgs e) {
            _browser.GoBack();
        }

        private void ForwardBtn_Click(object sender, EventArgs e) {
            _browser.GoForward();
        }

        private void HomeBtn_Click(object sender, EventArgs e) {
            _browser.Navigate(_homeUrl);
        }

        private void OpenBrowserBtn_Click(object sender, EventArgs e) {
            Process.Start(_browser.Url.AbsoluteUri);
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
            EnableDisableContextMenu();
        }

        private void EnableDisableContextMenu() {
            _backMnu.Enabled = _browser.CanGoBack;
            _forwardMnu.Enabled = _browser.CanGoForward;

            var doc = (IHTMLDocument2)_browser.Document.DomDocument;
            _copyMnu.Enabled = doc.queryCommandEnabled("copy");
        }

        private void BackMnu_Click(object sender, EventArgs e) {
            _browser.GoBack();
        }

        private void ForwardMnu_Click(object sender, EventArgs e) {
            _browser.GoForward();
        }

        private void CopyMnu_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("copy", false, null);
        }

        private void SelectAllMnu_Click(object sender, EventArgs e) {
            _browser.Document.ExecCommand("selectAll", false, null);
        }
    }
}
