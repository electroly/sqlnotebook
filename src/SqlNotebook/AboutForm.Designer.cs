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
            this._githubLnk = new System.Windows.Forms.LinkLabel();
            this._websiteLnk = new System.Windows.Forms.LinkLabel();
            this._browserPanel = new System.Windows.Forms.Panel();
            this._browser = new System.Windows.Forms.WebBrowser();
            this._table = new System.Windows.Forms.TableLayoutPanel();
            this._buttonFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._linkFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._browserPanel.SuspendLayout();
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
            // _githubLnk
            // 
            this._githubLnk.AutoSize = true;
            this._githubLnk.Location = new System.Drawing.Point(199, 0);
            this._githubLnk.Name = "_githubLnk";
            this._githubLnk.Size = new System.Drawing.Size(170, 25);
            this._githubLnk.TabIndex = 6;
            this._githubLnk.TabStop = true;
            this._githubLnk.Text = "View GitHub project";
            this._githubLnk.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.GithubLnk_LinkClicked);
            // 
            // _websiteLnk
            // 
            this._websiteLnk.AutoSize = true;
            this._websiteLnk.Location = new System.Drawing.Point(3, 0);
            this._websiteLnk.Name = "_websiteLnk";
            this._websiteLnk.Size = new System.Drawing.Size(190, 25);
            this._websiteLnk.TabIndex = 7;
            this._websiteLnk.TabStop = true;
            this._websiteLnk.Text = "Visit sqlnotebook.com";
            this._websiteLnk.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WebsiteLnk_LinkClicked);
            // 
            // _browserPanel
            // 
            this._browserPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._browserPanel.Controls.Add(this._browser);
            this._browserPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._browserPanel.Location = new System.Drawing.Point(3, 34);
            this._browserPanel.Name = "_browserPanel";
            this._browserPanel.Size = new System.Drawing.Size(662, 462);
            this._browserPanel.TabIndex = 9;
            // 
            // _browser
            // 
            this._browser.AllowWebBrowserDrop = false;
            this._browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._browser.IsWebBrowserContextMenuEnabled = false;
            this._browser.Location = new System.Drawing.Point(0, 0);
            this._browser.MinimumSize = new System.Drawing.Size(20, 20);
            this._browser.Name = "_browser";
            this._browser.ScriptErrorsSuppressed = true;
            this._browser.Size = new System.Drawing.Size(660, 460);
            this._browser.TabIndex = 0;
            this._browser.WebBrowserShortcutsEnabled = false;
            // 
            // _table
            // 
            this._table.ColumnCount = 1;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._table.Controls.Add(this._buttonFlow, 0, 3);
            this._table.Controls.Add(this._browserPanel, 0, 2);
            this._table.Controls.Add(this._linkFlow, 0, 0);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 4;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.Size = new System.Drawing.Size(668, 655);
            this._table.TabIndex = 10;
            // 
            // _buttonFlow
            // 
            this._buttonFlow.Controls.Add(this._okBtn);
            this._buttonFlow.Dock = System.Windows.Forms.DockStyle.Right;
            this._buttonFlow.Location = new System.Drawing.Point(448, 502);
            this._buttonFlow.Name = "_buttonFlow";
            this._buttonFlow.Size = new System.Drawing.Size(217, 150);
            this._buttonFlow.TabIndex = 11;
            // 
            // _linkFlow
            // 
            this._linkFlow.AutoSize = true;
            this._linkFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._linkFlow.Controls.Add(this._websiteLnk);
            this._linkFlow.Controls.Add(this._githubLnk);
            this._linkFlow.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._linkFlow.Location = new System.Drawing.Point(3, 3);
            this._linkFlow.Name = "_linkFlow";
            this._linkFlow.Size = new System.Drawing.Size(662, 25);
            this._linkFlow.TabIndex = 12;
            this._linkFlow.WrapContents = false;
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
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(484, 383);
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About SQL Notebook";
            this._browserPanel.ResumeLayout(false);
            this._table.ResumeLayout(false);
            this._table.PerformLayout();
            this._buttonFlow.ResumeLayout(false);
            this._buttonFlow.PerformLayout();
            this._linkFlow.ResumeLayout(false);
            this._linkFlow.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.LinkLabel _githubLnk;
        private System.Windows.Forms.LinkLabel _websiteLnk;
        private System.Windows.Forms.Panel _browserPanel;
        private System.Windows.Forms.WebBrowser _browser;
        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow;
        private System.Windows.Forms.FlowLayoutPanel _linkFlow;
        private System.Windows.Forms.Button _okBtn;
    }
}