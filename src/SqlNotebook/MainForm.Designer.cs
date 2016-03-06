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
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.newNotebookToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveNotebookAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this._aboutMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this._importBtn = new System.Windows.Forms.ToolStripDropDownButton();
            this._importFileMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.postgreSQLDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.recentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentServersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton3 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this._executeBtn = new System.Windows.Forms.ToolStripButton();
            this._toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this._toolStripContainer.TopToolStripPanel.SuspendLayout();
            this._toolStripContainer.SuspendLayout();
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
            this._toolStripContainer.ContentPanel.Size = new System.Drawing.Size(812, 596);
            this._toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this._toolStripContainer.Name = "_toolStripContainer";
            this._toolStripContainer.Size = new System.Drawing.Size(812, 647);
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
            this._statusStrip.Location = new System.Drawing.Point(0, 0);
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Size = new System.Drawing.Size(812, 22);
            this._statusStrip.TabIndex = 0;
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
            this.toolStripButton2,
            this.toolStripButton5,
            this._importBtn,
            this.toolStripDropDownButton3,
            this.toolStripSeparator5,
            this._executeBtn});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Padding = new System.Windows.Forms.Padding(3, 2, 1, 0);
            this._toolStrip.ShowItemToolTips = false;
            this._toolStrip.Size = new System.Drawing.Size(812, 29);
            this._toolStrip.Stretch = true;
            this._toolStrip.TabIndex = 1;
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newNotebookToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveNotebookAsToolStripMenuItem,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
            this.toolStripDropDownButton1.Image = global::SqlNotebook.Properties.Resources.DatabaseTable;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Margin = new System.Windows.Forms.Padding(0, 1, 0, 4);
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Padding = new System.Windows.Forms.Padding(3, 0, 5, 0);
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(46, 22);
            this.toolStripDropDownButton1.Text = "&File";
            // 
            // newNotebookToolStripMenuItem
            // 
            this.newNotebookToolStripMenuItem.Image = global::SqlNotebook.Properties.Resources.PageWhite;
            this.newNotebookToolStripMenuItem.Name = "newNotebookToolStripMenuItem";
            this.newNotebookToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newNotebookToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.newNotebookToolStripMenuItem.Text = "New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::SqlNotebook.Properties.Resources.Folder;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.openToolStripMenuItem.Text = "Open...";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::SqlNotebook.Properties.Resources.Diskette;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveNotebookAsToolStripMenuItem
            // 
            this.saveNotebookAsToolStripMenuItem.Name = "saveNotebookAsToolStripMenuItem";
            this.saveNotebookAsToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.saveNotebookAsToolStripMenuItem.Text = "Save as...";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(189, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
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
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::SqlNotebook.Properties.Resources.ScriptAdd;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Margin = new System.Windows.Forms.Padding(0, 1, 0, 4);
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.toolStripButton2.Size = new System.Drawing.Size(89, 22);
            this.toolStripButton2.Text = "New query";
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.Image = global::SqlNotebook.Properties.Resources.ApplicationXpTerminalAdd;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Margin = new System.Windows.Forms.Padding(0, 1, 0, 4);
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.toolStripButton5.Size = new System.Drawing.Size(100, 22);
            this.toolStripButton5.Text = "New console";
            // 
            // _importBtn
            // 
            this._importBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._importFileMnu,
            this.postgreSQLDatabaseToolStripMenuItem,
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
            // postgreSQLDatabaseToolStripMenuItem
            // 
            this.postgreSQLDatabaseToolStripMenuItem.Name = "postgreSQLDatabaseToolStripMenuItem";
            this.postgreSQLDatabaseToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.postgreSQLDatabaseToolStripMenuItem.Text = "From &PostgreSQL...";
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
            // toolStripDropDownButton3
            // 
            this.toolStripDropDownButton3.Image = global::SqlNotebook.Properties.Resources.TextExports;
            this.toolStripDropDownButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton3.Margin = new System.Windows.Forms.Padding(0, 1, 0, 4);
            this.toolStripDropDownButton3.Name = "toolStripDropDownButton3";
            this.toolStripDropDownButton3.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.toolStripDropDownButton3.Size = new System.Drawing.Size(74, 22);
            this.toolStripDropDownButton3.Text = "&Export";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.AutoSize = false;
            this.toolStripSeparator5.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 23);
            // 
            // _executeBtn
            // 
            this._executeBtn.Enabled = false;
            this._executeBtn.Image = global::SqlNotebook.Properties.Resources.ControlPlayBlue;
            this._executeBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._executeBtn.Margin = new System.Windows.Forms.Padding(0, 1, 0, 4);
            this._executeBtn.Name = "_executeBtn";
            this._executeBtn.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this._executeBtn.Size = new System.Drawing.Size(72, 22);
            this._executeBtn.Text = "Execute";
            this._executeBtn.Click += new System.EventHandler(this.ExecuteBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 647);
            this.Controls.Add(this._toolStripContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(523, 359);
            this.Name = "MainForm";
            this.Text = "Untitled - SQL Notebook";
            this._toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
            this._toolStripContainer.BottomToolStripPanel.PerformLayout();
            this._toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this._toolStripContainer.TopToolStripPanel.PerformLayout();
            this._toolStripContainer.ResumeLayout(false);
            this._toolStripContainer.PerformLayout();
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer _toolStripContainer;
        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripDropDownButton _importBtn;
        private System.Windows.Forms.ToolStripMenuItem _importFileMnu;
        private System.Windows.Forms.ToolStripMenuItem postgreSQLDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem recentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem newNotebookToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveNotebookAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton3;
        private System.Windows.Forms.ToolStripButton _executeBtn;
        private System.Windows.Forms.ToolStripMenuItem _aboutMnu;
        private System.Windows.Forms.ToolStripMenuItem recentServersToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    }
}

