namespace SqlNotebook.Import.Database {
    partial class DatabaseImportRenameTableForm {
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
            this._oldNameLabel = new System.Windows.Forms.Label();
            this._newNameLabel = new System.Windows.Forms.Label();
            this._okBtn = new System.Windows.Forms.Button();
            this._cancelBtn = new System.Windows.Forms.Button();
            this._oldNameTxt = new System.Windows.Forms.TextBox();
            this._newNameTxt = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._topFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._buttonFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this._topFlow.SuspendLayout();
            this._buttonFlow.SuspendLayout();
            this.SuspendLayout();
            // 
            // _oldNameLabel
            // 
            this._oldNameLabel.AutoSize = true;
            this._oldNameLabel.Location = new System.Drawing.Point(3, 0);
            this._oldNameLabel.Name = "_oldNameLabel";
            this._oldNameLabel.Size = new System.Drawing.Size(171, 25);
            this._oldNameLabel.TabIndex = 3;
            this._oldNameLabel.Text = "Original table name:";
            // 
            // _newNameLabel
            // 
            this._newNameLabel.AutoSize = true;
            this._newNameLabel.Location = new System.Drawing.Point(3, 62);
            this._newNameLabel.Name = "_newNameLabel";
            this._newNameLabel.Size = new System.Drawing.Size(140, 25);
            this._newNameLabel.TabIndex = 2;
            this._newNameLabel.Text = "Imported name:";
            // 
            // _okBtn
            // 
            this._okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._okBtn.AutoSize = true;
            this._okBtn.Location = new System.Drawing.Point(3, 3);
            this._okBtn.Name = "_okBtn";
            this._okBtn.Size = new System.Drawing.Size(88, 35);
            this._okBtn.TabIndex = 1;
            this._okBtn.Text = "Rename";
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
            this._cancelBtn.TabIndex = 2;
            this._cancelBtn.Text = "Cancel";
            this._cancelBtn.UseVisualStyleBackColor = true;
            // 
            // _oldNameTxt
            // 
            this._oldNameTxt.Location = new System.Drawing.Point(3, 28);
            this._oldNameTxt.Name = "_oldNameTxt";
            this._oldNameTxt.ReadOnly = true;
            this._oldNameTxt.Size = new System.Drawing.Size(171, 31);
            this._oldNameTxt.TabIndex = 4;
            // 
            // _newNameTxt
            // 
            this._newNameTxt.Location = new System.Drawing.Point(3, 90);
            this._newNameTxt.Name = "_newNameTxt";
            this._newNameTxt.Size = new System.Drawing.Size(171, 31);
            this._newNameTxt.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this._topFlow, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._buttonFlow, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(804, 588);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // _topFlow
            // 
            this._topFlow.Controls.Add(this._oldNameLabel);
            this._topFlow.Controls.Add(this._oldNameTxt);
            this._topFlow.Controls.Add(this._newNameLabel);
            this._topFlow.Controls.Add(this._newNameTxt);
            this._topFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this._topFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._topFlow.Location = new System.Drawing.Point(3, 3);
            this._topFlow.Name = "_topFlow";
            this._topFlow.Size = new System.Drawing.Size(798, 535);
            this._topFlow.TabIndex = 0;
            // 
            // _buttonFlow
            // 
            this._buttonFlow.AutoSize = true;
            this._buttonFlow.Controls.Add(this._okBtn);
            this._buttonFlow.Controls.Add(this._cancelBtn);
            this._buttonFlow.Dock = System.Windows.Forms.DockStyle.Right;
            this._buttonFlow.Location = new System.Drawing.Point(613, 544);
            this._buttonFlow.Name = "_buttonFlow";
            this._buttonFlow.Size = new System.Drawing.Size(188, 41);
            this._buttonFlow.TabIndex = 1;
            this._buttonFlow.WrapContents = false;
            // 
            // ImportRenameTableForm
            // 
            this.AcceptButton = this._okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelBtn;
            this.ClientSize = new System.Drawing.Size(804, 588);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportRenameTableForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rename Table";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this._topFlow.ResumeLayout(false);
            this._topFlow.PerformLayout();
            this._buttonFlow.ResumeLayout(false);
            this._buttonFlow.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _oldNameTxt;
        private System.Windows.Forms.TextBox _newNameTxt;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel _topFlow;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow;
        private System.Windows.Forms.Label _oldNameLabel;
        private System.Windows.Forms.Label _newNameLabel;
        private System.Windows.Forms.Button _okBtn;
        private System.Windows.Forms.Button _cancelBtn;
    }
}