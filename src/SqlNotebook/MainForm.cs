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
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using SqlNotebook.Properties;
using SqlNotebookCore;
using SqlNotebookScript;
using WeifenLuo.WinFormsUI.Docking;

namespace SqlNotebook {
    public partial class MainForm : Form {
        private readonly DockPanel _dockPanel;
        private NotebookManager _manager;
        private Notebook _notebook;
        private readonly UserControlDockContent _contentsPane;
        private readonly Importer _importer;
        private readonly ExplorerControl _explorer;
        private string _filePath {  get { return _notebook.GetFilePath(); } }
        private bool _isNew;
        private bool _isDirty;

        private readonly Dictionary<NotebookItem, UserControlDockContent> _openItems
            = new Dictionary<NotebookItem, UserControlDockContent>();

        public MainForm(string filePath, bool isNew) {
            InitializeComponent();
            if (isNew) {
                _notebook = new Notebook(filePath, isNew);
            } else {
                var f = new WaitForm("SQL Notebook", $"Opening notebook \"{Path.GetFileNameWithoutExtension(filePath)}\"", () => {
                    _notebook = new Notebook(filePath, isNew);
                });
                f.StartPosition = FormStartPosition.CenterScreen;
                f.ShowDialog();
            }
            _isNew = isNew;
            SetTitle();
            _manager = new NotebookManager(_notebook);
            _importer = new Importer(_manager, this);
            _dockPanel = new DockPanel {
                Dock = DockStyle.Fill,
                Theme = new VS2012LightTheme(),
                DocumentStyle = DocumentStyle.DockingWindow
            };
            _toolStripContainer.ContentPanel.Controls.Add(_dockPanel);

            _contentsPane = new UserControlDockContent("Table of Contents", _explorer = new ExplorerControl(_manager, this),
                DockAreas.DockLeft | DockAreas.DockRight);
            _contentsPane.CloseButtonVisible = false;
            _contentsPane.Show(_dockPanel, DockState.DockLeft);

            _manager.NotebookItemOpenRequest += Manager_NotebookItemOpenRequest;
            _manager.NotebookItemCloseRequest += Manager_NotebookItemCloseRequest;
            _manager.NotebookItemsSaveRequest += Manager_NotebookItemsSaveRequest;
            _manager.NotebookDirty += (sender, e) => SetDirty();
            _manager.NotebookItemRename += Manager_NotebookItemRename;
            _manager.StatusUpdate += (sender, e) => BeginInvoke(new MethodInvoker((() => {
                _statusLbl.Text = e.Status;
                NativeMethods.EnableWindow(_dockPanel.Handle, !e.Status.Any());
                NativeMethods.EnableWindow(_toolStrip.Handle, !e.Status.Any());
                bool oldVisible = _statusProgressbar.Visible;
                bool newVisible = !string.IsNullOrWhiteSpace(e.Status);
                if (!oldVisible && newVisible) {
                    _statusProgressbar.Style = ProgressBarStyle.Continuous;
                    _statusProgressbar.Style = ProgressBarStyle.Marquee;
                }
                if (oldVisible != newVisible) {
                    if (newVisible) {
                        _statusLbl.Visible = true;
                        _statusProgressbar.Visible = true;
                    } else {
                        _statusProgressbar.Visible = false;
                        _statusLbl.Visible = false;
                    }
                }
            })));

            if (isNew) {
                _manager.NewNote("Getting Started", Resources.GettingStartedRtf);
                Load += (sender, e) => {
                    OpenItem(new NotebookItem(NotebookItemType.Note, "Getting Started"));
                    _isDirty = false;
                    SetTitle();
                };
            }

            Load += (sender, e) => _manager.Rescan();
        }

        private static class NativeMethods {
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);
        }

        private void Manager_NotebookItemRename(object sender, NotebookItemRenameEventArgs e) {
            UserControlDockContent ucdc;
            if (_openItems.TryGetValue(e.Item, out ucdc)) {
                ucdc.Text = e.NewName;
                ucdc.Content.ItemName = e.NewName;
                _openItems.Remove(e.Item);
                var newItem = new NotebookItem(e.Item.Type, e.NewName);
                _openItems.Add(newItem, ucdc);
            }
            _manager.Rescan();
        }

        private void Manager_NotebookItemsSaveRequest(object sender, EventArgs e) {
            Invoke(new MethodInvoker(SaveOpenItems));
        }

        private void Manager_NotebookItemCloseRequest(object sender, NotebookItemRequestEventArgs e) {
            UserControlDockContent ucdc;
            if (_openItems.TryGetValue(e.Item, out ucdc)) {
                ucdc.Close();
            }
        }

        private void SetTitle() {
            string prefix;
            if (_isNew) {
                prefix = "Untitled";
            } else {
                prefix = Path.GetFileNameWithoutExtension(_filePath);
            }
            var star = _isDirty ? "*" : "";
            Text = $"{prefix}{star} - SQL Notebook";
            _saveMnu.Enabled = _isDirty;
        }

        private void SetDirty() {
            if (!_isDirty) {
                _isDirty = true;
                BeginInvoke(new MethodInvoker(SetTitle));
            }
        }

        private void Manager_NotebookItemOpenRequest(object sender, NotebookItemRequestEventArgs e) {
            OpenItem(e.Item);
        }

        private void OpenItem(NotebookItem item) {
            UserControlDockContent wnd;
            if (_openItems.TryGetValue(item, out wnd)) {
                wnd.Activate();
                wnd.Focus();
                return;
            }

            UserControlDockContent f = null;
            if (item.Type == NotebookItemType.Console) {
                var doc = new ConsoleDocumentControl(item.Name, _manager, this);
                f = new UserControlDockContent(item.Name, doc) {
                    Icon = Resources.ApplicationXpTerminalIco
                };
                f.FormClosing += (sender2, e2) => {
                    _manager.SetItemData(doc.ItemName, doc.DocumentText);
                };
            } else if (item.Type == NotebookItemType.Note) {
                var doc = new NoteDocumentControl(item.Name, _manager);
                f = new UserControlDockContent(item.Name, doc) {
                    Icon = Resources.NoteIco
                };
                f.FormClosing += (sender2, e2) => {
                    _manager.SetItemData(doc.ItemName, doc.DocumentText);
                };
            } else {
                bool runImmediately = false;
                if (item.Type == NotebookItemType.Table || item.Type == NotebookItemType.View) {
                    var name = _manager.NewScript();
                    _manager.SetItemData(name, $"SELECT * FROM \"{item.Name.Replace("\"", "\"\"")}\" LIMIT 1000");
                    item = new NotebookItem(NotebookItemType.Script, name);
                    runImmediately = true;
                }

                var doc = new QueryDocumentControl(item.Name, _manager, this, runImmediately);
                f = new UserControlDockContent(item.Name, doc) {
                    Icon = Resources.ScriptIco
                };
                f.FormClosing += (sender2, e2) => {
                    _manager.SetItemData(doc.ItemName, doc.DocumentText);
                };
            }

            f.FormClosed += (sender2, e2) => {
                _openItems.Remove(item);
            };
            f.Show(_dockPanel);
            _openItems[item] = f;
        }


        protected override void OnFormClosed(FormClosedEventArgs e) {
            base.OnFormClosed(e);
            _notebook.Dispose();
            _notebook = null;
        }

        private void AboutMnu_Click(object sender, EventArgs e) {
            using (var frm = new AboutForm()) {
                frm.ShowDialog(this);
            }
        }

        private async void ImportFileMnu_Click(object sender, EventArgs e) {
            try {
                await _importer.DoFileImport();
            } catch (Exception ex) {
                ErrorBox("Import Error", ex.Message);
            }
        }

        private async void ImportPostgresMnu_Click(object sender, EventArgs e) {
            try {
                await _importer.DoDatabaseImport<PgImportSession>();
            } catch (Exception ex) {
                ErrorBox("Import Error", ex.Message);
            }
        }

        private async void ImportMssqlMnu_Click(object sender, EventArgs e) {
            try {
                await _importer.DoDatabaseImport<MsImportSession>();
            } catch (Exception ex) {
                ErrorBox("Import Error", ex.Message);
            }
        }

        private async void ImportMysqlMnu_Click(object sender, EventArgs e) {
            try {
                await _importer.DoDatabaseImport<MyImportSession>();
            } catch (Exception ex) {
                ErrorBox("Import Error", ex.Message);
            }
        }

        private void ErrorBox(string title, string message, string details = null) {
            var d = new TaskDialog {
                Caption = title,
                InstructionText = message,
                Text = details,
                OwnerWindowHandle = Handle,
                StartupLocation = TaskDialogStartupLocation.CenterOwner,
                Icon = TaskDialogStandardIcon.Error,
                StandardButtons = TaskDialogStandardButtons.Ok
            };
            using (d) {
                d.Show();
            }
        }

        private void NewConsoleBtn_Click(object sender, EventArgs e) {
            try {
                OpenItem(new NotebookItem(NotebookItemType.Console, _manager.NewConsole()));
            } catch (Exception ex) {
                ErrorBox("Notebook Error", "There was a problem creating the console.", ex.Message);
            }
        }

        private void NewScriptBtn_Click(object sender, EventArgs e) {
            try {
                OpenItem(new NotebookItem(NotebookItemType.Script, _manager.NewScript()));
            } catch (Exception ex) {
                ErrorBox("Notebook Error", "There was a problem creating the script.", ex.Message);
            }
        }

        private void NewNoteBtn_Click(object sender, EventArgs e) {
            try {
                OpenItem(new NotebookItem(NotebookItemType.Note, _manager.NewNote()));
            } catch (Exception ex) {
                ErrorBox("Notebook Error", "There was a problem creating the note.", ex.Message);
            }
        }

        public static bool IgnoreF5 = false;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if (keyData == Keys.Escape) {
                _notebook.Interrupt();
            } else if (keyData == Keys.F5) {
                if (IgnoreF5) {
                    return true;
                }
                var doc = _dockPanel.ActiveDocument as UserControlDockContent;
                if (doc != null) {
                    var queryDoc = doc.Content as QueryDocumentControl;
                    if (queryDoc != null) {
                        IgnoreF5 = true;
                        queryDoc.Execute().ContinueWith((t) => {
                            IgnoreF5 = false;
                        });
                        return true;
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ExitMnu_Click(object sender, EventArgs e) {
            Close();
        }

        private void NewMnu_Click(object sender, EventArgs e) {
            Process.Start(Application.ExecutablePath);
        }

        private void OpenMnu_Click(object sender, EventArgs e) {
            var f = new OpenFileDialog {
                AutoUpgradeEnabled = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = ".sqlnb",
                DereferenceLinks = true,
                Filter = "SQL Notebook files|*.sqlnb",
                Multiselect = false,
                SupportMultiDottedExtensions = true,
                Title = "Open Notebook",
                ValidateNames = true
            };
            using (f) {
                if (f.ShowDialog(this) == DialogResult.OK) {
                    Process.Start(Application.ExecutablePath, $"\"{f.FileName}");
                }
            }
        }

        private void SaveMnu_Click(object sender, EventArgs e) {
            SaveOrSaveAs();
        }

        private bool SaveOrSaveAs(bool saveAs = false) {
            SaveOpenItems();

            if (_isNew || saveAs) {
                var f = new SaveFileDialog {
                    AddExtension = true,
                    AutoUpgradeEnabled = true,
                    CheckPathExists = true,
                    DefaultExt = ".sqlnb",
                    Filter = "SQL Notebook files|*.sqlnb",
                    OverwritePrompt = true,
                    SupportMultiDottedExtensions = true,
                    Title = "Save Notebook As",
                    ValidateNames = true
                };
                using (f) {
                    if (f.ShowDialog(this) == DialogResult.OK) {
                        new WaitForm("Save", "Saving your notebook...", () => {
                            _manager.SaveAs(f.FileName);
                        }).ShowDialog(this);
                        _isNew = false;
                    } else {
                        return false;
                    }
                }
            } else {
                new WaitForm("Save", "Saving your notebook...", () => {
                    _manager.Save();
                }).ShowDialog(this);
            }

            // set this after doing the isNew stuff above so that if you click Save and then cancel the Save As dialog,
            // the file remains dirty. since untitled files are in stored in temporary files it doesn't matter that we
            // saved to it despite the cancelation.
            _isDirty = false;
            SetTitle();
            return true;
        }

        private void SaveOpenItems() {
            foreach (var x in _openItems) {
                _manager.SetItemData(x.Key.Name, x.Value.Content.DocumentText);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (_isDirty) {
                var shortFilename = _isNew ? "Untitled" : Path.GetFileNameWithoutExtension(_filePath);
                var d = new TaskDialog {
                    Caption = "SQL Notebook",
                    InstructionText = $"Do you want to save changes to {shortFilename}?",
                    OwnerWindowHandle = Handle,
                    StartupLocation = TaskDialogStartupLocation.CenterOwner
                };
                using (d) {
                    var saveBtn = new TaskDialogButton("save", "&Save");
                    saveBtn.Click += (sender2, e2) => d.Close(TaskDialogResult.Yes);
                    d.Controls.Add(saveBtn);

                    var dontSaveBtn = new TaskDialogButton("no", "Do&n't Save");
                    dontSaveBtn.Click += (sender2, e2) => d.Close(TaskDialogResult.No);
                    d.Controls.Add(dontSaveBtn);

                    var cancelBtn = new TaskDialogButton("cancel", "Cancel");
                    cancelBtn.Click += (sender2, e2) => d.Close(TaskDialogResult.Cancel);
                    d.Controls.Add(cancelBtn);

                    switch (d.Show()) {
                        case TaskDialogResult.Yes:
                            if (!SaveOrSaveAs()) {
                                e.Cancel = true;
                            }
                            break;
                        case TaskDialogResult.Cancel:
                            e.Cancel = true;
                            return;
                    }
                }
            }
        }

        private void RecentFilesMnu_DropDownOpening(object sender, EventArgs e) {
            PopulateRecentMenu(typeof(IFileImportSession), _recentFilesMnu, _recentFilesNoneMnu);
        }

        private void RecentServersMnu_DropDownOpening(object sender, EventArgs e) {
            PopulateRecentMenu(typeof(IDatabaseImportSession), _recentServersMnu, _recentServersNoneMnu);
        }

        private void PopulateRecentMenu(Type sessionType, ToolStripMenuItem menu, ToolStripMenuItem noneMenu) {
            while (menu.DropDownItems.Count > 1) {
                menu.DropDownItems.RemoveAt(1);
            }

            var items = RecentDataSources.List
                .Where(x => x.ImportSessionType.GetInterfaces().Contains(sessionType))
                .Reverse();

            foreach (var item in items) {
                var tsmi = new ToolStripMenuItem(item.DisplayName);
                tsmi.Click += async (sender2, e2) => {
                    await new Importer(_manager, this).DoRecentImport(item);
                };
                menu.DropDownItems.Add(tsmi);
            }

            noneMenu.Visible = !items.Any();
        }

        private void SaveAsMnu_Click(object sender, EventArgs e) {
            SaveOrSaveAs(saveAs: true);
        }

        private async void ExportMnu_Click(object sender, EventArgs e) {
            SaveOpenItems();

            var scripts = _manager.Items.Where(x => x.Type == NotebookItemType.Script).Select(x => x.Name);
            string scriptName;
            bool doSaveAs;

            using (var f = new ExportForm(scripts)) {
                var result = f.ShowDialog(this);
                scriptName = f.ScriptName;
                if (result == DialogResult.Yes) {
                    doSaveAs = false; // open
                } else if (result == DialogResult.No) {
                    doSaveAs = true; // save
                } else {
                    return;
                }
            }

            string filePath;
            if (doSaveAs) {
                var f = new SaveFileDialog {
                    AddExtension = true,
                    AutoUpgradeEnabled = true,
                    CheckPathExists = true,
                    DefaultExt = ".csv",
                    Filter = "CSV files|*.csv",
                    OverwritePrompt = true,
                    SupportMultiDottedExtensions = true,
                    Title = "Save CSV As",
                    ValidateNames = true
                };
                using (f) {
                    if (f.ShowDialog(this) == DialogResult.OK) {
                        filePath = f.FileName;
                    } else {
                        return;
                    }
                }
            } else {
                filePath = GetTemporaryExportFilePath();
            }

            _manager.PushStatus("Running the selected script. Press ESC to cancel.");
            try {
                var output = await Task.Run(() => _manager.ExecuteScript($"EXECUTE \"{scriptName.Replace("\"", "\"\"")}\""));
                _manager.PopStatus();
                _manager.PushStatus("Writing CSV file. Please wait.");
                await Task.Run(() => {
                    using (var stream = File.CreateText(filePath)) {
                        output.WriteCsv(stream);
                    }
                });
            } catch (Exception ex) {
                ErrorBox("Export Error", "The data export failed.", ex.Message);
                return;
            } finally {
                _manager.PopStatus();
            }

            if (doSaveAs) {
                // if the user selected "save as", then open Explorer and select the file
                Process.Start(new ProcessStartInfo {
                    FileName = "explorer.exe",
                    Arguments = $"/e, /select, \"{filePath}\""
                });
            } else {
                // if the user selected "open", then open the CSV file directly
                Process.Start(filePath);
            }
        }

        private static string GetTemporaryExportFilePath() {
            var tempPath = Path.GetTempPath();
            for (int i = 1; i < 100; i++) {
                var tempFilePath = Path.Combine(tempPath, $"Exported{(i == 1 ? "" : i.ToString())}.csv");
                try {
                    File.Delete(tempFilePath); // make sure the file isn't in use
                    return tempFilePath;
                } catch { }
            }
            return Path.Combine(tempPath, $"{Guid.NewGuid()}.csv");
        }

        private void SqliteDocMnu_Click(object sender, EventArgs e) {
            Process.Start("https://sqlite.org/docs.html");
        }

        private void WikiMnu_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/electroly/sqlnotebook/wiki");
        }

        private void ReportIssueMnu_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/electroly/sqlnotebook/issues/new");
        }
    }
}
 