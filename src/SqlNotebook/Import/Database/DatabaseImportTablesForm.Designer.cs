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
            this._importTable = new System.Windows.Forms.TableLayoutPanel();
            this._listBox = new System.Windows.Forms.CheckedListBox();
            this._opsFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._editTableBtn = new System.Windows.Forms.Button();
            this._selectAllBtn = new System.Windows.Forms.Button();
            this._selectNoneBtn = new System.Windows.Forms.Button();
            this._okBtn = new System.Windows.Forms.Button();
            this._cancelBtn = new System.Windows.Forms.Button();
            this._table = new System.Windows.Forms.TableLayoutPanel();
            this._buttonFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._importTablesLabel = new System.Windows.Forms.Label();
            this._methodLabel = new System.Windows.Forms.Label();
            this._buttonFlow2 = new System.Windows.Forms.FlowLayoutPanel();
            this._viewSqlButton = new System.Windows.Forms.Button();
            this._queryFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._addQueryButton = new System.Windows.Forms.Button();
            this._methodFlow.SuspendLayout();
            this._importTable.SuspendLayout();
            this._opsFlow.SuspendLayout();
            this._table.SuspendLayout();
            this._buttonFlow.SuspendLayout();
            this._buttonFlow2.SuspendLayout();
            this._queryFlow.SuspendLayout();
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
            // _importTable
            // 
            this._importTable.BackColor = System.Drawing.Color.White;
            this._importTable.ColumnCount = 2;
            this._table.SetColumnSpan(this._importTable, 2);
            this._importTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._importTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._importTable.Controls.Add(this._listBox, 0, 0);
            this._importTable.Controls.Add(this._opsFlow, 1, 1);
            this._importTable.Controls.Add(this._queryFlow, 0, 1);
            this._importTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this._importTable.Location = new System.Drawing.Point(3, 28);
            this._importTable.Name = "_importTable";
            this._importTable.RowCount = 2;
            this._importTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._importTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._importTable.Size = new System.Drawing.Size(782, 721);
            this._importTable.TabIndex = 13;
            // 
            // _listBox
            // 
            this._listBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._importTable.SetColumnSpan(this._listBox, 2);
            this._listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._listBox.IntegralHeight = false;
            this._listBox.Location = new System.Drawing.Point(3, 3);
            this._listBox.Name = "_listBox";
            this._listBox.Size = new System.Drawing.Size(776, 668);
            this._listBox.TabIndex = 6;
            this._listBox.SelectedIndexChanged += new System.EventHandler(this.ListBox_SelectedIndexChanged);
            // 
            // _opsFlow
            // 
            this._opsFlow.AutoSize = true;
            this._opsFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._opsFlow.Controls.Add(this._editTableBtn);
            this._opsFlow.Controls.Add(this._selectAllBtn);
            this._opsFlow.Controls.Add(this._selectNoneBtn);
            this._opsFlow.Dock = System.Windows.Forms.DockStyle.Right;
            this._opsFlow.Location = new System.Drawing.Point(422, 677);
            this._opsFlow.Name = "_opsFlow";
            this._opsFlow.Size = new System.Drawing.Size(357, 41);
            this._opsFlow.TabIndex = 7;
            this._opsFlow.WrapContents = false;
            // 
            // _renameTableBtn
            // 
            this._editTableBtn.AutoSize = true;
            this._editTableBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this._editTableBtn.Location = new System.Drawing.Point(3, 3);
            this._editTableBtn.Name = "_renameTableBtn";
            this._editTableBtn.Size = new System.Drawing.Size(113, 35);
            this._editTableBtn.TabIndex = 7;
            this._editTableBtn.Text = "&Edit...";
            this._editTableBtn.UseVisualStyleBackColor = true;
            this._editTableBtn.Click += new System.EventHandler(this.EditTableBtn_Click);
            // 
            // _selectAllBtn
            // 
            this._selectAllBtn.AutoSize = true;
            this._selectAllBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this._selectAllBtn.Location = new System.Drawing.Point(122, 3);
            this._selectAllBtn.Name = "_selectAllBtn";
            this._selectAllBtn.Size = new System.Drawing.Size(113, 35);
            this._selectAllBtn.TabIndex = 11;
            this._selectAllBtn.Text = "Select &all";
            this._selectAllBtn.UseVisualStyleBackColor = true;
            this._selectAllBtn.Click += new System.EventHandler(this.SelectAllBtn_Click);
            // 
            // _selectNoneBtn
            // 
            this._selectNoneBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._selectNoneBtn.AutoSize = true;
            this._selectNoneBtn.Location = new System.Drawing.Point(241, 3);
            this._selectNoneBtn.Name = "_selectNoneBtn";
            this._selectNoneBtn.Size = new System.Drawing.Size(113, 35);
            this._selectNoneBtn.TabIndex = 12;
            this._selectNoneBtn.Text = "Select &none";
            this._selectNoneBtn.UseVisualStyleBackColor = true;
            this._selectNoneBtn.Click += new System.EventHandler(this.SelectNoneBtn_Click);
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
            this._table.Controls.Add(this._importTable, 0, 1);
            this._table.Controls.Add(this._buttonFlow, 1, 4);
            this._table.Controls.Add(this._importTablesLabel, 0, 0);
            this._table.Controls.Add(this._methodLabel, 0, 2);
            this._table.Controls.Add(this._buttonFlow2, 0, 4);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 5;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
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
            // _queryFlow
            // 
            this._queryFlow.AutoSize = true;
            this._queryFlow.Controls.Add(this._addQueryButton);
            this._queryFlow.Location = new System.Drawing.Point(3, 677);
            this._queryFlow.Name = "_queryFlow";
            this._queryFlow.Size = new System.Drawing.Size(124, 41);
            this._queryFlow.TabIndex = 8;
            // 
            // _addQueryButton
            // 
            this._addQueryButton.AutoSize = true;
            this._addQueryButton.Location = new System.Drawing.Point(3, 3);
            this._addQueryButton.Name = "_addQueryButton";
            this._addQueryButton.Size = new System.Drawing.Size(118, 35);
            this._addQueryButton.TabIndex = 0;
            this._addQueryButton.Text = "Add &query...";
            this._addQueryButton.UseVisualStyleBackColor = true;
            this._addQueryButton.Click += new System.EventHandler(this.AddQueryButton_Click);
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
            this._importTable.ResumeLayout(false);
            this._importTable.PerformLayout();
            this._opsFlow.ResumeLayout(false);
            this._opsFlow.PerformLayout();
            this._table.ResumeLayout(false);
            this._table.PerformLayout();
            this._buttonFlow.ResumeLayout(false);
            this._buttonFlow.PerformLayout();
            this._buttonFlow2.ResumeLayout(false);
            this._queryFlow.ResumeLayout(false);
            this._queryFlow.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button _okBtn;
        private System.Windows.Forms.Button _cancelBtn;
        private System.Windows.Forms.CheckedListBox _listBox;
        private System.Windows.Forms.Button _editTableBtn;
        private System.Windows.Forms.RadioButton _methodLinkRad;
        private System.Windows.Forms.RadioButton _methodCopyRad;
        private System.Windows.Forms.Button _selectNoneBtn;
        private System.Windows.Forms.Button _selectAllBtn;
        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow;
        private System.Windows.Forms.TableLayoutPanel _importTable;
        private System.Windows.Forms.FlowLayoutPanel _opsFlow;
        private System.Windows.Forms.FlowLayoutPanel _methodFlow;
        private System.Windows.Forms.Label _importTablesLabel;
        private System.Windows.Forms.Label _methodLabel;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow2;
        private System.Windows.Forms.Button _viewSqlButton;
        private System.Windows.Forms.FlowLayoutPanel _queryFlow;
        private System.Windows.Forms.Button _addQueryButton;
    }
}