namespace SqlNotebook {
    partial class ExplorerControl {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExplorerControl));
            this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._openMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._deleteMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._renameMnu = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._toolbarNewPageButton = new System.Windows.Forms.ToolStripButton();
            this._toolbarNewScriptButton = new System.Windows.Forms.ToolStripButton();
            this._contextMenuStrip.SuspendLayout();
            this._toolStripContainer.TopToolStripPanel.SuspendLayout();
            this._toolStripContainer.SuspendLayout();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _contextMenuStrip
            // 
            this._contextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this._contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._openMnu,
            this.toolStripSeparator1,
            this._deleteMnu,
            this._renameMnu});
            this._contextMenuStrip.Name = "_contextMenuStrip";
            this._contextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this._contextMenuStrip.Size = new System.Drawing.Size(148, 106);
            this._contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip_Opening);
            // 
            // _openMnu
            // 
            this._openMnu.Name = "_openMnu";
            this._openMnu.Size = new System.Drawing.Size(147, 32);
            this._openMnu.Text = "&Open";
            this._openMnu.Click += new System.EventHandler(this.OpenMnu_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(144, 6);
            // 
            // _deleteMnu
            // 
            this._deleteMnu.Name = "_deleteMnu";
            this._deleteMnu.Size = new System.Drawing.Size(147, 32);
            this._deleteMnu.Text = "&Delete";
            this._deleteMnu.Click += new System.EventHandler(this.DeleteMnu_Click);
            // 
            // _renameMnu
            // 
            this._renameMnu.Name = "_renameMnu";
            this._renameMnu.Size = new System.Drawing.Size(147, 32);
            this._renameMnu.Text = "&Rename";
            this._renameMnu.Click += new System.EventHandler(this.RenameMnu_Click);
            // 
            // _toolStripContainer
            // 
            // 
            // _toolStripContainer.ContentPanel
            // 
            this._toolStripContainer.ContentPanel.Size = new System.Drawing.Size(340, 574);
            this._toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this._toolStripContainer.Name = "_toolStripContainer";
            this._toolStripContainer.Size = new System.Drawing.Size(340, 608);
            this._toolStripContainer.TabIndex = 2;
            this._toolStripContainer.Text = "toolStripContainer1";
            // 
            // _toolStripContainer.TopToolStripPanel
            // 
            this._toolStripContainer.TopToolStripPanel.BackColor = System.Drawing.SystemColors.Window;
            this._toolStripContainer.TopToolStripPanel.Controls.Add(this._toolStrip);
            // 
            // _toolStrip
            // 
            this._toolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._toolbarNewPageButton,
            this._toolbarNewScriptButton});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this._toolStrip.Size = new System.Drawing.Size(340, 34);
            this._toolStrip.Stretch = true;
            this._toolStrip.TabIndex = 4;
            // 
            // _toolbarNewPageButton
            // 
            this._toolbarNewPageButton.Image = ((System.Drawing.Image)(resources.GetObject("_toolbarNewPageButton.Image")));
            this._toolbarNewPageButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolbarNewPageButton.Name = "_toolbarNewPageButton";
            this._toolbarNewPageButton.Size = new System.Drawing.Size(119, 29);
            this._toolbarNewPageButton.Text = "Add page";
            this._toolbarNewPageButton.ToolTipText = "Add a new page to the notebook";
            this._toolbarNewPageButton.Click += new System.EventHandler(this.ToolbarNewPageButton_Click);
            // 
            // _toolbarNewScriptButton
            // 
            this._toolbarNewScriptButton.Image = ((System.Drawing.Image)(resources.GetObject("_toolbarNewScriptButton.Image")));
            this._toolbarNewScriptButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolbarNewScriptButton.Name = "_toolbarNewScriptButton";
            this._toolbarNewScriptButton.Size = new System.Drawing.Size(122, 29);
            this._toolbarNewScriptButton.Text = "Add script";
            this._toolbarNewScriptButton.ToolTipText = "Add a new SQL script to the notebook";
            this._toolbarNewScriptButton.Click += new System.EventHandler(this.ToolbarNewScriptButton_Click);
            // 
            // ExplorerControl
            // 
            this.Controls.Add(this._toolStripContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "ExplorerControl";
            this.Size = new System.Drawing.Size(340, 608);
            this._contextMenuStrip.ResumeLayout(false);
            this._toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this._toolStripContainer.TopToolStripPanel.PerformLayout();
            this._toolStripContainer.ResumeLayout(false);
            this._toolStripContainer.PerformLayout();
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _deleteMnu;
        private System.Windows.Forms.ToolStripMenuItem _renameMnu;
        private System.Windows.Forms.ToolStripContainer _toolStripContainer;
        private System.Windows.Forms.ToolStripMenuItem _openMnu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _toolbarNewPageButton;
        private System.Windows.Forms.ToolStripButton _toolbarNewScriptButton;
    }
}