namespace SqlNotebook {
    partial class SendToTableForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendToTableForm));
            this._nameLabel = new System.Windows.Forms.Label();
            this._picture = new System.Windows.Forms.PictureBox();
            this._okBtn = new System.Windows.Forms.Button();
            this._cancelBtn = new System.Windows.Forms.Button();
            this._nameTxt = new System.Windows.Forms.TextBox();
            this._table = new System.Windows.Forms.TableLayoutPanel();
            this._buttonFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._picturePanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this._picture)).BeginInit();
            this._table.SuspendLayout();
            this._buttonFlow.SuspendLayout();
            this._picturePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _nameLabel
            // 
            this._nameLabel.AutoSize = true;
            this._nameLabel.Location = new System.Drawing.Point(44, 0);
            this._nameLabel.Name = "_nameLabel";
            this._nameLabel.Size = new System.Drawing.Size(144, 25);
            this._nameLabel.TabIndex = 0;
            this._nameLabel.Text = "&New table name:";
            // 
            // _picture
            // 
            this._picture.Image = ((System.Drawing.Image)(resources.GetObject("_picture.Image")));
            this._picture.Location = new System.Drawing.Point(0, 0);
            this._picture.Name = "_picture";
            this._picture.Size = new System.Drawing.Size(32, 32);
            this._picture.TabIndex = 4;
            this._picture.TabStop = false;
            // 
            // _okBtn
            // 
            this._okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._okBtn.AutoSize = true;
            this._okBtn.Location = new System.Drawing.Point(3, 3);
            this._okBtn.Name = "_okBtn";
            this._okBtn.Size = new System.Drawing.Size(88, 35);
            this._okBtn.TabIndex = 2;
            this._okBtn.Text = "Send";
            this._okBtn.UseVisualStyleBackColor = true;
            this._okBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // _cancelBtn
            // 
            this._cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelBtn.AutoSize = true;
            this._cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelBtn.Location = new System.Drawing.Point(97, 3);
            this._cancelBtn.Name = "_cancelBtn";
            this._cancelBtn.Size = new System.Drawing.Size(88, 35);
            this._cancelBtn.TabIndex = 3;
            this._cancelBtn.Text = "Cancel";
            this._cancelBtn.UseVisualStyleBackColor = true;
            // 
            // _nameTxt
            // 
            this._nameTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._nameTxt.Location = new System.Drawing.Point(44, 28);
            this._nameTxt.Name = "_nameTxt";
            this._nameTxt.Size = new System.Drawing.Size(620, 31);
            this._nameTxt.TabIndex = 1;
            // 
            // _table
            // 
            this._table.ColumnCount = 2;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.Controls.Add(this._nameTxt, 1, 1);
            this._table.Controls.Add(this._nameLabel, 1, 0);
            this._table.Controls.Add(this._buttonFlow, 0, 2);
            this._table.Controls.Add(this._picturePanel, 0, 0);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 3;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.Size = new System.Drawing.Size(667, 633);
            this._table.TabIndex = 5;
            // 
            // _buttonFlow
            // 
            this._buttonFlow.AutoSize = true;
            this._buttonFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._table.SetColumnSpan(this._buttonFlow, 2);
            this._buttonFlow.Controls.Add(this._okBtn);
            this._buttonFlow.Controls.Add(this._cancelBtn);
            this._buttonFlow.Dock = System.Windows.Forms.DockStyle.Right;
            this._buttonFlow.Location = new System.Drawing.Point(476, 65);
            this._buttonFlow.Name = "_buttonFlow";
            this._buttonFlow.Size = new System.Drawing.Size(188, 565);
            this._buttonFlow.TabIndex = 5;
            this._buttonFlow.WrapContents = false;
            // 
            // _picturePanel
            // 
            this._picturePanel.AutoSize = true;
            this._picturePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._picturePanel.Controls.Add(this._picture);
            this._picturePanel.Location = new System.Drawing.Point(3, 3);
            this._picturePanel.Name = "_picturePanel";
            this._table.SetRowSpan(this._picturePanel, 2);
            this._picturePanel.Size = new System.Drawing.Size(35, 35);
            this._picturePanel.TabIndex = 6;
            // 
            // SendToTableForm
            // 
            this.AcceptButton = this._okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelBtn;
            this.ClientSize = new System.Drawing.Size(667, 633);
            this.Controls.Add(this._table);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SendToTableForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Send to Table";
            ((System.ComponentModel.ISupportInitialize)(this._picture)).EndInit();
            this._table.ResumeLayout(false);
            this._table.PerformLayout();
            this._buttonFlow.ResumeLayout(false);
            this._buttonFlow.PerformLayout();
            this._picturePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _okBtn;
        private System.Windows.Forms.Button _cancelBtn;
        private System.Windows.Forms.TextBox _nameTxt;
        private System.Windows.Forms.Label _nameLabel;
        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow;
        private System.Windows.Forms.PictureBox _picture;
        private System.Windows.Forms.Panel _picturePanel;
    }
}