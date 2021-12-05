namespace SqlNotebook.Import.Database {
    partial class DatabaseImportTablesForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseImportTablesForm));
            this._methodFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._methodCopyRad = new System.Windows.Forms.RadioButton();
            this._methodLinkRad = new System.Windows.Forms.RadioButton();
            this._okBtn = new System.Windows.Forms.Button();
            this._cancelBtn = new System.Windows.Forms.Button();
            this._table = new System.Windows.Forms.TableLayoutPanel();
            this._buttonFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._methodLabel = new System.Windows.Forms.Label();
            this._buttonFlow2 = new System.Windows.Forms.FlowLayoutPanel();
            this._viewSqlButton = new System.Windows.Forms.Button();
            this._selectionTable = new System.Windows.Forms.TableLayoutPanel();
            this._selectionButtonsFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._addButton = new System.Windows.Forms.Button();
            this._removeButton = new System.Windows.Forms.Button();
            this._srcPanel = new System.Windows.Forms.Panel();
            this._dstPanel = new System.Windows.Forms.Panel();
            this._srcLabel = new System.Windows.Forms.Label();
            this._dstLabel = new System.Windows.Forms.Label();
            this._dstToolStrip = new System.Windows.Forms.ToolStrip();
            this._editTableButton = new System.Windows.Forms.ToolStripButton();
            this._srcToolStrip = new System.Windows.Forms.ToolStrip();
            this._addQueryButton = new System.Windows.Forms.ToolStripButton();
            this._srcFilterText = new System.Windows.Forms.ToolStripTextBox();
            this._srcFilterClearButton = new System.Windows.Forms.ToolStripButton();
            this._middleLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this._methodFlow.SuspendLayout();
            this._table.SuspendLayout();
            this._buttonFlow.SuspendLayout();
            this._buttonFlow2.SuspendLayout();
            this._selectionTable.SuspendLayout();
            this._selectionButtonsFlow.SuspendLayout();
            this._dstToolStrip.SuspendLayout();
            this._srcToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _methodFlow
            // 
            this._methodFlow.AutoSize = true;
            this._methodFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._methodFlow.BackColor = System.Drawing.Color.White;
            this._table.SetColumnSpan(this._methodFlow, 2);
            this._methodFlow.Controls.Add(this._methodCopyRad);
            this._methodFlow.Controls.Add(this._methodLinkRad);
            this._methodFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this._methodFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._methodFlow.Location = new System.Drawing.Point(3, 780);
            this._methodFlow.Name = "_methodFlow";
            this._methodFlow.Size = new System.Drawing.Size(971, 70);
            this._methodFlow.TabIndex = 2;
            this._methodFlow.WrapContents = false;
            // 
            // _methodCopyRad
            // 
            this._methodCopyRad.AutoSize = true;
            this._methodCopyRad.Checked = true;
            this._methodCopyRad.Location = new System.Drawing.Point(3, 3);
            this._methodCopyRad.Name = "_methodCopyRad";
            this._methodCopyRad.Size = new System.Drawing.Size(426, 29);
            this._methodCopyRad.TabIndex = 1;
            this._methodCopyRad.TabStop = true;
            this._methodCopyRad.Text = "&Copy source data into notebook (recommended)";
            this._methodCopyRad.UseVisualStyleBackColor = true;
            // 
            // _methodLinkRad
            // 
            this._methodLinkRad.AutoSize = true;
            this._methodLinkRad.Location = new System.Drawing.Point(3, 38);
            this._methodLinkRad.Name = "_methodLinkRad";
            this._methodLinkRad.Size = new System.Drawing.Size(236, 29);
            this._methodLinkRad.TabIndex = 0;
            this._methodLinkRad.Text = "&Live database connection";
            this._methodLinkRad.UseVisualStyleBackColor = true;
            // 
            // _okBtn
            // 
            this._okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._okBtn.AutoSize = true;
            this._okBtn.Location = new System.Drawing.Point(3, 3);
            this._okBtn.Name = "_okBtn";
            this._okBtn.Size = new System.Drawing.Size(88, 35);
            this._okBtn.TabIndex = 3;
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
            this._cancelBtn.TabIndex = 4;
            this._cancelBtn.Text = "Cancel";
            this._cancelBtn.UseVisualStyleBackColor = true;
            // 
            // _table
            // 
            this._table.ColumnCount = 2;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._table.Controls.Add(this._methodFlow, 0, 3);
            this._table.Controls.Add(this._buttonFlow, 1, 4);
            this._table.Controls.Add(this._methodLabel, 0, 2);
            this._table.Controls.Add(this._buttonFlow2, 0, 4);
            this._table.Controls.Add(this._selectionTable, 0, 1);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 5;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._table.Size = new System.Drawing.Size(977, 900);
            this._table.TabIndex = 6;
            // 
            // _buttonFlow
            // 
            this._buttonFlow.AutoSize = true;
            this._buttonFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._buttonFlow.Controls.Add(this._okBtn);
            this._buttonFlow.Controls.Add(this._cancelBtn);
            this._buttonFlow.Dock = System.Windows.Forms.DockStyle.Right;
            this._buttonFlow.Location = new System.Drawing.Point(786, 856);
            this._buttonFlow.Name = "_buttonFlow";
            this._buttonFlow.Size = new System.Drawing.Size(188, 41);
            this._buttonFlow.TabIndex = 6;
            this._buttonFlow.WrapContents = false;
            // 
            // _methodLabel
            // 
            this._methodLabel.AutoSize = true;
            this._table.SetColumnSpan(this._methodLabel, 2);
            this._methodLabel.Location = new System.Drawing.Point(3, 752);
            this._methodLabel.Name = "_methodLabel";
            this._methodLabel.Size = new System.Drawing.Size(75, 25);
            this._methodLabel.TabIndex = 14;
            this._methodLabel.Text = "Method";
            // 
            // _buttonFlow2
            // 
            this._buttonFlow2.AutoSize = true;
            this._buttonFlow2.Controls.Add(this._viewSqlButton);
            this._buttonFlow2.Location = new System.Drawing.Point(3, 856);
            this._buttonFlow2.Name = "_buttonFlow2";
            this._buttonFlow2.Size = new System.Drawing.Size(118, 40);
            this._buttonFlow2.TabIndex = 15;
            // 
            // _viewSqlButton
            // 
            this._viewSqlButton.Location = new System.Drawing.Point(3, 3);
            this._viewSqlButton.Name = "_viewSqlButton";
            this._viewSqlButton.Size = new System.Drawing.Size(112, 34);
            this._viewSqlButton.TabIndex = 0;
            this._viewSqlButton.Text = "View SQL";
            this._viewSqlButton.UseVisualStyleBackColor = true;
            this._viewSqlButton.Click += new System.EventHandler(this.ViewSqlButton_Click);
            // 
            // _selectionTable
            // 
            this._selectionTable.BackColor = System.Drawing.Color.White;
            this._selectionTable.ColumnCount = 3;
            this._table.SetColumnSpan(this._selectionTable, 2);
            this._selectionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._selectionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._selectionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._selectionTable.Controls.Add(this._selectionButtonsFlow, 1, 3);
            this._selectionTable.Controls.Add(this._srcPanel, 0, 2);
            this._selectionTable.Controls.Add(this._dstPanel, 2, 2);
            this._selectionTable.Controls.Add(this._srcLabel, 0, 0);
            this._selectionTable.Controls.Add(this._dstLabel, 2, 0);
            this._selectionTable.Controls.Add(this._dstToolStrip, 2, 1);
            this._selectionTable.Controls.Add(this._srcToolStrip, 0, 1);
            this._selectionTable.Controls.Add(this._middleLabel, 1, 0);
            this._selectionTable.Controls.Add(this.panel1, 1, 1);
            this._selectionTable.Controls.Add(this.panel2, 1, 2);
            this._selectionTable.Controls.Add(this.panel3, 1, 4);
            this._selectionTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this._selectionTable.Location = new System.Drawing.Point(3, 3);
            this._selectionTable.Name = "_selectionTable";
            this._selectionTable.RowCount = 5;
            this._selectionTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._selectionTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._selectionTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._selectionTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._selectionTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._selectionTable.Size = new System.Drawing.Size(971, 746);
            this._selectionTable.TabIndex = 19;
            // 
            // _selectionButtonsFlow
            // 
            this._selectionButtonsFlow.AutoSize = true;
            this._selectionButtonsFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._selectionButtonsFlow.Controls.Add(this._addButton);
            this._selectionButtonsFlow.Controls.Add(this._removeButton);
            this._selectionButtonsFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._selectionButtonsFlow.Location = new System.Drawing.Point(426, 362);
            this._selectionButtonsFlow.Name = "_selectionButtonsFlow";
            this._selectionButtonsFlow.Size = new System.Drawing.Size(118, 80);
            this._selectionButtonsFlow.TabIndex = 0;
            this._selectionButtonsFlow.WrapContents = false;
            // 
            // _addButton
            // 
            this._addButton.Enabled = false;
            this._addButton.Location = new System.Drawing.Point(3, 3);
            this._addButton.Name = "_addButton";
            this._addButton.Size = new System.Drawing.Size(112, 34);
            this._addButton.TabIndex = 0;
            this._addButton.Text = "Add 🠖";
            this._addButton.UseVisualStyleBackColor = true;
            this._addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // _removeButton
            // 
            this._removeButton.Enabled = false;
            this._removeButton.Location = new System.Drawing.Point(3, 43);
            this._removeButton.Name = "_removeButton";
            this._removeButton.Size = new System.Drawing.Size(112, 34);
            this._removeButton.TabIndex = 1;
            this._removeButton.Text = "🠔 Remove";
            this._removeButton.UseVisualStyleBackColor = true;
            this._removeButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // _srcPanel
            // 
            this._srcPanel.BackColor = System.Drawing.Color.White;
            this._srcPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._srcPanel.Location = new System.Drawing.Point(3, 62);
            this._srcPanel.Name = "_srcPanel";
            this._selectionTable.SetRowSpan(this._srcPanel, 3);
            this._srcPanel.Size = new System.Drawing.Size(417, 681);
            this._srcPanel.TabIndex = 1;
            // 
            // _dstPanel
            // 
            this._dstPanel.BackColor = System.Drawing.Color.White;
            this._dstPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dstPanel.Location = new System.Drawing.Point(550, 62);
            this._dstPanel.Name = "_dstPanel";
            this._selectionTable.SetRowSpan(this._dstPanel, 3);
            this._dstPanel.Size = new System.Drawing.Size(418, 681);
            this._dstPanel.TabIndex = 2;
            // 
            // _srcLabel
            // 
            this._srcLabel.AutoSize = true;
            this._srcLabel.Location = new System.Drawing.Point(3, 0);
            this._srcLabel.Name = "_srcLabel";
            this._srcLabel.Size = new System.Drawing.Size(153, 25);
            this._srcLabel.TabIndex = 3;
            this._srcLabel.Text = "Source (database)";
            // 
            // _dstLabel
            // 
            this._dstLabel.AutoSize = true;
            this._dstLabel.Location = new System.Drawing.Point(550, 0);
            this._dstLabel.Name = "_dstLabel";
            this._dstLabel.Size = new System.Drawing.Size(195, 25);
            this._dstLabel.TabIndex = 4;
            this._dstLabel.Text = "Destination (notebook)";
            // 
            // _dstToolStrip
            // 
            this._dstToolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dstToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._dstToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this._dstToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._editTableButton});
            this._dstToolStrip.Location = new System.Drawing.Point(547, 25);
            this._dstToolStrip.Name = "_dstToolStrip";
            this._dstToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this._dstToolStrip.ShowItemToolTips = false;
            this._dstToolStrip.Size = new System.Drawing.Size(424, 34);
            this._dstToolStrip.Stretch = true;
            this._dstToolStrip.TabIndex = 16;
            this._dstToolStrip.Text = "toolStrip1";
            // 
            // _editTableButton
            // 
            this._editTableButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._editTableButton.Name = "_editTableButton";
            this._editTableButton.Size = new System.Drawing.Size(58, 29);
            this._editTableButton.Text = "&Edit...";
            this._editTableButton.Click += new System.EventHandler(this.EditTableBtn_Click);
            // 
            // _srcToolStrip
            // 
            this._srcToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._srcToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this._srcToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._addQueryButton,
            this._srcFilterText,
            this._srcFilterClearButton});
            this._srcToolStrip.Location = new System.Drawing.Point(0, 25);
            this._srcToolStrip.Name = "_srcToolStrip";
            this._srcToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this._srcToolStrip.Size = new System.Drawing.Size(423, 34);
            this._srcToolStrip.TabIndex = 17;
            // 
            // _addQueryButton
            // 
            this._addQueryButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._addQueryButton.Name = "_addQueryButton";
            this._addQueryButton.Size = new System.Drawing.Size(76, 29);
            this._addQueryButton.Text = "&Query...";
            this._addQueryButton.ToolTipText = "Import data from a remote SQL query.";
            this._addQueryButton.Click += new System.EventHandler(this.AddQueryButton_Click);
            // 
            // _srcFilterText
            // 
            this._srcFilterText.Name = "_srcFilterText";
            this._srcFilterText.Size = new System.Drawing.Size(100, 34);
            this._srcFilterText.TextChanged += new System.EventHandler(this.SrcFilterText_TextChanged);
            // 
            // _srcFilterClearButton
            // 
            this._srcFilterClearButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._srcFilterClearButton.Image = ((System.Drawing.Image)(resources.GetObject("_srcFilterClearButton.Image")));
            this._srcFilterClearButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._srcFilterClearButton.Name = "_srcFilterClearButton";
            this._srcFilterClearButton.Size = new System.Drawing.Size(34, 29);
            this._srcFilterClearButton.Text = "Clear search terms";
            this._srcFilterClearButton.Click += new System.EventHandler(this.SrcFilterClearButton_Click);
            // 
            // _middleLabel
            // 
            this._middleLabel.AutoSize = true;
            this._middleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._middleLabel.Location = new System.Drawing.Point(426, 0);
            this._middleLabel.Name = "_middleLabel";
            this._middleLabel.Size = new System.Drawing.Size(118, 25);
            this._middleLabel.TabIndex = 18;
            this._middleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(423, 25);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(124, 34);
            this.panel1.TabIndex = 19;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.SelectionTable_Paint);
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(423, 59);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(124, 300);
            this.panel2.TabIndex = 20;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.SelectionTable_Paint);
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(423, 445);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(124, 301);
            this.panel3.TabIndex = 21;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.SelectionTable_Paint);
            // 
            // DatabaseImportTablesForm
            // 
            this.AcceptButton = this._okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelBtn;
            this.ClientSize = new System.Drawing.Size(977, 900);
            this.Controls.Add(this._table);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(321, 379);
            this.Name = "DatabaseImportTablesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Database Import";
            this._methodFlow.ResumeLayout(false);
            this._methodFlow.PerformLayout();
            this._table.ResumeLayout(false);
            this._table.PerformLayout();
            this._buttonFlow.ResumeLayout(false);
            this._buttonFlow.PerformLayout();
            this._buttonFlow2.ResumeLayout(false);
            this._selectionTable.ResumeLayout(false);
            this._selectionTable.PerformLayout();
            this._selectionButtonsFlow.ResumeLayout(false);
            this._dstToolStrip.ResumeLayout(false);
            this._dstToolStrip.PerformLayout();
            this._srcToolStrip.ResumeLayout(false);
            this._srcToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button _okBtn;
        private System.Windows.Forms.Button _cancelBtn;
        private System.Windows.Forms.RadioButton _methodLinkRad;
        private System.Windows.Forms.RadioButton _methodCopyRad;
        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow;
        private System.Windows.Forms.FlowLayoutPanel _methodFlow;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow2;
        private System.Windows.Forms.Button _viewSqlButton;
        private System.Windows.Forms.ToolStrip _dstToolStrip;
        private System.Windows.Forms.ToolStripButton _editTableButton;
        private System.Windows.Forms.Label _methodLabel;
        private System.Windows.Forms.TableLayoutPanel _selectionTable;
        private System.Windows.Forms.FlowLayoutPanel _selectionButtonsFlow;
        private System.Windows.Forms.Button _addButton;
        private System.Windows.Forms.Button _removeButton;
        private System.Windows.Forms.Panel _srcPanel;
        private System.Windows.Forms.Panel _dstPanel;
        private System.Windows.Forms.Label _srcLabel;
        private System.Windows.Forms.Label _dstLabel;
        private System.Windows.Forms.ToolStrip _srcToolStrip;
        private System.Windows.Forms.ToolStripTextBox _srcFilterText;
        private System.Windows.Forms.ToolStripButton _srcFilterClearButton;
        private System.Windows.Forms.ToolStripButton _addQueryButton;
        private System.Windows.Forms.Label _middleLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
    }
}