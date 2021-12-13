namespace SqlNotebook.Import.Csv {
    partial class ImportCsvOptionsControl {
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
            this.components = new System.ComponentModel.Container();
            this._ifTableExistsLabel = new System.Windows.Forms.Label();
            this._tableOutputTitle = new System.Windows.Forms.Label();
            this._headerChk = new System.Windows.Forms.CheckBox();
            this._fileInputTitle = new System.Windows.Forms.Label();
            this._tableCmb = new System.Windows.Forms.ComboBox();
            this._skipLinesTxt = new System.Windows.Forms.NumericUpDown();
            this._tableLabel = new System.Windows.Forms.Label();
            this._skipLinesLabel = new System.Windows.Forms.Label();
            this._ifConversionFailsLabel = new System.Windows.Forms.Label();
            this._ifExistsCmb = new System.Windows.Forms.ComboBox();
            this._encodingCmb = new System.Windows.Forms.ComboBox();
            this._encodingLabel = new System.Windows.Forms.Label();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this._separatorLabel = new System.Windows.Forms.Label();
            this._separatorCombo = new System.Windows.Forms.ComboBox();
            this._convertFailCmb = new System.Windows.Forms.ComboBox();
            this._sourceFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._skipLinesFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._encodingFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._separatorFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._flow = new System.Windows.Forms.FlowLayoutPanel();
            this._targetFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._tableNameFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._ifExistsFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._targetFlow2 = new System.Windows.Forms.FlowLayoutPanel();
            this._convertFailFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._blankValuesFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._blankValuesLabel = new System.Windows.Forms.Label();
            this._blankValuesCombo = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this._skipLinesTxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this._sourceFlow.SuspendLayout();
            this._skipLinesFlow.SuspendLayout();
            this._encodingFlow.SuspendLayout();
            this._separatorFlow.SuspendLayout();
            this._flow.SuspendLayout();
            this._targetFlow.SuspendLayout();
            this._tableNameFlow.SuspendLayout();
            this._ifExistsFlow.SuspendLayout();
            this._targetFlow2.SuspendLayout();
            this._convertFailFlow.SuspendLayout();
            this._blankValuesFlow.SuspendLayout();
            this.SuspendLayout();
            // 
            // _ifTableExistsLabel
            // 
            this._ifTableExistsLabel.AutoSize = true;
            this._ifTableExistsLabel.Location = new System.Drawing.Point(3, 0);
            this._ifTableExistsLabel.Name = "_ifTableExistsLabel";
            this._ifTableExistsLabel.Size = new System.Drawing.Size(149, 25);
            this._ifTableExistsLabel.TabIndex = 23;
            this._ifTableExistsLabel.Text = "If the table exists:";
            this._ifTableExistsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _tableOutputTitle
            // 
            this._tableOutputTitle.AutoSize = true;
            this._tableOutputTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._tableOutputTitle.Location = new System.Drawing.Point(3, 136);
            this._tableOutputTitle.Name = "_tableOutputTitle";
            this._tableOutputTitle.Size = new System.Drawing.Size(66, 25);
            this._tableOutputTitle.TabIndex = 27;
            this._tableOutputTitle.Text = "Target";
            this._tableOutputTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _headerChk
            // 
            this._headerChk.AutoSize = true;
            this._headerChk.Checked = true;
            this._headerChk.CheckState = System.Windows.Forms.CheckState.Checked;
            this._headerChk.Location = new System.Drawing.Point(3, 104);
            this._headerChk.Name = "_headerChk";
            this._headerChk.Size = new System.Drawing.Size(264, 29);
            this._headerChk.TabIndex = 26;
            this._headerChk.Text = "File includes column headers";
            this._headerChk.UseVisualStyleBackColor = true;
            // 
            // _fileInputTitle
            // 
            this._fileInputTitle.AutoSize = true;
            this._fileInputTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._fileInputTitle.Location = new System.Drawing.Point(3, 0);
            this._fileInputTitle.Name = "_fileInputTitle";
            this._fileInputTitle.Size = new System.Drawing.Size(70, 25);
            this._fileInputTitle.TabIndex = 21;
            this._fileInputTitle.Text = "Source";
            this._fileInputTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _tableCmb
            // 
            this._tableCmb.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this._tableCmb.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this._tableCmb.FormattingEnabled = true;
            this._tableCmb.Location = new System.Drawing.Point(3, 28);
            this._tableCmb.Name = "_tableCmb";
            this._tableCmb.Size = new System.Drawing.Size(235, 33);
            this._tableCmb.TabIndex = 22;
            // 
            // _skipLinesTxt
            // 
            this._skipLinesTxt.Location = new System.Drawing.Point(3, 28);
            this._skipLinesTxt.Name = "_skipLinesTxt";
            this._skipLinesTxt.Size = new System.Drawing.Size(71, 31);
            this._skipLinesTxt.TabIndex = 25;
            this._skipLinesTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _tableLabel
            // 
            this._tableLabel.AutoSize = true;
            this._tableLabel.Location = new System.Drawing.Point(3, 0);
            this._tableLabel.Name = "_tableLabel";
            this._tableLabel.Size = new System.Drawing.Size(151, 25);
            this._tableLabel.TabIndex = 20;
            this._tableLabel.Text = "Import into table:";
            this._tableLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _skipLinesLabel
            // 
            this._skipLinesLabel.AutoSize = true;
            this._skipLinesLabel.Location = new System.Drawing.Point(3, 0);
            this._skipLinesLabel.Name = "_skipLinesLabel";
            this._skipLinesLabel.Size = new System.Drawing.Size(90, 25);
            this._skipLinesLabel.TabIndex = 24;
            this._skipLinesLabel.Text = "Skip lines:";
            this._skipLinesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _ifConversionFailsLabel
            // 
            this._ifConversionFailsLabel.AutoSize = true;
            this._ifConversionFailsLabel.Location = new System.Drawing.Point(3, 0);
            this._ifConversionFailsLabel.Name = "_ifConversionFailsLabel";
            this._ifConversionFailsLabel.Size = new System.Drawing.Size(194, 25);
            this._ifConversionFailsLabel.TabIndex = 31;
            this._ifConversionFailsLabel.Text = "If data conversion fails:";
            this._ifConversionFailsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _ifExistsCmb
            // 
            this._ifExistsCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._ifExistsCmb.FormattingEnabled = true;
            this._ifExistsCmb.Location = new System.Drawing.Point(3, 28);
            this._ifExistsCmb.Name = "_ifExistsCmb";
            this._ifExistsCmb.Size = new System.Drawing.Size(179, 33);
            this._ifExistsCmb.TabIndex = 33;
            // 
            // _encodingCmb
            // 
            this._encodingCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._encodingCmb.FormattingEnabled = true;
            this._encodingCmb.Location = new System.Drawing.Point(3, 28);
            this._encodingCmb.Name = "_encodingCmb";
            this._encodingCmb.Size = new System.Drawing.Size(235, 33);
            this._encodingCmb.TabIndex = 36;
            // 
            // _encodingLabel
            // 
            this._encodingLabel.AutoSize = true;
            this._encodingLabel.Location = new System.Drawing.Point(3, 0);
            this._encodingLabel.Name = "_encodingLabel";
            this._encodingLabel.Size = new System.Drawing.Size(125, 25);
            this._encodingLabel.TabIndex = 37;
            this._encodingLabel.Text = "Text encoding:";
            this._encodingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // _separatorLabel
            // 
            this._separatorLabel.AutoSize = true;
            this._separatorLabel.Location = new System.Drawing.Point(3, 0);
            this._separatorLabel.Name = "_separatorLabel";
            this._separatorLabel.Size = new System.Drawing.Size(93, 25);
            this._separatorLabel.TabIndex = 38;
            this._separatorLabel.Text = "&Separator:";
            // 
            // _separatorCombo
            // 
            this._separatorCombo.FormattingEnabled = true;
            this._separatorCombo.Items.AddRange(new object[] {
            ",",
            ";",
            "Tab"});
            this._separatorCombo.Location = new System.Drawing.Point(3, 28);
            this._separatorCombo.Name = "_separatorCombo";
            this._separatorCombo.Size = new System.Drawing.Size(182, 33);
            this._separatorCombo.TabIndex = 39;
            this._separatorCombo.Text = ",";
            // 
            // _convertFailCmb
            // 
            this._convertFailCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._convertFailCmb.FormattingEnabled = true;
            this._convertFailCmb.Location = new System.Drawing.Point(3, 28);
            this._convertFailCmb.Name = "_convertFailCmb";
            this._convertFailCmb.Size = new System.Drawing.Size(179, 33);
            this._convertFailCmb.TabIndex = 34;
            // 
            // _sourceFlow
            // 
            this._sourceFlow.AutoSize = true;
            this._sourceFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._sourceFlow.Controls.Add(this._skipLinesFlow);
            this._sourceFlow.Controls.Add(this._encodingFlow);
            this._sourceFlow.Controls.Add(this._separatorFlow);
            this._sourceFlow.Location = new System.Drawing.Point(3, 28);
            this._sourceFlow.Name = "_sourceFlow";
            this._sourceFlow.Size = new System.Drawing.Size(543, 70);
            this._sourceFlow.TabIndex = 40;
            this._sourceFlow.WrapContents = false;
            // 
            // _skipLinesFlow
            // 
            this._skipLinesFlow.AutoSize = true;
            this._skipLinesFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._skipLinesFlow.Controls.Add(this._skipLinesLabel);
            this._skipLinesFlow.Controls.Add(this._skipLinesTxt);
            this._skipLinesFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._skipLinesFlow.Location = new System.Drawing.Point(3, 3);
            this._skipLinesFlow.Name = "_skipLinesFlow";
            this._skipLinesFlow.Size = new System.Drawing.Size(96, 62);
            this._skipLinesFlow.TabIndex = 0;
            this._skipLinesFlow.WrapContents = false;
            // 
            // _encodingFlow
            // 
            this._encodingFlow.AutoSize = true;
            this._encodingFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._encodingFlow.Controls.Add(this._encodingLabel);
            this._encodingFlow.Controls.Add(this._encodingCmb);
            this._encodingFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._encodingFlow.Location = new System.Drawing.Point(105, 3);
            this._encodingFlow.Name = "_encodingFlow";
            this._encodingFlow.Size = new System.Drawing.Size(241, 64);
            this._encodingFlow.TabIndex = 1;
            this._encodingFlow.WrapContents = false;
            // 
            // _separatorFlow
            // 
            this._separatorFlow.AutoSize = true;
            this._separatorFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._separatorFlow.Controls.Add(this._separatorLabel);
            this._separatorFlow.Controls.Add(this._separatorCombo);
            this._separatorFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._separatorFlow.Location = new System.Drawing.Point(352, 3);
            this._separatorFlow.Name = "_separatorFlow";
            this._separatorFlow.Size = new System.Drawing.Size(188, 64);
            this._separatorFlow.TabIndex = 2;
            this._separatorFlow.WrapContents = false;
            // 
            // _flow
            // 
            this._flow.AutoSize = true;
            this._flow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._flow.Controls.Add(this._fileInputTitle);
            this._flow.Controls.Add(this._sourceFlow);
            this._flow.Controls.Add(this._headerChk);
            this._flow.Controls.Add(this._tableOutputTitle);
            this._flow.Controls.Add(this._targetFlow);
            this._flow.Controls.Add(this._targetFlow2);
            this._flow.Dock = System.Windows.Forms.DockStyle.Fill;
            this._flow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._flow.Location = new System.Drawing.Point(0, 0);
            this._flow.Name = "_flow";
            this._flow.Size = new System.Drawing.Size(1145, 826);
            this._flow.TabIndex = 39;
            this._flow.WrapContents = false;
            // 
            // _targetFlow
            // 
            this._targetFlow.AutoSize = true;
            this._targetFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._targetFlow.Controls.Add(this._tableNameFlow);
            this._targetFlow.Controls.Add(this._ifExistsFlow);
            this._targetFlow.Location = new System.Drawing.Point(3, 164);
            this._targetFlow.Name = "_targetFlow";
            this._targetFlow.Size = new System.Drawing.Size(438, 70);
            this._targetFlow.TabIndex = 41;
            this._targetFlow.WrapContents = false;
            // 
            // _tableNameFlow
            // 
            this._tableNameFlow.AutoSize = true;
            this._tableNameFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._tableNameFlow.Controls.Add(this._tableLabel);
            this._tableNameFlow.Controls.Add(this._tableCmb);
            this._tableNameFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._tableNameFlow.Location = new System.Drawing.Point(3, 3);
            this._tableNameFlow.Name = "_tableNameFlow";
            this._tableNameFlow.Size = new System.Drawing.Size(241, 64);
            this._tableNameFlow.TabIndex = 0;
            this._tableNameFlow.WrapContents = false;
            // 
            // _ifExistsFlow
            // 
            this._ifExistsFlow.AutoSize = true;
            this._ifExistsFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._ifExistsFlow.Controls.Add(this._ifTableExistsLabel);
            this._ifExistsFlow.Controls.Add(this._ifExistsCmb);
            this._ifExistsFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._ifExistsFlow.Location = new System.Drawing.Point(250, 3);
            this._ifExistsFlow.Name = "_ifExistsFlow";
            this._ifExistsFlow.Size = new System.Drawing.Size(185, 64);
            this._ifExistsFlow.TabIndex = 1;
            this._ifExistsFlow.WrapContents = false;
            // 
            // _targetFlow2
            // 
            this._targetFlow2.AutoSize = true;
            this._targetFlow2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._targetFlow2.Controls.Add(this._convertFailFlow);
            this._targetFlow2.Controls.Add(this._blankValuesFlow);
            this._targetFlow2.Location = new System.Drawing.Point(3, 240);
            this._targetFlow2.Name = "_targetFlow2";
            this._targetFlow2.Size = new System.Drawing.Size(413, 70);
            this._targetFlow2.TabIndex = 43;
            this._targetFlow2.WrapContents = false;
            // 
            // _convertFailFlow
            // 
            this._convertFailFlow.AutoSize = true;
            this._convertFailFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._convertFailFlow.Controls.Add(this._ifConversionFailsLabel);
            this._convertFailFlow.Controls.Add(this._convertFailCmb);
            this._convertFailFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._convertFailFlow.Location = new System.Drawing.Point(3, 3);
            this._convertFailFlow.Name = "_convertFailFlow";
            this._convertFailFlow.Size = new System.Drawing.Size(200, 64);
            this._convertFailFlow.TabIndex = 42;
            this._convertFailFlow.WrapContents = false;
            // 
            // _blankValuesFlow
            // 
            this._blankValuesFlow.AutoSize = true;
            this._blankValuesFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._blankValuesFlow.Controls.Add(this._blankValuesLabel);
            this._blankValuesFlow.Controls.Add(this._blankValuesCombo);
            this._blankValuesFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._blankValuesFlow.Location = new System.Drawing.Point(209, 3);
            this._blankValuesFlow.Name = "_blankValuesFlow";
            this._blankValuesFlow.Size = new System.Drawing.Size(201, 64);
            this._blankValuesFlow.TabIndex = 43;
            this._blankValuesFlow.WrapContents = false;
            // 
            // _blankValuesLabel
            // 
            this._blankValuesLabel.AutoSize = true;
            this._blankValuesLabel.Location = new System.Drawing.Point(3, 0);
            this._blankValuesLabel.Name = "_blankValuesLabel";
            this._blankValuesLabel.Size = new System.Drawing.Size(195, 25);
            this._blankValuesLabel.TabIndex = 0;
            this._blankValuesLabel.Text = "Import blank values as:";
            // 
            // _blankValuesCombo
            // 
            this._blankValuesCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._blankValuesCombo.FormattingEnabled = true;
            this._blankValuesCombo.Location = new System.Drawing.Point(3, 28);
            this._blankValuesCombo.Name = "_blankValuesCombo";
            this._blankValuesCombo.Size = new System.Drawing.Size(182, 33);
            this._blankValuesCombo.TabIndex = 1;
            // 
            // ImportCsvOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this._flow);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "ImportCsvOptionsControl";
            this.Size = new System.Drawing.Size(1145, 826);
            ((System.ComponentModel.ISupportInitialize)(this._skipLinesTxt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this._sourceFlow.ResumeLayout(false);
            this._sourceFlow.PerformLayout();
            this._skipLinesFlow.ResumeLayout(false);
            this._skipLinesFlow.PerformLayout();
            this._encodingFlow.ResumeLayout(false);
            this._encodingFlow.PerformLayout();
            this._separatorFlow.ResumeLayout(false);
            this._separatorFlow.PerformLayout();
            this._flow.ResumeLayout(false);
            this._flow.PerformLayout();
            this._targetFlow.ResumeLayout(false);
            this._targetFlow.PerformLayout();
            this._tableNameFlow.ResumeLayout(false);
            this._tableNameFlow.PerformLayout();
            this._ifExistsFlow.ResumeLayout(false);
            this._ifExistsFlow.PerformLayout();
            this._targetFlow2.ResumeLayout(false);
            this._targetFlow2.PerformLayout();
            this._convertFailFlow.ResumeLayout(false);
            this._convertFailFlow.PerformLayout();
            this._blankValuesFlow.ResumeLayout(false);
            this._blankValuesFlow.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label _ifTableExistsLabel;
        private System.Windows.Forms.Label _tableOutputTitle;
        private System.Windows.Forms.CheckBox _headerChk;
        private System.Windows.Forms.Label _fileInputTitle;
        private System.Windows.Forms.ComboBox _tableCmb;
        private System.Windows.Forms.NumericUpDown _skipLinesTxt;
        private System.Windows.Forms.Label _tableLabel;
        private System.Windows.Forms.Label _skipLinesLabel;
        private System.Windows.Forms.Label _ifConversionFailsLabel;
        private System.Windows.Forms.ComboBox _ifExistsCmb;
        private System.Windows.Forms.ComboBox _encodingCmb;
        private System.Windows.Forms.Label _encodingLabel;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.Label _separatorLabel;
        private System.Windows.Forms.ComboBox _separatorCombo;
        private System.Windows.Forms.ComboBox _convertFailCmb;
        private System.Windows.Forms.FlowLayoutPanel _sourceFlow;
        private System.Windows.Forms.FlowLayoutPanel _skipLinesFlow;
        private System.Windows.Forms.FlowLayoutPanel _encodingFlow;
        private System.Windows.Forms.FlowLayoutPanel _separatorFlow;
        private System.Windows.Forms.FlowLayoutPanel _flow;
        private System.Windows.Forms.FlowLayoutPanel _targetFlow;
        private System.Windows.Forms.FlowLayoutPanel _tableNameFlow;
        private System.Windows.Forms.FlowLayoutPanel _ifExistsFlow;
        private System.Windows.Forms.FlowLayoutPanel _convertFailFlow;
        private System.Windows.Forms.FlowLayoutPanel _targetFlow2;
        private System.Windows.Forms.FlowLayoutPanel _blankValuesFlow;
        private System.Windows.Forms.Label _blankValuesLabel;
        private System.Windows.Forms.ComboBox _blankValuesCombo;
    }
}
