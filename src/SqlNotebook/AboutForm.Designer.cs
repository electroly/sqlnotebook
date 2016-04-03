namespace SqlNotebook {
    partial class AboutForm {
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
            System.Windows.Forms.Button _okBtn;
            System.Windows.Forms.PictureBox pictureBox1;
            this._thirdPartyTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._githubLnk = new System.Windows.Forms.LinkLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            _okBtn = new System.Windows.Forms.Button();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _okBtn
            // 
            _okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            _okBtn.Location = new System.Drawing.Point(368, 306);
            _okBtn.Name = "_okBtn";
            _okBtn.Size = new System.Drawing.Size(88, 26);
            _okBtn.TabIndex = 0;
            _okBtn.Text = "OK";
            _okBtn.UseVisualStyleBackColor = true;
            _okBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // pictureBox1
            // 
            pictureBox1.Image = global::SqlNotebook.Properties.Resources.SqlNotebookIcon48;
            pictureBox1.Location = new System.Drawing.Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(48, 48);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // _thirdPartyTxt
            // 
            this._thirdPartyTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._thirdPartyTxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._thirdPartyTxt.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._thirdPartyTxt.Location = new System.Drawing.Point(12, 98);
            this._thirdPartyTxt.Multiline = true;
            this._thirdPartyTxt.Name = "_thirdPartyTxt";
            this._thirdPartyTxt.ReadOnly = true;
            this._thirdPartyTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._thirdPartyTxt.Size = new System.Drawing.Size(444, 202);
            this._thirdPartyTxt.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(9, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Third party licenses:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Window;
            this.label2.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(66, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 29);
            this.label2.TabIndex = 4;
            this.label2.Text = "SQL Notebook";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Window;
            this.label3.Location = new System.Drawing.Point(68, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Copyright © 2016 Brian Luft.";
            // 
            // _githubLnk
            // 
            this._githubLnk.AutoSize = true;
            this._githubLnk.BackColor = System.Drawing.SystemColors.Window;
            this._githubLnk.Location = new System.Drawing.Point(227, 40);
            this._githubLnk.Name = "_githubLnk";
            this._githubLnk.Size = new System.Drawing.Size(90, 15);
            this._githubLnk.TabIndex = 6;
            this._githubLnk.TabStop = true;
            this._githubLnk.Text = "View on GitHub";
            this._githubLnk.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.GithubLnk_LinkClicked);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this._githubLnk);
            this.panel1.Controls.Add(pictureBox1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(-1, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(470, 74);
            this.panel1.TabIndex = 7;
            // 
            // AboutForm
            // 
            this.AcceptButton = _okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 344);
            this.Controls.Add(this._thirdPartyTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(_okBtn);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(484, 383);
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About SQL Notebook";
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _thirdPartyTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel _githubLnk;
        private System.Windows.Forms.Panel panel1;
    }
}