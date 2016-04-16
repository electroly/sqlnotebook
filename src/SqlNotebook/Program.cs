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
using System.Linq;
using System.Windows.Forms;

namespace SqlNotebook {
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var clickOnceFiles = AppDomain.CurrentDomain.SetupInformation.ActivationArguments?.ActivationData ?? new string[0];
            foreach (var extraFilePath in clickOnceFiles.Skip(1)) {
                Process.Start(Application.ExecutablePath, $"\"{extraFilePath}");
            }

            string filePath;
            bool isNew;
            if (clickOnceFiles.Any()) {
                filePath = clickOnceFiles.First();
                isNew = false;
            } else if (Environment.GetCommandLineArgs().Length == 2) {
                filePath = Environment.GetCommandLineArgs()[1];
                isNew = false;
            } else {
                filePath = Path.GetTempFileName();
                isNew = true;
            }

            if (!File.Exists(filePath)) {
                MessageBox.Show("File does not exist: " + filePath, "SQL Notebook", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try {
                Application.Run(new MainForm(filePath, isNew));
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "SQL Notebook", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                if (isNew) {
                    try { File.Delete(filePath); } catch { }
                }
            }
        }
    }
}
