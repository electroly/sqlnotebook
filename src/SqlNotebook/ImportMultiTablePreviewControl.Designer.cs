namespace SqlNotebook {
    partial class ImportMultiTablePreviewControl {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportMultiTablePreviewControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this._splitter = new System.Windows.Forms.SplitContainer();
            this._tablesLst = new System.Windows.Forms.ListView();
            this._imageList = new System.Windows.Forms.ImageList(this.components);
            this._spacerPanel = new System.Windows.Forms.Panel();
            this._previewGrid = new System.Windows.Forms.DataGridView();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._generatePreviewBtn = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this._splitter)).BeginInit();
            this._splitter.Panel1.SuspendLayout();
            this._splitter.Panel2.SuspendLayout();
            this._splitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._previewGrid)).BeginInit();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _splitter
            // 
            this._splitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._splitter.Location = new System.Drawing.Point(0, 30);
            this._splitter.Name = "_splitter";
            // 
            // _splitter.Panel1
            // 
            this._splitter.Panel1.Controls.Add(this._tablesLst);
            this._splitter.Panel1.Controls.Add(this._spacerPanel);
            // 
            // _splitter.Panel2
            // 
            this._splitter.Panel2.Controls.Add(this._previewGrid);
            this._splitter.Size = new System.Drawing.Size(705, 412);
            this._splitter.SplitterDistance = 256;
            this._splitter.SplitterWidth = 11;
            this._splitter.TabIndex = 0;
            // 
            // _tablesLst
            // 
            this._tablesLst.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._tablesLst.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tablesLst.Location = new System.Drawing.Point(0, 10);
            this._tablesLst.MultiSelect = false;
            this._tablesLst.Name = "_tablesLst";
            this._tablesLst.Size = new System.Drawing.Size(256, 402);
            this._tablesLst.SmallImageList = this._imageList;
            this._tablesLst.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this._tablesLst.TabIndex = 0;
            this._tablesLst.UseCompatibleStateImageBehavior = false;
            this._tablesLst.View = System.Windows.Forms.View.List;
            this._tablesLst.SelectedIndexChanged += new System.EventHandler(this.TablesLst_SelectedIndexChanged);
            // 
            // _imageList
            // 
            this._imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_imageList.ImageStream")));
            this._imageList.TransparentColor = System.Drawing.Color.Transparent;
            this._imageList.Images.SetKeyName(0, "table.png");
            // 
            // _spacerPanel
            // 
            this._spacerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._spacerPanel.Location = new System.Drawing.Point(0, 0);
            this._spacerPanel.Name = "_spacerPanel";
            this._spacerPanel.Size = new System.Drawing.Size(256, 10);
            this._spacerPanel.TabIndex = 1;
            // 
            // _previewGrid
            // 
            this._previewGrid.AllowUserToAddRows = false;
            this._previewGrid.AllowUserToDeleteRows = false;
            this._previewGrid.AllowUserToResizeRows = false;
            this._previewGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this._previewGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._previewGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this._previewGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._previewGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._previewGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._previewGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this._previewGrid.Location = new System.Drawing.Point(0, 0);
            this._previewGrid.Name = "_previewGrid";
            this._previewGrid.ReadOnly = true;
            this._previewGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this._previewGrid.RowHeadersVisible = false;
            this._previewGrid.RowHeadersWidth = 25;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._previewGrid.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this._previewGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._previewGrid.ShowCellErrors = false;
            this._previewGrid.ShowCellToolTips = false;
            this._previewGrid.ShowEditingIcon = false;
            this._previewGrid.ShowRowErrors = false;
            this._previewGrid.Size = new System.Drawing.Size(438, 412);
            this._previewGrid.TabIndex = 5;
            // 
            // _toolStrip
            // 
            this._toolStrip.AutoSize = false;
            this._toolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._generatePreviewBtn});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this._toolStrip.ShowItemToolTips = false;
            this._toolStrip.Size = new System.Drawing.Size(705, 30);
            this._toolStrip.Stretch = true;
            this._toolStrip.TabIndex = 1;
            // 
            // _generatePreviewBtn
            // 
            this._generatePreviewBtn.Image = global::SqlNotebook.Properties.Resources.Magnifier;
            this._generatePreviewBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._generatePreviewBtn.Margin = new System.Windows.Forms.Padding(2, 1, 0, 2);
            this._generatePreviewBtn.Name = "_generatePreviewBtn";
            this._generatePreviewBtn.Padding = new System.Windows.Forms.Padding(3);
            this._generatePreviewBtn.Size = new System.Drawing.Size(163, 27);
            this._generatePreviewBtn.Text = "Generate import preview";
            this._generatePreviewBtn.Click += new System.EventHandler(this.GeneratePreviewBtn_Click);
            // 
            // ImportMultiTablePreviewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this._splitter);
            this.Controls.Add(this._toolStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ImportMultiTablePreviewControl";
            this.Size = new System.Drawing.Size(705, 442);
            this._splitter.Panel1.ResumeLayout(false);
            this._splitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitter)).EndInit();
            this._splitter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._previewGrid)).EndInit();
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer _splitter;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _generatePreviewBtn;
        private System.Windows.Forms.ListView _tablesLst;
        private System.Windows.Forms.ImageList _imageList;
        private System.Windows.Forms.DataGridView _previewGrid;
        private System.Windows.Forms.Panel _spacerPanel;
    }
}
