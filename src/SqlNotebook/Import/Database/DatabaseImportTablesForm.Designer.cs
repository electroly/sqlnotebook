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
            this._methodFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._methodCopyRad = new System.Windows.Forms.RadioButton();
            this._methodLinkRad = new System.Windows.Forms.RadioButton();
            this._listBox = new System.Windows.Forms.CheckedListBox();
            this._okBtn = new System.Windows.Forms.Button();
            this._cancelBtn = new System.Windows.Forms.Button();
            this._table = new System.Windows.Forms.TableLayoutPanel();
            this._buttonFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._importTablesLabel = new System.Windows.Forms.Label();
            this._methodLabel = new System.Windows.Forms.Label();
            this._buttonFlow2 = new System.Windows.Forms.FlowLayoutPanel();
            this._viewSqlButton = new System.Windows.Forms.Button();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._addQueryButton = new System.Windows.Forms.ToolStripButton();
            this._editTableButton = new System.Windows.Forms.ToolStripButton();
            this._selectAllButton = new System.Windows.Forms.ToolStripButton();
            this._selectNoneButton = new System.Windows.Forms.ToolStripButton();
            this._listPanel = new System.Windows.Forms.Panel();
            this._methodFlow.SuspendLayout();
            this._table.SuspendLayout();
            this._buttonFlow.SuspendLayout();
            this._buttonFlow2.SuspendLayout();
            this._toolStrip.SuspendLayout();
            this._listPanel.SuspendLayout();
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
            this._methodFlow.Size = new System.Drawing.Size(782, 70);
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
            // _listBox
            // 
            this._listBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._listBox.IntegralHeight = false;
            this._listBox.Location = new System.Drawing.Point(0, 0);
            this._listBox.Margin = new System.Windows.Forms.Padding(0);
            this._listBox.Name = "_listBox";
            this._listBox.Size = new System.Drawing.Size(788, 693);
            this._listBox.TabIndex = 6;
            this._listBox.SelectedIndexChanged += new System.EventHandler(this.ListBox_SelectedIndexChanged);
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
            this._table.Controls.Add(this._methodFlow, 0, 4);
            this._table.Controls.Add(this._buttonFlow, 1, 5);
            this._table.Controls.Add(this._importTablesLabel, 0, 0);
            this._table.Controls.Add(this._methodLabel, 0, 3);
            this._table.Controls.Add(this._buttonFlow2, 0, 5);
            this._table.Controls.Add(this._toolStrip, 0, 1);
            this._table.Controls.Add(this._listPanel, 0, 2);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 6;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._table.Size = new System.Drawing.Size(788, 900);
            this._table.TabIndex = 6;
            // 
            // _buttonFlow
            // 
            this._buttonFlow.AutoSize = true;
            this._buttonFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._buttonFlow.Controls.Add(this._okBtn);
            this._buttonFlow.Controls.Add(this._cancelBtn);
            this._buttonFlow.Dock = System.Windows.Forms.DockStyle.Right;
            this._buttonFlow.Location = new System.Drawing.Point(597, 856);
            this._buttonFlow.Name = "_buttonFlow";
            this._buttonFlow.Size = new System.Drawing.Size(188, 41);
            this._buttonFlow.TabIndex = 6;
            this._buttonFlow.WrapContents = false;
            // 
            // _importTablesLabel
            // 
            this._importTablesLabel.AutoSize = true;
            this._table.SetColumnSpan(this._importTablesLabel, 2);
            this._importTablesLabel.Location = new System.Drawing.Point(3, 0);
            this._importTablesLabel.Name = "_importTablesLabel";
            this._importTablesLabel.Size = new System.Drawing.Size(120, 25);
            this._importTablesLabel.TabIndex = 7;
            this._importTablesLabel.Text = "Import Tables";
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
            // _toolStrip
            // 
            this._table.SetColumnSpan(this._toolStrip, 2);
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._addQueryButton,
            this._editTableButton,
            this._selectAllButton,
            this._selectNoneButton});
            this._toolStrip.Location = new System.Drawing.Point(0, 25);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this._toolStrip.Size = new System.Drawing.Size(788, 34);
            this._toolStrip.Stretch = true;
            this._toolStrip.TabIndex = 16;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _addQueryButton
            // 
            this._addQueryButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._addQueryButton.Name = "_addQueryButton";
            this._addQueryButton.Size = new System.Drawing.Size(112, 29);
            this._addQueryButton.Text = "Add &query...";
            this._addQueryButton.Click += new System.EventHandler(this.AddQueryButton_Click);
            // 
            // _editTableButton
            // 
            this._editTableButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._editTableButton.Name = "_editTableButton";
            this._editTableButton.Size = new System.Drawing.Size(58, 29);
            this._editTableButton.Text = "&Edit...";
            this._editTableButton.Click += new System.EventHandler(this.EditTableBtn_Click);
            // 
            // _selectAllButton
            // 
            this._selectAllButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._selectAllButton.Name = "_selectAllButton";
            this._selectAllButton.Size = new System.Drawing.Size(84, 29);
            this._selectAllButton.Text = "Select &all";
            this._selectAllButton.Click += new System.EventHandler(this.SelectAllBtn_Click);
            // 
            // _selectNoneButton
            // 
            this._selectNoneButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._selectNoneButton.Name = "_selectNoneButton";
            this._selectNoneButton.Size = new System.Drawing.Size(107, 29);
            this._selectNoneButton.Text = "Select &none";
            this._selectNoneButton.Click += new System.EventHandler(this.SelectNoneBtn_Click);
            // 
            // _listPanel
            // 
            this._listPanel.BackColor = System.Drawing.SystemColors.Window;
            this._table.SetColumnSpan(this._listPanel, 2);
            this._listPanel.Controls.Add(this._listBox);
            this._listPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._listPanel.Location = new System.Drawing.Point(0, 59);
            this._listPanel.Margin = new System.Windows.Forms.Padding(0);
            this._listPanel.Name = "_listPanel";
            this._listPanel.Size = new System.Drawing.Size(788, 693);
            this._listPanel.TabIndex = 17;
            // 
            // DatabaseImportTablesForm
            // 
            this.AcceptButton = this._okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelBtn;
            this.ClientSize = new System.Drawing.Size(788, 900);
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
            this.Text = "Import";
            this._methodFlow.ResumeLayout(false);
            this._methodFlow.PerformLayout();
            this._table.ResumeLayout(false);
            this._table.PerformLayout();
            this._buttonFlow.ResumeLayout(false);
            this._buttonFlow.PerformLayout();
            this._buttonFlow2.ResumeLayout(false);
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this._listPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button _okBtn;
        private System.Windows.Forms.Button _cancelBtn;
        private System.Windows.Forms.CheckedListBox _listBox;
        private System.Windows.Forms.RadioButton _methodLinkRad;
        private System.Windows.Forms.RadioButton _methodCopyRad;
        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow;
        private System.Windows.Forms.FlowLayoutPanel _methodFlow;
        private System.Windows.Forms.Label _importTablesLabel;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow2;
        private System.Windows.Forms.Button _viewSqlButton;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _addQueryButton;
        private System.Windows.Forms.ToolStripButton _editTableButton;
        private System.Windows.Forms.ToolStripButton _selectAllButton;
        private System.Windows.Forms.ToolStripButton _selectNoneButton;
        private System.Windows.Forms.Label _methodLabel;
        private System.Windows.Forms.Panel _listPanel;
    }
}