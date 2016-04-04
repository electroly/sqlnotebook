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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this._statusStrip = new System.Windows.Forms.StatusStrip();
            this._statusLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this._statusProgressbar = new System.Windows.Forms.ToolStripProgressBar();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this._newMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._openMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._saveMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this._exitMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this._aboutMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._newNoteBtn = new System.Windows.Forms.ToolStripButton();
            this._newConsoleBtn = new System.Windows.Forms.ToolStripButton();
            this._newScriptBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._importBtn = new System.Windows.Forms.ToolStripDropDownButton();
            this._importFileMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._importPostgresMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.recentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentServersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton4 = new System.Windows.Forms.ToolStripDropDownButton();
            this._toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this._toolStripContainer.TopToolStripPanel.SuspendLayout();
            this._toolStripContainer.SuspendLayout();
            this._statusStrip.SuspendLayout();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
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
            this._toolStripContainer.ContentPanel.Size = new System.Drawing.Size(898, 729);
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
            this._statusLbl,
            this._statusProgressbar});
            this._statusStrip.Location = new System.Drawing.Point(0, 0);
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Size = new System.Drawing.Size(898, 22);
            this._statusStrip.TabIndex = 0;
            // 
            // _statusLbl
            // 
            this._statusLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._statusLbl.Image = global::SqlNotebook.Properties.Resources.Hourglass;
            this._statusLbl.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._statusLbl.Name = "_statusLbl";
            this._statusLbl.Size = new System.Drawing.Size(777, 17);
            this._statusLbl.Spring = true;
            this._statusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._statusLbl.Visible = false;
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
            this.toolStripDropDownButton2,
            this.toolStripSeparator3,
            this._newNoteBtn,
            this._newConsoleBtn,
            this._newScriptBtn,
            this.toolStripSeparator1,
            this._importBtn,
            this.toolStripDropDownButton4});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Padding = new System.Windows.Forms.Padding(3, 2, 1, 0);
            this._toolStrip.ShowItemToolTips = false;
            this._toolStrip.Size = new System.Drawing.Size(898, 29);
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
            this.toolStripSeparator4,
            this._exitMnu});
            this.toolStripDropDownButton1.Image = global::SqlNotebook.Properties.Resources.DatabaseTable;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Margin = new System.Windows.Forms.Padding(0, 1, 0, 4);
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Padding = new System.Windows.Forms.Padding(3, 0, 5, 0);
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(46, 22);
            this.toolStripDropDownButton1.Text = "&File";
            // 
            // _newMnu
            // 
            this._newMnu.Image = global::SqlNotebook.Properties.Resources.PageWhite;
            this._newMnu.Name = "_newMnu";
            this._newMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this._newMnu.Size = new System.Drawing.Size(155, 22);
            this._newMnu.Text = "New";
            this._newMnu.Click += new System.EventHandler(this.NewMnu_Click);
            // 
            // _openMnu
            // 
            this._openMnu.Image = global::SqlNotebook.Properties.Resources.Folder;
            this._openMnu.Name = "_openMnu";
            this._openMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this._openMnu.Size = new System.Drawing.Size(155, 22);
            this._openMnu.Text = "Open...";
            this._openMnu.Click += new System.EventHandler(this.OpenMnu_Click);
            // 
            // _saveMnu
            // 
            this._saveMnu.Image = global::SqlNotebook.Properties.Resources.Diskette;
            this._saveMnu.Name = "_saveMnu";
            this._saveMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this._saveMnu.Size = new System.Drawing.Size(155, 22);
            this._saveMnu.Text = "Save";
            this._saveMnu.Click += new System.EventHandler(this.SaveMnu_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(152, 6);
            // 
            // _exitMnu
            // 
            this._exitMnu.Name = "_exitMnu";
            this._exitMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this._exitMnu.Size = new System.Drawing.Size(155, 22);
            this._exitMnu.Text = "E&xit";
            this._exitMnu.Click += new System.EventHandler(this.ExitMnu_Click);
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._aboutMnu});
            this.toolStripDropDownButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton2.Image")));
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Margin = new System.Windows.Forms.Padding(0, 1, 0, 4);
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(50, 22);
            this.toolStripDropDownButton2.Text = "&Help";
            // 
            // _aboutMnu
            // 
            this._aboutMnu.Name = "_aboutMnu";
            this._aboutMnu.Size = new System.Drawing.Size(187, 22);
            this._aboutMnu.Text = "About SQL Notebook";
            this._aboutMnu.Click += new System.EventHandler(this.AboutMnu_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AutoSize = false;
            this.toolStripSeparator3.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 23);
            // 
            // _newNoteBtn
            // 
            this._newNoteBtn.Image = global::SqlNotebook.Properties.Resources.NoteAdd;
            this._newNoteBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._newNoteBtn.Margin = new System.Windows.Forms.Padding(0, 1, 0, 4);
            this._newNoteBtn.Name = "_newNoteBtn";
            this._newNoteBtn.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this._newNoteBtn.Size = new System.Drawing.Size(83, 22);
            this._newNoteBtn.Text = "New note";
            this._newNoteBtn.Click += new System.EventHandler(this.NewNoteBtn_Click);
            // 
            // _newConsoleBtn
            // 
            this._newConsoleBtn.Image = global::SqlNotebook.Properties.Resources.ApplicationXpTerminalAdd;
            this._newConsoleBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._newConsoleBtn.Margin = new System.Windows.Forms.Padding(0, 1, 0, 4);
            this._newConsoleBtn.Name = "_newConsoleBtn";
            this._newConsoleBtn.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this._newConsoleBtn.Size = new System.Drawing.Size(100, 22);
            this._newConsoleBtn.Text = "New console";
            this._newConsoleBtn.Click += new System.EventHandler(this.NewConsoleBtn_Click);
            // 
            // _newScriptBtn
            // 
            this._newScriptBtn.Image = global::SqlNotebook.Properties.Resources.ScriptAdd;
            this._newScriptBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._newScriptBtn.Margin = new System.Windows.Forms.Padding(0, 1, 0, 4);
            this._newScriptBtn.Name = "_newScriptBtn";
            this._newScriptBtn.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this._newScriptBtn.Size = new System.Drawing.Size(88, 22);
            this._newScriptBtn.Text = "New script";
            this._newScriptBtn.Click += new System.EventHandler(this.NewScriptBtn_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // _importBtn
            // 
            this._importBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._importFileMnu,
            this._importPostgresMnu,
            this.toolStripSeparator2,
            this.recentToolStripMenuItem,
            this.recentServersToolStripMenuItem});
            this._importBtn.Image = global::SqlNotebook.Properties.Resources.TextImports;
            this._importBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._importBtn.Margin = new System.Windows.Forms.Padding(0, 1, 0, 4);
            this._importBtn.Name = "_importBtn";
            this._importBtn.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this._importBtn.Size = new System.Drawing.Size(77, 22);
            this._importBtn.Text = "&Import";
            // 
            // _importFileMnu
            // 
            this._importFileMnu.Name = "_importFileMnu";
            this._importFileMnu.Size = new System.Drawing.Size(175, 22);
            this._importFileMnu.Text = "From &file...";
            this._importFileMnu.Click += new System.EventHandler(this.ImportFileMnu_Click);
            // 
            // _importPostgresMnu
            // 
            this._importPostgresMnu.Name = "_importPostgresMnu";
            this._importPostgresMnu.Size = new System.Drawing.Size(175, 22);
            this._importPostgresMnu.Text = "From &PostgreSQL...";
            this._importPostgresMnu.Click += new System.EventHandler(this.ImportPostgresMnu_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(172, 6);
            // 
            // recentToolStripMenuItem
            // 
            this.recentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneToolStripMenuItem});
            this.recentToolStripMenuItem.Name = "recentToolStripMenuItem";
            this.recentToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.recentToolStripMenuItem.Text = "Recent files";
            // 
            // noneToolStripMenuItem
            // 
            this.noneToolStripMenuItem.Enabled = false;
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.noneToolStripMenuItem.Text = "(none)";
            // 
            // recentServersToolStripMenuItem
            // 
            this.recentServersToolStripMenuItem.Name = "recentServersToolStripMenuItem";
            this.recentServersToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.recentServersToolStripMenuItem.Text = "Recent servers";
            // 
            // toolStripDropDownButton4
            // 
            this.toolStripDropDownButton4.Image = global::SqlNotebook.Properties.Resources.TextExports;
            this.toolStripDropDownButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton4.Margin = new System.Windows.Forms.Padding(0, 1, 0, 4);
            this.toolStripDropDownButton4.Name = "toolStripDropDownButton4";
            this.toolStripDropDownButton4.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.toolStripDropDownButton4.Size = new System.Drawing.Size(74, 22);
            this.toolStripDropDownButton4.Text = "&Export";
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
        private System.Windows.Forms.ToolStripMenuItem recentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem _newMnu;
        private System.Windows.Forms.ToolStripMenuItem _openMnu;
        private System.Windows.Forms.ToolStripMenuItem _saveMnu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem _exitMnu;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripMenuItem _aboutMnu;
        private System.Windows.Forms.ToolStripMenuItem recentServersToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton _newScriptBtn;
        private System.Windows.Forms.ToolStripButton _newNoteBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton4;
        private System.Windows.Forms.ToolStripStatusLabel _statusLbl;
        private System.Windows.Forms.ToolStripProgressBar _statusProgressbar;
    }
}

