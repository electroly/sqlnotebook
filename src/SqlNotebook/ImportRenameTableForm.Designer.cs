namespace SqlNotebook {
    partial class ImportRenameTableForm {
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Button _okBtn;
            System.Windows.Forms.Button _cancelBtn;
            this._oldNameTxt = new System.Windows.Forms.TextBox();
            this._newNameTxt = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            _okBtn = new System.Windows.Forms.Button();
            _cancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(114, 15);
            label1.TabIndex = 3;
            label1.Text = "Original table name:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 63);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(67, 15);
            label2.TabIndex = 2;
            label2.Text = "New name:";
            // 
            // _okBtn
            // 
            _okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            _okBtn.Location = new System.Drawing.Point(99, 124);
            _okBtn.Name = "_okBtn";
            _okBtn.Size = new System.Drawing.Size(88, 26);
            _okBtn.TabIndex = 1;
            _okBtn.Text = "Rename";
            _okBtn.UseVisualStyleBackColor = true;
            _okBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // _cancelBtn
            // 
            _cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            _cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            _cancelBtn.Location = new System.Drawing.Point(193, 124);
            _cancelBtn.Name = "_cancelBtn";
            _cancelBtn.Size = new System.Drawing.Size(88, 26);
            _cancelBtn.TabIndex = 2;
            _cancelBtn.Text = "Cancel";
            _cancelBtn.UseVisualStyleBackColor = true;
            // 
            // _oldNameTxt
            // 
            this._oldNameTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._oldNameTxt.Location = new System.Drawing.Point(12, 27);
            this._oldNameTxt.Name = "_oldNameTxt";
            this._oldNameTxt.ReadOnly = true;
            this._oldNameTxt.Size = new System.Drawing.Size(269, 23);
            this._oldNameTxt.TabIndex = 4;
            // 
            // _newNameTxt
            // 
            this._newNameTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._newNameTxt.Location = new System.Drawing.Point(12, 81);
            this._newNameTxt.Name = "_newNameTxt";
            this._newNameTxt.Size = new System.Drawing.Size(269, 23);
            this._newNameTxt.TabIndex = 0;
            // 
            // ImportRenameTableForm
            // 
            this.AcceptButton = _okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = _cancelBtn;
            this.ClientSize = new System.Drawing.Size(293, 162);
            this.Controls.Add(_cancelBtn);
            this.Controls.Add(_okBtn);
            this.Controls.Add(this._newNameTxt);
            this.Controls.Add(label2);
            this.Controls.Add(this._oldNameTxt);
            this.Controls.Add(label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportRenameTableForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rename Table";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _oldNameTxt;
        private System.Windows.Forms.TextBox _newNameTxt;
    }
}