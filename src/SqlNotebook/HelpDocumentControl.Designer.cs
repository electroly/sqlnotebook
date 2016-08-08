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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpDocumentControl));
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._homeBtn = new System.Windows.Forms.ToolStripButton();
            this._backBtn = new System.Windows.Forms.ToolStripButton();
            this._forwardBtn = new System.Windows.Forms.ToolStripButton();
            this._openBrowserBtn = new System.Windows.Forms.ToolStripButton();
            this._browserPanel = new System.Windows.Forms.Panel();
            this._progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
            this._toolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._homeBtn,
            this._backBtn,
            this._forwardBtn,
            this._openBrowserBtn,
            this._progressBar});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this._toolStrip.ShowItemToolTips = false;
            this._toolStrip.Size = new System.Drawing.Size(661, 30);
            this._toolStrip.Stretch = true;
            this._toolStrip.TabIndex = 1;
            // 
            // _homeBtn
            // 
            this._homeBtn.Image = ((System.Drawing.Image)(resources.GetObject("_homeBtn.Image")));
            this._homeBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._homeBtn.Margin = new System.Windows.Forms.Padding(2, 1, 0, 2);
            this._homeBtn.Name = "_homeBtn";
            this._homeBtn.Padding = new System.Windows.Forms.Padding(3);
            this._homeBtn.Size = new System.Drawing.Size(66, 27);
            this._homeBtn.Text = "Home";
            this._homeBtn.Click += new System.EventHandler(this.HomeBtn_Click);
            // 
            // _backBtn
            // 
            this._backBtn.Image = ((System.Drawing.Image)(resources.GetObject("_backBtn.Image")));
            this._backBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._backBtn.Name = "_backBtn";
            this._backBtn.Padding = new System.Windows.Forms.Padding(3);
            this._backBtn.Size = new System.Drawing.Size(58, 27);
            this._backBtn.Text = "Back";
            this._backBtn.Click += new System.EventHandler(this.BackBtn_Click);
            // 
            // _forwardBtn
            // 
            this._forwardBtn.AutoSize = false;
            this._forwardBtn.Image = ((System.Drawing.Image)(resources.GetObject("_forwardBtn.Image")));
            this._forwardBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._forwardBtn.Name = "_forwardBtn";
            this._forwardBtn.Padding = new System.Windows.Forms.Padding(3);
            this._forwardBtn.Size = new System.Drawing.Size(76, 27);
            this._forwardBtn.Text = "Forward";
            this._forwardBtn.Click += new System.EventHandler(this.ForwardBtn_Click);
            // 
            // _openBrowserBtn
            // 
            this._openBrowserBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._openBrowserBtn.Image = ((System.Drawing.Image)(resources.GetObject("_openBrowserBtn.Image")));
            this._openBrowserBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._openBrowserBtn.Margin = new System.Windows.Forms.Padding(0, 1, 3, 2);
            this._openBrowserBtn.Name = "_openBrowserBtn";
            this._openBrowserBtn.Padding = new System.Windows.Forms.Padding(3);
            this._openBrowserBtn.Size = new System.Drawing.Size(164, 27);
            this._openBrowserBtn.Text = "Open in external browser";
            this._openBrowserBtn.Click += new System.EventHandler(this.OpenBrowserBtn_Click);
            // 
            // _browserPanel
            // 
            this._browserPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._browserPanel.Location = new System.Drawing.Point(0, 30);
            this._browserPanel.Name = "_browserPanel";
            this._browserPanel.Size = new System.Drawing.Size(661, 476);
            this._browserPanel.TabIndex = 2;
            // 
            // _progressBar
            // 
            this._progressBar.AutoSize = false;
            this._progressBar.MarqueeAnimationSpeed = 25;
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(100, 15);
            this._progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // HelpDocumentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this._browserPanel);
            this.Controls.Add(this._toolStrip);
            this.Name = "HelpDocumentControl";
            this.Size = new System.Drawing.Size(661, 506);
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _forwardBtn;
        private System.Windows.Forms.ToolStripButton _backBtn;
        private System.Windows.Forms.ToolStripButton _homeBtn;
        private System.Windows.Forms.ToolStripButton _openBrowserBtn;
        private System.Windows.Forms.Panel _browserPanel;
        private System.Windows.Forms.ToolStripProgressBar _progressBar;
    }
}
