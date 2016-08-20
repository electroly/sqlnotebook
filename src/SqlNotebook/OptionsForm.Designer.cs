namespace SqlNotebook {
    partial class OptionsForm {
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
            this._okBtn = new System.Windows.Forms.Button();
            this._cancelBtn = new System.Windows.Forms.Button();
            this._autoCreateChk = new System.Windows.Forms.CheckBox();
            this._autoCreateCmb = new System.Windows.Forms.ComboBox();
            this._helpExternalBrowserChk = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // _okBtn
            // 
            this._okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._okBtn.Location = new System.Drawing.Point(242, 85);
            this._okBtn.Name = "_okBtn";
            this._okBtn.Size = new System.Drawing.Size(88, 26);
            this._okBtn.TabIndex = 0;
            this._okBtn.Text = "OK";
            this._okBtn.UseVisualStyleBackColor = true;
            this._okBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // _cancelBtn
            // 
            this._cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelBtn.Location = new System.Drawing.Point(336, 85);
            this._cancelBtn.Name = "_cancelBtn";
            this._cancelBtn.Size = new System.Drawing.Size(88, 26);
            this._cancelBtn.TabIndex = 1;
            this._cancelBtn.Text = "Cancel";
            this._cancelBtn.UseVisualStyleBackColor = true;
            // 
            // _autoCreateChk
            // 
            this._autoCreateChk.AutoSize = true;
            this._autoCreateChk.Location = new System.Drawing.Point(12, 13);
            this._autoCreateChk.Name = "_autoCreateChk";
            this._autoCreateChk.Size = new System.Drawing.Size(235, 19);
            this._autoCreateChk.TabIndex = 2;
            this._autoCreateChk.Text = "Automatically create in new notebooks:";
            this._autoCreateChk.UseVisualStyleBackColor = true;
            this._autoCreateChk.CheckedChanged += new System.EventHandler(this.AutoCreateChk_CheckedChanged);
            // 
            // _autoCreateCmb
            // 
            this._autoCreateCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._autoCreateCmb.FormattingEnabled = true;
            this._autoCreateCmb.Items.AddRange(new object[] {
            "New note",
            "New console",
            "New script"});
            this._autoCreateCmb.Location = new System.Drawing.Point(253, 10);
            this._autoCreateCmb.Name = "_autoCreateCmb";
            this._autoCreateCmb.Size = new System.Drawing.Size(108, 23);
            this._autoCreateCmb.TabIndex = 3;
            // 
            // _helpExternalBrowserChk
            // 
            this._helpExternalBrowserChk.AutoSize = true;
            this._helpExternalBrowserChk.Location = new System.Drawing.Point(12, 39);
            this._helpExternalBrowserChk.Name = "_helpExternalBrowserChk";
            this._helpExternalBrowserChk.Size = new System.Drawing.Size(281, 19);
            this._helpExternalBrowserChk.TabIndex = 4;
            this._helpExternalBrowserChk.Text = "Use external browser for viewing documentation";
            this._helpExternalBrowserChk.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AcceptButton = this._okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelBtn;
            this.ClientSize = new System.Drawing.Size(436, 123);
            this.Controls.Add(this._helpExternalBrowserChk);
            this.Controls.Add(this._autoCreateCmb);
            this.Controls.Add(this._autoCreateChk);
            this.Controls.Add(this._cancelBtn);
            this.Controls.Add(this._okBtn);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _okBtn;
        private System.Windows.Forms.Button _cancelBtn;
        private System.Windows.Forms.CheckBox _autoCreateChk;
        private System.Windows.Forms.ComboBox _autoCreateCmb;
        private System.Windows.Forms.CheckBox _helpExternalBrowserChk;
    }
}