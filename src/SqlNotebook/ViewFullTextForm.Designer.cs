namespace SqlNotebook {
    partial class ViewFullTextForm {
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
            this._table = new System.Windows.Forms.TableLayoutPanel();
            this._textPanel = new System.Windows.Forms.Panel();
            this._buttonFlow1 = new System.Windows.Forms.FlowLayoutPanel();
            this._buttonFlow2 = new System.Windows.Forms.FlowLayoutPanel();
            this._copyButton = new System.Windows.Forms.Button();
            this._closeButton = new System.Windows.Forms.Button();
            this._table.SuspendLayout();
            this._buttonFlow1.SuspendLayout();
            this._buttonFlow2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _table
            // 
            this._table.ColumnCount = 2;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._table.Controls.Add(this._textPanel, 0, 0);
            this._table.Controls.Add(this._buttonFlow1, 0, 1);
            this._table.Controls.Add(this._buttonFlow2, 1, 1);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 2;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.Size = new System.Drawing.Size(800, 450);
            this._table.TabIndex = 0;
            // 
            // _textPanel
            // 
            this._table.SetColumnSpan(this._textPanel, 2);
            this._textPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._textPanel.Location = new System.Drawing.Point(3, 3);
            this._textPanel.Name = "_textPanel";
            this._textPanel.Size = new System.Drawing.Size(794, 397);
            this._textPanel.TabIndex = 0;
            // 
            // _buttonFlow1
            // 
            this._buttonFlow1.AutoSize = true;
            this._buttonFlow1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._buttonFlow1.Controls.Add(this._copyButton);
            this._buttonFlow1.Location = new System.Drawing.Point(3, 406);
            this._buttonFlow1.Name = "_buttonFlow1";
            this._buttonFlow1.Size = new System.Drawing.Size(172, 41);
            this._buttonFlow1.TabIndex = 1;
            // 
            // _buttonFlow2
            // 
            this._buttonFlow2.AutoSize = true;
            this._buttonFlow2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._buttonFlow2.Controls.Add(this._closeButton);
            this._buttonFlow2.Location = new System.Drawing.Point(679, 406);
            this._buttonFlow2.Name = "_buttonFlow2";
            this._buttonFlow2.Size = new System.Drawing.Size(118, 40);
            this._buttonFlow2.TabIndex = 2;
            // 
            // _copyButton
            // 
            this._copyButton.AutoSize = true;
            this._copyButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._copyButton.Location = new System.Drawing.Point(3, 3);
            this._copyButton.Name = "_copyButton";
            this._copyButton.Size = new System.Drawing.Size(166, 35);
            this._copyButton.TabIndex = 0;
            this._copyButton.Text = "Copy to clipboard";
            this._copyButton.UseVisualStyleBackColor = true;
            this._copyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // _closeButton
            // 
            this._closeButton.Location = new System.Drawing.Point(3, 3);
            this._closeButton.Name = "_closeButton";
            this._closeButton.Size = new System.Drawing.Size(112, 34);
            this._closeButton.TabIndex = 0;
            this._closeButton.Text = "Close";
            this._closeButton.UseVisualStyleBackColor = true;
            // 
            // ViewFullTextForm
            // 
            this.AcceptButton = this._closeButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._closeButton;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this._table);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ViewFullTextForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Full Text";
            this._table.ResumeLayout(false);
            this._table.PerformLayout();
            this._buttonFlow1.ResumeLayout(false);
            this._buttonFlow1.PerformLayout();
            this._buttonFlow2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.Panel _textPanel;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow1;
        private System.Windows.Forms.Button _copyButton;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow2;
        private System.Windows.Forms.Button _closeButton;
    }
}