namespace SqlNotebook {
    partial class NoteDocumentControl {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NoteDocumentControl));
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._fontNameCmb = new System.Windows.Forms.ToolStripComboBox();
            this._fontSizeCmb = new System.Windows.Forms.ToolStripComboBox();
            this._boldBtn = new System.Windows.Forms.ToolStripButton();
            this._italicBtn = new System.Windows.Forms.ToolStripButton();
            this._underlineBtn = new System.Windows.Forms.ToolStripButton();
            this._alignLeftBtn = new System.Windows.Forms.ToolStripButton();
            this._alignCenterBtn = new System.Windows.Forms.ToolStripButton();
            this._alignRightBtn = new System.Windows.Forms.ToolStripButton();
            this._outdentBtn = new System.Windows.Forms.ToolStripButton();
            this._indentBtn = new System.Windows.Forms.ToolStripButton();
            this._bulletListBtn = new System.Windows.Forms.ToolStripButton();
            this._numberListBtn = new System.Windows.Forms.ToolStripButton();
            this._tableBtn = new System.Windows.Forms.ToolStripButton();
            this._hrBtn = new System.Windows.Forms.ToolStripButton();
            this._browser = new System.Windows.Forms.WebBrowser();
            this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._cutMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._copyMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._pasteMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._selectAllMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._timer = new System.Windows.Forms.Timer(this.components);
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._toolStrip.SuspendLayout();
            this._contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(159, 6);
            // 
            // _toolStrip
            // 
            this._toolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fontNameCmb,
            this._fontSizeCmb,
            this._boldBtn,
            this._italicBtn,
            this._underlineBtn,
            this._alignLeftBtn,
            this._alignCenterBtn,
            this._alignRightBtn,
            this._outdentBtn,
            this._indentBtn,
            this._bulletListBtn,
            this._numberListBtn,
            this._tableBtn,
            this._hrBtn});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this._toolStrip.Size = new System.Drawing.Size(662, 30);
            this._toolStrip.Stretch = true;
            this._toolStrip.TabIndex = 1;
            // 
            // _fontNameCmb
            // 
            this._fontNameCmb.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this._fontNameCmb.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this._fontNameCmb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._fontNameCmb.Margin = new System.Windows.Forms.Padding(2, 0, 1, 0);
            this._fontNameCmb.Name = "_fontNameCmb";
            this._fontNameCmb.Size = new System.Drawing.Size(125, 30);
            this._fontNameCmb.TextChanged += new System.EventHandler(this.FontNameCmb_TextChanged);
            // 
            // _fontSizeCmb
            // 
            this._fontSizeCmb.AutoSize = false;
            this._fontSizeCmb.DropDownWidth = 50;
            this._fontSizeCmb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._fontSizeCmb.Items.AddRange(new object[] {
            "8",
            "10",
            "12",
            "14",
            "18",
            "24",
            "36"});
            this._fontSizeCmb.Margin = new System.Windows.Forms.Padding(2, 0, 1, 0);
            this._fontSizeCmb.Name = "_fontSizeCmb";
            this._fontSizeCmb.Size = new System.Drawing.Size(50, 23);
            this._fontSizeCmb.TextChanged += new System.EventHandler(this.FontSizeCmb_TextChanged);
            // 
            // _boldBtn
            // 
            this._boldBtn.AutoSize = false;
            this._boldBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._boldBtn.Image = ((System.Drawing.Image)(resources.GetObject("_boldBtn.Image")));
            this._boldBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._boldBtn.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this._boldBtn.Name = "_boldBtn";
            this._boldBtn.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this._boldBtn.Size = new System.Drawing.Size(27, 27);
            this._boldBtn.ToolTipText = "Bold (Ctrl+B)";
            this._boldBtn.Click += new System.EventHandler(this.BoldBtn_Click);
            // 
            // _italicBtn
            // 
            this._italicBtn.AutoSize = false;
            this._italicBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._italicBtn.Image = ((System.Drawing.Image)(resources.GetObject("_italicBtn.Image")));
            this._italicBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._italicBtn.Name = "_italicBtn";
            this._italicBtn.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this._italicBtn.Size = new System.Drawing.Size(27, 27);
            this._italicBtn.ToolTipText = "Italic (Ctrl+I)";
            this._italicBtn.Click += new System.EventHandler(this.ItalicBtn_Click);
            // 
            // _underlineBtn
            // 
            this._underlineBtn.AutoSize = false;
            this._underlineBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._underlineBtn.Image = ((System.Drawing.Image)(resources.GetObject("_underlineBtn.Image")));
            this._underlineBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._underlineBtn.Name = "_underlineBtn";
            this._underlineBtn.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this._underlineBtn.Size = new System.Drawing.Size(27, 27);
            this._underlineBtn.ToolTipText = "Underline (Ctrl+U)";
            this._underlineBtn.Click += new System.EventHandler(this.UnderlineBtn_Click);
            // 
            // _alignLeftBtn
            // 
            this._alignLeftBtn.AutoSize = false;
            this._alignLeftBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._alignLeftBtn.Image = ((System.Drawing.Image)(resources.GetObject("_alignLeftBtn.Image")));
            this._alignLeftBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._alignLeftBtn.Margin = new System.Windows.Forms.Padding(7, 1, 0, 2);
            this._alignLeftBtn.Name = "_alignLeftBtn";
            this._alignLeftBtn.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this._alignLeftBtn.Size = new System.Drawing.Size(27, 27);
            this._alignLeftBtn.ToolTipText = "Align left (Ctrl+L)";
            this._alignLeftBtn.Click += new System.EventHandler(this.AlignLeftBtn_Click);
            // 
            // _alignCenterBtn
            // 
            this._alignCenterBtn.AutoSize = false;
            this._alignCenterBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._alignCenterBtn.Image = ((System.Drawing.Image)(resources.GetObject("_alignCenterBtn.Image")));
            this._alignCenterBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._alignCenterBtn.Name = "_alignCenterBtn";
            this._alignCenterBtn.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this._alignCenterBtn.Size = new System.Drawing.Size(27, 27);
            this._alignCenterBtn.ToolTipText = "Center (Ctrl+E)";
            this._alignCenterBtn.Click += new System.EventHandler(this.AlignCenterBtn_Click);
            // 
            // _alignRightBtn
            // 
            this._alignRightBtn.AutoSize = false;
            this._alignRightBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._alignRightBtn.Image = ((System.Drawing.Image)(resources.GetObject("_alignRightBtn.Image")));
            this._alignRightBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._alignRightBtn.Name = "_alignRightBtn";
            this._alignRightBtn.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this._alignRightBtn.Size = new System.Drawing.Size(27, 27);
            this._alignRightBtn.ToolTipText = "Align right (Ctrl+R)";
            this._alignRightBtn.Click += new System.EventHandler(this.AlignRightBtn_Click);
            // 
            // _outdentBtn
            // 
            this._outdentBtn.AutoSize = false;
            this._outdentBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._outdentBtn.Image = ((System.Drawing.Image)(resources.GetObject("_outdentBtn.Image")));
            this._outdentBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._outdentBtn.Margin = new System.Windows.Forms.Padding(7, 1, 0, 2);
            this._outdentBtn.Name = "_outdentBtn";
            this._outdentBtn.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this._outdentBtn.Size = new System.Drawing.Size(27, 27);
            this._outdentBtn.ToolTipText = "Decrease indentation";
            this._outdentBtn.Click += new System.EventHandler(this.OutdentBtn_Click);
            // 
            // _indentBtn
            // 
            this._indentBtn.AutoSize = false;
            this._indentBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._indentBtn.Image = ((System.Drawing.Image)(resources.GetObject("_indentBtn.Image")));
            this._indentBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._indentBtn.Name = "_indentBtn";
            this._indentBtn.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this._indentBtn.Size = new System.Drawing.Size(27, 27);
            this._indentBtn.ToolTipText = "Increase indentation";
            this._indentBtn.Click += new System.EventHandler(this.IndentBtn_Click);
            // 
            // _bulletListBtn
            // 
            this._bulletListBtn.AutoSize = false;
            this._bulletListBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._bulletListBtn.Image = ((System.Drawing.Image)(resources.GetObject("_bulletListBtn.Image")));
            this._bulletListBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._bulletListBtn.Margin = new System.Windows.Forms.Padding(7, 1, 0, 2);
            this._bulletListBtn.Name = "_bulletListBtn";
            this._bulletListBtn.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this._bulletListBtn.Size = new System.Drawing.Size(27, 27);
            this._bulletListBtn.ToolTipText = "Bulleted list";
            this._bulletListBtn.Click += new System.EventHandler(this.BulletListBtn_Click);
            // 
            // _numberListBtn
            // 
            this._numberListBtn.AutoSize = false;
            this._numberListBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._numberListBtn.Image = ((System.Drawing.Image)(resources.GetObject("_numberListBtn.Image")));
            this._numberListBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._numberListBtn.Name = "_numberListBtn";
            this._numberListBtn.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this._numberListBtn.Size = new System.Drawing.Size(27, 27);
            this._numberListBtn.ToolTipText = "Numbered list";
            this._numberListBtn.Click += new System.EventHandler(this.NumberListBtn_Click);
            // 
            // _tableBtn
            // 
            this._tableBtn.AutoSize = false;
            this._tableBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._tableBtn.Image = ((System.Drawing.Image)(resources.GetObject("_tableBtn.Image")));
            this._tableBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tableBtn.Margin = new System.Windows.Forms.Padding(7, 1, 0, 2);
            this._tableBtn.Name = "_tableBtn";
            this._tableBtn.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this._tableBtn.Size = new System.Drawing.Size(27, 27);
            this._tableBtn.ToolTipText = "Insert table...";
            this._tableBtn.Click += new System.EventHandler(this.TableBtn_Click);
            // 
            // _hrBtn
            // 
            this._hrBtn.AutoSize = false;
            this._hrBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._hrBtn.Image = ((System.Drawing.Image)(resources.GetObject("_hrBtn.Image")));
            this._hrBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._hrBtn.Name = "_hrBtn";
            this._hrBtn.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this._hrBtn.Size = new System.Drawing.Size(27, 27);
            this._hrBtn.ToolTipText = "Insert horizontal line";
            this._hrBtn.Click += new System.EventHandler(this.HrBtn_Click);
            // 
            // _browser
            // 
            this._browser.AllowNavigation = false;
            this._browser.AllowWebBrowserDrop = false;
            this._browser.ContextMenuStrip = this._contextMenuStrip;
            this._browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._browser.IsWebBrowserContextMenuEnabled = false;
            this._browser.Location = new System.Drawing.Point(0, 30);
            this._browser.MinimumSize = new System.Drawing.Size(20, 20);
            this._browser.Name = "_browser";
            this._browser.ScriptErrorsSuppressed = true;
            this._browser.Size = new System.Drawing.Size(662, 504);
            this._browser.TabIndex = 2;
            this._browser.WebBrowserShortcutsEnabled = false;
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
            this._contextMenuStrip.Size = new System.Drawing.Size(163, 98);
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
            // _timer
            // 
            this._timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // NoteDocumentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this._browser);
            this.Controls.Add(this._toolStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "NoteDocumentControl";
            this.Size = new System.Drawing.Size(662, 534);
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this._contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripComboBox _fontNameCmb;
        private System.Windows.Forms.ToolStripButton _boldBtn;
        private System.Windows.Forms.ToolStripComboBox _fontSizeCmb;
        private System.Windows.Forms.ToolStripButton _italicBtn;
        private System.Windows.Forms.ToolStripButton _underlineBtn;
        private System.Windows.Forms.ToolStripButton _alignLeftBtn;
        private System.Windows.Forms.ToolStripButton _alignCenterBtn;
        private System.Windows.Forms.ToolStripButton _alignRightBtn;
        private System.Windows.Forms.ToolStripButton _bulletListBtn;
        private System.Windows.Forms.ToolStripButton _numberListBtn;
        private System.Windows.Forms.ToolStripButton _tableBtn;
        private System.Windows.Forms.ToolStripButton _hrBtn;
        private System.Windows.Forms.ToolStripButton _outdentBtn;
        private System.Windows.Forms.ToolStripButton _indentBtn;
        private System.Windows.Forms.WebBrowser _browser;
        private System.Windows.Forms.Timer _timer;
        private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _cutMnu;
        private System.Windows.Forms.ToolStripMenuItem _copyMnu;
        private System.Windows.Forms.ToolStripMenuItem _pasteMnu;
        private System.Windows.Forms.ToolStripMenuItem _selectAllMnu;
    }
}
