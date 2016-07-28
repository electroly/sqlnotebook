namespace SqlNotebook {
    partial class ImportXlsSheetControl {
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
            System.Windows.Forms.Panel panel3;
            System.Windows.Forms.Panel panel2;
            System.Windows.Forms.Panel panel1;
            this._propGrid = new System.Windows.Forms.PropertyGrid();
            this._bottomSplitContainer = new System.Windows.Forms.SplitContainer();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this._optionsLbl = new System.Windows.Forms.Label();
            this._columnsPanel = new System.Windows.Forms.Panel();
            this._columnsLbl = new System.Windows.Forms.Label();
            this._outerSplitContainer = new System.Windows.Forms.SplitContainer();
            this._previewPanel = new System.Windows.Forms.Panel();
            panel3 = new System.Windows.Forms.Panel();
            panel2 = new System.Windows.Forms.Panel();
            panel1 = new System.Windows.Forms.Panel();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._bottomSplitContainer)).BeginInit();
            this._bottomSplitContainer.Panel1.SuspendLayout();
            this._bottomSplitContainer.Panel2.SuspendLayout();
            this._bottomSplitContainer.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._outerSplitContainer)).BeginInit();
            this._outerSplitContainer.Panel1.SuspendLayout();
            this._outerSplitContainer.Panel2.SuspendLayout();
            this._outerSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel3.Controls.Add(this._propGrid);
            panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            panel3.Location = new System.Drawing.Point(0, 19);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(470, 346);
            panel3.TabIndex = 52;
            // 
            // _propGrid
            // 
            this._propGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._propGrid.HelpBackColor = System.Drawing.Color.White;
            this._propGrid.HelpVisible = false;
            this._propGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this._propGrid.Location = new System.Drawing.Point(0, 0);
            this._propGrid.Name = "_propGrid";
            this._propGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this._propGrid.Size = new System.Drawing.Size(468, 344);
            this._propGrid.TabIndex = 51;
            this._propGrid.ToolbarVisible = false;
            this._propGrid.ViewBackColor = System.Drawing.Color.White;
            this._propGrid.ViewBorderColor = System.Drawing.Color.White;
            // 
            // panel2
            // 
            panel2.Controls.Add(this._bottomSplitContainer);
            panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            panel2.Location = new System.Drawing.Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(990, 365);
            panel2.TabIndex = 50;
            // 
            // _bottomSplitContainer
            // 
            this._bottomSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._bottomSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._bottomSplitContainer.Location = new System.Drawing.Point(0, 0);
            this._bottomSplitContainer.Name = "_bottomSplitContainer";
            // 
            // _bottomSplitContainer.Panel1
            // 
            this._bottomSplitContainer.Panel1.Controls.Add(this.linkLabel1);
            this._bottomSplitContainer.Panel1.Controls.Add(panel3);
            this._bottomSplitContainer.Panel1.Controls.Add(this._optionsLbl);
            // 
            // _bottomSplitContainer.Panel2
            // 
            this._bottomSplitContainer.Panel2.Controls.Add(this._columnsPanel);
            this._bottomSplitContainer.Panel2.Controls.Add(this._columnsLbl);
            this._bottomSplitContainer.Size = new System.Drawing.Size(990, 365);
            this._bottomSplitContainer.SplitterDistance = 470;
            this._bottomSplitContainer.SplitterWidth = 11;
            this._bottomSplitContainer.TabIndex = 49;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(69, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(191, 15);
            this.linkLabel1.TabIndex = 53;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Set cell range from selection above";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _optionsLbl
            // 
            this._optionsLbl.AutoSize = true;
            this._optionsLbl.Dock = System.Windows.Forms.DockStyle.Top;
            this._optionsLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._optionsLbl.Location = new System.Drawing.Point(0, 0);
            this._optionsLbl.Name = "_optionsLbl";
            this._optionsLbl.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this._optionsLbl.Size = new System.Drawing.Size(50, 19);
            this._optionsLbl.TabIndex = 51;
            this._optionsLbl.Text = "Options";
            // 
            // _columnsPanel
            // 
            this._columnsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._columnsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._columnsPanel.Location = new System.Drawing.Point(0, 19);
            this._columnsPanel.Name = "_columnsPanel";
            this._columnsPanel.Size = new System.Drawing.Size(509, 346);
            this._columnsPanel.TabIndex = 48;
            // 
            // _columnsLbl
            // 
            this._columnsLbl.AutoSize = true;
            this._columnsLbl.Dock = System.Windows.Forms.DockStyle.Top;
            this._columnsLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._columnsLbl.Location = new System.Drawing.Point(0, 0);
            this._columnsLbl.Name = "_columnsLbl";
            this._columnsLbl.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this._columnsLbl.Size = new System.Drawing.Size(54, 19);
            this._columnsLbl.TabIndex = 47;
            this._columnsLbl.Text = "Columns";
            // 
            // panel1
            // 
            panel1.Controls.Add(this._outerSplitContainer);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Padding = new System.Windows.Forms.Padding(11);
            panel1.Size = new System.Drawing.Size(1012, 673);
            panel1.TabIndex = 51;
            // 
            // _outerSplitContainer
            // 
            this._outerSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._outerSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this._outerSplitContainer.Location = new System.Drawing.Point(11, 11);
            this._outerSplitContainer.Name = "_outerSplitContainer";
            this._outerSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _outerSplitContainer.Panel1
            // 
            this._outerSplitContainer.Panel1.Controls.Add(this._previewPanel);
            // 
            // _outerSplitContainer.Panel2
            // 
            this._outerSplitContainer.Panel2.Controls.Add(panel2);
            this._outerSplitContainer.Size = new System.Drawing.Size(990, 651);
            this._outerSplitContainer.SplitterDistance = 275;
            this._outerSplitContainer.SplitterWidth = 11;
            this._outerSplitContainer.TabIndex = 50;
            // 
            // _previewPanel
            // 
            this._previewPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._previewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._previewPanel.Location = new System.Drawing.Point(0, 0);
            this._previewPanel.Name = "_previewPanel";
            this._previewPanel.Size = new System.Drawing.Size(990, 275);
            this._previewPanel.TabIndex = 47;
            // 
            // ImportXlsSheetControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ImportXlsSheetControl";
            this.Size = new System.Drawing.Size(1012, 673);
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            this._bottomSplitContainer.Panel1.ResumeLayout(false);
            this._bottomSplitContainer.Panel1.PerformLayout();
            this._bottomSplitContainer.Panel2.ResumeLayout(false);
            this._bottomSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._bottomSplitContainer)).EndInit();
            this._bottomSplitContainer.ResumeLayout(false);
            panel1.ResumeLayout(false);
            this._outerSplitContainer.Panel1.ResumeLayout(false);
            this._outerSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._outerSplitContainer)).EndInit();
            this._outerSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label _columnsLbl;
        private System.Windows.Forms.Panel _columnsPanel;
        private System.Windows.Forms.SplitContainer _bottomSplitContainer;
        private System.Windows.Forms.Panel _previewPanel;
        private System.Windows.Forms.SplitContainer _outerSplitContainer;
        private System.Windows.Forms.PropertyGrid _propGrid;
        private System.Windows.Forms.Label _optionsLbl;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}
