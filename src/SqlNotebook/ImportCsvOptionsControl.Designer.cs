namespace SqlNotebook {
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
            this._convertFailCmb = new System.Windows.Forms.ComboBox();
            this._columnNamesLabel = new System.Windows.Forms.Label();
            this._encodingCmb = new System.Windows.Forms.ComboBox();
            this._encodingLabel = new System.Windows.Forms.Label();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this._skipLinesTxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _ifTableExistsLabel
            // 
            this._ifTableExistsLabel.AutoSize = true;
            this._ifTableExistsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ifTableExistsLabel.Location = new System.Drawing.Point(3, 200);
            this._ifTableExistsLabel.Name = "_ifTableExistsLabel";
            this._ifTableExistsLabel.Size = new System.Drawing.Size(194, 39);
            this._ifTableExistsLabel.TabIndex = 23;
            this._ifTableExistsLabel.Text = "If the table exists:";
            this._ifTableExistsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _tableOutputTitle
            // 
            this._tableOutputTitle.AutoSize = true;
            this._tableOutputTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableOutputTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._tableOutputTitle.Location = new System.Drawing.Point(3, 136);
            this._tableOutputTitle.Name = "_tableOutputTitle";
            this._tableOutputTitle.Size = new System.Drawing.Size(194, 25);
            this._tableOutputTitle.TabIndex = 27;
            this._tableOutputTitle.Text = "Target";
            this._tableOutputTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _headerChk
            // 
            this._headerChk.AutoSize = true;
            this._headerChk.Checked = true;
            this._headerChk.CheckState = System.Windows.Forms.CheckState.Checked;
            this._headerChk.Location = new System.Drawing.Point(203, 65);
            this._headerChk.Name = "_headerChk";
            this._headerChk.Size = new System.Drawing.Size(264, 29);
            this._headerChk.TabIndex = 26;
            this._headerChk.Text = "File includes column headers";
            this._headerChk.UseVisualStyleBackColor = true;
            // 
            // _fileInputTitle
            // 
            this._fileInputTitle.AutoSize = true;
            this._fileInputTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this._fileInputTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._fileInputTitle.Location = new System.Drawing.Point(3, 0);
            this._fileInputTitle.Name = "_fileInputTitle";
            this._fileInputTitle.Size = new System.Drawing.Size(194, 25);
            this._fileInputTitle.TabIndex = 21;
            this._fileInputTitle.Text = "Source";
            this._fileInputTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _tableCmb
            // 
            this._tableCmb.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this._tableCmb.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this._tableCmb.FormattingEnabled = true;
            this._tableCmb.Location = new System.Drawing.Point(203, 164);
            this._tableCmb.Name = "_tableCmb";
            this._tableCmb.Size = new System.Drawing.Size(235, 33);
            this._tableCmb.TabIndex = 22;
            // 
            // _skipLinesTxt
            // 
            this._skipLinesTxt.Location = new System.Drawing.Point(203, 28);
            this._skipLinesTxt.Name = "_skipLinesTxt";
            this._skipLinesTxt.Size = new System.Drawing.Size(71, 31);
            this._skipLinesTxt.TabIndex = 25;
            this._skipLinesTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _tableLabel
            // 
            this._tableLabel.AutoSize = true;
            this._tableLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLabel.Location = new System.Drawing.Point(3, 161);
            this._tableLabel.Name = "_tableLabel";
            this._tableLabel.Size = new System.Drawing.Size(194, 39);
            this._tableLabel.TabIndex = 20;
            this._tableLabel.Text = "Import into table:";
            this._tableLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _skipLinesLabel
            // 
            this._skipLinesLabel.AutoSize = true;
            this._skipLinesLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._skipLinesLabel.Location = new System.Drawing.Point(3, 25);
            this._skipLinesLabel.Name = "_skipLinesLabel";
            this._skipLinesLabel.Size = new System.Drawing.Size(194, 37);
            this._skipLinesLabel.TabIndex = 24;
            this._skipLinesLabel.Text = "Initial lines to skip:";
            this._skipLinesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _ifConversionFailsLabel
            // 
            this._ifConversionFailsLabel.AutoSize = true;
            this._ifConversionFailsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ifConversionFailsLabel.Location = new System.Drawing.Point(3, 239);
            this._ifConversionFailsLabel.Name = "_ifConversionFailsLabel";
            this._ifConversionFailsLabel.Size = new System.Drawing.Size(194, 39);
            this._ifConversionFailsLabel.TabIndex = 31;
            this._ifConversionFailsLabel.Text = "If data conversion fails:";
            this._ifConversionFailsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _ifExistsCmb
            // 
            this._ifExistsCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._ifExistsCmb.FormattingEnabled = true;
            this._ifExistsCmb.Location = new System.Drawing.Point(203, 203);
            this._ifExistsCmb.Name = "_ifExistsCmb";
            this._ifExistsCmb.Size = new System.Drawing.Size(179, 33);
            this._ifExistsCmb.TabIndex = 33;
            // 
            // _convertFailCmb
            // 
            this._convertFailCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._convertFailCmb.FormattingEnabled = true;
            this._convertFailCmb.Location = new System.Drawing.Point(203, 242);
            this._convertFailCmb.Name = "_convertFailCmb";
            this._convertFailCmb.Size = new System.Drawing.Size(179, 33);
            this._convertFailCmb.TabIndex = 34;
            // 
            // _columnNamesLabel
            // 
            this._columnNamesLabel.AutoSize = true;
            this._columnNamesLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._columnNamesLabel.Location = new System.Drawing.Point(3, 62);
            this._columnNamesLabel.Name = "_columnNamesLabel";
            this._columnNamesLabel.Size = new System.Drawing.Size(194, 35);
            this._columnNamesLabel.TabIndex = 35;
            this._columnNamesLabel.Text = "Column names:";
            this._columnNamesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _encodingCmb
            // 
            this._encodingCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._encodingCmb.FormattingEnabled = true;
            this._encodingCmb.Location = new System.Drawing.Point(203, 100);
            this._encodingCmb.Name = "_encodingCmb";
            this._encodingCmb.Size = new System.Drawing.Size(235, 33);
            this._encodingCmb.TabIndex = 36;
            // 
            // _encodingLabel
            // 
            this._encodingLabel.AutoSize = true;
            this._encodingLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._encodingLabel.Location = new System.Drawing.Point(3, 97);
            this._encodingLabel.Name = "_encodingLabel";
            this._encodingLabel.Size = new System.Drawing.Size(194, 39);
            this._encodingLabel.TabIndex = 37;
            this._encodingLabel.Text = "Text encoding:";
            this._encodingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this._fileInputTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._convertFailCmb, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this._encodingCmb, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this._ifConversionFailsLabel, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this._encodingLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this._skipLinesLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._ifTableExistsLabel, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this._skipLinesTxt, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._tableCmb, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this._tableOutputTitle, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this._tableLabel, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this._columnNamesLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this._headerChk, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this._ifExistsCmb, 1, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(692, 684);
            this.tableLayoutPanel1.TabIndex = 38;
            // 
            // ImportCsvOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "ImportCsvOptionsControl";
            this.Size = new System.Drawing.Size(692, 684);
            ((System.ComponentModel.ISupportInitialize)(this._skipLinesTxt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
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
        private System.Windows.Forms.ComboBox _convertFailCmb;
        private System.Windows.Forms.Label _columnNamesLabel;
        private System.Windows.Forms.ComboBox _encodingCmb;
        private System.Windows.Forms.Label _encodingLabel;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
