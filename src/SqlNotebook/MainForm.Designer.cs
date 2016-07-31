namespace SqlNotebook
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
            this._toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this._statusStrip = new System.Windows.Forms.StatusStrip();
            this._openTransactionLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this._statusLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this._cancelLnk = new System.Windows.Forms.ToolStripStatusLabel();
            this._statusProgressbar = new System.Windows.Forms.ToolStripProgressBar();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this._newMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._openMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._saveMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._saveAsMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._exitMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._importBtn = new System.Windows.Forms.ToolStripDropDownButton();
            this._importFileMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._importMssqlMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._importPostgresMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._importMysqlMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._recentFilesMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._recentFilesNoneMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._recentServersMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._recentServersNoneMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._exportMnu = new System.Windows.Forms.ToolStripDropDownButton();
            this.toCSVFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toSQLScriptsqlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toMicrosoftSQLServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toPostgreSQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toMySQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.recentFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentServersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this._viewDocMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._reportIssueMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._aboutMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this._newNoteBtn = new System.Windows.Forms.ToolStripButton();
            this._newConsoleBtn = new System.Windows.Forms.ToolStripButton();
            this._newScriptBtn = new System.Windows.Forms.ToolStripButton();
            this._optionsMnu = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this._toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this._toolStripContainer.TopToolStripPanel.SuspendLayout();
            this._toolStripContainer.SuspendLayout();
            this._statusStrip.SuspendLayout();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new System.Drawing.Size(200, 6);
            // 
            // _toolStripContainer
            // 
            // 
            // _toolStripContainer.BottomToolStripPanel
            // 
            this._toolStripContainer.BottomToolStripPanel.Controls.Add(this._statusStrip);
            // 
            // _toolStripContainer.ContentPanel
            // 
            this._toolStripContainer.ContentPanel.Size = new System.Drawing.Size(898, 727);
            this._toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this._toolStripContainer.Name = "_toolStripContainer";
            this._toolStripContainer.Size = new System.Drawing.Size(898, 780);
            this._toolStripContainer.TabIndex = 0;
            this._toolStripContainer.Text = "toolStripContainer1";
            // 
            // _toolStripContainer.TopToolStripPanel
            // 
            this._toolStripContainer.TopToolStripPanel.BackColor = System.Drawing.SystemColors.Control;
            this._toolStripContainer.TopToolStripPanel.Controls.Add(this._toolStrip);
            // 
            // _statusStrip
            // 
            this._statusStrip.BackColor = System.Drawing.SystemColors.Control;
            this._statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._openTransactionLbl,
            this._statusLbl,
            this._cancelLnk,
            this._statusProgressbar});
            this._statusStrip.Location = new System.Drawing.Point(0, 0);
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Size = new System.Drawing.Size(898, 22);
            this._statusStrip.TabIndex = 0;
            // 
            // _openTransactionLbl
            // 
            this._openTransactionLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._openTransactionLbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._openTransactionLbl.Name = "_openTransactionLbl";
            this._openTransactionLbl.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this._openTransactionLbl.Size = new System.Drawing.Size(193, 17);
            this._openTransactionLbl.Text = "A database transaction is active.";
            this._openTransactionLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._openTransactionLbl.Visible = false;
            // 
            // _statusLbl
            // 
            this._statusLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._statusLbl.Image = global::SqlNotebook.Properties.Resources.Hourglass;
            this._statusLbl.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._statusLbl.Name = "_statusLbl";
            this._statusLbl.Size = new System.Drawing.Size(729, 17);
            this._statusLbl.Spring = true;
            this._statusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._statusLbl.Visible = false;
            // 
            // _cancelLnk
            // 
            this._cancelLnk.IsLink = true;
            this._cancelLnk.Margin = new System.Windows.Forms.Padding(0, 3, 5, 2);
            this._cancelLnk.Name = "_cancelLnk";
            this._cancelLnk.Size = new System.Drawing.Size(43, 17);
            this._cancelLnk.Text = "Cancel";
            this._cancelLnk.Visible = false;
            this._cancelLnk.Click += new System.EventHandler(this.CancelLnk_Click);
            // 
            // _statusProgressbar
            // 
            this._statusProgressbar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._statusProgressbar.Margin = new System.Windows.Forms.Padding(1, 3, 5, 3);
            this._statusProgressbar.MarqueeAnimationSpeed = 25;
            this._statusProgressbar.Name = "_statusProgressbar";
            this._statusProgressbar.Size = new System.Drawing.Size(100, 16);
            this._statusProgressbar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this._statusProgressbar.Visible = false;
            // 
            // _toolStrip
            // 
            this._toolStrip.BackColor = System.Drawing.Color.Transparent;
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this._importBtn,
            this._exportMnu,
            this.toolStripDropDownButton2,
            this.toolStripSeparator5,
            this._newNoteBtn,
            this._newConsoleBtn,
            this._newScriptBtn});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this._toolStrip.ShowItemToolTips = false;
            this._toolStrip.Size = new System.Drawing.Size(898, 31);
            this._toolStrip.Stretch = true;
            this._toolStrip.TabIndex = 1;
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._newMnu,
            this._openMnu,
            this._saveMnu,
            this._saveAsMnu,
            toolStripSeparator4,
            this._optionsMnu,
            toolStripSeparator7,
            this._exitMnu});
            this.toolStripDropDownButton1.Image = global::SqlNotebook.Properties.Resources.DatabaseTable;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Margin = new System.Windows.Forms.Padding(0, 1, 0, 4);
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Padding = new System.Windows.Forms.Padding(3, 3, 5, 3);
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(46, 26);
            this.toolStripDropDownButton1.Text = "&File";
            // 
            // _newMnu
            // 
            this._newMnu.Image = global::SqlNotebook.Properties.Resources.PageWhite;
            this._newMnu.Name = "_newMnu";
            this._newMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this._newMnu.Size = new System.Drawing.Size(155, 22);
            this._newMnu.Text = "&New";
            this._newMnu.Click += new System.EventHandler(this.NewMnu_Click);
            // 
            // _openMnu
            // 
            this._openMnu.Image = global::SqlNotebook.Properties.Resources.Folder;
            this._openMnu.Name = "_openMnu";
            this._openMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this._openMnu.Size = new System.Drawing.Size(155, 22);
            this._openMnu.Text = "&Open...";
            this._openMnu.Click += new System.EventHandler(this.OpenMnu_Click);
            // 
            // _saveMnu
            // 
            this._saveMnu.Image = global::SqlNotebook.Properties.Resources.Diskette;
            this._saveMnu.Name = "_saveMnu";
            this._saveMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this._saveMnu.Size = new System.Drawing.Size(155, 22);
            this._saveMnu.Text = "&Save";
            this._saveMnu.Click += new System.EventHandler(this.SaveMnu_Click);
            // 
            // _saveAsMnu
            // 
            this._saveAsMnu.Name = "_saveAsMnu";
            this._saveAsMnu.Size = new System.Drawing.Size(155, 22);
            this._saveAsMnu.Text = "Save &as";
            this._saveAsMnu.Click += new System.EventHandler(this.SaveAsMnu_Click);
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(152, 6);
            // 
            // _exitMnu
            // 
            this._exitMnu.Name = "_exitMnu";
            this._exitMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this._exitMnu.Size = new System.Drawing.Size(155, 22);
            this._exitMnu.Text = "E&xit";
            this._exitMnu.Click += new System.EventHandler(this.ExitMnu_Click);
            // 
            // _importBtn
            // 
            this._importBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._importBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._importFileMnu,
            this._importMssqlMnu,
            this._importPostgresMnu,
            this._importMysqlMnu,
            this.toolStripSeparator2,
            this._recentFilesMnu,
            this._recentServersMnu});
            this._importBtn.Image = global::SqlNotebook.Properties.Resources.TextImports;
            this._importBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._importBtn.Margin = new System.Windows.Forms.Padding(0, 1, 0, 4);
            this._importBtn.Name = "_importBtn";
            this._importBtn.Padding = new System.Windows.Forms.Padding(0, 3, 5, 3);
            this._importBtn.Size = new System.Drawing.Size(61, 26);
            this._importBtn.Text = "&Import";
            // 
            // _importFileMnu
            // 
            this._importFileMnu.Name = "_importFileMnu";
            this._importFileMnu.Size = new System.Drawing.Size(224, 22);
            this._importFileMnu.Text = "From &file...";
            this._importFileMnu.Click += new System.EventHandler(this.ImportFileMnu_Click);
            // 
            // _importMssqlMnu
            // 
            this._importMssqlMnu.Name = "_importMssqlMnu";
            this._importMssqlMnu.Size = new System.Drawing.Size(224, 22);
            this._importMssqlMnu.Text = "From &Microsoft SQL Server...";
            this._importMssqlMnu.Click += new System.EventHandler(this.ImportMssqlMnu_Click);
            // 
            // _importPostgresMnu
            // 
            this._importPostgresMnu.Name = "_importPostgresMnu";
            this._importPostgresMnu.Size = new System.Drawing.Size(224, 22);
            this._importPostgresMnu.Text = "From &PostgreSQL...";
            this._importPostgresMnu.Click += new System.EventHandler(this.ImportPostgresMnu_Click);
            // 
            // _importMysqlMnu
            // 
            this._importMysqlMnu.Name = "_importMysqlMnu";
            this._importMysqlMnu.Size = new System.Drawing.Size(224, 22);
            this._importMysqlMnu.Text = "From M&ySQL...";
            this._importMysqlMnu.Click += new System.EventHandler(this.ImportMysqlMnu_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(221, 6);
            // 
            // _recentFilesMnu
            // 
            this._recentFilesMnu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._recentFilesNoneMnu});
            this._recentFilesMnu.Name = "_recentFilesMnu";
            this._recentFilesMnu.Size = new System.Drawing.Size(224, 22);
            this._recentFilesMnu.Text = "Recent files";
            this._recentFilesMnu.DropDownOpening += new System.EventHandler(this.RecentFilesMnu_DropDownOpening);
            // 
            // _recentFilesNoneMnu
            // 
            this._recentFilesNoneMnu.Enabled = false;
            this._recentFilesNoneMnu.Name = "_recentFilesNoneMnu";
            this._recentFilesNoneMnu.Size = new System.Drawing.Size(109, 22);
            this._recentFilesNoneMnu.Text = "(none)";
            // 
            // _recentServersMnu
            // 
            this._recentServersMnu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._recentServersNoneMnu});
            this._recentServersMnu.Name = "_recentServersMnu";
            this._recentServersMnu.Size = new System.Drawing.Size(224, 22);
            this._recentServersMnu.Text = "Recent servers";
            this._recentServersMnu.DropDownOpening += new System.EventHandler(this.RecentServersMnu_DropDownOpening);
            // 
            // _recentServersNoneMnu
            // 
            this._recentServersNoneMnu.Enabled = false;
            this._recentServersNoneMnu.Name = "_recentServersNoneMnu";
            this._recentServersNoneMnu.Size = new System.Drawing.Size(109, 22);
            this._recentServersNoneMnu.Text = "(none)";
            // 
            // _exportMnu
            // 
            this._exportMnu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._exportMnu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toCSVFileToolStripMenuItem,
            this.toExcelToolStripMenuItem,
            this.toRToolStripMenuItem,
            this.toSQLScriptsqlToolStripMenuItem,
            this.toolStripSeparator1,
            this.toMicrosoftSQLServerToolStripMenuItem,
            this.toPostgreSQLToolStripMenuItem,
            this.toMySQLToolStripMenuItem,
            this.toolStripSeparator3,
            this.recentFilesToolStripMenuItem,
            this.recentServersToolStripMenuItem});
            this._exportMnu.Image = global::SqlNotebook.Properties.Resources.TextExports;
            this._exportMnu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._exportMnu.Margin = new System.Windows.Forms.Padding(0, 1, 0, 4);
            this._exportMnu.Name = "_exportMnu";
            this._exportMnu.Padding = new System.Windows.Forms.Padding(0, 3, 5, 3);
            this._exportMnu.Size = new System.Drawing.Size(58, 26);
            this._exportMnu.Text = "&Export";
            // 
            // toCSVFileToolStripMenuItem
            // 
            this.toCSVFileToolStripMenuItem.Name = "toCSVFileToolStripMenuItem";
            this.toCSVFileToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.toCSVFileToolStripMenuItem.Text = "To &CSV file (.csv)...";
            this.toCSVFileToolStripMenuItem.Click += new System.EventHandler(this.ExportMnu_Click);
            // 
            // toExcelToolStripMenuItem
            // 
            this.toExcelToolStripMenuItem.Name = "toExcelToolStripMenuItem";
            this.toExcelToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.toExcelToolStripMenuItem.Text = "To E&xcel file (.xlsx)...";
            this.toExcelToolStripMenuItem.Visible = false;
            // 
            // toRToolStripMenuItem
            // 
            this.toRToolStripMenuItem.Name = "toRToolStripMenuItem";
            this.toRToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.toRToolStripMenuItem.Text = "To &R file (.rdata)...";
            this.toRToolStripMenuItem.Visible = false;
            // 
            // toSQLScriptsqlToolStripMenuItem
            // 
            this.toSQLScriptsqlToolStripMenuItem.Name = "toSQLScriptsqlToolStripMenuItem";
            this.toSQLScriptsqlToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.toSQLScriptsqlToolStripMenuItem.Text = "To &SQL script (.sql)...";
            this.toSQLScriptsqlToolStripMenuItem.Visible = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(206, 6);
            this.toolStripSeparator1.Visible = false;
            // 
            // toMicrosoftSQLServerToolStripMenuItem
            // 
            this.toMicrosoftSQLServerToolStripMenuItem.Name = "toMicrosoftSQLServerToolStripMenuItem";
            this.toMicrosoftSQLServerToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.toMicrosoftSQLServerToolStripMenuItem.Text = "To &Microsoft SQL Server...";
            this.toMicrosoftSQLServerToolStripMenuItem.Visible = false;
            // 
            // toPostgreSQLToolStripMenuItem
            // 
            this.toPostgreSQLToolStripMenuItem.Name = "toPostgreSQLToolStripMenuItem";
            this.toPostgreSQLToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.toPostgreSQLToolStripMenuItem.Text = "To &PostgreSQL...";
            this.toPostgreSQLToolStripMenuItem.Visible = false;
            // 
            // toMySQLToolStripMenuItem
            // 
            this.toMySQLToolStripMenuItem.Name = "toMySQLToolStripMenuItem";
            this.toMySQLToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.toMySQLToolStripMenuItem.Text = "To M&ySQL...";
            this.toMySQLToolStripMenuItem.Visible = false;
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(206, 6);
            this.toolStripSeparator3.Visible = false;
            // 
            // recentFilesToolStripMenuItem
            // 
            this.recentFilesToolStripMenuItem.Name = "recentFilesToolStripMenuItem";
            this.recentFilesToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.recentFilesToolStripMenuItem.Text = "Recent files";
            this.recentFilesToolStripMenuItem.Visible = false;
            // 
            // recentServersToolStripMenuItem
            // 
            this.recentServersToolStripMenuItem.Name = "recentServersToolStripMenuItem";
            this.recentServersToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.recentServersToolStripMenuItem.Text = "Recent servers";
            this.recentServersToolStripMenuItem.Visible = false;
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._viewDocMnu,
            this._reportIssueMnu,
            toolStripSeparator6,
            this._aboutMnu});
            this.toolStripDropDownButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton2.Image")));
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Margin = new System.Windows.Forms.Padding(0, 1, 0, 4);
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Padding = new System.Windows.Forms.Padding(0, 3, 5, 3);
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(50, 26);
            this.toolStripDropDownButton2.Text = "&Help";
            // 
            // _viewDocMnu
            // 
            this._viewDocMnu.Image = global::SqlNotebook.Properties.Resources.Help;
            this._viewDocMnu.Name = "_viewDocMnu";
            this._viewDocMnu.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this._viewDocMnu.Size = new System.Drawing.Size(203, 22);
            this._viewDocMnu.Text = "View documentation";
            this._viewDocMnu.Click += new System.EventHandler(this.ViewDocMnu_Click);
            // 
            // _reportIssueMnu
            // 
            this._reportIssueMnu.Name = "_reportIssueMnu";
            this._reportIssueMnu.Size = new System.Drawing.Size(203, 22);
            this._reportIssueMnu.Text = "Report an issue";
            this._reportIssueMnu.Click += new System.EventHandler(this.ReportIssueMnu_Click);
            // 
            // _aboutMnu
            // 
            this._aboutMnu.Name = "_aboutMnu";
            this._aboutMnu.Size = new System.Drawing.Size(203, 22);
            this._aboutMnu.Text = "About SQL Notebook";
            this._aboutMnu.Click += new System.EventHandler(this.AboutMnu_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 31);
            // 
            // _newNoteBtn
            // 
            this._newNoteBtn.Image = global::SqlNotebook.Properties.Resources.NoteAdd;
            this._newNoteBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._newNoteBtn.Margin = new System.Windows.Forms.Padding(0, 1, 0, 4);
            this._newNoteBtn.Name = "_newNoteBtn";
            this._newNoteBtn.Padding = new System.Windows.Forms.Padding(0, 3, 5, 3);
            this._newNoteBtn.Size = new System.Drawing.Size(83, 26);
            this._newNoteBtn.Text = "New note";
            this._newNoteBtn.Click += new System.EventHandler(this.NewNoteBtn_Click);
            // 
            // _newConsoleBtn
            // 
            this._newConsoleBtn.Image = global::SqlNotebook.Properties.Resources.ApplicationXpTerminalAdd;
            this._newConsoleBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._newConsoleBtn.Margin = new System.Windows.Forms.Padding(0, 1, 0, 4);
            this._newConsoleBtn.Name = "_newConsoleBtn";
            this._newConsoleBtn.Padding = new System.Windows.Forms.Padding(0, 3, 5, 3);
            this._newConsoleBtn.Size = new System.Drawing.Size(100, 26);
            this._newConsoleBtn.Text = "New console";
            this._newConsoleBtn.Click += new System.EventHandler(this.NewConsoleBtn_Click);
            // 
            // _newScriptBtn
            // 
            this._newScriptBtn.Image = global::SqlNotebook.Properties.Resources.ScriptAdd;
            this._newScriptBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._newScriptBtn.Margin = new System.Windows.Forms.Padding(0, 1, 0, 4);
            this._newScriptBtn.Name = "_newScriptBtn";
            this._newScriptBtn.Padding = new System.Windows.Forms.Padding(0, 3, 5, 3);
            this._newScriptBtn.Size = new System.Drawing.Size(88, 26);
            this._newScriptBtn.Text = "New script";
            this._newScriptBtn.Click += new System.EventHandler(this.NewScriptBtn_Click);
            // 
            // _optionsMnu
            // 
            this._optionsMnu.Name = "_optionsMnu";
            this._optionsMnu.Size = new System.Drawing.Size(155, 22);
            this._optionsMnu.Text = "Options...";
            this._optionsMnu.Click += new System.EventHandler(this.OptionsMnu_Click);
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new System.Drawing.Size(152, 6);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 780);
            this.Controls.Add(this._toolStripContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(523, 359);
            this.Name = "MainForm";
            this.Text = "Untitled - SQL Notebook";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this._toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
            this._toolStripContainer.BottomToolStripPanel.PerformLayout();
            this._toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this._toolStripContainer.TopToolStripPanel.PerformLayout();
            this._toolStripContainer.ResumeLayout(false);
            this._toolStripContainer.PerformLayout();
            this._statusStrip.ResumeLayout(false);
            this._statusStrip.PerformLayout();
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer _toolStripContainer;
        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _newConsoleBtn;
        private System.Windows.Forms.ToolStripDropDownButton _importBtn;
        private System.Windows.Forms.ToolStripMenuItem _importFileMnu;
        private System.Windows.Forms.ToolStripMenuItem _importPostgresMnu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem _recentFilesMnu;
        private System.Windows.Forms.ToolStripMenuItem _recentFilesNoneMnu;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem _newMnu;
        private System.Windows.Forms.ToolStripMenuItem _openMnu;
        private System.Windows.Forms.ToolStripMenuItem _saveMnu;
        private System.Windows.Forms.ToolStripMenuItem _exitMnu;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripMenuItem _aboutMnu;
        private System.Windows.Forms.ToolStripMenuItem _recentServersMnu;
        private System.Windows.Forms.ToolStripButton _newScriptBtn;
        private System.Windows.Forms.ToolStripButton _newNoteBtn;
        private System.Windows.Forms.ToolStripStatusLabel _statusLbl;
        private System.Windows.Forms.ToolStripProgressBar _statusProgressbar;
        private System.Windows.Forms.ToolStripMenuItem _importMssqlMnu;
        private System.Windows.Forms.ToolStripMenuItem _importMysqlMnu;
        private System.Windows.Forms.ToolStripMenuItem _recentServersNoneMnu;
        private System.Windows.Forms.ToolStripMenuItem _saveAsMnu;
        private System.Windows.Forms.ToolStripMenuItem _reportIssueMnu;
        private System.Windows.Forms.ToolStripMenuItem _viewDocMnu;
        private System.Windows.Forms.ToolStripStatusLabel _openTransactionLbl;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripDropDownButton _exportMnu;
        private System.Windows.Forms.ToolStripMenuItem toCSVFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toExcelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toSQLScriptsqlToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toMicrosoftSQLServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toPostgreSQLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toMySQLToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem recentFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentServersToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel _cancelLnk;
        private System.Windows.Forms.ToolStripMenuItem _optionsMnu;
    }
}

