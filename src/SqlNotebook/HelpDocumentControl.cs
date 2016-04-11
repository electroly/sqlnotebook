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
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class HelpDocumentControl : UserControl, IDocumentControl {
        private Func<int> _getPortFunc;

        string IDocumentControl.ItemName { get; set; }
        string IDocumentControl.DocumentText { get; } = "";

        public HelpDocumentControl(Func<int> getPortFunc) {
            InitializeComponent();
            _browser.DocumentTitleChanged += (sender, e) => UpdateToolbar();
            _browser.CanGoBackChanged += (sender, e) => UpdateToolbar();
            _browser.CanGoForwardChanged += (sender, e) => UpdateToolbar();
            _browser.Navigating += (sender, e) => _titleLbl.Text = "Loading...";
            _browser.Navigated += (sender, e) => UpdateToolbar();
            _getPortFunc = getPortFunc;
        }

        public void Navigate(string url) {
            _browser.Navigate(url);
        }
        
        private void UpdateToolbar() {
            _titleLbl.Text = _browser.DocumentTitle;
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
            _browser.Navigate($"http://127.0.0.1:{_getPortFunc()}/index.html");
        }
    }
}
