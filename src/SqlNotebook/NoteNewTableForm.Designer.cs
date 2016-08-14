namespace SqlNotebook {
    partial class NoteNewTableForm {
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
            System.Windows.Forms.PictureBox pictureBox1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NoteNewTableForm));
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Button _cancelBtn;
            System.Windows.Forms.Button _okBtn;
            System.Windows.Forms.Label label2;
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this._rowsTxt = new System.Windows.Forms.NumericUpDown();
            this._columnsTxt = new System.Windows.Forms.NumericUpDown();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            label1 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            _cancelBtn = new System.Windows.Forms.Button();
            _okBtn = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._rowsTxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._columnsTxt)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            pictureBox1.Location = new System.Drawing.Point(12, 18);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(32, 32);
            pictureBox1.TabIndex = 9;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(59, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(38, 15);
            label1.TabIndex = 1;
            label1.Text = "&Rows:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(137, 9);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(58, 15);
            label3.TabIndex = 4;
            label3.Text = "&Columns:";
            // 
            // _cancelBtn
            // 
            _cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            _cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            _cancelBtn.Location = new System.Drawing.Point(155, 69);
            _cancelBtn.Name = "_cancelBtn";
            _cancelBtn.Size = new System.Drawing.Size(88, 26);
            _cancelBtn.TabIndex = 7;
            _cancelBtn.Text = "Cancel";
            _cancelBtn.UseVisualStyleBackColor = true;
            // 
            // _okBtn
            // 
            _okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            _okBtn.Location = new System.Drawing.Point(61, 69);
            _okBtn.Name = "_okBtn";
            _okBtn.Size = new System.Drawing.Size(88, 26);
            _okBtn.TabIndex = 6;
            _okBtn.Text = "Insert";
            _okBtn.UseVisualStyleBackColor = true;
            _okBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // _rowsTxt
            // 
            this._rowsTxt.Location = new System.Drawing.Point(62, 27);
            this._rowsTxt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._rowsTxt.Name = "_rowsTxt";
            this._rowsTxt.Size = new System.Drawing.Size(51, 23);
            this._rowsTxt.TabIndex = 2;
            this._rowsTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._rowsTxt.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(119, 29);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(15, 15);
            label2.TabIndex = 3;
            label2.Text = "×";
            // 
            // _columnsTxt
            // 
            this._columnsTxt.Location = new System.Drawing.Point(140, 27);
            this._columnsTxt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._columnsTxt.Name = "_columnsTxt";
            this._columnsTxt.Size = new System.Drawing.Size(51, 23);
            this._columnsTxt.TabIndex = 5;
            this._columnsTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._columnsTxt.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // NoteNewTableForm
            // 
            this.AcceptButton = _okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = _cancelBtn;
            this.ClientSize = new System.Drawing.Size(255, 107);
            this.Controls.Add(this._columnsTxt);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(this._rowsTxt);
            this.Controls.Add(label1);
            this.Controls.Add(pictureBox1);
            this.Controls.Add(_cancelBtn);
            this.Controls.Add(_okBtn);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NoteNewTableForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Insert Table";
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._rowsTxt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._columnsTxt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.NumericUpDown _rowsTxt;
        private System.Windows.Forms.NumericUpDown _columnsTxt;
    }
}