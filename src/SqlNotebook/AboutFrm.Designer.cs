namespace SqlNotebook {
    partial class AboutFrm {
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
            System.Windows.Forms.TextBox textBox1;
            System.Windows.Forms.Button _okBtn;
            System.Windows.Forms.PictureBox pictureBox1;
            textBox1 = new System.Windows.Forms.TextBox();
            _okBtn = new System.Windows.Forms.Button();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBox1.Location = new System.Drawing.Point(66, 12);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new System.Drawing.Size(266, 127);
            textBox1.TabIndex = 1;
            textBox1.Text = "SQL Notebook\r\nCopyright © 2016 Brian Luft\r\nhttps://github.com/electroly/sqlnotebo" +
    "ok\r\n\r\nFarm-Fresh Icons\r\nCopyright © 2016 FatCow Web Hosting\r\nhttps://www.fatcow." +
    "com/free-icons";
            // 
            // _okBtn
            // 
            _okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            _okBtn.Location = new System.Drawing.Point(244, 142);
            _okBtn.Name = "_okBtn";
            _okBtn.Size = new System.Drawing.Size(88, 26);
            _okBtn.TabIndex = 0;
            _okBtn.Text = "OK";
            _okBtn.UseVisualStyleBackColor = true;
            _okBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // pictureBox1
            // 
            pictureBox1.Image = global::SqlNotebook.Properties.Resources.SqlNotebookIcon48;
            pictureBox1.Location = new System.Drawing.Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(48, 48);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // AboutFrm
            // 
            this.AcceptButton = _okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 180);
            this.Controls.Add(_okBtn);
            this.Controls.Add(textBox1);
            this.Controls.Add(pictureBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutFrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About SQL Notebook";
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}