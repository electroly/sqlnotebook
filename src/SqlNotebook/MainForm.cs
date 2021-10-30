using SqlNotebook.Import;
using SqlNotebook.Import.Database;
using SqlNotebook.Properties;
using SqlNotebookCore;
using SqlNotebookScript.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace SqlNotebook {
    public partial class MainForm : ZForm {
        private readonly DockPanel _dockPanel;
        private readonly NotebookManager _manager;
        private Notebook _notebook;
        private readonly UserControlDockContent _contentsDockContent;
        private readonly ConsoleControl _console;
        private readonly UserControlDockContent _consoleDockContent;
        private readonly DatabaseImporter _databaseImporter;
        private readonly ExplorerControl _explorer;
        private string FilePath => _notebook.GetFilePath();
        private bool _isNew;
        private readonly CueToolStripTextBox _searchTxt;
        private readonly Slot<bool> _isDirty = new();
        private readonly Slot<bool> _isTransactionOpen = new();

        private readonly Dictionary<NotebookItem, UserControlDockContent> _openItems
            = new Dictionary<NotebookItem, UserControlDockContent>();

        public MainForm(string filePath, bool isNew) {
            InitializeComponent();

            WaitForm.MainAppForm = this;
            Icon = Resources.SqlNotebookIcon;
            
            _menuStrip.SetMenuAppearance();
            _menuStrip.Items.Insert(0, _searchTxt = new CueToolStripTextBox {
                Alignment = ToolStripItemAlignment.Right,
                CueText = "Search Help",
                ToolTipText = "Search the built-in documentation (Ctrl+H)",
                AutoSize = false,
                Margin = new Padding(0, 0, 5, 0)
            });
            _searchTxt.InnerTextBox.KeyDown += (sender, e) => {
                if (e.KeyCode == Keys.Enter) {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    OpenHelp(_searchTxt.Text);
                    _searchTxt.Text = "";
                }
            };

            Ui ui = new(this, 140, 45, false);
            MinimumSize = new(Size.Width / 2, Size.Height / 2);
            Padding = new(0, 0, 0, ui.SizeGripHeight);
            ui.Init(_saveBtn, Resources.Diskette, Resources.diskette32);
            ui.Init(_newScriptBtn, Resources.ScriptAdd, Resources.script_add32);
            ui.Init(_newMnu, Resources.PageWhite, Resources.page_white32);
            ui.Init(_openMnu, Resources.Folder, Resources.folder32);
            ui.Init(_saveMnu, Resources.Diskette, Resources.diskette32);
            ui.Init(_newScriptMnu, Resources.ScriptAdd, Resources.script_add32);
            ui.Init(_importFromFileMnu, Resources.TextImports, Resources.text_imports32);
            ui.Init(_exportCsvMnu, Resources.TextExports, Resources.text_exports32);
            ui.Init(_viewDocMnu, Resources.Help, Resources.help32);
            ui.Init(_contentsMnu, Resources.list, Resources.list32);
            ui.Init(_consoleMnu, Resources.zone_select, Resources.zone_select32);
            ui.Init(_searchTxt);
            ui.Init(_transactionCommitMenu, Resources.accept_button, Resources.accept_button32);

            // Setting the ForeColor of menu items in the designer doesn't seem to work; it gets overwritten. So we do
            // it here.
            _updateAvailableMenu.Paint += (sender, e) => {
                e.Graphics.DrawRectangle(Pens.Blue, new(0, 0, _updateAvailableMenu.Width - 1, _updateAvailableMenu.Height - 1));
            };
            _transactionMenu.Paint += (sender, e) => {
                e.Graphics.DrawRectangle(Pens.Red, new(0, 0, _transactionMenu.Width - 1, _transactionMenu.Height - 1));
            };
            _updateAvailableMenu.Margin = _transactionMenu.Margin = _searchTxt.Margin =
                new(ui.XWidth(1), 0, 0, 0);

            if (isNew) {
                _notebook = new Notebook(filePath, isNew);
            } else {
                _notebook = WaitForm.Go(null, "SQL Notebook", $"Opening notebook \"{Path.GetFileNameWithoutExtension(filePath)}\"", out var success, () =>
                    new Notebook(filePath, isNew));
                if (!success) {
                    Close();
                }
            }
            _isNew = isNew;
            _manager = new NotebookManager(_notebook, _isTransactionOpen);
            _databaseImporter = new DatabaseImporter(_manager, this);
            _dockPanel = new DockPanel {
                Dock = DockStyle.Fill,
                Theme = new VS2012LightTheme {
                    ToolStripRenderer = new MenuRenderer()
                },
                DocumentStyle = DocumentStyle.DockingWindow,
                DefaultFloatWindowSize = ui.XSize(150, 50),
                ShowDocumentIcon = false,
                DockLeftPortion = ui.XWidth(35)
            };
            _toolStripContainer.ContentPanel.Controls.Add(_dockPanel);

            _contentsDockContent = new("Table of Contents", 
                _explorer = new(_manager, this),
                DockAreas.DockLeft | DockAreas.DockRight);
            _contentsDockContent.CloseButtonVisible = false;
            _contentsDockContent.Show(_dockPanel, DockState.DockLeftAutoHide);

            _manager.NotebookItemOpenRequest += Manager_NotebookItemOpenRequest;
            _manager.NotebookItemCloseRequest += Manager_NotebookItemCloseRequest;
            _manager.NotebookItemsSaveRequest += Manager_NotebookItemsSaveRequest;
            _manager.NotebookDirty += (sender, e) => SetDirty();
            _manager.NotebookItemRename += Manager_NotebookItemRename;
            _manager.HandleHotkeyRequest += Manager_HandleHotkeyRequest;

            Load += delegate {
                Slot.Bind(
                    () => BeginInvoke(new MethodInvoker(() =>
                        _importMnu.Enabled = _saveAsMnu.Enabled = !_isTransactionOpen
                    )),
                    _isTransactionOpen);
                Slot.Bind(
                    () => BeginInvoke(new MethodInvoker(() =>
                        _saveBtn.Enabled = _saveMnu.Enabled = _isDirty && !_isTransactionOpen
                    )),
                    _isDirty, _isTransactionOpen);
                _isDirty.Change += (a, b) => SetTitle();
                Slot.Bind(
                    () => BeginInvoke(new MethodInvoker(() =>
                        _transactionMenu.Visible = _isTransactionOpen
                    )),
                    _isTransactionOpen);
            };

            if (isNew) {
                // create a new script by default in a new notebook
                Load += (sender, e) => {
                    var name = _manager.NewScript();
                    OpenItem(new NotebookItem(NotebookItemType.Script, name));
                    _isDirty.Value = false;
                };
            }

            _console = new(this, _manager);
            _consoleDockContent = new("Console", _console, DockAreas.DockBottom);
            _consoleDockContent.CloseButtonVisible = false;
            _consoleDockContent.Show(_dockPanel);
            _consoleDockContent.DockState = DockState.DockBottomAutoHide;
            _consoleDockContent.AutoHidePortion = 0.6;
            _dockPanel.DockBottomPortion = 0.6;

            Load += async (sender, e) => {
                _manager.Rescan();

                var updateAvailable = await IsUpdateAvailable();
                if (updateAvailable) {
                    _updateAvailableMenu.Visible = true;
                }
            };
            SetTitle();

            // No rush on this one; clean up any update MSIs we might have around
            Task.Run(DeleteUpdateDir);
        }

        private void Manager_HandleHotkeyRequest(object sender, HotkeyEventArgs e) {
            switch (e.KeyData) {
                case (Keys.Control | Keys.N): _newMnu.PerformClick(); break;
                case (Keys.Control | Keys.O): _openMnu.PerformClick(); break;
                case (Keys.Control | Keys.S): _saveMnu.PerformClick(); break;
                case (Keys.Alt | Keys.F4): _exitMnu.PerformClick(); break;
                case (Keys.Control | Keys.Shift | Keys.S): _newScriptMnu.PerformClick(); break;
                case Keys.F1: _viewDocMnu.PerformClick(); break;
                case (Keys.Control | Keys.H): _searchDocMnu.PerformClick(); break;
            }
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
                prefix = Path.GetFileNameWithoutExtension(FilePath);
            }
            var star = _isDirty ? "*" : "";
            BeginInvoke(new MethodInvoker(() => {
                Text = $"{prefix}{star} - SQL Notebook";
            }));
        }

        private void SetDirty() {
            _isDirty.Value = true;
        }

        private void Manager_NotebookItemOpenRequest(object sender, NotebookItemRequestEventArgs e) {
            OpenItem(e.Item);
        }

        private void ApplySaveOnClose(UserControlDockContent f, IDocumentControl doc) {
            f.FormClosing += (sender, e) => doc.Save();
        }

        private void OpenItem(NotebookItem item) {
            if (_openItems.TryGetValue(item, out var userControlDockContent)) {
                userControlDockContent.Activate();
                userControlDockContent.Focus();
                return;
            }

            UserControlDockContent f = null;
            Func<string> getName = null;
            var dockAreas = DockAreas.Document | DockAreas.Float;
            if (item.Type == NotebookItemType.Script) {
                QueryDocumentControl doc = new(item.Name, _manager, this);
                
                f = new UserControlDockContent(item.Name, doc, dockAreas) {
                    Icon = Resources.script32Ico
                };
                ApplySaveOnClose(f, doc);
                getName = () => doc.ItemName;
            } else if (item.Type == NotebookItemType.Table || item.Type == NotebookItemType.View) {
                var doc = new TableDocumentControl(_manager, item.Name);
                f = new UserControlDockContent(item.Name, doc, dockAreas) {
                    Icon = Resources.table32Ico
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

        private void ImportFileMnu_Click(object sender, EventArgs e) {
            using OpenFileDialog openFrm = new() {
                AutoUpgradeEnabled = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DereferenceLinks = true,
                Filter = FileImporter.Filter,
                SupportMultiDottedExtensions = true,
                Title = "Import from File"
            };
            if (openFrm.ShowDialog(this) != DialogResult.OK) {
                return;
            }
            var filePath = openFrm.FileName;

            try {
                FileImporter.Start(this, filePath, _manager);
            } catch (Exception ex) {
                MessageForm.ShowError(this, "Import Error", ex.Message);
            }
        }

        private void ImportPostgresMnu_Click(object sender, EventArgs e) {
            try {
                _databaseImporter.DoDatabaseImport<PostgreSqlImportSession>();
            } catch (Exception ex) {
                ErrorBox("Import Error", ex.Message);
            }
        }

        private void ImportMssqlMnu_Click(object sender, EventArgs e) {
            try {
                _databaseImporter.DoDatabaseImport<SqlServerImportSession>();
            } catch (Exception ex) {
                ErrorBox("Import Error", ex.Message);
            }
        }

        private void ImportMysqlMnu_Click(object sender, EventArgs e) {
            try {
                _databaseImporter.DoDatabaseImport<MySqlImportSession>();
            } catch (Exception ex) {
                ErrorBox("Import Error", ex.Message);
            }
        }

        private void ErrorBox(string title, string message, string details = null) {
            MessageForm.ShowError(this, title, message, details);
        }

        private void NewScriptBtn_Click(object sender, EventArgs e) {
            try {
                OpenItem(new NotebookItem(NotebookItemType.Script, _manager.NewScript()));
            } catch (Exception ex) {
                ErrorBox("Notebook Error", "There was a problem creating the script.", ex.Message);
            }
        }

        private static bool _ignoreF5 = false;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if (keyData == Keys.F5) {
                if (_ignoreF5) {
                    return true;
                }
                if (_dockPanel.ActiveDocument is UserControlDockContent doc) {
                    if (doc.Content is QueryDocumentControl queryDoc) {
                        _ignoreF5 = true;
                        queryDoc.Execute();
                        _ignoreF5 = false;
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
                        WaitForm.Go(this, "Save", "Saving your notebook...", out var success, () => {
                            _manager.SaveAs(f.FileName);
                        });
                        if (!success) {
                            return false;
                        }
                        _isNew = false;
                    } else {
                        return false;
                    }
                }
            } else {
                WaitForm.Go(this, "Save", "Saving your notebook...", out _, () => {
                    _manager.Save();
                });
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
            var success = PrepareToCloseFile();
            if (!success) {
                e.Cancel = true;
            }
        }

        private bool PrepareToCloseFile() {
            var isTransactionActive = false;
            _notebook.Invoke(() => {
                isTransactionActive = _notebook.IsTransactionActive();
            });
            if (isTransactionActive) {
                ErrorBox("SQL Notebook", "A transaction is active.",
                    "Execute either \"COMMIT\" or \"ROLLBACK\" to end the transaction before exiting.");
                return false;
            }

            if (_isDirty) {
                var shortFilename = _isNew ? "Untitled" : Path.GetFileNameWithoutExtension(FilePath);

                var result = MessageBox.Show(this,
                    $"Save changes to {shortFilename}?",
                    "SQL Notebook",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes) {
                    try {
                        if (!SaveOrSaveAs()) {
                            return false;
                        }
                    } catch (Exception ex) {
                        MessageForm.ShowError(this, "SQL Notebook", ex.Message);
                        return false;
                    }
                } else if (result == DialogResult.Cancel) {
                    return false;
                }
            }
            return true;
        }

        private void SaveAsMnu_Click(object sender, EventArgs e) {
            SaveOrSaveAs(saveAs: true);
        }

        private void ExportMnu_Click(object sender, EventArgs e) {
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

            WaitForm.Go(this, "Export", $"Exporting to \"{Path.GetFileName(filePath)}\"...", out var success, () => {
                var output = _manager.ExecuteScript(
                    item.Type == NotebookItemType.Script
                    ? $"EXECUTE {item.Name.DoubleQuote()}"
                    : $"SELECT * FROM {item.Name.DoubleQuote()}");
                using var stream = File.CreateText(filePath);
                output.WriteCsv(stream);
            });
            if (!success) {
                return;
            }

            if (doSaveAs) {
                // if the user selected "save as", then open Explorer and select the file
                Process.Start(new ProcessStartInfo {
                    FileName = "explorer.exe",
                    Arguments = $"/e, /select, \"{filePath}\""
                });
            } else {
                // if the user selected "open", then open the CSV file directly
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
        }

        private static string GetTemporaryExportFilePath() {
            var tempPath = Path.GetTempPath();
            for (var i = 1; i < 100; i++) {
                var tempFilePath = Path.Combine(tempPath, $"Exported{(i == 1 ? "" : i.ToString())}.csv");
                try {
                    File.Delete(tempFilePath); // make sure the file isn't in use
                    return tempFilePath;
                } catch { }
            }
            return Path.Combine(tempPath, $"{Guid.NewGuid()}.csv");
        }

        private void ReportIssueMnu_Click(object sender, EventArgs e) {
            Process.Start(new ProcessStartInfo("https://github.com/electroly/sqlnotebook/issues/new") { UseShellExecute = true });
        }

        private void OpenHelp(string keywords) {
            var results = WaitForm.Go(this, "Help", "Searching help...", out var success, () =>
                HelpSearcher.Search(keywords));
            if (!success) {
                return;
            }
            using HelpSearchResultsForm resultsForm = new(keywords, results);
            resultsForm.ShowDialog(this);
        }

        private void ViewDocMnu_Click(object sender, EventArgs e) {
            var htmlFilePath = Path.Combine(
                Path.GetDirectoryName(Application.ExecutablePath), "doc", "doc.html");
            Process.Start(new ProcessStartInfo(htmlFilePath) { UseShellExecute = true });
        }

        private void SearchDocMnu_Click(object sender, EventArgs e) {
            _searchTxt.Select();
            _searchTxt.Focus();
        }

        public static async Task<bool> IsUpdateAvailable() {
            try {
                using var client = new WebClient();
                var appversion = await client.DownloadStringTaskAsync(
                    "https://sqlnotebook.com/appversion.txt?" + DateTime.Now.Date);
                var data = appversion.Split('\n').Select(x => x.Trim()).ToList();
                var version = data[0];
                if (Application.ProductVersion != version) {
                    return true;
                }
            } catch {
                // bummer
            }

            return false;
        }

        private void ContentsMnu_Click(object sender, EventArgs e) {
            switch (_contentsDockContent.DockState) {
                case DockState.DockLeftAutoHide:
                case DockState.DockRightAutoHide:
                    _dockPanel.ActiveAutoHideContent = _contentsDockContent;
                    break;
            }

            _explorer.TakeFocus();
        }

        private void ConsoleMnu_Click(object sender, EventArgs e) {
            if (_consoleDockContent.DockState == DockState.DockBottomAutoHide) {
                _dockPanel.ActiveAutoHideContent = _consoleDockContent;
            }

            _console.TakeFocus();
        }

        private void WindowMnu_DropDownOpening(object sender, EventArgs e) {
            while (_windowMnu.DropDownItems.Count > 2) {
                _windowMnu.DropDownItems.RemoveAt(2);
            }
            if (_dockPanel.Documents.Any()) {
                _windowMnu.DropDownItems.Add(new ToolStripSeparator());
            }
            foreach (var x in _dockPanel.Documents) {
                var text = x.DockHandler.TabText;
                ToolStripMenuItem item = new(text);
                item.Click += delegate {
                    x.DockHandler.Activate();
                };
                _windowMnu.DropDownItems.Add(item);
            }
        }

        private void TransactionCommitMenu_Click(object sender, EventArgs e) {
            WaitForm.Go(this, "Active Transaction", "Committing transaction...", out var success, () => {
                _manager.ExecuteScript("COMMIT");
            });
            _manager.Rescan();
        }

        private void TransactionRollbackMenu_Click(object sender, EventArgs e) {
            WaitForm.Go(this, "Active Transaction", "Rolling back transaction...", out var success, () => {
                _manager.ExecuteScript("ROLLBACK");
            });
            _manager.Rescan();
        }

        private void UpdateAvailableMenu_Click(object sender, EventArgs e) {
            var (version, msiUrl) = WaitForm.Go(this, "Software Update", "Checking for updates...", out var success, () => {
                var appversion = Program.HttpClient.GetStringAsync(
                    "https://sqlnotebook.com/appversion.txt?").GetAwaiter().GetResult();
                var data = appversion.Split('\n').Select(x => x.Trim()).ToList();
                if (data.Count < 2) {
                    throw new Exception("The appversion.txt data is malformed.");
                }
                return (data[0], data[1]);
            });

            var confirmation = MessageBox.Show($"Version {version} is available. Install now?", "SQL Notebook",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (confirmation != DialogResult.OK) {
                return;
            }

            var msiFilePath = WaitForm.Go(this, "Software Update", "Downloading update...", out success, () => {
                var dir = GetUpdateDir();
                var msiFilePath = Path.Combine(dir, $"SQLNotebook_{version}_{DateTime.Now.Ticks}.msi");
                using var downloadStream = Program.HttpClient.GetStreamAsync(msiUrl).GetAwaiter().GetResult();
                using var fileStream = File.Create(msiFilePath);
                downloadStream.CopyTo(fileStream);
                return msiFilePath;
            });
            if (!success) {
                return;
            }

            Process.Start(new ProcessStartInfo {
                FileName = msiFilePath,
                UseShellExecute = true,
            });
            Close();
        }

        private static void DeleteUpdateDir() {
            try {
                Directory.Delete(Path.Combine(Path.GetTempPath(), "SqlNotebookUpdate"), true);
            } catch { }
        }

        private static string GetUpdateDir() {
            var dir = Path.Combine(Path.GetTempPath(), "SqlNotebookUpdate");
            Directory.CreateDirectory(dir);
            foreach (var filePath in Directory.GetFiles(dir)) {
                try {
                    File.Delete(filePath);
                } catch { }
            }
            return dir;
        }
    }
}
 