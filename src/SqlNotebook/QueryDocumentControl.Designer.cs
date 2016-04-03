namespace SqlNotebook {
    partial class QueryDocumentControl {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryDocumentControl));
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this._sqlPanel = new System.Windows.Forms.Panel();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this._grid = new System.Windows.Forms.DataGridView();
            this._resultToolStrip = new System.Windows.Forms.ToolStrip();
            this._executeBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._prevBtn = new System.Windows.Forms.ToolStripButton();
            this._resultSetLbl = new System.Windows.Forms.ToolStripLabel();
            this._nextBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._rowCountLbl = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this._resultToolStrip.SuspendLayout();
            this.SuspendLayout();
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
            this._splitContainer.Panel1.Controls.Add(this._sqlPanel);
            // 
            // _splitContainer.Panel2
            // 
            this._splitContainer.Panel2.Controls.Add(this.toolStripContainer1);
            this._splitContainer.Panel2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._splitContainer.Size = new System.Drawing.Size(482, 410);
            this._splitContainer.SplitterDistance = 158;
            this._splitContainer.SplitterWidth = 5;
            this._splitContainer.TabIndex = 0;
            // 
            // _sqlPanel
            // 
            this._sqlPanel.BackColor = System.Drawing.SystemColors.Window;
            this._sqlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._sqlPanel.Location = new System.Drawing.Point(0, 0);
            this._sqlPanel.Name = "_sqlPanel";
            this._sqlPanel.Size = new System.Drawing.Size(482, 158);
            this._sqlPanel.TabIndex = 0;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this._grid);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(482, 222);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(482, 247);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.BackColor = System.Drawing.SystemColors.Window;
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this._resultToolStrip);
            // 
            // _grid
            // 
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AllowUserToResizeRows = false;
            this._grid.BackgroundColor = System.Drawing.SystemColors.Window;
            this._grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._grid.Location = new System.Drawing.Point(0, 0);
            this._grid.Name = "_grid";
            this._grid.ReadOnly = true;
            this._grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this._grid.RowHeadersVisible = false;
            this._grid.RowHeadersWidth = 25;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._grid.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._grid.ShowCellErrors = false;
            this._grid.ShowCellToolTips = false;
            this._grid.ShowEditingIcon = false;
            this._grid.ShowRowErrors = false;
            this._grid.Size = new System.Drawing.Size(482, 222);
            this._grid.TabIndex = 0;
            // 
            // _resultToolStrip
            // 
            this._resultToolStrip.BackColor = System.Drawing.SystemColors.Window;
            this._resultToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._resultToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._resultToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._executeBtn,
            this.toolStripSeparator2,
            this._prevBtn,
            this._resultSetLbl,
            this._nextBtn,
            this.toolStripSeparator1,
            this._rowCountLbl});
            this._resultToolStrip.Location = new System.Drawing.Point(0, 0);
            this._resultToolStrip.Name = "_resultToolStrip";
            this._resultToolStrip.ShowItemToolTips = false;
            this._resultToolStrip.Size = new System.Drawing.Size(482, 25);
            this._resultToolStrip.Stretch = true;
            this._resultToolStrip.TabIndex = 0;
            // 
            // _executeBtn
            // 
            this._executeBtn.Image = global::SqlNotebook.Properties.Resources.ControlPlayBlue;
            this._executeBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._executeBtn.Name = "_executeBtn";
            this._executeBtn.Size = new System.Drawing.Size(67, 22);
            this._executeBtn.Text = "Execute";
            this._executeBtn.Click += new System.EventHandler(this.ExecuteBtn_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // _prevBtn
            // 
            this._prevBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._prevBtn.Image = ((System.Drawing.Image)(resources.GetObject("_prevBtn.Image")));
            this._prevBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._prevBtn.Name = "_prevBtn";
            this._prevBtn.Size = new System.Drawing.Size(23, 22);
            this._prevBtn.Text = "toolStripButton1";
            this._prevBtn.Click += new System.EventHandler(this.PrevBtn_Click);
            // 
            // _resultSetLbl
            // 
            this._resultSetLbl.Name = "_resultSetLbl";
            this._resultSetLbl.Size = new System.Drawing.Size(36, 22);
            this._resultSetLbl.Text = "1 of 1";
            // 
            // _nextBtn
            // 
            this._nextBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._nextBtn.Image = ((System.Drawing.Image)(resources.GetObject("_nextBtn.Image")));
            this._nextBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._nextBtn.Name = "_nextBtn";
            this._nextBtn.Size = new System.Drawing.Size(23, 22);
            this._nextBtn.Text = "toolStripButton2";
            this._nextBtn.Click += new System.EventHandler(this.NextBtn_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // _rowCountLbl
            // 
            this._rowCountLbl.Name = "_rowCountLbl";
            this._rowCountLbl.Size = new System.Drawing.Size(41, 22);
            this._rowCountLbl.Text = "0 rows";
            // 
            // QueryDocumentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._splitContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "QueryDocumentControl";
            this.Size = new System.Drawing.Size(482, 410);
            this._splitContainer.Panel1.ResumeLayout(false);
            this._splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).EndInit();
            this._splitContainer.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this._resultToolStrip.ResumeLayout(false);
            this._resultToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer _splitContainer;
        private System.Windows.Forms.Panel _sqlPanel;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip _resultToolStrip;
        private System.Windows.Forms.ToolStripButton _executeBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.ToolStripButton _prevBtn;
        private System.Windows.Forms.ToolStripButton _nextBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel _resultSetLbl;
        private System.Windows.Forms.ToolStripLabel _rowCountLbl;
    }
}
