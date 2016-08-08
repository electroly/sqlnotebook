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
            this.label2 = new System.Windows.Forms.Label();
            this._githubLnk = new System.Windows.Forms.LinkLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this._websiteLnk = new System.Windows.Forms.LinkLabel();
            this._browserPanel = new System.Windows.Forms.Panel();
            _okBtn = new System.Windows.Forms.Button();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _okBtn
            // 
            _okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            _okBtn.Location = new System.Drawing.Point(568, 447);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Window;
            this.label2.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(66, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(206, 39);
            this.label2.TabIndex = 4;
            this.label2.Text = "SQL Notebook";
            // 
            // _githubLnk
            // 
            this._githubLnk.AutoSize = true;
            this._githubLnk.BackColor = System.Drawing.SystemColors.Window;
            this._githubLnk.Location = new System.Drawing.Point(201, 45);
            this._githubLnk.Name = "_githubLnk";
            this._githubLnk.Size = new System.Drawing.Size(113, 15);
            this._githubLnk.TabIndex = 6;
            this._githubLnk.TabStop = true;
            this._githubLnk.Text = "View GitHub project";
            this._githubLnk.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.GithubLnk_LinkClicked);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this._websiteLnk);
            this.panel1.Controls.Add(this._githubLnk);
            this.panel1.Controls.Add(pictureBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(-1, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(670, 74);
            this.panel1.TabIndex = 7;
            // 
            // _websiteLnk
            // 
            this._websiteLnk.AutoSize = true;
            this._websiteLnk.BackColor = System.Drawing.SystemColors.Window;
            this._websiteLnk.Location = new System.Drawing.Point(70, 45);
            this._websiteLnk.Name = "_websiteLnk";
            this._websiteLnk.Size = new System.Drawing.Size(125, 15);
            this._websiteLnk.TabIndex = 7;
            this._websiteLnk.TabStop = true;
            this._websiteLnk.Text = "Visit sqlnotebook.com";
            this._websiteLnk.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WebsiteLnk_LinkClicked);
            // 
            // _browserPanel
            // 
            this._browserPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._browserPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._browserPanel.Location = new System.Drawing.Point(-1, 72);
            this._browserPanel.Name = "_browserPanel";
            this._browserPanel.Size = new System.Drawing.Size(670, 369);
            this._browserPanel.TabIndex = 9;
            // 
            // AboutForm
            // 
            this.AcceptButton = _okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 485);
            this.Controls.Add(this._browserPanel);
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

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel _githubLnk;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel _browserPanel;
        private System.Windows.Forms.LinkLabel _websiteLnk;
    }
}