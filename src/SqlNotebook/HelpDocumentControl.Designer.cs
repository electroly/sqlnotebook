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
            System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpDocumentControl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._homeBtn = new System.Windows.Forms.ToolStripButton();
            this._backBtn = new System.Windows.Forms.ToolStripButton();
            this._forwardBtn = new System.Windows.Forms.ToolStripButton();
            this._titleLbl = new System.Windows.Forms.ToolStripLabel();
            this._browser = new System.Windows.Forms.WebBrowser();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 29);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Window;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._homeBtn,
            this._backBtn,
            this._forwardBtn,
            toolStripSeparator1,
            this._titleLbl});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(661, 29);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 1;
            // 
            // _homeBtn
            // 
            this._homeBtn.Image = ((System.Drawing.Image)(resources.GetObject("_homeBtn.Image")));
            this._homeBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._homeBtn.Name = "_homeBtn";
            this._homeBtn.Padding = new System.Windows.Forms.Padding(3);
            this._homeBtn.Size = new System.Drawing.Size(66, 26);
            this._homeBtn.Text = "Home";
            this._homeBtn.Click += new System.EventHandler(this.HomeBtn_Click);
            // 
            // _backBtn
            // 
            this._backBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._backBtn.Image = ((System.Drawing.Image)(resources.GetObject("_backBtn.Image")));
            this._backBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._backBtn.Name = "_backBtn";
            this._backBtn.Padding = new System.Windows.Forms.Padding(3);
            this._backBtn.Size = new System.Drawing.Size(26, 26);
            this._backBtn.Text = "toolStripButton11";
            this._backBtn.Click += new System.EventHandler(this.BackBtn_Click);
            // 
            // _forwardBtn
            // 
            this._forwardBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._forwardBtn.Image = ((System.Drawing.Image)(resources.GetObject("_forwardBtn.Image")));
            this._forwardBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._forwardBtn.Name = "_forwardBtn";
            this._forwardBtn.Padding = new System.Windows.Forms.Padding(3);
            this._forwardBtn.Size = new System.Drawing.Size(26, 26);
            this._forwardBtn.Text = "toolStripButton11";
            this._forwardBtn.Click += new System.EventHandler(this.ForwardBtn_Click);
            // 
            // _titleLbl
            // 
            this._titleLbl.Name = "_titleLbl";
            this._titleLbl.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this._titleLbl.Size = new System.Drawing.Size(57, 26);
            this._titleLbl.Text = "(no title)";
            // 
            // _browser
            // 
            this._browser.AllowWebBrowserDrop = false;
            this._browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._browser.Location = new System.Drawing.Point(0, 29);
            this._browser.MinimumSize = new System.Drawing.Size(20, 20);
            this._browser.Name = "_browser";
            this._browser.ScriptErrorsSuppressed = true;
            this._browser.Size = new System.Drawing.Size(661, 477);
            this._browser.TabIndex = 2;
            // 
            // HelpDocumentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this._browser);
            this.Controls.Add(this.toolStrip1);
            this.Name = "HelpDocumentControl";
            this.Size = new System.Drawing.Size(661, 506);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton _forwardBtn;
        private System.Windows.Forms.WebBrowser _browser;
        private System.Windows.Forms.ToolStripButton _backBtn;
        private System.Windows.Forms.ToolStripLabel _titleLbl;
        private System.Windows.Forms.ToolStripButton _homeBtn;
    }
}
