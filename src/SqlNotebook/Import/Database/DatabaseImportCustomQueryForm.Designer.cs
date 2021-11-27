namespace SqlNotebook.Import.Database {
    partial class DatabaseImportCustomQueryForm {
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
            this._table = new System.Windows.Forms.TableLayoutPanel();
            this._splitter = new System.Windows.Forms.SplitContainer();
            this._sqlTable = new System.Windows.Forms.TableLayoutPanel();
            this._previewToolStrip = new System.Windows.Forms.ToolStrip();
            this._executeButton = new System.Windows.Forms.ToolStripButton();
            this._sqlLabel = new System.Windows.Forms.Label();
            this._sqlPanel = new System.Windows.Forms.Panel();
            this._previewTable = new System.Windows.Forms.TableLayoutPanel();
            this._previewPanel = new System.Windows.Forms.Panel();
            this._previewLabel = new System.Windows.Forms.Label();
            this._buttonFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._okButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this._importLabel = new System.Windows.Forms.Label();
            this._importFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._targetNameLabel = new System.Windows.Forms.Label();
            this._targetNameText = new System.Windows.Forms.TextBox();
            this._table.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitter)).BeginInit();
            this._splitter.Panel1.SuspendLayout();
            this._splitter.Panel2.SuspendLayout();
            this._splitter.SuspendLayout();
            this._sqlTable.SuspendLayout();
            this._previewToolStrip.SuspendLayout();
            this._previewTable.SuspendLayout();
            this._buttonFlow.SuspendLayout();
            this._importFlow.SuspendLayout();
            this.SuspendLayout();
            // 
            // _table
            // 
            this._table.ColumnCount = 1;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.Controls.Add(this._splitter, 0, 2);
            this._table.Controls.Add(this._buttonFlow, 0, 3);
            this._table.Controls.Add(this._importLabel, 0, 0);
            this._table.Controls.Add(this._importFlow, 0, 1);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 4;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.Size = new System.Drawing.Size(1181, 869);
            this._table.TabIndex = 0;
            // 
            // _splitter
            // 
            this._splitter.Cursor = System.Windows.Forms.Cursors.HSplit;
            this._splitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._splitter.Location = new System.Drawing.Point(0, 68);
            this._splitter.Margin = new System.Windows.Forms.Padding(0);
            this._splitter.Name = "_splitter";
            this._splitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _splitter.Panel1
            // 
            this._splitter.Panel1.Controls.Add(this._sqlTable);
            // 
            // _splitter.Panel2
            // 
            this._splitter.Panel2.Controls.Add(this._previewTable);
            this._splitter.Panel2Collapsed = true;
            this._splitter.Size = new System.Drawing.Size(1181, 755);
            this._splitter.SplitterDistance = 353;
            this._splitter.TabIndex = 0;
            // 
            // _sqlTable
            // 
            this._sqlTable.ColumnCount = 1;
            this._sqlTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._sqlTable.Controls.Add(this._previewToolStrip, 0, 1);
            this._sqlTable.Controls.Add(this._sqlLabel, 0, 0);
            this._sqlTable.Controls.Add(this._sqlPanel, 0, 2);
            this._sqlTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this._sqlTable.Location = new System.Drawing.Point(0, 0);
            this._sqlTable.Name = "_sqlTable";
            this._sqlTable.RowCount = 3;
            this._sqlTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._sqlTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._sqlTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._sqlTable.Size = new System.Drawing.Size(1181, 755);
            this._sqlTable.TabIndex = 0;
            // 
            // _previewToolStrip
            // 
            this._previewToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._previewToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this._previewToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._executeButton});
            this._previewToolStrip.Location = new System.Drawing.Point(0, 25);
            this._previewToolStrip.Name = "_previewToolStrip";
            this._previewToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this._previewToolStrip.Size = new System.Drawing.Size(1181, 34);
            this._previewToolStrip.Stretch = true;
            this._previewToolStrip.TabIndex = 4;
            // 
            // _executeButton
            // 
            this._executeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._executeButton.Name = "_executeButton";
            this._executeButton.Size = new System.Drawing.Size(174, 29);
            this._executeButton.Text = "Execute Preview (F5)";
            this._executeButton.Click += new System.EventHandler(this.PreviewButton_Click);
            // 
            // _sqlLabel
            // 
            this._sqlLabel.AutoSize = true;
            this._sqlLabel.Location = new System.Drawing.Point(3, 0);
            this._sqlLabel.Name = "_sqlLabel";
            this._sqlLabel.Size = new System.Drawing.Size(110, 25);
            this._sqlLabel.TabIndex = 0;
            this._sqlLabel.Text = "Remote SQL";
            // 
            // _sqlPanel
            // 
            this._sqlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._sqlPanel.Location = new System.Drawing.Point(3, 62);
            this._sqlPanel.Name = "_sqlPanel";
            this._sqlPanel.Size = new System.Drawing.Size(1175, 690);
            this._sqlPanel.TabIndex = 1;
            // 
            // _previewTable
            // 
            this._previewTable.BackColor = System.Drawing.Color.White;
            this._previewTable.ColumnCount = 1;
            this._previewTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._previewTable.Controls.Add(this._previewPanel, 0, 1);
            this._previewTable.Controls.Add(this._previewLabel, 0, 0);
            this._previewTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this._previewTable.Location = new System.Drawing.Point(0, 0);
            this._previewTable.Name = "_previewTable";
            this._previewTable.RowCount = 2;
            this._previewTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._previewTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._previewTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._previewTable.Size = new System.Drawing.Size(150, 46);
            this._previewTable.TabIndex = 0;
            // 
            // _previewPanel
            // 
            this._previewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._previewPanel.Location = new System.Drawing.Point(3, 28);
            this._previewPanel.Name = "_previewPanel";
            this._previewPanel.Size = new System.Drawing.Size(144, 15);
            this._previewPanel.TabIndex = 1;
            // 
            // _previewLabel
            // 
            this._previewLabel.AutoSize = true;
            this._previewLabel.Location = new System.Drawing.Point(3, 0);
            this._previewLabel.Name = "_previewLabel";
            this._previewLabel.Size = new System.Drawing.Size(132, 25);
            this._previewLabel.TabIndex = 2;
            this._previewLabel.Text = "Results Preview";
            // 
            // _buttonFlow
            // 
            this._buttonFlow.AutoSize = true;
            this._buttonFlow.Controls.Add(this._okButton);
            this._buttonFlow.Controls.Add(this._cancelButton);
            this._buttonFlow.Dock = System.Windows.Forms.DockStyle.Right;
            this._buttonFlow.Location = new System.Drawing.Point(942, 826);
            this._buttonFlow.Name = "_buttonFlow";
            this._buttonFlow.Size = new System.Drawing.Size(236, 40);
            this._buttonFlow.TabIndex = 1;
            // 
            // _okButton
            // 
            this._okButton.Location = new System.Drawing.Point(3, 3);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(112, 34);
            this._okButton.TabIndex = 0;
            this._okButton.Text = "OK";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // _cancelButton
            // 
            this._cancelButton.Location = new System.Drawing.Point(121, 3);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(112, 34);
            this._cancelButton.TabIndex = 1;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _importLabel
            // 
            this._importLabel.AutoSize = true;
            this._importLabel.Location = new System.Drawing.Point(3, 0);
            this._importLabel.Name = "_importLabel";
            this._importLabel.Size = new System.Drawing.Size(120, 25);
            this._importLabel.TabIndex = 2;
            this._importLabel.Text = "Import Target";
            // 
            // _importFlow
            // 
            this._importFlow.AutoSize = true;
            this._importFlow.BackColor = System.Drawing.Color.White;
            this._importFlow.Controls.Add(this._targetNameLabel);
            this._importFlow.Controls.Add(this._targetNameText);
            this._importFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this._importFlow.Location = new System.Drawing.Point(3, 28);
            this._importFlow.Name = "_importFlow";
            this._importFlow.Size = new System.Drawing.Size(1175, 37);
            this._importFlow.TabIndex = 3;
            this._importFlow.WrapContents = false;
            // 
            // _targetNameLabel
            // 
            this._targetNameLabel.AutoSize = true;
            this._targetNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._targetNameLabel.Location = new System.Drawing.Point(3, 0);
            this._targetNameLabel.Name = "_targetNameLabel";
            this._targetNameLabel.Size = new System.Drawing.Size(140, 37);
            this._targetNameLabel.TabIndex = 0;
            this._targetNameLabel.Text = "Imported &name:";
            this._targetNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _targetNameText
            // 
            this._targetNameText.Location = new System.Drawing.Point(149, 3);
            this._targetNameText.Name = "_targetNameText";
            this._targetNameText.Size = new System.Drawing.Size(150, 31);
            this._targetNameText.TabIndex = 1;
            // 
            // DatabaseImportCustomQueryForm
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(1181, 869);
            this.Controls.Add(this._table);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DatabaseImportCustomQueryForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Custom Query";
            this._table.ResumeLayout(false);
            this._table.PerformLayout();
            this._splitter.Panel1.ResumeLayout(false);
            this._splitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitter)).EndInit();
            this._splitter.ResumeLayout(false);
            this._sqlTable.ResumeLayout(false);
            this._sqlTable.PerformLayout();
            this._previewToolStrip.ResumeLayout(false);
            this._previewToolStrip.PerformLayout();
            this._previewTable.ResumeLayout(false);
            this._previewTable.PerformLayout();
            this._buttonFlow.ResumeLayout(false);
            this._importFlow.ResumeLayout(false);
            this._importFlow.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.SplitContainer _splitter;
        private System.Windows.Forms.TableLayoutPanel _previewTable;
        private System.Windows.Forms.Panel _previewPanel;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.TableLayoutPanel _sqlTable;
        private System.Windows.Forms.Label _sqlLabel;
        private System.Windows.Forms.Panel _sqlPanel;
        private System.Windows.Forms.Label _previewLabel;
        private System.Windows.Forms.ToolStrip _previewToolStrip;
        private System.Windows.Forms.ToolStripButton _executeButton;
        private System.Windows.Forms.Label _importLabel;
        private System.Windows.Forms.FlowLayoutPanel _importFlow;
        private System.Windows.Forms.Label _targetNameLabel;
        private System.Windows.Forms.TextBox _targetNameText;
    }
}