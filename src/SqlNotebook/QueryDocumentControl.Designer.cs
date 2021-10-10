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
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this._sqlPanel = new System.Windows.Forms.Panel();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this._grid = new System.Windows.Forms.DataGridView();
            this._resultToolStrip = new System.Windows.Forms.ToolStrip();
            this._executeBtn = new System.Windows.Forms.ToolStripButton();
            this._prevBtn = new System.Windows.Forms.ToolStripButton();
            this._resultSetLbl = new System.Windows.Forms.ToolStripLabel();
            this._nextBtn = new System.Windows.Forms.ToolStripButton();
            this._sendToMnu = new System.Windows.Forms.ToolStripDropDownButton();
            this._sendTableMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this._sendCsvMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._sendExcelMnu = new System.Windows.Forms.ToolStripMenuItem();
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
            this._splitContainer.Panel2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this._splitContainer.Size = new System.Drawing.Size(694, 614);
            this._splitContainer.SplitterDistance = 300;
            this._splitContainer.SplitterWidth = 5;
            this._splitContainer.TabIndex = 0;
            // 
            // _sqlPanel
            // 
            this._sqlPanel.BackColor = System.Drawing.SystemColors.Window;
            this._sqlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._sqlPanel.Location = new System.Drawing.Point(0, 0);
            this._sqlPanel.Name = "_sqlPanel";
            this._sqlPanel.Size = new System.Drawing.Size(694, 300);
            this._sqlPanel.TabIndex = 0;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this._grid);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(694, 277);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(694, 309);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.BackColor = System.Drawing.SystemColors.Window;
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this._resultToolStrip);
            this.toolStripContainer1.TopToolStripPanel.Cursor = System.Windows.Forms.Cursors.Arrow;
            // 
            // _grid
            // 
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AllowUserToResizeRows = false;
            this._grid.BackgroundColor = System.Drawing.SystemColors.Window;
            this._grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.Cursor = System.Windows.Forms.Cursors.Arrow;
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this._grid.Location = new System.Drawing.Point(0, 0);
            this._grid.Name = "_grid";
            this._grid.ReadOnly = true;
            this._grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this._grid.RowHeadersVisible = false;
            this._grid.RowHeadersWidth = 25;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._grid.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._grid.ShowCellErrors = false;
            this._grid.ShowCellToolTips = false;
            this._grid.ShowEditingIcon = false;
            this._grid.ShowRowErrors = false;
            this._grid.Size = new System.Drawing.Size(694, 277);
            this._grid.TabIndex = 0;
            // 
            // _resultToolStrip
            // 
            this._resultToolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._resultToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._resultToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._resultToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this._resultToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._executeBtn,
            this._prevBtn,
            this._resultSetLbl,
            this._nextBtn,
            this._sendToMnu,
            this._rowCountLbl});
            this._resultToolStrip.Location = new System.Drawing.Point(0, 0);
            this._resultToolStrip.Name = "_resultToolStrip";
            this._resultToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this._resultToolStrip.ShowItemToolTips = false;
            this._resultToolStrip.Size = new System.Drawing.Size(694, 32);
            this._resultToolStrip.Stretch = true;
            this._resultToolStrip.TabIndex = 0;
            // 
            // _executeBtn
            // 
            this._executeBtn.AutoSize = false;
            this._executeBtn.Image = global::SqlNotebook.Properties.Resources.ControlPlayBlue;
            this._executeBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._executeBtn.Margin = new System.Windows.Forms.Padding(2, 1, 0, 2);
            this._executeBtn.Name = "_executeBtn";
            this._executeBtn.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this._executeBtn.Size = new System.Drawing.Size(67, 27);
            this._executeBtn.Text = "Execute";
            this._executeBtn.Click += new System.EventHandler(this.ExecuteBtn_Click);
            // 
            // _prevBtn
            // 
            this._prevBtn.AutoSize = false;
            this._prevBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._prevBtn.Image = global::SqlNotebook.Properties.Resources.resultset_previous;
            this._prevBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._prevBtn.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this._prevBtn.Name = "_prevBtn";
            this._prevBtn.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this._prevBtn.Size = new System.Drawing.Size(30, 27);
            this._prevBtn.Text = "toolStripButton1";
            this._prevBtn.Visible = false;
            this._prevBtn.Click += new System.EventHandler(this.PrevBtn_Click);
            // 
            // _resultSetLbl
            // 
            this._resultSetLbl.Name = "_resultSetLbl";
            this._resultSetLbl.Size = new System.Drawing.Size(59, 27);
            this._resultSetLbl.Text = "1 of 1";
            this._resultSetLbl.Visible = false;
            // 
            // _nextBtn
            // 
            this._nextBtn.AutoSize = false;
            this._nextBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._nextBtn.Image = global::SqlNotebook.Properties.Resources.resultset_next;
            this._nextBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._nextBtn.Name = "_nextBtn";
            this._nextBtn.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this._nextBtn.Size = new System.Drawing.Size(30, 27);
            this._nextBtn.Text = "toolStripButton2";
            this._nextBtn.Visible = false;
            this._nextBtn.Click += new System.EventHandler(this.NextBtn_Click);
            // 
            // _sendToMnu
            // 
            this._sendToMnu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._sendToMnu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._sendTableMnu,
            this.toolStripSeparator4,
            this._sendCsvMnu,
            this._sendExcelMnu});
            this._sendToMnu.Image = global::SqlNotebook.Properties.Resources.TextExports;
            this._sendToMnu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._sendToMnu.Margin = new System.Windows.Forms.Padding(8, 1, 0, 2);
            this._sendToMnu.Name = "_sendToMnu";
            this._sendToMnu.Size = new System.Drawing.Size(92, 29);
            this._sendToMnu.Text = "Send to";
            this._sendToMnu.Visible = false;
            // 
            // _sendTableMnu
            // 
            this._sendTableMnu.Name = "_sendTableMnu";
            this._sendTableMnu.Size = new System.Drawing.Size(331, 34);
            this._sendTableMnu.Text = "Table...";
            this._sendTableMnu.Click += new System.EventHandler(this.SendTableMnu_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(328, 6);
            this.toolStripSeparator4.Visible = false;
            // 
            // _sendCsvMnu
            // 
            this._sendCsvMnu.Name = "_sendCsvMnu";
            this._sendCsvMnu.Size = new System.Drawing.Size(331, 34);
            this._sendCsvMnu.Text = "CSV text file...";
            this._sendCsvMnu.Visible = false;
            // 
            // _sendExcelMnu
            // 
            this._sendExcelMnu.Name = "_sendExcelMnu";
            this._sendExcelMnu.Size = new System.Drawing.Size(331, 34);
            this._sendExcelMnu.Text = "Microsoft Excel workbook...";
            this._sendExcelMnu.Visible = false;
            // 
            // _rowCountLbl
            // 
            this._rowCountLbl.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._rowCountLbl.Margin = new System.Windows.Forms.Padding(0, 1, 8, 2);
            this._rowCountLbl.Name = "_rowCountLbl";
            this._rowCountLbl.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this._rowCountLbl.Size = new System.Drawing.Size(5, 29);
            // 
            // QueryDocumentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._splitContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "QueryDocumentControl";
            this.Size = new System.Drawing.Size(694, 614);
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
        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.ToolStripButton _prevBtn;
        private System.Windows.Forms.ToolStripButton _nextBtn;
        private System.Windows.Forms.ToolStripLabel _resultSetLbl;
        private System.Windows.Forms.ToolStripLabel _rowCountLbl;
        private System.Windows.Forms.ToolStripDropDownButton _sendToMnu;
        private System.Windows.Forms.ToolStripMenuItem _sendCsvMnu;
        private System.Windows.Forms.ToolStripMenuItem _sendTableMnu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem _sendExcelMnu;
    }
}
