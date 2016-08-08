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
            System.Windows.Forms.ProgressBar progressBar1;
            this._loadingPanel = new System.Windows.Forms.Panel();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            this._loadingPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _loadingPanel
            // 
            this._loadingPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._loadingPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._loadingPanel.Controls.Add(progressBar1);
            this._loadingPanel.Location = new System.Drawing.Point(0, 0);
            this._loadingPanel.Name = "_loadingPanel";
            this._loadingPanel.Size = new System.Drawing.Size(567, 30);
            this._loadingPanel.TabIndex = 0;
            // 
            // progressBar1
            // 
            progressBar1.Location = new System.Drawing.Point(8, 7);
            progressBar1.MarqueeAnimationSpeed = 25;
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(100, 15);
            progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            progressBar1.TabIndex = 0;
            // 
            // NoteDocumentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this._loadingPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "NoteDocumentControl";
            this.Size = new System.Drawing.Size(567, 492);
            this._loadingPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _loadingPanel;
    }
}
