﻿// SQL Notebook
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class WaitForm : Form {
        public Task WaitTask;
        public Exception ResultException { get; private set; }

        public WaitForm(string title, string text, Action action) {
            InitializeComponent();
            Text = title;
            _infoTxt.Text = text;

            WaitTask = Task.Run(() => {
                try {
                    action();
                } catch (Exception ex) {
                    ResultException = ex;
                }
                if (IsHandleCreated) {
                    Invoke(new MethodInvoker(() => {
                        DialogResult = ResultException == null ? DialogResult.OK : DialogResult.Abort;
                        Close();
                    }));
                }
            });

            this.Shown += (sender, e) => {
                if (WaitTask.IsCompleted) {
                    DialogResult = ResultException == null ? DialogResult.OK : DialogResult.Abort;
                    Close();
                }
            };
        }

        public DialogResult ShowDialog(IWin32Window owner, int waitMsecBeforeShowing) {
            WaitTask.Wait(waitMsecBeforeShowing);
            if (WaitTask.IsCompleted) {
                return ResultException == null ? DialogResult.OK : DialogResult.Abort;
            } else {
                return ShowDialog(owner);
            }
        }
    }
}