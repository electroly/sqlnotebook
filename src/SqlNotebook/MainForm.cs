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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using SqlNotebook.Properties;
using SqlNotebookCore;
using SqlNotebookScript.Interpreter;
using SqlNotebookScript.Utils;
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
        private UserControlDockContent _helpDoc;
        private HelpServer _helpServer;
        private CueToolStripTextBox _searchTxt;
        private Slot<bool> _isDirty = new Slot<bool>();
        private Slot<bool> _operationInProgress = new Slot<bool>();
        private Slot<bool> _isTransactionOpen = new Slot<bool>();

        private readonly Dictionary<NotebookItem, UserControlDockContent> _openItems
            = new Dictionary<NotebookItem, UserControlDockContent>();

        public MainForm(string filePath, bool isNew) {
            InitializeComponent();
            _toolStrip.Items.Add(_searchTxt = new CueToolStripTextBox {
                Alignment = ToolStripItemAlignment.Right,
                CueText = "Search Help",
                AutoSize = false
            });
            _searchTxt.InnerTextBox.KeyDown += async (sender, e) => {
                if (e.KeyCode == Keys.Enter) {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    var text = _searchTxt.Text;
                    await OpenHelp("/search?q=" + System.Net.WebUtility.UrlEncode(text));
                    _searchTxt.Text = "";
                }
            };

            if (isNew) {
                _notebook = new Notebook(filePath, isNew);
            } else {
                var f = new WaitForm("SQL Notebook", $"Opening notebook \"{Path.GetFileNameWithoutExtension(filePath)}\"", () => {
                    _notebook = new Notebook(filePath, isNew);
                });
                using (f) {
                    f.StartPosition = FormStartPosition.CenterScreen;
                    f.ShowDialog();
                    if (f.ResultException != null) {
                        throw f.ResultException;
                    }
                }
            }
            _isNew = isNew;
            _manager = new NotebookManager(_notebook, _isTransactionOpen);
            _importer = new Importer(_manager, this);
            _dockPanel = new DockPanel {
                Dock = DockStyle.Fill,
                Theme = new VS2012LightTheme(),
                DocumentStyle = DocumentStyle.DockingWindow,
                DefaultFloatWindowSize = new Size(700, 700)
            };
            _toolStripContainer.ContentPanel.Controls.Add(_dockPanel);

            _contentsPane = new UserControlDockContent("Table of Contents", 
                _explorer = new ExplorerControl(_manager, this, _operationInProgress),
                DockAreas.DockLeft | DockAreas.DockRight);
            _contentsPane.CloseButtonVisible = false;
            _contentsPane.Show(_dockPanel, DockState.DockLeft);

            _manager.NotebookItemOpenRequest += Manager_NotebookItemOpenRequest;
            _manager.NotebookItemCloseRequest += Manager_NotebookItemCloseRequest;
            _manager.NotebookItemsSaveRequest += Manager_NotebookItemsSaveRequest;
            _manager.NotebookDirty += (sender, e) => SetDirty();
            _manager.NotebookItemRename += Manager_NotebookItemRename;
            _manager.StatusUpdate += Manager_StatusUpdate;

            // show a progressbar in the taskbar button and the statusbar when a operation is in progress
            _operationInProgress.Change += (oldValue, newValue) => {
                if (!oldValue && newValue) {
                    this.BeginTaskbarProgress();
                    _statusLbl.Visible = true;

                    // this restarts the statusbar progress marquee from the beginning
                    _statusProgressbar.Style = ProgressBarStyle.Continuous;
                    _statusProgressbar.Style = ProgressBarStyle.Marquee;
                    _statusProgressbar.Visible = true;

                    _cancelLnk.IsLink = true;
                    _cancelLnk.Text = "Cancel";
                    _cancelLnk.Visible = true;
                } else if (oldValue && !newValue) {
                    _notebook.EndUserCancel();
                    this.EndTaskbarProgress();
                    _statusProgressbar.Visible = false;
                    _statusLbl.Visible = false;
                    _cancelLnk.Visible = false;
                }
            };

            Slot.Bind(
                () => _importBtn.Enabled = _saveAsMnu.Enabled = !_operationInProgress && !_isTransactionOpen,
                _operationInProgress, _isTransactionOpen);
            Slot.Bind(
                () => _exportMnu.Enabled = !_operationInProgress,
                _operationInProgress);
            Slot.Bind(
                () => _saveMnu.Enabled = !_operationInProgress && _isDirty && !_isTransactionOpen,
                _operationInProgress, _isDirty, _isTransactionOpen);
            _isDirty.Change += (a, b) => SetTitle();
            Slot.Bind(
                () => BeginInvoke(new MethodInvoker(() => _openTransactionLbl.Visible = _isTransactionOpen)),
                _isTransactionOpen);

            if (isNew) {
                switch (Settings.Default.AutoCreateInNewNotebooks) {
                    case 0: // "Getting Started" note
                        
                        Load += (sender, e) => {
                            var name = _manager.NewNote("Getting Started", Resources.GettingStartedRtf);
                            OpenItem(new NotebookItem(NotebookItemType.Note, name));
                            _isDirty.Value = false;
                        };
                        break;

                    case 1: // New note
                        Load += (sender, e) => {
                            var name = _manager.NewNote();
                            OpenItem(new NotebookItem(NotebookItemType.Note, name));
                            _isDirty.Value = false;
                        };
                        break;

                    case 2: // New console
                        Load += (sender, e) => {
                            var name = _manager.NewConsole();
                            OpenItem(new NotebookItem(NotebookItemType.Console, name));
                            _isDirty.Value = false;
                        };
                        break;

                    case 3: // New script
                        Load += (sender, e) => {
                            var name = _manager.NewScript();
                            OpenItem(new NotebookItem(NotebookItemType.Script, name));
                            _isDirty.Value = false;
                        };
                        break;
                }

            }

            Load += (sender, e) => _manager.Rescan();
            SetTitle();
        }

        private void Manager_StatusUpdate(object sender, StatusUpdateEventArgs e) {
            BeginInvoke(new MethodInvoker((() => {
                _operationInProgress.Value = e.Status.Any();
                _statusLbl.Text = e.Status;
            })));
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
            var isTableOrView = e.Item.Type == NotebookItemType.Table || e.Item.Type == NotebookItemType.View;
            _manager.Rescan(notebookItemsOnly: !isTableOrView);
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
        }

        private void SetDirty() {
            _isDirty.Value = true;
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
            Func<string> getName = null;
            var dockAreas = DockAreas.Document | DockAreas.Float;
            if (item.Type == NotebookItemType.Console) {
                var doc = new ConsoleDocumentControl(item.Name, _manager, this, _operationInProgress);
                f = new UserControlDockContent(item.Name, doc, dockAreas) {
                    Icon = Resources.ApplicationXpTerminalIco
                };
                f.FormClosing += (sender2, e2) => doc.Save();
                getName = () => doc.ItemName;
            } else if (item.Type == NotebookItemType.Note) {
                var doc = new NoteDocumentControl(item.Name, _manager);
                f = new UserControlDockContent(item.Name, doc, dockAreas) {
                    Icon = Resources.NoteIco
                };
                f.FormClosing += (sender2, e2) => doc.Save();
                getName = () => doc.ItemName;
            } else if (item.Type == NotebookItemType.Script) {
                var doc = new QueryDocumentControl(item.Name, _manager, this, _operationInProgress);
                f = new UserControlDockContent(item.Name, doc, dockAreas) {
                    Icon = Resources.ScriptIco
                };
                f.FormClosing += (sender2, e2) => doc.Save();
                getName = () => doc.ItemName;
            } else if (item.Type == NotebookItemType.Table || item.Type == NotebookItemType.View) {
                var doc = new TableDocumentControl(_manager, item.Name, this);
                f = new UserControlDockContent(item.Name, doc, dockAreas) {
                    Icon = Resources.TableIco
                };
            }

            if (getName != null) {
                f.FormClosed += (sender2, e2) => {
                    // the item may have been renamed since we opened it, so make sure to use getName() for the current
                    // name instead of closing over item.Name.
                    _openItems.Remove(new NotebookItem(item.Type, getName()));
                };
               _openItems[item] = f;
            }

            f.Show(_dockPanel);
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
            var openFrm = new OpenFileDialog {
                AutoUpgradeEnabled = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DereferenceLinks = true,
                Filter = string.Join("|",
                    "All data files|*.csv;*.txt;*.xls;*.xlsx;*.json",
                    "Comma-separated values|*.csv;*.txt",
                    "Excel workbooks|*.xls;*.xlsx",
                    "JSON files|*.json"),
                SupportMultiDottedExtensions = true,
                Title = "Import from File"
            };
            string filePath;
            using (openFrm) {
                if (openFrm.ShowDialog(this) == DialogResult.OK) {
                    filePath = openFrm.FileName;
                } else {
                    return;
                }
            }

            var extension = Path.GetExtension(filePath).ToLower();
            switch (extension) {
                case ".csv":
                case ".txt":
                    await CsvImportProcess.Start(this, filePath, _manager);
                    break;

                default:
                    MessageDialog.ShowError(this, "Import Error", $"The file type \"{extension}\" is not supported.");
                    break;
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
            MessageDialog.ShowError(this, title, message, details);
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
            if (keyData == Keys.F5) {
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
                        }).ShowDialogAndDispose(this);
                        _isNew = false;
                    } else {
                        return false;
                    }
                }
            } else {
                new WaitForm("Save", "Saving your notebook...", () => {
                    _manager.Save();
                }).ShowDialogAndDispose(this);
            }

            // set this after doing the isNew stuff above so that if you click Save and then cancel the Save As dialog,
            // the file remains dirty. since untitled files are in stored in temporary files it doesn't matter that we
            // saved to it despite the cancelation.
            _isDirty.Value = false;

            _manager.Rescan();
            SetTitle();
            return true;
        }

        private void SaveOpenItems() {
            foreach (var x in _openItems.Values) {
                x.Content.Save();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (_operationInProgress) {
                ErrorBox("SQL Notebook", "An operation is in progress.", 
                    "Please cancel the operation or wait for it to complete before exiting from SQL Notebook.");
                e.Cancel = true;
                return;
            }

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
            var tables = _manager.Items.Where(x => x.Type == NotebookItemType.Table).Select(x => x.Name);
            var views = _manager.Items.Where(x => x.Type == NotebookItemType.View).Select(x => x.Name);
            NotebookItem item;
            bool doSaveAs;

            using (var f = new ExportForm(scripts, tables, views)) {
                var result = f.ShowDialog(this);
                item = f.NotebookItem;
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

            _manager.PushStatus("Running the selected script...");
            try {
                string sql;
                if (item.Type == NotebookItemType.Script) {
                    sql = $"EXECUTE {item.Name.DoubleQuote()}";
                } else {
                    sql = $"SELECT * FROM {item.Name.DoubleQuote()}";
                }

                var output = await Task.Run(() => _manager.ExecuteScript(sql));
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

        private void ReportIssueMnu_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/electroly/sqlnotebook/issues/new");
        }

        private async Task OpenHelp(string path) {
            if (_helpServer == null) {
                _manager.PushStatus("Starting help server...");
                await Task.Run(() => _helpServer = new HelpServer());
                _manager.PopStatus();
            }

            string url = $"http://127.0.0.1:{_helpServer.PortNumber}{path}";

            BeginInvoke(new MethodInvoker(() => {
                HelpDocumentControl helpCtl;
                if (_helpDoc != null) {
                    helpCtl = (HelpDocumentControl)_helpDoc.Content;
                } else {
                    helpCtl = new HelpDocumentControl(() => _helpServer.PortNumber) { Dock = DockStyle.Fill };
                    _helpDoc = new UserControlDockContent("Help Viewer", helpCtl, DockAreas.Document | DockAreas.Float) {
                        Icon = Resources.HelpIco
                    };
                    _helpDoc.FormClosed += (sender, e) => _helpDoc = null;
                    _helpDoc.Show(_dockPanel);
                }
                helpCtl.Navigate(url);
                _helpDoc.Activate();
                _helpDoc.Focus();
            }));
        }

        private async void ViewDocMnu_Click(object sender, EventArgs e) {
            await OpenHelp($"/index.html");
        }

        private void CancelLnk_Click(object sender, EventArgs e) {
            _notebook.BeginUserCancel();
            _cancelLnk.IsLink = false;
            _cancelLnk.Text = "Canceling...";
        }

        private void OptionsMnu_Click(object sender, EventArgs e) {
            new OptionsForm().ShowDialogAndDispose(this);
        }
    }
}
 