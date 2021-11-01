namespace SqlNotebook {
    partial class TableDocumentControl {
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
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._scriptBtn = new System.Windows.Forms.ToolStripButton();
            this._tablePanel = new System.Windows.Forms.Panel();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
            this._toolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._scriptBtn});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this._toolStrip.ShowItemToolTips = false;
            this._toolStrip.Size = new System.Drawing.Size(745, 38);
            this._toolStrip.Stretch = true;
            this._toolStrip.TabIndex = 3;
            // 
            // _scriptBtn
            // 
            this._scriptBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._scriptBtn.Margin = new System.Windows.Forms.Padding(2, 1, 0, 2);
            this._scriptBtn.Name = "_scriptBtn";
            this._scriptBtn.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this._scriptBtn.Size = new System.Drawing.Size(148, 35);
            this._scriptBtn.Text = "Convert to script";
            this._scriptBtn.Click += new System.EventHandler(this.ScriptBtn_Click);
            // 
            // _tablePanel
            // 
            this._tablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tablePanel.Location = new System.Drawing.Point(0, 38);
            this._tablePanel.Margin = new System.Windows.Forms.Padding(0);
            this._tablePanel.Name = "_tablePanel";
            this._tablePanel.Size = new System.Drawing.Size(745, 601);
            this._tablePanel.TabIndex = 4;
            // 
            // TableDocumentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tablePanel);
            this.Controls.Add(this._toolStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "TableDocumentControl";
            this.Size = new System.Drawing.Size(745, 639);
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _scriptBtn;
        private System.Windows.Forms.Panel _tablePanel;
    }
}
