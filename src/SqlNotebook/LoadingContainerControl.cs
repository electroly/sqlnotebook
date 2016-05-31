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

using System.Windows.Forms;

namespace SqlNotebook {
    public partial class LoadingContainerControl : UserControl {
        private Control _control;
        private int _loadCount = 0;
        private string _errorMessage = null;

        public string ErrorMessage => _errorMessage;

        public Control ContainedControl {
            get {
                return _control;
            }
            set {
                _control = value;
                _control.Dock = DockStyle.Fill;
                Controls.Add(_control);
                _loadingLbl.Visible = false;
            }
        }

        public void PushLoad() {
            _loadCount++;
            ShowHideLoadingLbl();
        }

        public void PopLoad() {
            _loadCount--;
            ShowHideLoadingLbl();
        }

        public void SetError(string message) {
            _errorMessage = message;
            ShowHideLoadingLbl();
        }

        public void ClearError() {
            _errorMessage = null;
            ShowHideLoadingLbl();
        }

        private void ShowHideLoadingLbl() {
            _loadingLbl.Text = _errorMessage ?? "Please wait...";
            _loadingLbl.Visible = _errorMessage != null || _loadCount > 0;
            _loadingLbl.BringToFront();
        }

        public LoadingContainerControl() {
            InitializeComponent();
        }
    }
}
