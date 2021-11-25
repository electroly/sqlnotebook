
namespace SqlNotebook {
    partial class OptionsForm {
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
            this._buttonFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._okButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this._resetFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._resetButton = new System.Windows.Forms.Button();
            this._tabs = new System.Windows.Forms.TabControl();
            this._fontsColorsTab = new System.Windows.Forms.TabPage();
            this._fontsColorsFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._rowFlow1 = new System.Windows.Forms.FlowLayoutPanel();
            this._codeFontFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._codeFontLabel = new System.Windows.Forms.Label();
            this._codeFontButton = new System.Windows.Forms.Button();
            this._gridFontFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._gridFontLabel = new System.Windows.Forms.Label();
            this._gridFontButton = new System.Windows.Forms.Button();
            this._rowFlow2 = new System.Windows.Forms.FlowLayoutPanel();
            this._editorColorsFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._editorColorsLabel = new System.Windows.Forms.Label();
            this._editorPlainColorButton = new System.Windows.Forms.Button();
            this._editorKeywordColorButton = new System.Windows.Forms.Button();
            this._editorCommentColorButton = new System.Windows.Forms.Button();
            this._editorStringColorButton = new System.Windows.Forms.Button();
            this._editorLineNumbersColorButton = new System.Windows.Forms.Button();
            this._editorBackgroundColorButton = new System.Windows.Forms.Button();
            this._gridColorsFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._gridColorsLabel = new System.Windows.Forms.Label();
            this._gridPlainColorButton = new System.Windows.Forms.Button();
            this._gridHeaderColorButton = new System.Windows.Forms.Button();
            this._gridLineColorButton = new System.Windows.Forms.Button();
            this._gridBackgroundColorButton = new System.Windows.Forms.Button();
            this._table.SuspendLayout();
            this._buttonFlow.SuspendLayout();
            this._resetFlow.SuspendLayout();
            this._tabs.SuspendLayout();
            this._fontsColorsTab.SuspendLayout();
            this._fontsColorsFlow.SuspendLayout();
            this._rowFlow1.SuspendLayout();
            this._codeFontFlow.SuspendLayout();
            this._gridFontFlow.SuspendLayout();
            this._rowFlow2.SuspendLayout();
            this._editorColorsFlow.SuspendLayout();
            this._gridColorsFlow.SuspendLayout();
            this.SuspendLayout();
            // 
            // _table
            // 
            this._table.AutoSize = true;
            this._table.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._table.ColumnCount = 2;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._table.Controls.Add(this._buttonFlow, 1, 1);
            this._table.Controls.Add(this._resetFlow, 0, 1);
            this._table.Controls.Add(this._tabs, 0, 0);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 1;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.Size = new System.Drawing.Size(800, 566);
            this._table.TabIndex = 0;
            // 
            // _buttonFlow
            // 
            this._buttonFlow.AutoSize = true;
            this._buttonFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._buttonFlow.Controls.Add(this._okButton);
            this._buttonFlow.Controls.Add(this._cancelButton);
            this._buttonFlow.Dock = System.Windows.Forms.DockStyle.Right;
            this._buttonFlow.Location = new System.Drawing.Point(561, 485);
            this._buttonFlow.Name = "_buttonFlow";
            this._buttonFlow.Size = new System.Drawing.Size(236, 78);
            this._buttonFlow.TabIndex = 1;
            this._buttonFlow.WrapContents = false;
            // 
            // _okButton
            // 
            this._okButton.Location = new System.Drawing.Point(3, 3);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(112, 34);
            this._okButton.TabIndex = 0;
            this._okButton.Text = "Save";
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
            // _resetFlow
            // 
            this._resetFlow.AutoSize = true;
            this._resetFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._resetFlow.Controls.Add(this._resetButton);
            this._resetFlow.Location = new System.Drawing.Point(3, 485);
            this._resetFlow.Name = "_resetFlow";
            this._resetFlow.Size = new System.Drawing.Size(118, 40);
            this._resetFlow.TabIndex = 2;
            // 
            // _resetButton
            // 
            this._resetButton.Location = new System.Drawing.Point(3, 3);
            this._resetButton.Name = "_resetButton";
            this._resetButton.Size = new System.Drawing.Size(112, 34);
            this._resetButton.TabIndex = 0;
            this._resetButton.Text = "Reset";
            this._resetButton.UseVisualStyleBackColor = true;
            this._resetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // _tabs
            // 
            this._table.SetColumnSpan(this._tabs, 2);
            this._tabs.Controls.Add(this._fontsColorsTab);
            this._tabs.Location = new System.Drawing.Point(3, 3);
            this._tabs.Name = "_tabs";
            this._tabs.SelectedIndex = 0;
            this._tabs.Size = new System.Drawing.Size(591, 476);
            this._tabs.TabIndex = 3;
            // 
            // _fontsColorsTab
            // 
            this._fontsColorsTab.Controls.Add(this._fontsColorsFlow);
            this._fontsColorsTab.Location = new System.Drawing.Point(4, 34);
            this._fontsColorsTab.Name = "_fontsColorsTab";
            this._fontsColorsTab.Padding = new System.Windows.Forms.Padding(3);
            this._fontsColorsTab.Size = new System.Drawing.Size(583, 438);
            this._fontsColorsTab.TabIndex = 0;
            this._fontsColorsTab.Text = "Fonts and Colors";
            this._fontsColorsTab.UseVisualStyleBackColor = true;
            // 
            // _fontsColorsFlow
            // 
            this._fontsColorsFlow.AutoSize = true;
            this._fontsColorsFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._fontsColorsFlow.Controls.Add(this._rowFlow1);
            this._fontsColorsFlow.Controls.Add(this._rowFlow2);
            this._fontsColorsFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this._fontsColorsFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._fontsColorsFlow.Location = new System.Drawing.Point(3, 3);
            this._fontsColorsFlow.Name = "_fontsColorsFlow";
            this._fontsColorsFlow.Size = new System.Drawing.Size(577, 432);
            this._fontsColorsFlow.TabIndex = 0;
            this._fontsColorsFlow.WrapContents = false;
            // 
            // _rowFlow1
            // 
            this._rowFlow1.AutoSize = true;
            this._rowFlow1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._rowFlow1.Controls.Add(this._codeFontFlow);
            this._rowFlow1.Controls.Add(this._gridFontFlow);
            this._rowFlow1.Location = new System.Drawing.Point(3, 3);
            this._rowFlow1.Name = "_rowFlow1";
            this._rowFlow1.Size = new System.Drawing.Size(248, 71);
            this._rowFlow1.TabIndex = 3;
            this._rowFlow1.WrapContents = false;
            // 
            // _codeFontFlow
            // 
            this._codeFontFlow.AutoSize = true;
            this._codeFontFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._codeFontFlow.Controls.Add(this._codeFontLabel);
            this._codeFontFlow.Controls.Add(this._codeFontButton);
            this._codeFontFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._codeFontFlow.Location = new System.Drawing.Point(3, 3);
            this._codeFontFlow.Name = "_codeFontFlow";
            this._codeFontFlow.Size = new System.Drawing.Size(118, 65);
            this._codeFontFlow.TabIndex = 3;
            this._codeFontFlow.WrapContents = false;
            // 
            // _codeFontLabel
            // 
            this._codeFontLabel.AutoSize = true;
            this._codeFontLabel.Location = new System.Drawing.Point(3, 0);
            this._codeFontLabel.Name = "_codeFontLabel";
            this._codeFontLabel.Size = new System.Drawing.Size(101, 25);
            this._codeFontLabel.TabIndex = 0;
            this._codeFontLabel.Text = "Editor font:";
            // 
            // _codeFontButton
            // 
            this._codeFontButton.Location = new System.Drawing.Point(3, 28);
            this._codeFontButton.Name = "_codeFontButton";
            this._codeFontButton.Size = new System.Drawing.Size(112, 34);
            this._codeFontButton.TabIndex = 2;
            this._codeFontButton.Text = "Font Name";
            this._codeFontButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._codeFontButton.UseVisualStyleBackColor = true;
            this._codeFontButton.Click += new System.EventHandler(this.CodeFontButton_Click);
            // 
            // _gridFontFlow
            // 
            this._gridFontFlow.AutoSize = true;
            this._gridFontFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._gridFontFlow.Controls.Add(this._gridFontLabel);
            this._gridFontFlow.Controls.Add(this._gridFontButton);
            this._gridFontFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._gridFontFlow.Location = new System.Drawing.Point(127, 3);
            this._gridFontFlow.Name = "_gridFontFlow";
            this._gridFontFlow.Size = new System.Drawing.Size(118, 65);
            this._gridFontFlow.TabIndex = 2;
            this._gridFontFlow.WrapContents = false;
            // 
            // _gridFontLabel
            // 
            this._gridFontLabel.AutoSize = true;
            this._gridFontLabel.Location = new System.Drawing.Point(3, 0);
            this._gridFontLabel.Name = "_gridFontLabel";
            this._gridFontLabel.Size = new System.Drawing.Size(87, 25);
            this._gridFontLabel.TabIndex = 0;
            this._gridFontLabel.Text = "&Grid font:";
            // 
            // _gridFontButton
            // 
            this._gridFontButton.Location = new System.Drawing.Point(3, 28);
            this._gridFontButton.Name = "_gridFontButton";
            this._gridFontButton.Size = new System.Drawing.Size(112, 34);
            this._gridFontButton.TabIndex = 2;
            this._gridFontButton.Text = "Font Name";
            this._gridFontButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._gridFontButton.UseVisualStyleBackColor = true;
            this._gridFontButton.Click += new System.EventHandler(this.DataTableFontButton_Click);
            // 
            // _rowFlow2
            // 
            this._rowFlow2.AutoSize = true;
            this._rowFlow2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._rowFlow2.Controls.Add(this._editorColorsFlow);
            this._rowFlow2.Controls.Add(this._gridColorsFlow);
            this._rowFlow2.Location = new System.Drawing.Point(3, 80);
            this._rowFlow2.Name = "_rowFlow2";
            this._rowFlow2.Size = new System.Drawing.Size(252, 271);
            this._rowFlow2.TabIndex = 4;
            this._rowFlow2.WrapContents = false;
            // 
            // _editorColorsFlow
            // 
            this._editorColorsFlow.AutoSize = true;
            this._editorColorsFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._editorColorsFlow.Controls.Add(this._editorColorsLabel);
            this._editorColorsFlow.Controls.Add(this._editorPlainColorButton);
            this._editorColorsFlow.Controls.Add(this._editorKeywordColorButton);
            this._editorColorsFlow.Controls.Add(this._editorCommentColorButton);
            this._editorColorsFlow.Controls.Add(this._editorStringColorButton);
            this._editorColorsFlow.Controls.Add(this._editorLineNumbersColorButton);
            this._editorColorsFlow.Controls.Add(this._editorBackgroundColorButton);
            this._editorColorsFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._editorColorsFlow.Location = new System.Drawing.Point(3, 3);
            this._editorColorsFlow.Name = "_editorColorsFlow";
            this._editorColorsFlow.Size = new System.Drawing.Size(122, 265);
            this._editorColorsFlow.TabIndex = 0;
            this._editorColorsFlow.WrapContents = false;
            // 
            // _editorColorsLabel
            // 
            this._editorColorsLabel.AutoSize = true;
            this._editorColorsLabel.Location = new System.Drawing.Point(3, 0);
            this._editorColorsLabel.Name = "_editorColorsLabel";
            this._editorColorsLabel.Size = new System.Drawing.Size(116, 25);
            this._editorColorsLabel.TabIndex = 0;
            this._editorColorsLabel.Text = "Editor colors:";
            // 
            // _editorPlainColorButton
            // 
            this._editorPlainColorButton.Location = new System.Drawing.Point(3, 28);
            this._editorPlainColorButton.Name = "_editorPlainColorButton";
            this._editorPlainColorButton.Size = new System.Drawing.Size(112, 34);
            this._editorPlainColorButton.TabIndex = 1;
            this._editorPlainColorButton.Text = "Text";
            this._editorPlainColorButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._editorPlainColorButton.UseVisualStyleBackColor = true;
            this._editorPlainColorButton.Click += new System.EventHandler(this.EditorPlainColorButton_Click);
            this._editorPlainColorButton.Paint += new System.Windows.Forms.PaintEventHandler(this.EditorPlainColorButton_Paint);
            // 
            // _editorKeywordColorButton
            // 
            this._editorKeywordColorButton.Location = new System.Drawing.Point(3, 68);
            this._editorKeywordColorButton.Name = "_editorKeywordColorButton";
            this._editorKeywordColorButton.Size = new System.Drawing.Size(112, 34);
            this._editorKeywordColorButton.TabIndex = 2;
            this._editorKeywordColorButton.Text = "Keywords";
            this._editorKeywordColorButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._editorKeywordColorButton.UseVisualStyleBackColor = true;
            this._editorKeywordColorButton.Click += new System.EventHandler(this.EditorKeywordColorButton_Click);
            this._editorKeywordColorButton.Paint += new System.Windows.Forms.PaintEventHandler(this.EditorKeywordColorButton_Paint);
            // 
            // _editorCommentColorButton
            // 
            this._editorCommentColorButton.Location = new System.Drawing.Point(3, 108);
            this._editorCommentColorButton.Name = "_editorCommentColorButton";
            this._editorCommentColorButton.Size = new System.Drawing.Size(112, 34);
            this._editorCommentColorButton.TabIndex = 3;
            this._editorCommentColorButton.Text = "Comments";
            this._editorCommentColorButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._editorCommentColorButton.UseVisualStyleBackColor = true;
            this._editorCommentColorButton.Click += new System.EventHandler(this.EditorCommentColorButton_Click);
            this._editorCommentColorButton.Paint += new System.Windows.Forms.PaintEventHandler(this.EditorCommentColorButton_Paint);
            // 
            // _editorStringColorButton
            // 
            this._editorStringColorButton.Location = new System.Drawing.Point(3, 148);
            this._editorStringColorButton.Name = "_editorStringColorButton";
            this._editorStringColorButton.Size = new System.Drawing.Size(112, 34);
            this._editorStringColorButton.TabIndex = 4;
            this._editorStringColorButton.Text = "Strings";
            this._editorStringColorButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._editorStringColorButton.UseVisualStyleBackColor = true;
            this._editorStringColorButton.Click += new System.EventHandler(this.EditorStringColorButton_Click);
            this._editorStringColorButton.Paint += new System.Windows.Forms.PaintEventHandler(this.EditorStringColorButton_Paint);
            // 
            // _editorLineNumbersColorButton
            // 
            this._editorLineNumbersColorButton.Location = new System.Drawing.Point(3, 188);
            this._editorLineNumbersColorButton.Name = "_editorLineNumbersColorButton";
            this._editorLineNumbersColorButton.Size = new System.Drawing.Size(112, 34);
            this._editorLineNumbersColorButton.TabIndex = 6;
            this._editorLineNumbersColorButton.Text = "Line numbers";
            this._editorLineNumbersColorButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._editorLineNumbersColorButton.UseVisualStyleBackColor = true;
            this._editorLineNumbersColorButton.Click += new System.EventHandler(this.EditorLineNumbersColorButton_Click);
            this._editorLineNumbersColorButton.Paint += new System.Windows.Forms.PaintEventHandler(this.EditorLineNumbersColorButton_Paint);
            // 
            // _editorBackgroundColorButton
            // 
            this._editorBackgroundColorButton.Location = new System.Drawing.Point(3, 228);
            this._editorBackgroundColorButton.Name = "_editorBackgroundColorButton";
            this._editorBackgroundColorButton.Size = new System.Drawing.Size(112, 34);
            this._editorBackgroundColorButton.TabIndex = 5;
            this._editorBackgroundColorButton.Text = "Background";
            this._editorBackgroundColorButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._editorBackgroundColorButton.UseVisualStyleBackColor = true;
            this._editorBackgroundColorButton.Click += new System.EventHandler(this.EditorBackgroundColorButton_Click);
            this._editorBackgroundColorButton.Paint += new System.Windows.Forms.PaintEventHandler(this.EditorBackgroundColorButton_Paint);
            // 
            // _gridColorsFlow
            // 
            this._gridColorsFlow.AutoSize = true;
            this._gridColorsFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._gridColorsFlow.Controls.Add(this._gridColorsLabel);
            this._gridColorsFlow.Controls.Add(this._gridPlainColorButton);
            this._gridColorsFlow.Controls.Add(this._gridHeaderColorButton);
            this._gridColorsFlow.Controls.Add(this._gridLineColorButton);
            this._gridColorsFlow.Controls.Add(this._gridBackgroundColorButton);
            this._gridColorsFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._gridColorsFlow.Location = new System.Drawing.Point(131, 3);
            this._gridColorsFlow.Name = "_gridColorsFlow";
            this._gridColorsFlow.Size = new System.Drawing.Size(118, 185);
            this._gridColorsFlow.TabIndex = 1;
            this._gridColorsFlow.WrapContents = false;
            // 
            // _gridColorsLabel
            // 
            this._gridColorsLabel.AutoSize = true;
            this._gridColorsLabel.Location = new System.Drawing.Point(3, 0);
            this._gridColorsLabel.Name = "_gridColorsLabel";
            this._gridColorsLabel.Size = new System.Drawing.Size(102, 25);
            this._gridColorsLabel.TabIndex = 0;
            this._gridColorsLabel.Text = "Grid colors:";
            // 
            // _gridPlainColorButton
            // 
            this._gridPlainColorButton.Location = new System.Drawing.Point(3, 28);
            this._gridPlainColorButton.Name = "_gridPlainColorButton";
            this._gridPlainColorButton.Size = new System.Drawing.Size(112, 34);
            this._gridPlainColorButton.TabIndex = 1;
            this._gridPlainColorButton.Text = "Text";
            this._gridPlainColorButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._gridPlainColorButton.UseVisualStyleBackColor = true;
            this._gridPlainColorButton.Click += new System.EventHandler(this.GridPlainColorButton_Click);
            this._gridPlainColorButton.Paint += new System.Windows.Forms.PaintEventHandler(this.GridPlainColorButton_Paint);
            // 
            // _gridHeaderColorButton
            // 
            this._gridHeaderColorButton.Location = new System.Drawing.Point(3, 68);
            this._gridHeaderColorButton.Name = "_gridHeaderColorButton";
            this._gridHeaderColorButton.Size = new System.Drawing.Size(112, 34);
            this._gridHeaderColorButton.TabIndex = 2;
            this._gridHeaderColorButton.Text = "Header";
            this._gridHeaderColorButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._gridHeaderColorButton.UseVisualStyleBackColor = true;
            this._gridHeaderColorButton.Click += new System.EventHandler(this.GridHeaderColorButton_Click);
            this._gridHeaderColorButton.Paint += new System.Windows.Forms.PaintEventHandler(this.GridHeaderColorButton_Paint);
            // 
            // _gridLineColorButton
            // 
            this._gridLineColorButton.Location = new System.Drawing.Point(3, 108);
            this._gridLineColorButton.Name = "_gridLineColorButton";
            this._gridLineColorButton.Size = new System.Drawing.Size(112, 34);
            this._gridLineColorButton.TabIndex = 4;
            this._gridLineColorButton.Text = "Grid lines";
            this._gridLineColorButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._gridLineColorButton.UseVisualStyleBackColor = true;
            this._gridLineColorButton.Click += new System.EventHandler(this.GridLineColorButton_Click);
            this._gridLineColorButton.Paint += new System.Windows.Forms.PaintEventHandler(this.GridLineColorButton_Paint);
            // 
            // _gridBackgroundColorButton
            // 
            this._gridBackgroundColorButton.Location = new System.Drawing.Point(3, 148);
            this._gridBackgroundColorButton.Name = "_gridBackgroundColorButton";
            this._gridBackgroundColorButton.Size = new System.Drawing.Size(112, 34);
            this._gridBackgroundColorButton.TabIndex = 3;
            this._gridBackgroundColorButton.Text = "Background";
            this._gridBackgroundColorButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._gridBackgroundColorButton.UseVisualStyleBackColor = true;
            this._gridBackgroundColorButton.Click += new System.EventHandler(this.GridBackgroundColorButton_Click);
            this._gridBackgroundColorButton.Paint += new System.Windows.Forms.PaintEventHandler(this.GridBackgroundColorButton_Paint);
            // 
            // OptionsForm
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(800, 566);
            this.Controls.Add(this._table);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this._table.ResumeLayout(false);
            this._table.PerformLayout();
            this._buttonFlow.ResumeLayout(false);
            this._resetFlow.ResumeLayout(false);
            this._tabs.ResumeLayout(false);
            this._fontsColorsTab.ResumeLayout(false);
            this._fontsColorsTab.PerformLayout();
            this._fontsColorsFlow.ResumeLayout(false);
            this._fontsColorsFlow.PerformLayout();
            this._rowFlow1.ResumeLayout(false);
            this._rowFlow1.PerformLayout();
            this._codeFontFlow.ResumeLayout(false);
            this._codeFontFlow.PerformLayout();
            this._gridFontFlow.ResumeLayout(false);
            this._gridFontFlow.PerformLayout();
            this._rowFlow2.ResumeLayout(false);
            this._rowFlow2.PerformLayout();
            this._editorColorsFlow.ResumeLayout(false);
            this._editorColorsFlow.PerformLayout();
            this._gridColorsFlow.ResumeLayout(false);
            this._gridColorsFlow.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.FlowLayoutPanel _fontsColorsFlow;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.FlowLayoutPanel _gridFontFlow;
        private System.Windows.Forms.Label _gridFontLabel;
        private System.Windows.Forms.Button _gridFontButton;
        private System.Windows.Forms.FlowLayoutPanel _rowFlow1;
        private System.Windows.Forms.FlowLayoutPanel _codeFontFlow;
        private System.Windows.Forms.Label _codeFontLabel;
        private System.Windows.Forms.Button _codeFontButton;
        private System.Windows.Forms.FlowLayoutPanel _rowFlow2;
        private System.Windows.Forms.FlowLayoutPanel _editorColorsFlow;
        private System.Windows.Forms.Label _editorColorsLabel;
        private System.Windows.Forms.Button _editorPlainColorButton;
        private System.Windows.Forms.Button _editorKeywordColorButton;
        private System.Windows.Forms.Button _editorCommentColorButton;
        private System.Windows.Forms.Button _editorStringColorButton;
        private System.Windows.Forms.FlowLayoutPanel _gridColorsFlow;
        private System.Windows.Forms.Label _gridColorsLabel;
        private System.Windows.Forms.Button _gridPlainColorButton;
        private System.Windows.Forms.Button _gridHeaderColorButton;
        private System.Windows.Forms.Button _gridBackgroundColorButton;
        private System.Windows.Forms.Button _editorBackgroundColorButton;
        private System.Windows.Forms.FlowLayoutPanel _resetFlow;
        private System.Windows.Forms.Button _resetButton;
        private System.Windows.Forms.TabControl _tabs;
        private System.Windows.Forms.TabPage _fontsColorsTab;
        private System.Windows.Forms.Button _gridLineColorButton;
        private System.Windows.Forms.Button _editorLineNumbersColorButton;
    }
}