namespace SqlNotebook {
    partial class ImportCsvOptionsControl {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.label5 = new System.Windows.Forms.Label();
            this._tableOutputTitle = new System.Windows.Forms.Label();
            this._headerChk = new System.Windows.Forms.CheckBox();
            this._fileInputTitle = new System.Windows.Forms.Label();
            this._tableCmb = new System.Windows.Forms.ComboBox();
            this._skipLinesTxt = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._ifExistsCmb = new System.Windows.Forms.ComboBox();
            this._convertFailCmb = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this._encodingCmb = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._skipLinesTxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 187);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 15);
            this.label5.TabIndex = 23;
            this.label5.Text = "If the table exists:";
            // 
            // _tableOutputTitle
            // 
            this._tableOutputTitle.AutoSize = true;
            this._tableOutputTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._tableOutputTitle.Location = new System.Drawing.Point(11, 133);
            this._tableOutputTitle.Name = "_tableOutputTitle";
            this._tableOutputTitle.Size = new System.Drawing.Size(43, 15);
            this._tableOutputTitle.TabIndex = 27;
            this._tableOutputTitle.Text = "Target";
            // 
            // _headerChk
            // 
            this._headerChk.AutoSize = true;
            this._headerChk.Checked = true;
            this._headerChk.CheckState = System.Windows.Forms.CheckState.Checked;
            this._headerChk.Location = new System.Drawing.Point(146, 62);
            this._headerChk.Name = "_headerChk";
            this._headerChk.Size = new System.Drawing.Size(179, 19);
            this._headerChk.TabIndex = 26;
            this._headerChk.Text = "File includes column headers";
            this._headerChk.UseVisualStyleBackColor = true;
            // 
            // _fileInputTitle
            // 
            this._fileInputTitle.AutoSize = true;
            this._fileInputTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._fileInputTitle.Location = new System.Drawing.Point(11, 11);
            this._fileInputTitle.Name = "_fileInputTitle";
            this._fileInputTitle.Size = new System.Drawing.Size(46, 15);
            this._fileInputTitle.TabIndex = 21;
            this._fileInputTitle.Text = "Source";
            // 
            // _tableCmb
            // 
            this._tableCmb.FormattingEnabled = true;
            this._tableCmb.Location = new System.Drawing.Point(146, 154);
            this._tableCmb.Name = "_tableCmb";
            this._tableCmb.Size = new System.Drawing.Size(235, 23);
            this._tableCmb.TabIndex = 22;
            // 
            // _skipLinesTxt
            // 
            this._skipLinesTxt.Location = new System.Drawing.Point(146, 33);
            this._skipLinesTxt.Name = "_skipLinesTxt";
            this._skipLinesTxt.Size = new System.Drawing.Size(48, 23);
            this._skipLinesTxt.TabIndex = 25;
            this._skipLinesTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 15);
            this.label2.TabIndex = 20;
            this.label2.Text = "Import into table:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 15);
            this.label3.TabIndex = 24;
            this.label3.Text = "Initial lines to skip:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 216);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 15);
            this.label4.TabIndex = 31;
            this.label4.Text = "If data conversion fails:";
            // 
            // _ifExistsCmb
            // 
            this._ifExistsCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._ifExistsCmb.FormattingEnabled = true;
            this._ifExistsCmb.Location = new System.Drawing.Point(146, 183);
            this._ifExistsCmb.Name = "_ifExistsCmb";
            this._ifExistsCmb.Size = new System.Drawing.Size(179, 23);
            this._ifExistsCmb.TabIndex = 33;
            // 
            // _convertFailCmb
            // 
            this._convertFailCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._convertFailCmb.FormattingEnabled = true;
            this._convertFailCmb.Location = new System.Drawing.Point(146, 212);
            this._convertFailCmb.Name = "_convertFailCmb";
            this._convertFailCmb.Size = new System.Drawing.Size(179, 23);
            this._convertFailCmb.TabIndex = 34;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 15);
            this.label6.TabIndex = 35;
            this.label6.Text = "Column names:";
            // 
            // _encodingCmb
            // 
            this._encodingCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._encodingCmb.FormattingEnabled = true;
            this._encodingCmb.Location = new System.Drawing.Point(146, 87);
            this._encodingCmb.Name = "_encodingCmb";
            this._encodingCmb.Size = new System.Drawing.Size(235, 23);
            this._encodingCmb.TabIndex = 36;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 15);
            this.label7.TabIndex = 37;
            this.label7.Text = "Text encoding:";
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // ImportCsvOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.label7);
            this.Controls.Add(this._encodingCmb);
            this.Controls.Add(this.label6);
            this.Controls.Add(this._convertFailCmb);
            this.Controls.Add(this._ifExistsCmb);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this._tableOutputTitle);
            this.Controls.Add(this._headerChk);
            this.Controls.Add(this._fileInputTitle);
            this.Controls.Add(this._tableCmb);
            this.Controls.Add(this._skipLinesTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ImportCsvOptionsControl";
            this.Size = new System.Drawing.Size(413, 305);
            ((System.ComponentModel.ISupportInitialize)(this._skipLinesTxt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label _tableOutputTitle;
        private System.Windows.Forms.CheckBox _headerChk;
        private System.Windows.Forms.Label _fileInputTitle;
        private System.Windows.Forms.ComboBox _tableCmb;
        private System.Windows.Forms.NumericUpDown _skipLinesTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox _ifExistsCmb;
        private System.Windows.Forms.ComboBox _convertFailCmb;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox _encodingCmb;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ErrorProvider _errorProvider;
    }
}
