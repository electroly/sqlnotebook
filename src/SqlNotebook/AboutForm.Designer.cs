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
            this._okBtn = new System.Windows.Forms.Button();
            this._table = new System.Windows.Forms.TableLayoutPanel();
            this._buttonFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._linkFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._copyrightLabel = new System.Windows.Forms.Label();
            this._websiteLnk = new System.Windows.Forms.LinkLabel();
            this._githubLnk = new System.Windows.Forms.LinkLabel();
            this._licenseLnk = new System.Windows.Forms.LinkLabel();
            this._titleLabel = new System.Windows.Forms.Label();
            this._versionLabel = new System.Windows.Forms.Label();
            this._table.SuspendLayout();
            this._buttonFlow.SuspendLayout();
            this._linkFlow.SuspendLayout();
            this.SuspendLayout();
            // 
            // _okBtn
            // 
            this._okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._okBtn.AutoSize = true;
            this._okBtn.Location = new System.Drawing.Point(3, 3);
            this._okBtn.Name = "_okBtn";
            this._okBtn.Size = new System.Drawing.Size(88, 35);
            this._okBtn.TabIndex = 0;
            this._okBtn.Text = "Close";
            this._okBtn.UseVisualStyleBackColor = true;
            this._okBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // _table
            // 
            this._table.AutoSize = true;
            this._table.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._table.ColumnCount = 1;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.Controls.Add(this._buttonFlow, 0, 1);
            this._table.Controls.Add(this._linkFlow, 0, 0);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 2;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.Size = new System.Drawing.Size(668, 655);
            this._table.TabIndex = 10;
            // 
            // _buttonFlow
            // 
            this._buttonFlow.AutoSize = true;
            this._buttonFlow.Controls.Add(this._okBtn);
            this._buttonFlow.Dock = System.Windows.Forms.DockStyle.Right;
            this._buttonFlow.Location = new System.Drawing.Point(571, 611);
            this._buttonFlow.Name = "_buttonFlow";
            this._buttonFlow.Size = new System.Drawing.Size(94, 41);
            this._buttonFlow.TabIndex = 11;
            // 
            // _linkFlow
            // 
            this._linkFlow.Controls.Add(this._titleLabel);
            this._linkFlow.Controls.Add(this._versionLabel);
            this._linkFlow.Controls.Add(this._copyrightLabel);
            this._linkFlow.Controls.Add(this._websiteLnk);
            this._linkFlow.Controls.Add(this._githubLnk);
            this._linkFlow.Controls.Add(this._licenseLnk);
            this._linkFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this._linkFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._linkFlow.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._linkFlow.Location = new System.Drawing.Point(3, 3);
            this._linkFlow.Name = "_linkFlow";
            this._linkFlow.Size = new System.Drawing.Size(662, 602);
            this._linkFlow.TabIndex = 13;
            // 
            // _copyrightLabel
            // 
            this._copyrightLabel.AutoSize = true;
            this._copyrightLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._copyrightLabel.Location = new System.Drawing.Point(3, 75);
            this._copyrightLabel.Name = "_copyrightLabel";
            this._copyrightLabel.Size = new System.Drawing.Size(351, 30);
            this._copyrightLabel.TabIndex = 0;
            this._copyrightLabel.Text = "Copyright (C) 2016-2021 Brian Luft";
            // 
            // _websiteLnk
            // 
            this._websiteLnk.AutoSize = true;
            this._websiteLnk.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._websiteLnk.Location = new System.Drawing.Point(3, 105);
            this._websiteLnk.Name = "_websiteLnk";
            this._websiteLnk.Size = new System.Drawing.Size(227, 30);
            this._websiteLnk.TabIndex = 7;
            this._websiteLnk.TabStop = true;
            this._websiteLnk.Text = "Visit sqlnotebook.com";
            this._websiteLnk.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WebsiteLnk_LinkClicked);
            // 
            // _githubLnk
            // 
            this._githubLnk.AutoSize = true;
            this._githubLnk.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._githubLnk.Location = new System.Drawing.Point(3, 135);
            this._githubLnk.Name = "_githubLnk";
            this._githubLnk.Size = new System.Drawing.Size(208, 30);
            this._githubLnk.TabIndex = 6;
            this._githubLnk.TabStop = true;
            this._githubLnk.Text = "View GitHub project";
            this._githubLnk.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.GithubLnk_LinkClicked);
            // 
            // _licenseLnk
            // 
            this._licenseLnk.AutoSize = true;
            this._licenseLnk.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._licenseLnk.Location = new System.Drawing.Point(3, 165);
            this._licenseLnk.Name = "_licenseLnk";
            this._licenseLnk.Size = new System.Drawing.Size(249, 30);
            this._licenseLnk.TabIndex = 8;
            this._licenseLnk.TabStop = true;
            this._licenseLnk.Text = "View license information";
            this._licenseLnk.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LicenseLnk_LinkClicked);
            // 
            // _titleLabel
            // 
            this._titleLabel.AutoSize = true;
            this._titleLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._titleLabel.Location = new System.Drawing.Point(3, 0);
            this._titleLabel.Name = "_titleLabel";
            this._titleLabel.Size = new System.Drawing.Size(233, 45);
            this._titleLabel.TabIndex = 9;
            this._titleLabel.Text = "SQL Notebook";
            // 
            // _versionLabel
            // 
            this._versionLabel.AutoSize = true;
            this._versionLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._versionLabel.Location = new System.Drawing.Point(3, 45);
            this._versionLabel.Name = "_versionLabel";
            this._versionLabel.Size = new System.Drawing.Size(91, 30);
            this._versionLabel.TabIndex = 10;
            this._versionLabel.Text = "Version ";
            // 
            // AboutForm
            // 
            this.AcceptButton = this._okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._okBtn;
            this.ClientSize = new System.Drawing.Size(668, 655);
            this.Controls.Add(this._table);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About SQL Notebook";
            this._table.ResumeLayout(false);
            this._table.PerformLayout();
            this._buttonFlow.ResumeLayout(false);
            this._buttonFlow.PerformLayout();
            this._linkFlow.ResumeLayout(false);
            this._linkFlow.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow;
        private System.Windows.Forms.Button _okBtn;
        private System.Windows.Forms.LinkLabel _websiteLnk;
        private System.Windows.Forms.LinkLabel _githubLnk;
        private System.Windows.Forms.FlowLayoutPanel _linkFlow;
        private System.Windows.Forms.Label _copyrightLabel;
        private System.Windows.Forms.LinkLabel _licenseLnk;
        private System.Windows.Forms.Label _titleLabel;
        private System.Windows.Forms.Label _versionLabel;
    }
}