namespace SqlNotebook {
    partial class ConsoleDocumentControl {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleDocumentControl));
            this._browser = new System.Windows.Forms.WebBrowser();
            this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._cutMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._copyMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._pasteMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._selectAllMnu = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(159, 6);
            // 
            // _browser
            // 
            this._browser.AllowNavigation = false;
            this._browser.AllowWebBrowserDrop = false;
            this._browser.ContextMenuStrip = this._contextMenuStrip;
            this._browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._browser.IsWebBrowserContextMenuEnabled = false;
            this._browser.Location = new System.Drawing.Point(0, 0);
            this._browser.MinimumSize = new System.Drawing.Size(20, 20);
            this._browser.Name = "_browser";
            this._browser.ScriptErrorsSuppressed = true;
            this._browser.Size = new System.Drawing.Size(724, 732);
            this._browser.TabIndex = 0;
            // 
            // _contextMenuStrip
            // 
            this._contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._cutMnu,
            this._copyMnu,
            this._pasteMnu,
            toolStripSeparator1,
            this._selectAllMnu});
            this._contextMenuStrip.Name = "_contextMenuStrip";
            this._contextMenuStrip.Size = new System.Drawing.Size(163, 120);
            this._contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip_Opening);
            // 
            // _cutMnu
            // 
            this._cutMnu.Image = ((System.Drawing.Image)(resources.GetObject("_cutMnu.Image")));
            this._cutMnu.Name = "_cutMnu";
            this._cutMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this._cutMnu.Size = new System.Drawing.Size(162, 22);
            this._cutMnu.Text = "Cu&t";
            this._cutMnu.Click += new System.EventHandler(this.CutMnu_Click);
            // 
            // _copyMnu
            // 
            this._copyMnu.Image = ((System.Drawing.Image)(resources.GetObject("_copyMnu.Image")));
            this._copyMnu.Name = "_copyMnu";
            this._copyMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this._copyMnu.Size = new System.Drawing.Size(162, 22);
            this._copyMnu.Text = "&Copy";
            this._copyMnu.Click += new System.EventHandler(this.CopyMnu_Click);
            // 
            // _pasteMnu
            // 
            this._pasteMnu.Image = ((System.Drawing.Image)(resources.GetObject("_pasteMnu.Image")));
            this._pasteMnu.Name = "_pasteMnu";
            this._pasteMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this._pasteMnu.Size = new System.Drawing.Size(162, 22);
            this._pasteMnu.Text = "&Paste";
            this._pasteMnu.Click += new System.EventHandler(this.PasteMnu_Click);
            // 
            // _selectAllMnu
            // 
            this._selectAllMnu.Name = "_selectAllMnu";
            this._selectAllMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this._selectAllMnu.Size = new System.Drawing.Size(162, 22);
            this._selectAllMnu.Text = "Select &all";
            this._selectAllMnu.Click += new System.EventHandler(this.SelectAllMnu_Click);
            // 
            // ConsoleDocumentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this._browser);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ConsoleDocumentControl";
            this.Size = new System.Drawing.Size(724, 732);
            this._contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser _browser;
        private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _cutMnu;
        private System.Windows.Forms.ToolStripMenuItem _copyMnu;
        private System.Windows.Forms.ToolStripMenuItem _pasteMnu;
        private System.Windows.Forms.ToolStripMenuItem _selectAllMnu;
    }
}
