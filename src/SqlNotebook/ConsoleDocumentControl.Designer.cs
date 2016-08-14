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
            this._browser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // _browser
            // 
            this._browser.AllowNavigation = false;
            this._browser.AllowWebBrowserDrop = false;
            this._browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._browser.IsWebBrowserContextMenuEnabled = false;
            this._browser.Location = new System.Drawing.Point(0, 0);
            this._browser.MinimumSize = new System.Drawing.Size(20, 20);
            this._browser.Name = "_browser";
            this._browser.ScriptErrorsSuppressed = true;
            this._browser.Size = new System.Drawing.Size(724, 732);
            this._browser.TabIndex = 0;
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser _browser;
    }
}
