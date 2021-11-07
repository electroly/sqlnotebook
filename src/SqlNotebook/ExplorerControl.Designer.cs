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
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("Pages", System.Windows.Forms.HorizontalAlignment.Center);
            System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("Scripts", System.Windows.Forms.HorizontalAlignment.Center);
            System.Windows.Forms.ListViewGroup listViewGroup7 = new System.Windows.Forms.ListViewGroup("Tables", System.Windows.Forms.HorizontalAlignment.Center);
            System.Windows.Forms.ListViewGroup listViewGroup8 = new System.Windows.Forms.ListViewGroup("Views", System.Windows.Forms.HorizontalAlignment.Center);
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExplorerControl));
            this._list = new System.Windows.Forms.ListView();
            this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._openMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._deleteMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._renameMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._selectionLabel = new System.Windows.Forms.Label();
            this._detailsGrid = new System.Windows.Forms.DataGridView();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._toolbarNewPageButton = new System.Windows.Forms.ToolStripButton();
            this._toolbarNewScriptButton = new System.Windows.Forms.ToolStripButton();
            _nameColumn = new System.Windows.Forms.ColumnHeader();
            this._contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._detailsGrid)).BeginInit();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _nameColumn
            // 
            _nameColumn.Text = "";
            // 
            // _list
            // 
            this._list.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            _nameColumn});
            this._list.ContextMenuStrip = this._contextMenuStrip;
            this._list.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup5.Header = "Pages";
            listViewGroup5.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup5.Name = "Page";
            listViewGroup6.Header = "Scripts";
            listViewGroup6.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup6.Name = "Script";
            listViewGroup7.Header = "Tables";
            listViewGroup7.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup7.Name = "Table";
            listViewGroup8.Header = "Views";
            listViewGroup8.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup8.Name = "View";
            this._list.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup5,
            listViewGroup6,
            listViewGroup7,
            listViewGroup8});
            this._list.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this._list.HideSelection = false;
            this._list.LabelEdit = true;
            this._list.LabelWrap = false;
            this._list.Location = new System.Drawing.Point(0, 0);
            this._list.MultiSelect = false;
            this._list.Name = "_list";
            this._list.Size = new System.Drawing.Size(340, 282);
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
            this._contextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this._contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._openMnu,
            this.toolStripSeparator1,
            this._deleteMnu,
            this._renameMnu});
            this._contextMenuStrip.Name = "_contextMenuStrip";
            this._contextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this._contextMenuStrip.Size = new System.Drawing.Size(148, 106);
            this._contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip_Opening);
            // 
            // _openMnu
            // 
            this._openMnu.Name = "_openMnu";
            this._openMnu.Size = new System.Drawing.Size(147, 32);
            this._openMnu.Text = "&Open";
            this._openMnu.Click += new System.EventHandler(this.List_ItemActivate);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(144, 6);
            // 
            // _deleteMnu
            // 
            this._deleteMnu.Name = "_deleteMnu";
            this._deleteMnu.Size = new System.Drawing.Size(147, 32);
            this._deleteMnu.Text = "&Delete";
            this._deleteMnu.Click += new System.EventHandler(this.DeleteMnu_Click);
            // 
            // _renameMnu
            // 
            this._renameMnu.Name = "_renameMnu";
            this._renameMnu.Size = new System.Drawing.Size(147, 32);
            this._renameMnu.Text = "&Rename";
            this._renameMnu.Click += new System.EventHandler(this.RenameMnu_Click);
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
            this._splitContainer.Panel2.Controls.Add(this.tableLayoutPanel1);
            this._splitContainer.Size = new System.Drawing.Size(340, 574);
            this._splitContainer.SplitterDistance = 282;
            this._splitContainer.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this._selectionLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._detailsGrid, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(340, 288);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // _selectionLabel
            // 
            this._selectionLabel.AutoSize = true;
            this._selectionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._selectionLabel.Location = new System.Drawing.Point(3, 0);
            this._selectionLabel.Name = "_selectionLabel";
            this._selectionLabel.Size = new System.Drawing.Size(334, 25);
            this._selectionLabel.TabIndex = 1;
            this._selectionLabel.Text = "Selected item";
            // 
            // _detailsGrid
            // 
            this._detailsGrid.AllowUserToAddRows = false;
            this._detailsGrid.AllowUserToDeleteRows = false;
            this._detailsGrid.AllowUserToResizeColumns = false;
            this._detailsGrid.AllowUserToResizeRows = false;
            this._detailsGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this._detailsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._detailsGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._detailsGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this._detailsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._detailsGrid.ColumnHeadersVisible = false;
            this._detailsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameColumn,
            this.typeColumn});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._detailsGrid.DefaultCellStyle = dataGridViewCellStyle7;
            this._detailsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._detailsGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._detailsGrid.Location = new System.Drawing.Point(3, 28);
            this._detailsGrid.Name = "_detailsGrid";
            this._detailsGrid.ReadOnly = true;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._detailsGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this._detailsGrid.RowHeadersVisible = false;
            this._detailsGrid.RowHeadersWidth = 62;
            this._detailsGrid.RowTemplate.Height = 33;
            this._detailsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._detailsGrid.Size = new System.Drawing.Size(334, 257);
            this._detailsGrid.TabIndex = 2;
            // 
            // nameColumn
            // 
            this.nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameColumn.DataPropertyName = "Name";
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.nameColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.nameColumn.HeaderText = "Name";
            this.nameColumn.MinimumWidth = 8;
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
            // 
            // typeColumn
            // 
            this.typeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.typeColumn.DataPropertyName = "Type";
            this.typeColumn.HeaderText = "Type";
            this.typeColumn.MinimumWidth = 8;
            this.typeColumn.Name = "typeColumn";
            this.typeColumn.ReadOnly = true;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this._splitContainer);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(340, 574);
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
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this._toolStrip);
            // 
            // _toolStrip
            // 
            this._toolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._toolbarNewPageButton,
            this._toolbarNewScriptButton});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this._toolStrip.Size = new System.Drawing.Size(340, 34);
            this._toolStrip.Stretch = true;
            this._toolStrip.TabIndex = 4;
            // 
            // _toolbarNewPageButton
            // 
            this._toolbarNewPageButton.Image = ((System.Drawing.Image)(resources.GetObject("_toolbarNewPageButton.Image")));
            this._toolbarNewPageButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolbarNewPageButton.Name = "_toolbarNewPageButton";
            this._toolbarNewPageButton.Size = new System.Drawing.Size(117, 29);
            this._toolbarNewPageButton.Text = "Add Page";
            this._toolbarNewPageButton.Click += new System.EventHandler(this.ToolbarNewPageButton_Click);
            // 
            // _toolbarNewScriptButton
            // 
            this._toolbarNewScriptButton.Image = ((System.Drawing.Image)(resources.GetObject("_toolbarNewScriptButton.Image")));
            this._toolbarNewScriptButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolbarNewScriptButton.Name = "_toolbarNewScriptButton";
            this._toolbarNewScriptButton.Size = new System.Drawing.Size(124, 29);
            this._toolbarNewScriptButton.Text = "Add Script";
            this._toolbarNewScriptButton.Click += new System.EventHandler(this.ToolbarNewScriptButton_Click);
            // 
            // ExplorerControl
            // 
            this.Controls.Add(this.toolStripContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "ExplorerControl";
            this.Size = new System.Drawing.Size(340, 608);
            this._contextMenuStrip.ResumeLayout(false);
            this._splitContainer.Panel1.ResumeLayout(false);
            this._splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).EndInit();
            this._splitContainer.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._detailsGrid)).EndInit();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer _splitContainer;
        private System.Windows.Forms.ListView _list;
        private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _deleteMnu;
        private System.Windows.Forms.ToolStripMenuItem _renameMnu;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripMenuItem _openMnu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label _selectionLabel;
        private System.Windows.Forms.DataGridView _detailsGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn typeColumn;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _toolbarNewPageButton;
        private System.Windows.Forms.ToolStripButton _toolbarNewScriptButton;
    }
}