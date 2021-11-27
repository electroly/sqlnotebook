
namespace SqlNotebook {
    partial class QueryEmbeddedControl {
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
            System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
            System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
            System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
            System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
            System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryEmbeddedControl));
            this._split = new System.Windows.Forms.SplitContainer();
            this._sqlPanel = new System.Windows.Forms.Panel();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._executeButton = new System.Windows.Forms.ToolStripButton();
            this._sendMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this._sendTableMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._optionsMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this._optionsShowSqlMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._optionsShowResultsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._limitRowsOnPageMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._showResultsButton = new System.Windows.Forms.ToolStripButton();
            this._hideResultsButton = new System.Windows.Forms.ToolStripButton();
            this._transactionMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this._transactionCommitMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._transactionRollbackMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._transactionNoneMenu = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this._split)).BeginInit();
            this._split.Panel1.SuspendLayout();
            this._split.SuspendLayout();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(144, 34);
            toolStripMenuItem2.Text = "5";
            toolStripMenuItem2.Click += new System.EventHandler(this.LimitsRowsMenu_Click);
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new System.Drawing.Size(144, 34);
            toolStripMenuItem3.Text = "10";
            toolStripMenuItem3.Click += new System.EventHandler(this.LimitsRowsMenu_Click);
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new System.Drawing.Size(144, 34);
            toolStripMenuItem4.Text = "20";
            toolStripMenuItem4.Click += new System.EventHandler(this.LimitsRowsMenu_Click);
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new System.Drawing.Size(144, 34);
            toolStripMenuItem5.Text = "50";
            toolStripMenuItem5.Click += new System.EventHandler(this.LimitsRowsMenu_Click);
            // 
            // toolStripMenuItem6
            // 
            toolStripMenuItem6.Name = "toolStripMenuItem6";
            toolStripMenuItem6.Size = new System.Drawing.Size(144, 34);
            toolStripMenuItem6.Text = "100";
            toolStripMenuItem6.Click += new System.EventHandler(this.LimitsRowsMenu_Click);
            // 
            // _split
            // 
            this._split.Cursor = System.Windows.Forms.Cursors.HSplit;
            this._split.Dock = System.Windows.Forms.DockStyle.Fill;
            this._split.Location = new System.Drawing.Point(0, 0);
            this._split.Name = "_split";
            this._split.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _split.Panel1
            // 
            this._split.Panel1.Controls.Add(this._sqlPanel);
            this._split.Panel1.Controls.Add(this._toolStrip);
            this._split.Panel2Collapsed = true;
            this._split.Size = new System.Drawing.Size(1059, 713);
            this._split.SplitterDistance = 252;
            this._split.TabIndex = 0;
            // 
            // _sqlPanel
            // 
            this._sqlPanel.BackColor = System.Drawing.SystemColors.Window;
            this._sqlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._sqlPanel.Location = new System.Drawing.Point(0, 40);
            this._sqlPanel.Name = "_sqlPanel";
            this._sqlPanel.Size = new System.Drawing.Size(1059, 673);
            this._sqlPanel.TabIndex = 2;
            // 
            // _toolStrip
            // 
            this._toolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._executeButton,
            this._sendMenu,
            this._optionsMenu,
            this._showResultsButton,
            this._hideResultsButton,
            this._transactionMenu});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this._toolStrip.ShowItemToolTips = false;
            this._toolStrip.Size = new System.Drawing.Size(1059, 40);
            this._toolStrip.Stretch = true;
            this._toolStrip.TabIndex = 1;
            // 
            // _executeButton
            // 
            this._executeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._executeButton.Name = "_executeButton";
            this._executeButton.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this._executeButton.Size = new System.Drawing.Size(109, 35);
            this._executeButton.Text = "Execute (F5)";
            this._executeButton.Click += new System.EventHandler(this.ExecuteButton_Click);
            // 
            // _sendMenu
            // 
            this._sendMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._sendMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._sendTableMenu});
            this._sendMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._sendMenu.Name = "_sendMenu";
            this._sendMenu.Size = new System.Drawing.Size(70, 35);
            this._sendMenu.Text = "Send";
            // 
            // _sendTableMenu
            // 
            this._sendTableMenu.Name = "_sendTableMenu";
            this._sendTableMenu.Size = new System.Drawing.Size(188, 34);
            this._sendTableMenu.Text = "To table...";
            this._sendTableMenu.Click += new System.EventHandler(this.SendTableMenu_Click);
            // 
            // _optionsMenu
            // 
            this._optionsMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._optionsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._optionsShowSqlMenu,
            this._optionsShowResultsMenu,
            this._limitRowsOnPageMenu});
            this._optionsMenu.Image = ((System.Drawing.Image)(resources.GetObject("_optionsMenu.Image")));
            this._optionsMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._optionsMenu.Name = "_optionsMenu";
            this._optionsMenu.Size = new System.Drawing.Size(124, 35);
            this._optionsMenu.Text = "Appearance";
            // 
            // _optionsShowSqlMenu
            // 
            this._optionsShowSqlMenu.Checked = true;
            this._optionsShowSqlMenu.CheckOnClick = true;
            this._optionsShowSqlMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this._optionsShowSqlMenu.Name = "_optionsShowSqlMenu";
            this._optionsShowSqlMenu.Size = new System.Drawing.Size(364, 34);
            this._optionsShowSqlMenu.Text = "Show SQL on page";
            this._optionsShowSqlMenu.Click += new System.EventHandler(this.AppearanceMenu_Click);
            // 
            // _optionsShowResultsMenu
            // 
            this._optionsShowResultsMenu.Checked = true;
            this._optionsShowResultsMenu.CheckOnClick = true;
            this._optionsShowResultsMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this._optionsShowResultsMenu.Name = "_optionsShowResultsMenu";
            this._optionsShowResultsMenu.Size = new System.Drawing.Size(364, 34);
            this._optionsShowResultsMenu.Text = "Show results on page";
            this._optionsShowResultsMenu.Click += new System.EventHandler(this.AppearanceMenu_Click);
            // 
            // _limitRowsOnPageMenu
            // 
            this._limitRowsOnPageMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripMenuItem2,
            toolStripMenuItem3,
            toolStripMenuItem4,
            toolStripMenuItem5,
            toolStripMenuItem6});
            this._limitRowsOnPageMenu.Name = "_limitRowsOnPageMenu";
            this._limitRowsOnPageMenu.Size = new System.Drawing.Size(364, 34);
            this._limitRowsOnPageMenu.Text = "Maximum rows shown on page";
            // 
            // _showResultsButton
            // 
            this._showResultsButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._showResultsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._showResultsButton.Name = "_showResultsButton";
            this._showResultsButton.Size = new System.Drawing.Size(116, 35);
            this._showResultsButton.Text = "Show results";
            this._showResultsButton.Visible = false;
            this._showResultsButton.Click += new System.EventHandler(this.ShowResultsButton_Click);
            // 
            // _hideResultsButton
            // 
            this._hideResultsButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._hideResultsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._hideResultsButton.Name = "_hideResultsButton";
            this._hideResultsButton.Size = new System.Drawing.Size(109, 35);
            this._hideResultsButton.Text = "Hide results";
            this._hideResultsButton.Visible = false;
            this._hideResultsButton.Click += new System.EventHandler(this.HideResultsButton_Click);
            // 
            // _transactionMenu
            // 
            this._transactionMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._transactionMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._transactionCommitMenu,
            this._transactionRollbackMenu,
            this._transactionNoneMenu});
            this._transactionMenu.Image = ((System.Drawing.Image)(resources.GetObject("_transactionMenu.Image")));
            this._transactionMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._transactionMenu.Name = "_transactionMenu";
            this._transactionMenu.Size = new System.Drawing.Size(191, 35);
            this._transactionMenu.Text = "Transaction: Commit";
            // 
            // _transactionCommitMenu
            // 
            this._transactionCommitMenu.Checked = true;
            this._transactionCommitMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this._transactionCommitMenu.Name = "_transactionCommitMenu";
            this._transactionCommitMenu.Size = new System.Drawing.Size(332, 34);
            this._transactionCommitMenu.Text = "Implicit BEGIN ... COMMIT";
            this._transactionCommitMenu.Click += new System.EventHandler(this.TransactionCommitMenu_Click);
            // 
            // _transactionRollbackMenu
            // 
            this._transactionRollbackMenu.Name = "_transactionRollbackMenu";
            this._transactionRollbackMenu.Size = new System.Drawing.Size(332, 34);
            this._transactionRollbackMenu.Text = "Implicit BEGIN ... ROLLBACK";
            this._transactionRollbackMenu.Click += new System.EventHandler(this.TransactionRollbackMenu_Click);
            // 
            // _transactionNoneMenu
            // 
            this._transactionNoneMenu.Name = "_transactionNoneMenu";
            this._transactionNoneMenu.Size = new System.Drawing.Size(332, 34);
            this._transactionNoneMenu.Text = "None (auto-commit)";
            this._transactionNoneMenu.Click += new System.EventHandler(this.TransactionNoneMenu_Click);
            // 
            // QueryEmbeddedControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._split);
            this.Name = "QueryEmbeddedControl";
            this.Size = new System.Drawing.Size(1059, 713);
            this._split.Panel1.ResumeLayout(false);
            this._split.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._split)).EndInit();
            this._split.ResumeLayout(false);
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer _split;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _executeButton;
        private System.Windows.Forms.Panel _sqlPanel;
        private System.Windows.Forms.ToolStripDropDownButton _sendMenu;
        private System.Windows.Forms.ToolStripMenuItem _sendTableMenu;
        private System.Windows.Forms.ToolStripDropDownButton _optionsMenu;
        private System.Windows.Forms.ToolStripMenuItem _optionsShowSqlMenu;
        private System.Windows.Forms.ToolStripMenuItem _optionsShowResultsMenu;
        private System.Windows.Forms.ToolStripButton _hideResultsButton;
        private System.Windows.Forms.ToolStripButton _showResultsButton;
        private System.Windows.Forms.ToolStripMenuItem _limitRowsOnPageMenu;
        private System.Windows.Forms.ToolStripDropDownButton _transactionMenu;
        private System.Windows.Forms.ToolStripMenuItem _transactionCommitMenu;
        private System.Windows.Forms.ToolStripMenuItem _transactionRollbackMenu;
        private System.Windows.Forms.ToolStripMenuItem _transactionNoneMenu;
    }
}
