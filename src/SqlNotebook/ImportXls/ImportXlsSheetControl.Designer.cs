namespace SqlNotebook.ImportXls {
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
            this._propPanel = new System.Windows.Forms.Panel();
            this._propGrid = new System.Windows.Forms.PropertyGrid();
            this._optionsHeaderFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._optionsLbl = new System.Windows.Forms.Label();
            this._setCellRangeLnk = new System.Windows.Forms.LinkLabel();
            this._bottomSplitContainer = new System.Windows.Forms.SplitContainer();
            this._columnsPanel = new System.Windows.Forms.Panel();
            this._columnsLbl = new System.Windows.Forms.Label();
            this._outerSplitContainer = new System.Windows.Forms.SplitContainer();
            this._previewPanel = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this._top1000RowLbl = new System.Windows.Forms.Label();
            panel3 = new System.Windows.Forms.Panel();
            panel2 = new System.Windows.Forms.Panel();
            panel1 = new System.Windows.Forms.Panel();
            panel3.SuspendLayout();
            this._propPanel.SuspendLayout();
            this._optionsHeaderFlow.SuspendLayout();
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
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            panel3.Controls.Add(this._propPanel);
            panel3.Controls.Add(this._optionsHeaderFlow);
            panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            panel3.Location = new System.Drawing.Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(470, 575);
            panel3.TabIndex = 52;
            // 
            // _propPanel
            // 
            this._propPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._propPanel.Controls.Add(this._propGrid);
            this._propPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._propPanel.Location = new System.Drawing.Point(0, 29);
            this._propPanel.Name = "_propPanel";
            this._propPanel.Size = new System.Drawing.Size(470, 546);
            this._propPanel.TabIndex = 55;
            // 
            // _propGrid
            // 
            this._propGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._propGrid.HelpBackColor = System.Drawing.Color.White;
            this._propGrid.HelpVisible = false;
            this._propGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this._propGrid.Location = new System.Drawing.Point(0, 0);
            this._propGrid.Name = "_propGrid";
            this._propGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this._propGrid.Size = new System.Drawing.Size(468, 544);
            this._propGrid.TabIndex = 51;
            this._propGrid.ToolbarVisible = false;
            this._propGrid.ViewBackColor = System.Drawing.Color.White;
            this._propGrid.ViewBorderColor = System.Drawing.Color.White;
            this._propGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropGrid_PropertyValueChanged);
            // 
            // _optionsHeaderFlow
            // 
            this._optionsHeaderFlow.AutoSize = true;
            this._optionsHeaderFlow.Controls.Add(this._optionsLbl);
            this._optionsHeaderFlow.Controls.Add(this._setCellRangeLnk);
            this._optionsHeaderFlow.Dock = System.Windows.Forms.DockStyle.Top;
            this._optionsHeaderFlow.Location = new System.Drawing.Point(0, 0);
            this._optionsHeaderFlow.Name = "_optionsHeaderFlow";
            this._optionsHeaderFlow.Size = new System.Drawing.Size(470, 29);
            this._optionsHeaderFlow.TabIndex = 54;
            // 
            // _optionsLbl
            // 
            this._optionsLbl.AutoSize = true;
            this._optionsLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._optionsLbl.Location = new System.Drawing.Point(3, 0);
            this._optionsLbl.Name = "_optionsLbl";
            this._optionsLbl.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this._optionsLbl.Size = new System.Drawing.Size(79, 29);
            this._optionsLbl.TabIndex = 51;
            this._optionsLbl.Text = "Options";
            // 
            // _setCellRangeLnk
            // 
            this._setCellRangeLnk.AutoSize = true;
            this._setCellRangeLnk.Enabled = false;
            this._setCellRangeLnk.Location = new System.Drawing.Point(88, 0);
            this._setCellRangeLnk.Name = "_setCellRangeLnk";
            this._setCellRangeLnk.Size = new System.Drawing.Size(289, 25);
            this._setCellRangeLnk.TabIndex = 53;
            this._setCellRangeLnk.TabStop = true;
            this._setCellRangeLnk.Text = "Set cell range from selection above";
            this._setCellRangeLnk.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._setCellRangeLnk.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SetCellRangeLnk_LinkClicked);
            // 
            // panel2
            // 
            panel2.Controls.Add(this._bottomSplitContainer);
            panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            panel2.Location = new System.Drawing.Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(1046, 575);
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
            this._bottomSplitContainer.Panel1.Controls.Add(panel3);
            // 
            // _bottomSplitContainer.Panel2
            // 
            this._bottomSplitContainer.Panel2.Controls.Add(this._columnsPanel);
            this._bottomSplitContainer.Panel2.Controls.Add(this._columnsLbl);
            this._bottomSplitContainer.Size = new System.Drawing.Size(1046, 575);
            this._bottomSplitContainer.SplitterDistance = 470;
            this._bottomSplitContainer.SplitterWidth = 11;
            this._bottomSplitContainer.TabIndex = 49;
            // 
            // _columnsPanel
            // 
            this._columnsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._columnsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._columnsPanel.Location = new System.Drawing.Point(0, 29);
            this._columnsPanel.Name = "_columnsPanel";
            this._columnsPanel.Size = new System.Drawing.Size(565, 546);
            this._columnsPanel.TabIndex = 48;
            // 
            // _columnsLbl
            // 
            this._columnsLbl.AutoSize = true;
            this._columnsLbl.Dock = System.Windows.Forms.DockStyle.Top;
            this._columnsLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._columnsLbl.Location = new System.Drawing.Point(0, 0);
            this._columnsLbl.Name = "_columnsLbl";
            this._columnsLbl.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this._columnsLbl.Size = new System.Drawing.Size(85, 29);
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
            panel1.Size = new System.Drawing.Size(1068, 676);
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
            this._outerSplitContainer.Panel1.Controls.Add(this.flowLayoutPanel1);
            // 
            // _outerSplitContainer.Panel2
            // 
            this._outerSplitContainer.Panel2.Controls.Add(panel2);
            this._outerSplitContainer.Size = new System.Drawing.Size(1046, 654);
            this._outerSplitContainer.SplitterDistance = 68;
            this._outerSplitContainer.SplitterWidth = 11;
            this._outerSplitContainer.TabIndex = 50;
            // 
            // _previewPanel
            // 
            this._previewPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._previewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._previewPanel.Location = new System.Drawing.Point(0, 29);
            this._previewPanel.Name = "_previewPanel";
            this._previewPanel.Size = new System.Drawing.Size(1046, 39);
            this._previewPanel.TabIndex = 47;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this._top1000RowLbl);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1046, 29);
            this.flowLayoutPanel1.TabIndex = 48;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.label1.Size = new System.Drawing.Size(104, 29);
            this.label1.TabIndex = 52;
            this.label1.Text = "Worksheet";
            // 
            // _top1000RowLbl
            // 
            this._top1000RowLbl.AutoSize = true;
            this._top1000RowLbl.Dock = System.Windows.Forms.DockStyle.Top;
            this._top1000RowLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._top1000RowLbl.Location = new System.Drawing.Point(107, 0);
            this._top1000RowLbl.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this._top1000RowLbl.Name = "_top1000RowLbl";
            this._top1000RowLbl.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this._top1000RowLbl.Size = new System.Drawing.Size(210, 29);
            this._top1000RowLbl.TabIndex = 53;
            this._top1000RowLbl.Text = "(showing top 1000 rows)";
            // 
            // ImportXlsSheetControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "ImportXlsSheetControl";
            this.Size = new System.Drawing.Size(1068, 676);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            this._propPanel.ResumeLayout(false);
            this._optionsHeaderFlow.ResumeLayout(false);
            this._optionsHeaderFlow.PerformLayout();
            panel2.ResumeLayout(false);
            this._bottomSplitContainer.Panel1.ResumeLayout(false);
            this._bottomSplitContainer.Panel2.ResumeLayout(false);
            this._bottomSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._bottomSplitContainer)).EndInit();
            this._bottomSplitContainer.ResumeLayout(false);
            panel1.ResumeLayout(false);
            this._outerSplitContainer.Panel1.ResumeLayout(false);
            this._outerSplitContainer.Panel1.PerformLayout();
            this._outerSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._outerSplitContainer)).EndInit();
            this._outerSplitContainer.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
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
        private System.Windows.Forms.LinkLabel _setCellRangeLnk;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label _top1000RowLbl;
        private System.Windows.Forms.FlowLayoutPanel _optionsHeaderFlow;
        private System.Windows.Forms.Panel _propPanel;
    }
}
