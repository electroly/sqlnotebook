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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NoteDocumentControl));
            this._text = new System.Windows.Forms.RichTextBox();
            this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._fontMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._resetFontMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._fontBtn = new System.Windows.Forms.ToolStripButton();
            this._fontSizeUpBtn = new System.Windows.Forms.ToolStripButton();
            this._fontSizeDownBtn = new System.Windows.Forms.ToolStripButton();
            this._boldBtn = new System.Windows.Forms.ToolStripButton();
            this._italicBtn = new System.Windows.Forms.ToolStripButton();
            this._underlineBtn = new System.Windows.Forms.ToolStripButton();
            this._strikeBtn = new System.Windows.Forms.ToolStripButton();
            this._alignLeftBtn = new System.Windows.Forms.ToolStripButton();
            this._alignCenterBtn = new System.Windows.Forms.ToolStripButton();
            this._alignRightBtn = new System.Windows.Forms.ToolStripButton();
            this._contextMenuStrip.SuspendLayout();
            this._toolStripContainer.ContentPanel.SuspendLayout();
            this._toolStripContainer.TopToolStripPanel.SuspendLayout();
            this._toolStripContainer.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _text
            // 
            this._text.AutoWordSelection = true;
            this._text.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._text.ContextMenuStrip = this._contextMenuStrip;
            this._text.Dock = System.Windows.Forms.DockStyle.Fill;
            this._text.EnableAutoDragDrop = true;
            this._text.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._text.Location = new System.Drawing.Point(0, 0);
            this._text.Name = "_text";
            this._text.ShowSelectionMargin = true;
            this._text.Size = new System.Drawing.Size(567, 462);
            this._text.TabIndex = 0;
            this._text.Text = "Text";
            this._text.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.Text_LinkClicked);
            this._text.SelectionChanged += new System.EventHandler(this.Text_SelectionChanged);
            this._text.TextChanged += new System.EventHandler(this.Text_TextChanged);
            this._text.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Text_PreviewKeyDown);
            // 
            // _contextMenuStrip
            // 
            this._contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fontMnu,
            this._resetFontMnu});
            this._contextMenuStrip.Name = "_contextMenuStrip";
            this._contextMenuStrip.Size = new System.Drawing.Size(150, 48);
            // 
            // _fontMnu
            // 
            this._fontMnu.Name = "_fontMnu";
            this._fontMnu.Size = new System.Drawing.Size(149, 22);
            this._fontMnu.Text = "Change &font...";
            this._fontMnu.Click += new System.EventHandler(this.FontMnu_Click);
            // 
            // _resetFontMnu
            // 
            this._resetFontMnu.Name = "_resetFontMnu";
            this._resetFontMnu.Size = new System.Drawing.Size(149, 22);
            this._resetFontMnu.Text = "&Reset font";
            this._resetFontMnu.Click += new System.EventHandler(this.ResetFontMnu_Click);
            // 
            // _toolStripContainer
            // 
            // 
            // _toolStripContainer.ContentPanel
            // 
            this._toolStripContainer.ContentPanel.Controls.Add(this._text);
            this._toolStripContainer.ContentPanel.Size = new System.Drawing.Size(567, 462);
            this._toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this._toolStripContainer.Name = "_toolStripContainer";
            this._toolStripContainer.Size = new System.Drawing.Size(567, 492);
            this._toolStripContainer.TabIndex = 1;
            this._toolStripContainer.Text = "toolStripContainer1";
            // 
            // _toolStripContainer.TopToolStripPanel
            // 
            this._toolStripContainer.TopToolStripPanel.BackColor = System.Drawing.SystemColors.Control;
            this._toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fontBtn,
            this._fontSizeUpBtn,
            this._fontSizeDownBtn,
            this._boldBtn,
            this._italicBtn,
            this._underlineBtn,
            this._strikeBtn,
            this._alignLeftBtn,
            this._alignCenterBtn,
            this._alignRightBtn});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(567, 30);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 0;
            // 
            // _fontBtn
            // 
            this._fontBtn.AutoSize = false;
            this._fontBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._fontBtn.Image = ((System.Drawing.Image)(resources.GetObject("_fontBtn.Image")));
            this._fontBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._fontBtn.Margin = new System.Windows.Forms.Padding(2, 1, 0, 2);
            this._fontBtn.Name = "_fontBtn";
            this._fontBtn.Padding = new System.Windows.Forms.Padding(3);
            this._fontBtn.Size = new System.Drawing.Size(27, 27);
            this._fontBtn.Text = "toolStripButton11";
            this._fontBtn.Click += new System.EventHandler(this.FontMnu_Click);
            // 
            // _fontSizeUpBtn
            // 
            this._fontSizeUpBtn.AutoSize = false;
            this._fontSizeUpBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._fontSizeUpBtn.Image = ((System.Drawing.Image)(resources.GetObject("_fontSizeUpBtn.Image")));
            this._fontSizeUpBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._fontSizeUpBtn.Margin = new System.Windows.Forms.Padding(7, 1, 0, 2);
            this._fontSizeUpBtn.Name = "_fontSizeUpBtn";
            this._fontSizeUpBtn.Padding = new System.Windows.Forms.Padding(3);
            this._fontSizeUpBtn.Size = new System.Drawing.Size(27, 27);
            this._fontSizeUpBtn.Text = "toolStripButton1";
            this._fontSizeUpBtn.Click += new System.EventHandler(this.FontSizeUpBtn_Click);
            // 
            // _fontSizeDownBtn
            // 
            this._fontSizeDownBtn.AutoSize = false;
            this._fontSizeDownBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._fontSizeDownBtn.Image = ((System.Drawing.Image)(resources.GetObject("_fontSizeDownBtn.Image")));
            this._fontSizeDownBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._fontSizeDownBtn.Name = "_fontSizeDownBtn";
            this._fontSizeDownBtn.Padding = new System.Windows.Forms.Padding(3);
            this._fontSizeDownBtn.Size = new System.Drawing.Size(27, 27);
            this._fontSizeDownBtn.Text = "toolStripButton2";
            this._fontSizeDownBtn.Click += new System.EventHandler(this.FontSizeDownBtn_Click);
            // 
            // _boldBtn
            // 
            this._boldBtn.AutoSize = false;
            this._boldBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._boldBtn.Image = ((System.Drawing.Image)(resources.GetObject("_boldBtn.Image")));
            this._boldBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._boldBtn.Margin = new System.Windows.Forms.Padding(7, 1, 0, 2);
            this._boldBtn.Name = "_boldBtn";
            this._boldBtn.Padding = new System.Windows.Forms.Padding(3);
            this._boldBtn.Size = new System.Drawing.Size(27, 27);
            this._boldBtn.Text = "toolStripButton3";
            this._boldBtn.Click += new System.EventHandler(this.BoldBtn_Click);
            // 
            // _italicBtn
            // 
            this._italicBtn.AutoSize = false;
            this._italicBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._italicBtn.Image = ((System.Drawing.Image)(resources.GetObject("_italicBtn.Image")));
            this._italicBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._italicBtn.Name = "_italicBtn";
            this._italicBtn.Padding = new System.Windows.Forms.Padding(3);
            this._italicBtn.Size = new System.Drawing.Size(27, 27);
            this._italicBtn.Text = "toolStripButton4";
            this._italicBtn.Click += new System.EventHandler(this.ItalicBtn_Click);
            // 
            // _underlineBtn
            // 
            this._underlineBtn.AutoSize = false;
            this._underlineBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._underlineBtn.Image = ((System.Drawing.Image)(resources.GetObject("_underlineBtn.Image")));
            this._underlineBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._underlineBtn.Name = "_underlineBtn";
            this._underlineBtn.Padding = new System.Windows.Forms.Padding(3);
            this._underlineBtn.Size = new System.Drawing.Size(27, 27);
            this._underlineBtn.Text = "toolStripButton5";
            this._underlineBtn.Click += new System.EventHandler(this.UnderlineBtn_Click);
            // 
            // _strikeBtn
            // 
            this._strikeBtn.AutoSize = false;
            this._strikeBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._strikeBtn.Image = ((System.Drawing.Image)(resources.GetObject("_strikeBtn.Image")));
            this._strikeBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._strikeBtn.Name = "_strikeBtn";
            this._strikeBtn.Padding = new System.Windows.Forms.Padding(3);
            this._strikeBtn.Size = new System.Drawing.Size(27, 27);
            this._strikeBtn.Text = "toolStripButton6";
            this._strikeBtn.Click += new System.EventHandler(this.StrikeBtn_Click);
            // 
            // _alignLeftBtn
            // 
            this._alignLeftBtn.AutoSize = false;
            this._alignLeftBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._alignLeftBtn.Image = ((System.Drawing.Image)(resources.GetObject("_alignLeftBtn.Image")));
            this._alignLeftBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._alignLeftBtn.Margin = new System.Windows.Forms.Padding(7, 1, 0, 2);
            this._alignLeftBtn.Name = "_alignLeftBtn";
            this._alignLeftBtn.Padding = new System.Windows.Forms.Padding(3);
            this._alignLeftBtn.Size = new System.Drawing.Size(27, 27);
            this._alignLeftBtn.Text = "toolStripButton7";
            this._alignLeftBtn.Click += new System.EventHandler(this.AlignLeftBtn_Click);
            // 
            // _alignCenterBtn
            // 
            this._alignCenterBtn.AutoSize = false;
            this._alignCenterBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._alignCenterBtn.Image = ((System.Drawing.Image)(resources.GetObject("_alignCenterBtn.Image")));
            this._alignCenterBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._alignCenterBtn.Name = "_alignCenterBtn";
            this._alignCenterBtn.Padding = new System.Windows.Forms.Padding(3);
            this._alignCenterBtn.Size = new System.Drawing.Size(27, 27);
            this._alignCenterBtn.Text = "toolStripButton8";
            this._alignCenterBtn.Click += new System.EventHandler(this.AlignCenterBtn_Click);
            // 
            // _alignRightBtn
            // 
            this._alignRightBtn.AutoSize = false;
            this._alignRightBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._alignRightBtn.Image = ((System.Drawing.Image)(resources.GetObject("_alignRightBtn.Image")));
            this._alignRightBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._alignRightBtn.Name = "_alignRightBtn";
            this._alignRightBtn.Padding = new System.Windows.Forms.Padding(3);
            this._alignRightBtn.Size = new System.Drawing.Size(27, 27);
            this._alignRightBtn.Text = "toolStripButton9";
            this._alignRightBtn.Click += new System.EventHandler(this.AlignRightBtn_Click);
            // 
            // NoteDocumentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this._toolStripContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "NoteDocumentControl";
            this.Size = new System.Drawing.Size(567, 492);
            this._contextMenuStrip.ResumeLayout(false);
            this._toolStripContainer.ContentPanel.ResumeLayout(false);
            this._toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this._toolStripContainer.TopToolStripPanel.PerformLayout();
            this._toolStripContainer.ResumeLayout(false);
            this._toolStripContainer.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox _text;
        private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _fontMnu;
        private System.Windows.Forms.ToolStripMenuItem _resetFontMnu;
        private System.Windows.Forms.ToolStripContainer _toolStripContainer;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton _fontSizeUpBtn;
        private System.Windows.Forms.ToolStripButton _fontSizeDownBtn;
        private System.Windows.Forms.ToolStripButton _boldBtn;
        private System.Windows.Forms.ToolStripButton _italicBtn;
        private System.Windows.Forms.ToolStripButton _underlineBtn;
        private System.Windows.Forms.ToolStripButton _strikeBtn;
        private System.Windows.Forms.ToolStripButton _alignLeftBtn;
        private System.Windows.Forms.ToolStripButton _alignCenterBtn;
        private System.Windows.Forms.ToolStripButton _alignRightBtn;
        private System.Windows.Forms.ToolStripButton _fontBtn;
    }
}
