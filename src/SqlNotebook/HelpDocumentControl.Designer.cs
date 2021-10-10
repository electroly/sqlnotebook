namespace SqlNotebook {
    partial class HelpDocumentControl {
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
            System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpDocumentControl));
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._homeBtn = new System.Windows.Forms.ToolStripButton();
            this._backBtn = new System.Windows.Forms.ToolStripButton();
            this._forwardBtn = new System.Windows.Forms.ToolStripButton();
            this._openBrowserBtn = new System.Windows.Forms.ToolStripButton();
            this._browser = new System.Windows.Forms.WebBrowser();
            this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._backMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._forwardMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._copyMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._selectAllMnu = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._toolStrip.SuspendLayout();
            this._contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(242, 6);
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(242, 6);
            // 
            // _toolStrip
            // 
            this._toolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._homeBtn,
            this._backBtn,
            this._forwardBtn,
            this._openBrowserBtn});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this._toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this._toolStrip.ShowItemToolTips = false;
            this._toolStrip.Size = new System.Drawing.Size(1102, 40);
            this._toolStrip.Stretch = true;
            this._toolStrip.TabIndex = 1;
            // 
            // _homeBtn
            // 
            this._homeBtn.Image = global::SqlNotebook.Properties.Resources.house;
            this._homeBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._homeBtn.Margin = new System.Windows.Forms.Padding(2, 1, 0, 2);
            this._homeBtn.Name = "_homeBtn";
            this._homeBtn.Padding = new System.Windows.Forms.Padding(3);
            this._homeBtn.Size = new System.Drawing.Size(95, 37);
            this._homeBtn.Text = "Home";
            this._homeBtn.Click += new System.EventHandler(this.HomeBtn_Click);
            // 
            // _backBtn
            // 
            this._backBtn.Image = global::SqlNotebook.Properties.Resources.resultset_previous;
            this._backBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._backBtn.Name = "_backBtn";
            this._backBtn.Padding = new System.Windows.Forms.Padding(3);
            this._backBtn.Size = new System.Drawing.Size(82, 35);
            this._backBtn.Text = "Back";
            this._backBtn.Click += new System.EventHandler(this.BackBtn_Click);
            // 
            // _forwardBtn
            // 
            this._forwardBtn.Image = global::SqlNotebook.Properties.Resources.resultset_next;
            this._forwardBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._forwardBtn.Name = "_forwardBtn";
            this._forwardBtn.Padding = new System.Windows.Forms.Padding(3);
            this._forwardBtn.Size = new System.Drawing.Size(111, 35);
            this._forwardBtn.Text = "Forward";
            this._forwardBtn.Click += new System.EventHandler(this.ForwardBtn_Click);
            // 
            // _openBrowserBtn
            // 
            this._openBrowserBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._openBrowserBtn.Image = global::SqlNotebook.Properties.Resources.world_go;
            this._openBrowserBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._openBrowserBtn.Margin = new System.Windows.Forms.Padding(0, 1, 3, 2);
            this._openBrowserBtn.Name = "_openBrowserBtn";
            this._openBrowserBtn.Padding = new System.Windows.Forms.Padding(3);
            this._openBrowserBtn.Size = new System.Drawing.Size(244, 37);
            this._openBrowserBtn.Text = "Open in external browser";
            this._openBrowserBtn.Click += new System.EventHandler(this.OpenBrowserBtn_Click);
            // 
            // _browser
            // 
            this._browser.AllowWebBrowserDrop = false;
            this._browser.ContextMenuStrip = this._contextMenuStrip;
            this._browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._browser.IsWebBrowserContextMenuEnabled = false;
            this._browser.Location = new System.Drawing.Point(0, 40);
            this._browser.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this._browser.MinimumSize = new System.Drawing.Size(33, 38);
            this._browser.Name = "_browser";
            this._browser.ScriptErrorsSuppressed = true;
            this._browser.Size = new System.Drawing.Size(1102, 933);
            this._browser.TabIndex = 2;
            this._browser.WebBrowserShortcutsEnabled = false;
            // 
            // _contextMenuStrip
            // 
            this._contextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this._contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._backMnu,
            this._forwardMnu,
            toolStripSeparator1,
            this._copyMnu,
            toolStripSeparator2,
            this._selectAllMnu});
            this._contextMenuStrip.Name = "_contextMenuStrip";
            this._contextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this._contextMenuStrip.Size = new System.Drawing.Size(246, 144);
            this._contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip_Opening);
            // 
            // _backMnu
            // 
            this._backMnu.Image = ((System.Drawing.Image)(resources.GetObject("_backMnu.Image")));
            this._backMnu.Name = "_backMnu";
            this._backMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Left)));
            this._backMnu.Size = new System.Drawing.Size(245, 32);
            this._backMnu.Text = "&Back";
            this._backMnu.Click += new System.EventHandler(this.BackMnu_Click);
            // 
            // _forwardMnu
            // 
            this._forwardMnu.Image = ((System.Drawing.Image)(resources.GetObject("_forwardMnu.Image")));
            this._forwardMnu.Name = "_forwardMnu";
            this._forwardMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Right)));
            this._forwardMnu.Size = new System.Drawing.Size(245, 32);
            this._forwardMnu.Text = "&Forward";
            this._forwardMnu.Click += new System.EventHandler(this.ForwardMnu_Click);
            // 
            // _copyMnu
            // 
            this._copyMnu.Image = ((System.Drawing.Image)(resources.GetObject("_copyMnu.Image")));
            this._copyMnu.Name = "_copyMnu";
            this._copyMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this._copyMnu.Size = new System.Drawing.Size(245, 32);
            this._copyMnu.Text = "&Copy";
            this._copyMnu.Click += new System.EventHandler(this.CopyMnu_Click);
            // 
            // _selectAllMnu
            // 
            this._selectAllMnu.Name = "_selectAllMnu";
            this._selectAllMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this._selectAllMnu.Size = new System.Drawing.Size(245, 32);
            this._selectAllMnu.Text = "Select &all";
            this._selectAllMnu.Click += new System.EventHandler(this.SelectAllMnu_Click);
            // 
            // HelpDocumentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this._browser);
            this.Controls.Add(this._toolStrip);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "HelpDocumentControl";
            this.Size = new System.Drawing.Size(1102, 973);
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this._contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _forwardBtn;
        private System.Windows.Forms.ToolStripButton _backBtn;
        private System.Windows.Forms.ToolStripButton _homeBtn;
        private System.Windows.Forms.ToolStripButton _openBrowserBtn;
        private System.Windows.Forms.WebBrowser _browser;
        private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _backMnu;
        private System.Windows.Forms.ToolStripMenuItem _forwardMnu;
        private System.Windows.Forms.ToolStripMenuItem _copyMnu;
        private System.Windows.Forms.ToolStripMenuItem _selectAllMnu;
    }
}
