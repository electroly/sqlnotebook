namespace SqlNotebook {
    partial class ImportPreviewForm {
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
            this._methodGrp = new System.Windows.Forms.GroupBox();
            this._methodFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._methodLinkRad = new System.Windows.Forms.RadioButton();
            this._methodCopyRad = new System.Windows.Forms.RadioButton();
            this._tablesGrp = new System.Windows.Forms.GroupBox();
            this._importTable = new System.Windows.Forms.TableLayoutPanel();
            this._listBox = new System.Windows.Forms.CheckedListBox();
            this._opsFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._renameTableBtn = new System.Windows.Forms.Button();
            this._selectAllBtn = new System.Windows.Forms.Button();
            this._selectNoneBtn = new System.Windows.Forms.Button();
            this._okBtn = new System.Windows.Forms.Button();
            this._cancelBtn = new System.Windows.Forms.Button();
            this._csvHeaderChk = new System.Windows.Forms.CheckBox();
            this._table = new System.Windows.Forms.TableLayoutPanel();
            this._buttonFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._methodGrp.SuspendLayout();
            this._methodFlow.SuspendLayout();
            this._tablesGrp.SuspendLayout();
            this._importTable.SuspendLayout();
            this._opsFlow.SuspendLayout();
            this._table.SuspendLayout();
            this._buttonFlow.SuspendLayout();
            this.SuspendLayout();
            // 
            // _methodGrp
            // 
            this._methodGrp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._methodGrp.AutoSize = true;
            this._methodGrp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._methodGrp.Controls.Add(this._methodFlow);
            this._methodGrp.Location = new System.Drawing.Point(3, 750);
            this._methodGrp.Name = "_methodGrp";
            this._methodGrp.Size = new System.Drawing.Size(782, 100);
            this._methodGrp.TabIndex = 1;
            this._methodGrp.TabStop = false;
            this._methodGrp.Text = "Method";
            // 
            // _methodFlow
            // 
            this._methodFlow.AutoSize = true;
            this._methodFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._methodFlow.Controls.Add(this._methodCopyRad);
            this._methodFlow.Controls.Add(this._methodLinkRad);
            this._methodFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this._methodFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._methodFlow.Location = new System.Drawing.Point(3, 27);
            this._methodFlow.Name = "_methodFlow";
            this._methodFlow.Size = new System.Drawing.Size(776, 70);
            this._methodFlow.TabIndex = 2;
            this._methodFlow.WrapContents = false;
            // 
            // _methodLinkRad
            // 
            this._methodLinkRad.AutoSize = true;
            this._methodLinkRad.Location = new System.Drawing.Point(3, 38);
            this._methodLinkRad.Name = "_methodLinkRad";
            this._methodLinkRad.Size = new System.Drawing.Size(398, 29);
            this._methodLinkRad.TabIndex = 0;
            this._methodLinkRad.Text = "Create a live, read-only &link to the source data";
            this._methodLinkRad.UseVisualStyleBackColor = true;
            // 
            // _methodCopyRad
            // 
            this._methodCopyRad.AutoSize = true;
            this._methodCopyRad.Checked = true;
            this._methodCopyRad.Location = new System.Drawing.Point(3, 3);
            this._methodCopyRad.Name = "_methodCopyRad";
            this._methodCopyRad.Size = new System.Drawing.Size(375, 29);
            this._methodCopyRad.TabIndex = 1;
            this._methodCopyRad.TabStop = true;
            this._methodCopyRad.Text = "&Copy all source data into the notebook file";
            this._methodCopyRad.UseVisualStyleBackColor = true;
            // 
            // _tablesGrp
            // 
            this._tablesGrp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tablesGrp.Controls.Add(this._importTable);
            this._tablesGrp.Location = new System.Drawing.Point(3, 38);
            this._tablesGrp.Name = "_tablesGrp";
            this._tablesGrp.Size = new System.Drawing.Size(782, 706);
            this._tablesGrp.TabIndex = 0;
            this._tablesGrp.TabStop = false;
            this._tablesGrp.Text = "Tables to import";
            // 
            // _importTable
            // 
            this._importTable.ColumnCount = 2;
            this._importTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._importTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._importTable.Controls.Add(this._listBox, 0, 0);
            this._importTable.Controls.Add(this._opsFlow, 1, 0);
            this._importTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this._importTable.Location = new System.Drawing.Point(3, 27);
            this._importTable.Name = "_importTable";
            this._importTable.RowCount = 1;
            this._importTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._importTable.Size = new System.Drawing.Size(776, 676);
            this._importTable.TabIndex = 13;
            // 
            // _listBox
            // 
            this._listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._listBox.IntegralHeight = false;
            this._listBox.Location = new System.Drawing.Point(3, 3);
            this._listBox.Name = "_listBox";
            this._listBox.Size = new System.Drawing.Size(645, 670);
            this._listBox.TabIndex = 6;
            this._listBox.SelectedIndexChanged += new System.EventHandler(this.ListBox_SelectedIndexChanged);
            // 
            // _opsFlow
            // 
            this._opsFlow.AutoSize = true;
            this._opsFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._opsFlow.Controls.Add(this._renameTableBtn);
            this._opsFlow.Controls.Add(this._selectAllBtn);
            this._opsFlow.Controls.Add(this._selectNoneBtn);
            this._opsFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._opsFlow.Location = new System.Drawing.Point(654, 3);
            this._opsFlow.Name = "_opsFlow";
            this._opsFlow.Size = new System.Drawing.Size(119, 123);
            this._opsFlow.TabIndex = 7;
            // 
            // _renameTableBtn
            // 
            this._renameTableBtn.AutoSize = true;
            this._renameTableBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this._renameTableBtn.Location = new System.Drawing.Point(3, 3);
            this._renameTableBtn.Name = "_renameTableBtn";
            this._renameTableBtn.Size = new System.Drawing.Size(113, 35);
            this._renameTableBtn.TabIndex = 7;
            this._renameTableBtn.Text = "&Rename...";
            this._renameTableBtn.UseVisualStyleBackColor = true;
            this._renameTableBtn.Click += new System.EventHandler(this.RenameTableBtn_Click);
            // 
            // _selectAllBtn
            // 
            this._selectAllBtn.AutoSize = true;
            this._selectAllBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this._selectAllBtn.Location = new System.Drawing.Point(3, 44);
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
            this._selectNoneBtn.Location = new System.Drawing.Point(3, 85);
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
            // _csvHeaderChk
            // 
            this._csvHeaderChk.AutoSize = true;
            this._csvHeaderChk.Checked = true;
            this._csvHeaderChk.CheckState = System.Windows.Forms.CheckState.Checked;
            this._csvHeaderChk.Location = new System.Drawing.Point(3, 3);
            this._csvHeaderChk.Name = "_csvHeaderChk";
            this._csvHeaderChk.Size = new System.Drawing.Size(237, 29);
            this._csvHeaderChk.TabIndex = 5;
            this._csvHeaderChk.Text = "This file has a &header row";
            this._csvHeaderChk.UseVisualStyleBackColor = true;
            // 
            // _table
            // 
            this._table.ColumnCount = 1;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.Controls.Add(this._csvHeaderChk, 0, 0);
            this._table.Controls.Add(this._methodGrp, 0, 2);
            this._table.Controls.Add(this._tablesGrp, 0, 1);
            this._table.Controls.Add(this._buttonFlow, 0, 3);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 4;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
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
            // 
            // ImportPreviewForm
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
            this.Name = "ImportPreviewForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import";
            this._methodGrp.ResumeLayout(false);
            this._methodGrp.PerformLayout();
            this._methodFlow.ResumeLayout(false);
            this._methodFlow.PerformLayout();
            this._tablesGrp.ResumeLayout(false);
            this._importTable.ResumeLayout(false);
            this._importTable.PerformLayout();
            this._opsFlow.ResumeLayout(false);
            this._opsFlow.PerformLayout();
            this._table.ResumeLayout(false);
            this._table.PerformLayout();
            this._buttonFlow.ResumeLayout(false);
            this._buttonFlow.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button _okBtn;
        private System.Windows.Forms.Button _cancelBtn;
        private System.Windows.Forms.CheckedListBox _listBox;
        private System.Windows.Forms.Button _renameTableBtn;
        private System.Windows.Forms.RadioButton _methodLinkRad;
        private System.Windows.Forms.RadioButton _methodCopyRad;
        private System.Windows.Forms.Button _selectNoneBtn;
        private System.Windows.Forms.Button _selectAllBtn;
        private System.Windows.Forms.CheckBox _csvHeaderChk;
        private System.Windows.Forms.GroupBox _tablesGrp;
        private System.Windows.Forms.GroupBox _methodGrp;
        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow;
        private System.Windows.Forms.TableLayoutPanel _importTable;
        private System.Windows.Forms.FlowLayoutPanel _opsFlow;
        private System.Windows.Forms.FlowLayoutPanel _methodFlow;
    }
}