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
using CefSharp.WinForms;

namespace SqlNotebook {
    public partial class HelpDocumentControl : UserControl, IDocumentControl, IDocumentWithClosingEvent {
        private readonly ChromiumWebBrowser _browser;
        private string _title = "";
        private readonly string _homeUrl;

        string IDocumentControl.ItemName { get; set; }
        public void Save() { }

        public Action<string> SetTitleProc { get; set; } = x => { };

        public void OnClosing() {
            // CEF will crash if we don't remove the control before closing the form
            _browserPanel.Controls.Remove(_browser);
        }

        public HelpDocumentControl(string homeUrl, string initialUrl = "about:blank") {
            InitializeComponent();
            _homeUrl = homeUrl;
            _browser = new ChromiumWebBrowser(initialUrl) {
                Dock = DockStyle.Fill
            };
            _browser.TitleChanged += (sender, e) => {
                BeginInvoke(new MethodInvoker(() => {
                    _title = e.Title;
                    UpdateToolbar();
                }));
            };
            _browser.AddressChanged += (sender, e) => {
                BeginInvoke(new MethodInvoker(() => {
                    UpdateToolbar();
                }));
            };
            
            _browserPanel.Controls.Add(_browser);
            Disposed += (sender, e) => _browser.Dispose();
        }

        public void Navigate(string url) {
            _browser.Load(url);
        }
        
        private void UpdateToolbar() {
            SetTitleProc(_title);
            _backBtn.Enabled = _browser.CanGoBack;
            _forwardBtn.Enabled = _browser.CanGoForward;
            _progressBar.Visible = false;
        }

        private void BackBtn_Click(object sender, EventArgs e) {
            _browser.GetBrowser().GoBack();
        }

        private void ForwardBtn_Click(object sender, EventArgs e) {
            _browser.GetBrowser().GoForward();
        }

        private void HomeBtn_Click(object sender, EventArgs e) {
            _browser.Load(_homeUrl);
        }

        private void OpenBrowserBtn_Click(object sender, EventArgs e) {
            Process.Start(_browser.Address);
        }
    }
}
