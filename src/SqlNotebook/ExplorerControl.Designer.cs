namespace SqlNotebook {
    partial class ExplorerControl {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ColumnHeader _nameColumn;
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.ColumnHeader columnHeader2;
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Notes", System.Windows.Forms.HorizontalAlignment.Center);
            System.Windows.Forms.ListViewGroup listViewGroup8 = new System.Windows.Forms.ListViewGroup("Consoles", System.Windows.Forms.HorizontalAlignment.Center);
            System.Windows.Forms.ListViewGroup listViewGroup9 = new System.Windows.Forms.ListViewGroup("Scripts", System.Windows.Forms.HorizontalAlignment.Center);
            System.Windows.Forms.ListViewGroup listViewGroup10 = new System.Windows.Forms.ListViewGroup("Tables", System.Windows.Forms.HorizontalAlignment.Center);
            System.Windows.Forms.ListViewGroup listViewGroup11 = new System.Windows.Forms.ListViewGroup("Views", System.Windows.Forms.HorizontalAlignment.Center);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExplorerControl));
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Group", System.Windows.Forms.HorizontalAlignment.Center);
            this._list = new System.Windows.Forms.ListView();
            this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._renameMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._deleteMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._imageList = new System.Windows.Forms.ImageList(this.components);
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this._detailsLst = new System.Windows.Forms.ListView();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._newNoteBtn = new System.Windows.Forms.ToolStripButton();
            this._newConsoleBtn = new System.Windows.Forms.ToolStripButton();
            this._newScriptBtn = new System.Windows.Forms.ToolStripButton();
            this._renameBtn = new System.Windows.Forms.ToolStripButton();
            this._deleteBtn = new System.Windows.Forms.ToolStripButton();
            _nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _nameColumn
            // 
            _nameColumn.Text = "";
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Name";
            columnHeader1.Width = 157;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Info";
            columnHeader2.Width = 163;
            // 
            // _list
            // 
            this._list.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            _nameColumn});
            this._list.ContextMenuStrip = this._contextMenuStrip;
            this._list.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup1.Header = "Notes";
            listViewGroup1.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup1.Name = "Note";
            listViewGroup8.Header = "Consoles";
            listViewGroup8.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup8.Name = "Console";
            listViewGroup9.Header = "Scripts";
            listViewGroup9.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup9.Name = "Script";
            listViewGroup10.Header = "Tables";
            listViewGroup10.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup10.Name = "Table";
            listViewGroup11.Header = "Views";
            listViewGroup11.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup11.Name = "View";
            this._list.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup8,
            listViewGroup9,
            listViewGroup10,
            listViewGroup11});
            this._list.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this._list.LabelEdit = true;
            this._list.LabelWrap = false;
            this._list.Location = new System.Drawing.Point(0, 0);
            this._list.MultiSelect = false;
            this._list.Name = "_list";
            this._list.Size = new System.Drawing.Size(340, 307);
            this._list.SmallImageList = this._imageList;
            this._list.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this._list.TabIndex = 0;
            this._list.UseCompatibleStateImageBehavior = false;
            this._list.View = System.Windows.Forms.View.Details;
            this._list.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.List_AfterLabelEdit);
            this._list.ItemActivate += new System.EventHandler(this.List_ItemActivate);
            this._list.SelectedIndexChanged += new System.EventHandler(this.List_SelectedIndexChanged);
            this._list.KeyDown += new System.Windows.Forms.KeyEventHandler(this.List_KeyDown);
            // 
            // _contextMenuStrip
            // 
            this._contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._renameMnu,
            this._deleteMnu});
            this._contextMenuStrip.Name = "_contextMenuStrip";
            this._contextMenuStrip.Size = new System.Drawing.Size(118, 48);
            this._contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip_Opening);
            // 
            // _renameMnu
            // 
            this._renameMnu.Image = global::SqlNotebook.Properties.Resources.TextfieldRename;
            this._renameMnu.Name = "_renameMnu";
            this._renameMnu.Size = new System.Drawing.Size(117, 22);
            this._renameMnu.Text = "&Rename";
            this._renameMnu.Click += new System.EventHandler(this.RenameMnu_Click);
            // 
            // _deleteMnu
            // 
            this._deleteMnu.Image = global::SqlNotebook.Properties.Resources.Delete;
            this._deleteMnu.Name = "_deleteMnu";
            this._deleteMnu.Size = new System.Drawing.Size(117, 22);
            this._deleteMnu.Text = "&Delete";
            this._deleteMnu.Click += new System.EventHandler(this.DeleteMnu_Click);
            // 
            // _imageList
            // 
            this._imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_imageList.ImageStream")));
            this._imageList.TransparentColor = System.Drawing.Color.Transparent;
            this._imageList.Images.SetKeyName(0, "note.png");
            this._imageList.Images.SetKeyName(1, "application_xp_terminal.png");
            this._imageList.Images.SetKeyName(2, "script.png");
            this._imageList.Images.SetKeyName(3, "table.png");
            this._imageList.Images.SetKeyName(4, "filter.png");
            this._imageList.Images.SetKeyName(5, "bullet_black.png");
            this._imageList.Images.SetKeyName(6, "bullet_key.png");
            // 
            // _splitContainer
            // 
            this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer.Location = new System.Drawing.Point(0, 0);
            this._splitContainer.Name = "_splitContainer";
            this._splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _splitContainer.Panel1
            // 
            this._splitContainer.Panel1.BackColor = System.Drawing.SystemColors.Window;
            this._splitContainer.Panel1.Controls.Add(this._list);
            // 
            // _splitContainer.Panel2
            // 
            this._splitContainer.Panel2.Controls.Add(this._detailsLst);
            this._splitContainer.Size = new System.Drawing.Size(340, 578);
            this._splitContainer.SplitterDistance = 307;
            this._splitContainer.TabIndex = 1;
            // 
            // _detailsLst
            // 
            this._detailsLst.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._detailsLst.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader1,
            columnHeader2});
            this._detailsLst.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup2.Header = "Group";
            listViewGroup2.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup2.Name = "Group";
            this._detailsLst.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup2});
            this._detailsLst.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this._detailsLst.Location = new System.Drawing.Point(0, 0);
            this._detailsLst.MultiSelect = false;
            this._detailsLst.Name = "_detailsLst";
            this._detailsLst.Size = new System.Drawing.Size(340, 267);
            this._detailsLst.SmallImageList = this._imageList;
            this._detailsLst.TabIndex = 0;
            this._detailsLst.UseCompatibleStateImageBehavior = false;
            this._detailsLst.View = System.Windows.Forms.View.Details;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this._splitContainer);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(340, 578);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(340, 608);
            this.toolStripContainer1.TabIndex = 2;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.BackColor = System.Drawing.SystemColors.Window;
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._newNoteBtn,
            this._newConsoleBtn,
            this._newScriptBtn,
            this._renameBtn,
            this._deleteBtn});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(340, 30);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // _newNoteBtn
            // 
            this._newNoteBtn.AutoSize = false;
            this._newNoteBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._newNoteBtn.Image = global::SqlNotebook.Properties.Resources.NoteAdd;
            this._newNoteBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._newNoteBtn.Margin = new System.Windows.Forms.Padding(2, 1, 0, 2);
            this._newNoteBtn.Name = "_newNoteBtn";
            this._newNoteBtn.Size = new System.Drawing.Size(27, 27);
            this._newNoteBtn.ToolTipText = "New note";
            this._newNoteBtn.Click += new System.EventHandler(this.NewNoteBtn_Click);
            // 
            // _newConsoleBtn
            // 
            this._newConsoleBtn.AutoSize = false;
            this._newConsoleBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._newConsoleBtn.Image = global::SqlNotebook.Properties.Resources.ApplicationXpTerminalAdd;
            this._newConsoleBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._newConsoleBtn.Name = "_newConsoleBtn";
            this._newConsoleBtn.Size = new System.Drawing.Size(27, 27);
            this._newConsoleBtn.ToolTipText = "New console";
            this._newConsoleBtn.Click += new System.EventHandler(this.NewConsoleBtn_Click);
            // 
            // _newScriptBtn
            // 
            this._newScriptBtn.AutoSize = false;
            this._newScriptBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._newScriptBtn.Image = global::SqlNotebook.Properties.Resources.ScriptAdd;
            this._newScriptBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._newScriptBtn.Name = "_newScriptBtn";
            this._newScriptBtn.Size = new System.Drawing.Size(27, 27);
            this._newScriptBtn.ToolTipText = "New script";
            this._newScriptBtn.Click += new System.EventHandler(this.NewScriptBtn_Click);
            // 
            // _renameBtn
            // 
            this._renameBtn.AutoSize = false;
            this._renameBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._renameBtn.Image = global::SqlNotebook.Properties.Resources.TextfieldRename;
            this._renameBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._renameBtn.Margin = new System.Windows.Forms.Padding(7, 1, 0, 2);
            this._renameBtn.Name = "_renameBtn";
            this._renameBtn.Size = new System.Drawing.Size(27, 27);
            this._renameBtn.ToolTipText = "Rename selected item";
            this._renameBtn.Click += new System.EventHandler(this.RenameBtn_Click);
            // 
            // _deleteBtn
            // 
            this._deleteBtn.AutoSize = false;
            this._deleteBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._deleteBtn.Image = global::SqlNotebook.Properties.Resources.Delete;
            this._deleteBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._deleteBtn.Name = "_deleteBtn";
            this._deleteBtn.Size = new System.Drawing.Size(27, 27);
            this._deleteBtn.ToolTipText = "Delete selected item";
            this._deleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // ExplorerControl
            // 
            this.Controls.Add(this.toolStripContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ExplorerControl";
            this.Size = new System.Drawing.Size(340, 608);
            this._contextMenuStrip.ResumeLayout(false);
            this._splitContainer.Panel1.ResumeLayout(false);
            this._splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).EndInit();
            this._splitContainer.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer _splitContainer;
        private System.Windows.Forms.ListView _detailsLst;
        private System.Windows.Forms.ListView _list;
        private System.Windows.Forms.ImageList _imageList;
        private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _deleteMnu;
        private System.Windows.Forms.ToolStripMenuItem _renameMnu;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton _newNoteBtn;
        private System.Windows.Forms.ToolStripButton _newConsoleBtn;
        private System.Windows.Forms.ToolStripButton _newScriptBtn;
        private System.Windows.Forms.ToolStripButton _renameBtn;
        private System.Windows.Forms.ToolStripButton _deleteBtn;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
    }
}