namespace SqlNotebook {
    partial class ImportConnectionForm {
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
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            this._propertyGrid = new System.Windows.Forms.PropertyGrid();
            this._okBtn = new System.Windows.Forms.Button();
            this._cancelBtn = new System.Windows.Forms.Button();
            this._toggleBtn = new System.Windows.Forms.Button();
            this._passwordTxt = new System.Windows.Forms.TextBox();
            this._usernameTxt = new System.Windows.Forms.TextBox();
            this._databaseTxt = new System.Windows.Forms.TextBox();
            this._serverTxt = new System.Windows.Forms.TextBox();
            this._basicPnl = new System.Windows.Forms.Panel();
            this._windowsAuthChk = new System.Windows.Forms.CheckBox();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this._basicPnl.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(0, 90);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(60, 15);
            label3.TabIndex = 6;
            label3.Text = "Password:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(0, 61);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(63, 15);
            label4.TabIndex = 4;
            label4.Text = "Username:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(0, 32);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(91, 15);
            label2.TabIndex = 2;
            label2.Text = "Database name:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(0, 3);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(85, 15);
            label1.TabIndex = 0;
            label1.Text = "Server address:";
            // 
            // _propertyGrid
            // 
            this._propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._propertyGrid.Location = new System.Drawing.Point(12, 12);
            this._propertyGrid.Name = "_propertyGrid";
            this._propertyGrid.Size = new System.Drawing.Size(356, 140);
            this._propertyGrid.TabIndex = 9;
            this._propertyGrid.ToolbarVisible = false;
            this._propertyGrid.Visible = false;
            // 
            // _okBtn
            // 
            this._okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okBtn.Location = new System.Drawing.Point(186, 169);
            this._okBtn.Name = "_okBtn";
            this._okBtn.Size = new System.Drawing.Size(88, 26);
            this._okBtn.TabIndex = 10;
            this._okBtn.Text = "Connect";
            this._okBtn.UseVisualStyleBackColor = true;
            this._okBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // _cancelBtn
            // 
            this._cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelBtn.Location = new System.Drawing.Point(280, 169);
            this._cancelBtn.Name = "_cancelBtn";
            this._cancelBtn.Size = new System.Drawing.Size(88, 26);
            this._cancelBtn.TabIndex = 11;
            this._cancelBtn.Text = "Cancel";
            this._cancelBtn.UseVisualStyleBackColor = true;
            // 
            // _toggleBtn
            // 
            this._toggleBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._toggleBtn.Location = new System.Drawing.Point(12, 169);
            this._toggleBtn.Name = "_toggleBtn";
            this._toggleBtn.Size = new System.Drawing.Size(156, 26);
            this._toggleBtn.TabIndex = 12;
            this._toggleBtn.Text = "Show advanced options";
            this._toggleBtn.UseVisualStyleBackColor = true;
            this._toggleBtn.Click += new System.EventHandler(this.ToggleBtn_Click);
            // 
            // _passwordTxt
            // 
            this._passwordTxt.Location = new System.Drawing.Point(116, 87);
            this._passwordTxt.Name = "_passwordTxt";
            this._passwordTxt.PasswordChar = '*';
            this._passwordTxt.Size = new System.Drawing.Size(240, 23);
            this._passwordTxt.TabIndex = 7;
            // 
            // _usernameTxt
            // 
            this._usernameTxt.Location = new System.Drawing.Point(116, 58);
            this._usernameTxt.Name = "_usernameTxt";
            this._usernameTxt.Size = new System.Drawing.Size(240, 23);
            this._usernameTxt.TabIndex = 5;
            // 
            // _databaseTxt
            // 
            this._databaseTxt.Location = new System.Drawing.Point(116, 29);
            this._databaseTxt.Name = "_databaseTxt";
            this._databaseTxt.Size = new System.Drawing.Size(240, 23);
            this._databaseTxt.TabIndex = 3;
            // 
            // _serverTxt
            // 
            this._serverTxt.Location = new System.Drawing.Point(116, 0);
            this._serverTxt.Name = "_serverTxt";
            this._serverTxt.Size = new System.Drawing.Size(240, 23);
            this._serverTxt.TabIndex = 1;
            // 
            // _basicPnl
            // 
            this._basicPnl.Controls.Add(this._windowsAuthChk);
            this._basicPnl.Controls.Add(this._serverTxt);
            this._basicPnl.Controls.Add(this._passwordTxt);
            this._basicPnl.Controls.Add(label1);
            this._basicPnl.Controls.Add(label3);
            this._basicPnl.Controls.Add(label2);
            this._basicPnl.Controls.Add(this._usernameTxt);
            this._basicPnl.Controls.Add(this._databaseTxt);
            this._basicPnl.Controls.Add(label4);
            this._basicPnl.Location = new System.Drawing.Point(12, 12);
            this._basicPnl.Name = "_basicPnl";
            this._basicPnl.Size = new System.Drawing.Size(356, 140);
            this._basicPnl.TabIndex = 0;
            // 
            // _windowsAuthChk
            // 
            this._windowsAuthChk.AutoSize = true;
            this._windowsAuthChk.Location = new System.Drawing.Point(116, 116);
            this._windowsAuthChk.Name = "_windowsAuthChk";
            this._windowsAuthChk.Size = new System.Drawing.Size(177, 19);
            this._windowsAuthChk.TabIndex = 8;
            this._windowsAuthChk.Text = "Use Windows authentication";
            this._windowsAuthChk.UseVisualStyleBackColor = true;
            this._windowsAuthChk.CheckedChanged += new System.EventHandler(this.WindowsAuthChk_CheckedChanged);
            // 
            // ImportConnectionForm
            // 
            this.AcceptButton = this._okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelBtn;
            this.ClientSize = new System.Drawing.Size(380, 207);
            this.Controls.Add(this._basicPnl);
            this.Controls.Add(this._toggleBtn);
            this.Controls.Add(this._cancelBtn);
            this.Controls.Add(this._okBtn);
            this.Controls.Add(this._propertyGrid);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportConnectionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect to [...]";
            this._basicPnl.ResumeLayout(false);
            this._basicPnl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid _propertyGrid;
        private System.Windows.Forms.Button _okBtn;
        private System.Windows.Forms.Button _cancelBtn;
        private System.Windows.Forms.Button _toggleBtn;
        private System.Windows.Forms.TextBox _passwordTxt;
        private System.Windows.Forms.TextBox _usernameTxt;
        private System.Windows.Forms.TextBox _databaseTxt;
        private System.Windows.Forms.TextBox _serverTxt;
        private System.Windows.Forms.Panel _basicPnl;
        private System.Windows.Forms.CheckBox _windowsAuthChk;
    }
}