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
using System.IO;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using SqlNotebook.Properties;

namespace SqlNotebook {
    public partial class AboutForm : Form {
        private readonly ChromiumWebBrowser _browser;

        public sealed class AboutLifeSpanHandler : ILifeSpanHandler {
            public bool DoClose(IWebBrowser browserControl, IBrowser browser) => false;
            public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser) { }
            public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser) { }

            public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser,
            IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition,
            bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo,
            IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser) {
                newBrowser = null;
                Process.Start(targetUrl);
                return true;
            }
        }

        public AboutForm() {
            InitializeComponent();
            var filePath = Path.GetTempFileName();
            Disposed += (sender, e) => File.Delete(filePath);
            File.WriteAllText(filePath, Resources.ThirdPartyLicensesHtml);
            _browser = new ChromiumWebBrowser(filePath) {
                Dock = DockStyle.Fill,
                LifeSpanHandler = new AboutLifeSpanHandler()
            };
            Disposed += (sender, e) => _browser.Dispose();
            _browserPanel.Controls.Add(_browser);
            Text += $" {Application.ProductVersion}";
        }

        private void OkBtn_Click(object sender, EventArgs e) {
            Close();
        }

        private void GithubLnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("https://github.com/electroly/sqlnotebook");
        }

        private void WebsiteLnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("https://sqlnotebook.com/");
        }
    }
}
