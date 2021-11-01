
namespace SqlNotebook {
    partial class FontForm {
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
            this._rowFlow1 = new System.Windows.Forms.FlowLayoutPanel();
            this._columnFlow1 = new System.Windows.Forms.FlowLayoutPanel();
            this._fontLabel = new System.Windows.Forms.Label();
            this._fontList = new System.Windows.Forms.ListBox();
            this._columnFlow2 = new System.Windows.Forms.FlowLayoutPanel();
            this._sizeLabel = new System.Windows.Forms.Label();
            this._sizeList = new System.Windows.Forms.ListBox();
            this._columnFlow3 = new System.Windows.Forms.FlowLayoutPanel();
            this._styleLabel = new System.Windows.Forms.Label();
            this._boldCheck = new System.Windows.Forms.CheckBox();
            this._buttonFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._okButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this._rowFlow2 = new System.Windows.Forms.FlowLayoutPanel();
            this._previewLabel = new System.Windows.Forms.Label();
            this._previewPanel = new System.Windows.Forms.Panel();
            this._table.SuspendLayout();
            this._rowFlow1.SuspendLayout();
            this._columnFlow1.SuspendLayout();
            this._columnFlow2.SuspendLayout();
            this._columnFlow3.SuspendLayout();
            this._buttonFlow.SuspendLayout();
            this._rowFlow2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _table
            // 
            this._table.ColumnCount = 1;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.Controls.Add(this._rowFlow1, 0, 0);
            this._table.Controls.Add(this._buttonFlow, 0, 2);
            this._table.Controls.Add(this._rowFlow2, 0, 1);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 3;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.Size = new System.Drawing.Size(800, 450);
            this._table.TabIndex = 0;
            // 
            // _rowFlow1
            // 
            this._rowFlow1.AutoSize = true;
            this._rowFlow1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._rowFlow1.Controls.Add(this._columnFlow1);
            this._rowFlow1.Controls.Add(this._columnFlow2);
            this._rowFlow1.Controls.Add(this._columnFlow3);
            this._rowFlow1.Location = new System.Drawing.Point(3, 3);
            this._rowFlow1.Name = "_rowFlow1";
            this._rowFlow1.Size = new System.Drawing.Size(470, 166);
            this._rowFlow1.TabIndex = 0;
            this._rowFlow1.WrapContents = false;
            // 
            // _columnFlow1
            // 
            this._columnFlow1.AutoSize = true;
            this._columnFlow1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._columnFlow1.Controls.Add(this._fontLabel);
            this._columnFlow1.Controls.Add(this._fontList);
            this._columnFlow1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._columnFlow1.Location = new System.Drawing.Point(3, 3);
            this._columnFlow1.Name = "_columnFlow1";
            this._columnFlow1.Size = new System.Drawing.Size(186, 160);
            this._columnFlow1.TabIndex = 0;
            this._columnFlow1.WrapContents = false;
            // 
            // _fontLabel
            // 
            this._fontLabel.AutoSize = true;
            this._fontLabel.Location = new System.Drawing.Point(3, 0);
            this._fontLabel.Name = "_fontLabel";
            this._fontLabel.Size = new System.Drawing.Size(52, 25);
            this._fontLabel.TabIndex = 0;
            this._fontLabel.Text = "&Font:";
            // 
            // _fontList
            // 
            this._fontList.FormattingEnabled = true;
            this._fontList.ItemHeight = 25;
            this._fontList.Location = new System.Drawing.Point(3, 28);
            this._fontList.Name = "_fontList";
            this._fontList.Size = new System.Drawing.Size(180, 129);
            this._fontList.TabIndex = 1;
            this._fontList.SelectedIndexChanged += new System.EventHandler(this.FontList_SelectedIndexChanged);
            // 
            // _columnFlow2
            // 
            this._columnFlow2.AutoSize = true;
            this._columnFlow2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._columnFlow2.Controls.Add(this._sizeLabel);
            this._columnFlow2.Controls.Add(this._sizeList);
            this._columnFlow2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._columnFlow2.Location = new System.Drawing.Point(195, 3);
            this._columnFlow2.Name = "_columnFlow2";
            this._columnFlow2.Size = new System.Drawing.Size(186, 160);
            this._columnFlow2.TabIndex = 1;
            this._columnFlow2.WrapContents = false;
            // 
            // _sizeLabel
            // 
            this._sizeLabel.AutoSize = true;
            this._sizeLabel.Location = new System.Drawing.Point(3, 0);
            this._sizeLabel.Name = "_sizeLabel";
            this._sizeLabel.Size = new System.Drawing.Size(47, 25);
            this._sizeLabel.TabIndex = 0;
            this._sizeLabel.Text = "&Size:";
            // 
            // _sizeList
            // 
            this._sizeList.FormattingEnabled = true;
            this._sizeList.ItemHeight = 25;
            this._sizeList.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "11",
            "12",
            "14",
            "16",
            "18",
            "20",
            "22",
            "24",
            "26",
            "28",
            "36"});
            this._sizeList.Location = new System.Drawing.Point(3, 28);
            this._sizeList.Name = "_sizeList";
            this._sizeList.Size = new System.Drawing.Size(180, 129);
            this._sizeList.TabIndex = 1;
            this._sizeList.SelectedIndexChanged += new System.EventHandler(this.SizeList_SelectedIndexChanged);
            // 
            // _columnFlow3
            // 
            this._columnFlow3.AutoSize = true;
            this._columnFlow3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._columnFlow3.Controls.Add(this._styleLabel);
            this._columnFlow3.Controls.Add(this._boldCheck);
            this._columnFlow3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._columnFlow3.Location = new System.Drawing.Point(387, 3);
            this._columnFlow3.Name = "_columnFlow3";
            this._columnFlow3.Size = new System.Drawing.Size(80, 60);
            this._columnFlow3.TabIndex = 2;
            this._columnFlow3.WrapContents = false;
            // 
            // _styleLabel
            // 
            this._styleLabel.AutoSize = true;
            this._styleLabel.Location = new System.Drawing.Point(3, 0);
            this._styleLabel.Name = "_styleLabel";
            this._styleLabel.Size = new System.Drawing.Size(53, 25);
            this._styleLabel.TabIndex = 0;
            this._styleLabel.Text = "Style:";
            // 
            // _boldCheck
            // 
            this._boldCheck.AutoSize = true;
            this._boldCheck.Location = new System.Drawing.Point(3, 28);
            this._boldCheck.Name = "_boldCheck";
            this._boldCheck.Size = new System.Drawing.Size(74, 29);
            this._boldCheck.TabIndex = 1;
            this._boldCheck.Text = "&Bold";
            this._boldCheck.UseVisualStyleBackColor = true;
            this._boldCheck.CheckedChanged += new System.EventHandler(this.BoldCheck_CheckedChanged);
            // 
            // _buttonFlow
            // 
            this._buttonFlow.AutoSize = true;
            this._buttonFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._buttonFlow.Controls.Add(this._okButton);
            this._buttonFlow.Controls.Add(this._cancelButton);
            this._buttonFlow.Dock = System.Windows.Forms.DockStyle.Right;
            this._buttonFlow.Location = new System.Drawing.Point(561, 300);
            this._buttonFlow.Name = "_buttonFlow";
            this._buttonFlow.Size = new System.Drawing.Size(236, 147);
            this._buttonFlow.TabIndex = 1;
            this._buttonFlow.WrapContents = false;
            // 
            // _okButton
            // 
            this._okButton.Location = new System.Drawing.Point(3, 3);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(112, 34);
            this._okButton.TabIndex = 0;
            this._okButton.Text = "OK";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // _cancelButton
            // 
            this._cancelButton.Location = new System.Drawing.Point(121, 3);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(112, 34);
            this._cancelButton.TabIndex = 1;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _rowFlow2
            // 
            this._rowFlow2.AutoSize = true;
            this._rowFlow2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._rowFlow2.Controls.Add(this._previewLabel);
            this._rowFlow2.Controls.Add(this._previewPanel);
            this._rowFlow2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._rowFlow2.Location = new System.Drawing.Point(3, 175);
            this._rowFlow2.Name = "_rowFlow2";
            this._rowFlow2.Size = new System.Drawing.Size(257, 119);
            this._rowFlow2.TabIndex = 2;
            this._rowFlow2.WrapContents = false;
            // 
            // _previewLabel
            // 
            this._previewLabel.AutoSize = true;
            this._previewLabel.Location = new System.Drawing.Point(3, 0);
            this._previewLabel.Name = "_previewLabel";
            this._previewLabel.Size = new System.Drawing.Size(76, 25);
            this._previewLabel.TabIndex = 0;
            this._previewLabel.Text = "Preview:";
            // 
            // _previewPanel
            // 
            this._previewPanel.BackColor = System.Drawing.Color.White;
            this._previewPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._previewPanel.ForeColor = System.Drawing.Color.Black;
            this._previewPanel.Location = new System.Drawing.Point(3, 28);
            this._previewPanel.Name = "_previewPanel";
            this._previewPanel.Size = new System.Drawing.Size(251, 88);
            this._previewPanel.TabIndex = 1;
            // 
            // FontForm
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this._table);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FontForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Font";
            this._table.ResumeLayout(false);
            this._table.PerformLayout();
            this._rowFlow1.ResumeLayout(false);
            this._rowFlow1.PerformLayout();
            this._columnFlow1.ResumeLayout(false);
            this._columnFlow1.PerformLayout();
            this._columnFlow2.ResumeLayout(false);
            this._columnFlow2.PerformLayout();
            this._columnFlow3.ResumeLayout(false);
            this._columnFlow3.PerformLayout();
            this._buttonFlow.ResumeLayout(false);
            this._rowFlow2.ResumeLayout(false);
            this._rowFlow2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.FlowLayoutPanel _rowFlow1;
        private System.Windows.Forms.FlowLayoutPanel _columnFlow1;
        private System.Windows.Forms.Label _fontLabel;
        private System.Windows.Forms.ListBox _fontList;
        private System.Windows.Forms.FlowLayoutPanel _columnFlow2;
        private System.Windows.Forms.Label _sizeLabel;
        private System.Windows.Forms.ListBox _sizeList;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow;
        private System.Windows.Forms.FlowLayoutPanel _columnFlow3;
        private System.Windows.Forms.Label _styleLabel;
        private System.Windows.Forms.CheckBox _boldCheck;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.FlowLayoutPanel _rowFlow2;
        private System.Windows.Forms.Label _previewLabel;
        private System.Windows.Forms.Panel _previewPanel;
    }
}