namespace SqlNotebook {
    partial class LoadingContainerControl {
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
            this._loadingLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _loadingLbl
            // 
            this._loadingLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._loadingLbl.ForeColor = System.Drawing.Color.Silver;
            this._loadingLbl.Location = new System.Drawing.Point(0, 0);
            this._loadingLbl.Name = "_loadingLbl";
            this._loadingLbl.Size = new System.Drawing.Size(531, 444);
            this._loadingLbl.TabIndex = 0;
            this._loadingLbl.Text = "Please wait...";
            this._loadingLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoadingContainerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this._loadingLbl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "LoadingContainerControl";
            this.Size = new System.Drawing.Size(531, 444);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _loadingLbl;
    }
}
