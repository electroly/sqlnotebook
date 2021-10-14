namespace SqlNotebook {
    partial class ImportCsvForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportCsvForm));
            this._buttonFlow2 = new System.Windows.Forms.FlowLayoutPanel();
            this._okBtn = new System.Windows.Forms.Button();
            this._cancelBtn = new System.Windows.Forms.Button();
            this._table = new System.Windows.Forms.TableLayoutPanel();
            this._outerSplit = new System.Windows.Forms.SplitContainer();
            this._originalFileTable = new System.Windows.Forms.TableLayoutPanel();
            this._originalFileLabel = new System.Windows.Forms.Label();
            this._originalFilePanel = new System.Windows.Forms.Panel();
            this._lowerSplit = new System.Windows.Forms.SplitContainer();
            this._optionsTable = new System.Windows.Forms.TableLayoutPanel();
            this._optionsLabel = new System.Windows.Forms.Label();
            this._optionsPanel = new System.Windows.Forms.Panel();
            this._columnsTable = new System.Windows.Forms.TableLayoutPanel();
            this._columnsLabel = new System.Windows.Forms.Label();
            this._columnsPanel = new System.Windows.Forms.Panel();
            this._buttonFlow1 = new System.Windows.Forms.FlowLayoutPanel();
            this._previewButton = new System.Windows.Forms.Button();
            this._buttonFlow2.SuspendLayout();
            this._table.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._outerSplit)).BeginInit();
            this._outerSplit.Panel1.SuspendLayout();
            this._outerSplit.Panel2.SuspendLayout();
            this._outerSplit.SuspendLayout();
            this._originalFileTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._lowerSplit)).BeginInit();
            this._lowerSplit.Panel1.SuspendLayout();
            this._lowerSplit.Panel2.SuspendLayout();
            this._lowerSplit.SuspendLayout();
            this._optionsTable.SuspendLayout();
            this._columnsTable.SuspendLayout();
            this._buttonFlow1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _buttonFlow2
            // 
            this._buttonFlow2.AutoSize = true;
            this._buttonFlow2.Controls.Add(this._okBtn);
            this._buttonFlow2.Controls.Add(this._cancelBtn);
            this._buttonFlow2.Dock = System.Windows.Forms.DockStyle.Right;
            this._buttonFlow2.Location = new System.Drawing.Point(736, 642);
            this._buttonFlow2.Name = "_buttonFlow2";
            this._buttonFlow2.Size = new System.Drawing.Size(188, 41);
            this._buttonFlow2.TabIndex = 0;
            this._buttonFlow2.WrapContents = false;
            // 
            // _okBtn
            // 
            this._okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._okBtn.AutoSize = true;
            this._okBtn.Location = new System.Drawing.Point(3, 3);
            this._okBtn.Name = "_okBtn";
            this._okBtn.Size = new System.Drawing.Size(88, 35);
            this._okBtn.TabIndex = 5;
            this._okBtn.Text = "Import";
            this._okBtn.UseVisualStyleBackColor = true;
            this._okBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // _cancelBtn
            // 
            this._cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelBtn.AutoSize = true;
            this._cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelBtn.Location = new System.Drawing.Point(97, 3);
            this._cancelBtn.Name = "_cancelBtn";
            this._cancelBtn.Size = new System.Drawing.Size(88, 35);
            this._cancelBtn.TabIndex = 6;
            this._cancelBtn.Text = "Cancel";
            this._cancelBtn.UseVisualStyleBackColor = true;
            // 
            // _table
            // 
            this._table.ColumnCount = 2;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._table.Controls.Add(this._buttonFlow2, 1, 1);
            this._table.Controls.Add(this._outerSplit, 0, 0);
            this._table.Controls.Add(this._buttonFlow1, 0, 1);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 2;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.Size = new System.Drawing.Size(927, 686);
            this._table.TabIndex = 11;
            // 
            // _outerSplit
            // 
            this._table.SetColumnSpan(this._outerSplit, 2);
            this._outerSplit.Cursor = System.Windows.Forms.Cursors.VSplit;
            this._outerSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this._outerSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this._outerSplit.Location = new System.Drawing.Point(3, 3);
            this._outerSplit.Name = "_outerSplit";
            this._outerSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _outerSplit.Panel1
            // 
            this._outerSplit.Panel1.Controls.Add(this._originalFileTable);
            // 
            // _outerSplit.Panel2
            // 
            this._outerSplit.Panel2.Controls.Add(this._lowerSplit);
            this._outerSplit.Size = new System.Drawing.Size(921, 633);
            this._outerSplit.SplitterDistance = 307;
            this._outerSplit.TabIndex = 1;
            // 
            // _originalFileTable
            // 
            this._originalFileTable.ColumnCount = 1;
            this._originalFileTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._originalFileTable.Controls.Add(this._originalFileLabel, 0, 0);
            this._originalFileTable.Controls.Add(this._originalFilePanel, 0, 1);
            this._originalFileTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this._originalFileTable.Location = new System.Drawing.Point(0, 0);
            this._originalFileTable.Name = "_originalFileTable";
            this._originalFileTable.RowCount = 2;
            this._originalFileTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._originalFileTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._originalFileTable.Size = new System.Drawing.Size(921, 307);
            this._originalFileTable.TabIndex = 1;
            // 
            // _originalFileLabel
            // 
            this._originalFileLabel.AutoSize = true;
            this._originalFileLabel.Location = new System.Drawing.Point(3, 0);
            this._originalFileLabel.Name = "_originalFileLabel";
            this._originalFileLabel.Size = new System.Drawing.Size(105, 25);
            this._originalFileLabel.TabIndex = 0;
            this._originalFileLabel.Text = "Original File";
            // 
            // _originalFilePanel
            // 
            this._originalFilePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._originalFilePanel.Location = new System.Drawing.Point(0, 25);
            this._originalFilePanel.Margin = new System.Windows.Forms.Padding(0);
            this._originalFilePanel.Name = "_originalFilePanel";
            this._originalFilePanel.Size = new System.Drawing.Size(921, 282);
            this._originalFilePanel.TabIndex = 1;
            // 
            // _lowerSplit
            // 
            this._lowerSplit.Cursor = System.Windows.Forms.Cursors.VSplit;
            this._lowerSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lowerSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._lowerSplit.Location = new System.Drawing.Point(0, 0);
            this._lowerSplit.Name = "_lowerSplit";
            // 
            // _lowerSplit.Panel1
            // 
            this._lowerSplit.Panel1.Controls.Add(this._optionsTable);
            // 
            // _lowerSplit.Panel2
            // 
            this._lowerSplit.Panel2.Controls.Add(this._columnsTable);
            this._lowerSplit.Size = new System.Drawing.Size(921, 322);
            this._lowerSplit.SplitterDistance = 464;
            this._lowerSplit.TabIndex = 0;
            // 
            // _optionsTable
            // 
            this._optionsTable.ColumnCount = 1;
            this._optionsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._optionsTable.Controls.Add(this._optionsLabel, 0, 0);
            this._optionsTable.Controls.Add(this._optionsPanel, 0, 1);
            this._optionsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this._optionsTable.Location = new System.Drawing.Point(0, 0);
            this._optionsTable.Name = "_optionsTable";
            this._optionsTable.RowCount = 2;
            this._optionsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._optionsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._optionsTable.Size = new System.Drawing.Size(464, 322);
            this._optionsTable.TabIndex = 0;
            // 
            // _optionsLabel
            // 
            this._optionsLabel.AutoSize = true;
            this._optionsLabel.Location = new System.Drawing.Point(3, 0);
            this._optionsLabel.Name = "_optionsLabel";
            this._optionsLabel.Size = new System.Drawing.Size(76, 25);
            this._optionsLabel.TabIndex = 0;
            this._optionsLabel.Text = "Options";
            // 
            // _optionsPanel
            // 
            this._optionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._optionsPanel.Location = new System.Drawing.Point(0, 25);
            this._optionsPanel.Margin = new System.Windows.Forms.Padding(0);
            this._optionsPanel.Name = "_optionsPanel";
            this._optionsPanel.Size = new System.Drawing.Size(464, 297);
            this._optionsPanel.TabIndex = 1;
            // 
            // _columnsTable
            // 
            this._columnsTable.ColumnCount = 1;
            this._columnsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._columnsTable.Controls.Add(this._columnsLabel, 0, 0);
            this._columnsTable.Controls.Add(this._columnsPanel, 0, 1);
            this._columnsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this._columnsTable.Location = new System.Drawing.Point(0, 0);
            this._columnsTable.Name = "_columnsTable";
            this._columnsTable.RowCount = 2;
            this._columnsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._columnsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._columnsTable.Size = new System.Drawing.Size(453, 322);
            this._columnsTable.TabIndex = 0;
            // 
            // _columnsLabel
            // 
            this._columnsLabel.AutoSize = true;
            this._columnsLabel.Location = new System.Drawing.Point(3, 0);
            this._columnsLabel.Name = "_columnsLabel";
            this._columnsLabel.Size = new System.Drawing.Size(82, 25);
            this._columnsLabel.TabIndex = 0;
            this._columnsLabel.Text = "Columns";
            // 
            // _columnsPanel
            // 
            this._columnsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._columnsPanel.Location = new System.Drawing.Point(0, 25);
            this._columnsPanel.Margin = new System.Windows.Forms.Padding(0);
            this._columnsPanel.Name = "_columnsPanel";
            this._columnsPanel.Size = new System.Drawing.Size(453, 297);
            this._columnsPanel.TabIndex = 1;
            // 
            // _buttonFlow1
            // 
            this._buttonFlow1.AutoSize = true;
            this._buttonFlow1.Controls.Add(this._previewButton);
            this._buttonFlow1.Location = new System.Drawing.Point(3, 642);
            this._buttonFlow1.Name = "_buttonFlow1";
            this._buttonFlow1.Size = new System.Drawing.Size(118, 41);
            this._buttonFlow1.TabIndex = 2;
            // 
            // _previewButton
            // 
            this._previewButton.AutoSize = true;
            this._previewButton.Location = new System.Drawing.Point(3, 3);
            this._previewButton.Name = "_previewButton";
            this._previewButton.Size = new System.Drawing.Size(112, 35);
            this._previewButton.TabIndex = 3;
            this._previewButton.Text = "Preview";
            this._previewButton.UseVisualStyleBackColor = true;
            this._previewButton.Click += new System.EventHandler(this.PreviewButton_Click);
            // 
            // ImportCsvForm
            // 
            this.AcceptButton = this._okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelBtn;
            this.ClientSize = new System.Drawing.Size(927, 686);
            this.Controls.Add(this._table);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(848, 725);
            this.Name = "ImportCsvForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CSV Import";
            this._buttonFlow2.ResumeLayout(false);
            this._buttonFlow2.PerformLayout();
            this._table.ResumeLayout(false);
            this._table.PerformLayout();
            this._outerSplit.Panel1.ResumeLayout(false);
            this._outerSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._outerSplit)).EndInit();
            this._outerSplit.ResumeLayout(false);
            this._originalFileTable.ResumeLayout(false);
            this._originalFileTable.PerformLayout();
            this._lowerSplit.Panel1.ResumeLayout(false);
            this._lowerSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._lowerSplit)).EndInit();
            this._lowerSplit.ResumeLayout(false);
            this._optionsTable.ResumeLayout(false);
            this._optionsTable.PerformLayout();
            this._columnsTable.ResumeLayout(false);
            this._columnsTable.PerformLayout();
            this._buttonFlow1.ResumeLayout(false);
            this._buttonFlow1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel _buttonFlow2;
        private System.Windows.Forms.Button _okBtn;
        private System.Windows.Forms.Button _cancelBtn;
        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.SplitContainer _outerSplit;
        private System.Windows.Forms.SplitContainer _lowerSplit;
        private System.Windows.Forms.TableLayoutPanel _optionsTable;
        private System.Windows.Forms.Label _optionsLabel;
        private System.Windows.Forms.TableLayoutPanel _columnsTable;
        private System.Windows.Forms.Label _columnsLabel;
        private System.Windows.Forms.Panel _optionsPanel;
        private System.Windows.Forms.Panel _columnsPanel;
        private System.Windows.Forms.TableLayoutPanel _originalFileTable;
        private System.Windows.Forms.Label _originalFileLabel;
        private System.Windows.Forms.Panel _originalFilePanel;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow1;
        private System.Windows.Forms.Button _previewButton;
    }
}