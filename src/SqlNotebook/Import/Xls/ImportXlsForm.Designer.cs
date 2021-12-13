
namespace SqlNotebook.Import.Xls {
    partial class ImportXlsForm {
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
            this._previewButton = new System.Windows.Forms.Button();
            this._columnsLabel = new System.Windows.Forms.Label();
            this._columnsPanel = new System.Windows.Forms.Panel();
            this._columnsTable = new System.Windows.Forms.TableLayoutPanel();
            this._optionsLabel = new System.Windows.Forms.Label();
            this._optionsOuterTable = new System.Windows.Forms.TableLayoutPanel();
            this._optionsScrollPanel = new System.Windows.Forms.Panel();
            this._optionsFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._sourceLabel = new System.Windows.Forms.Label();
            this._srcTable = new System.Windows.Forms.TableLayoutPanel();
            this._useSelectionLink = new System.Windows.Forms.LinkLabel();
            this._rowRangeLabel = new System.Windows.Forms.Label();
            this._rowEndText = new System.Windows.Forms.TextBox();
            this._rowToLabel = new System.Windows.Forms.Label();
            this._rowStartText = new System.Windows.Forms.TextBox();
            this._columnRangeLabel = new System.Windows.Forms.Label();
            this._specificRowsCheck = new System.Windows.Forms.CheckBox();
            this._columnEndText = new System.Windows.Forms.TextBox();
            this._columnToLabel = new System.Windows.Forms.Label();
            this._columnStartText = new System.Windows.Forms.TextBox();
            this._specificColumnsCheck = new System.Windows.Forms.CheckBox();
            this._columnNamesCheck = new System.Windows.Forms.CheckBox();
            this._stopAtFirstBlankRowCheck = new System.Windows.Forms.CheckBox();
            this._targetLabel = new System.Windows.Forms.Label();
            this._dstFlow1 = new System.Windows.Forms.FlowLayoutPanel();
            this._tableNameFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._tableNameLabel = new System.Windows.Forms.Label();
            this._tableNameCombo = new System.Windows.Forms.ComboBox();
            this._ifTableExistsFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._ifTableExistsLabel = new System.Windows.Forms.Label();
            this._ifExistsCombo = new System.Windows.Forms.ComboBox();
            this._dstFlow2 = new System.Windows.Forms.FlowLayoutPanel();
            this._convertFailFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._ifConversionFailsLabel = new System.Windows.Forms.Label();
            this._convertFailCombo = new System.Windows.Forms.ComboBox();
            this._blankValuesFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._blankValuesLabel = new System.Windows.Forms.Label();
            this._blankValuesCombo = new System.Windows.Forms.ComboBox();
            this._buttonFlow1 = new System.Windows.Forms.FlowLayoutPanel();
            this._lowerSplit = new System.Windows.Forms.SplitContainer();
            this._originalFileLabel = new System.Windows.Forms.Label();
            this._originalFileTable = new System.Windows.Forms.TableLayoutPanel();
            this._originalFilePanel = new System.Windows.Forms.Panel();
            this._sheetTable = new System.Windows.Forms.TableLayoutPanel();
            this._sheetLabel = new System.Windows.Forms.Label();
            this._sheetCombo = new System.Windows.Forms.ComboBox();
            this._outerSplit = new System.Windows.Forms.SplitContainer();
            this._table = new System.Windows.Forms.TableLayoutPanel();
            this._buttonFlow2 = new System.Windows.Forms.FlowLayoutPanel();
            this._okButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this._updateTimer = new System.Windows.Forms.Timer(this.components);
            this._columnsTable.SuspendLayout();
            this._optionsOuterTable.SuspendLayout();
            this._optionsScrollPanel.SuspendLayout();
            this._optionsFlow.SuspendLayout();
            this._srcTable.SuspendLayout();
            this._dstFlow1.SuspendLayout();
            this._tableNameFlow.SuspendLayout();
            this._ifTableExistsFlow.SuspendLayout();
            this._dstFlow2.SuspendLayout();
            this._convertFailFlow.SuspendLayout();
            this._blankValuesFlow.SuspendLayout();
            this._buttonFlow1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._lowerSplit)).BeginInit();
            this._lowerSplit.Panel1.SuspendLayout();
            this._lowerSplit.Panel2.SuspendLayout();
            this._lowerSplit.SuspendLayout();
            this._originalFileTable.SuspendLayout();
            this._sheetTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._outerSplit)).BeginInit();
            this._outerSplit.Panel1.SuspendLayout();
            this._outerSplit.Panel2.SuspendLayout();
            this._outerSplit.SuspendLayout();
            this._table.SuspendLayout();
            this._buttonFlow2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _previewButton
            // 
            this._previewButton.AutoSize = true;
            this._previewButton.Location = new System.Drawing.Point(3, 3);
            this._previewButton.Name = "_previewButton";
            this._previewButton.Size = new System.Drawing.Size(112, 35);
            this._previewButton.TabIndex = 3;
            this._previewButton.Text = "Preview";
            this._previewButton.UseVisualStyleBackColor = true;
            this._previewButton.Click += new System.EventHandler(this.PreviewButton_Click);
            // 
            // _columnsLabel
            // 
            this._columnsLabel.AutoSize = true;
            this._columnsLabel.Location = new System.Drawing.Point(3, 0);
            this._columnsLabel.Name = "_columnsLabel";
            this._columnsLabel.Size = new System.Drawing.Size(82, 25);
            this._columnsLabel.TabIndex = 0;
            this._columnsLabel.Text = "Columns";
            // 
            // _columnsPanel
            // 
            this._columnsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._columnsPanel.Location = new System.Drawing.Point(0, 25);
            this._columnsPanel.Margin = new System.Windows.Forms.Padding(0);
            this._columnsPanel.Name = "_columnsPanel";
            this._columnsPanel.Size = new System.Drawing.Size(464, 488);
            this._columnsPanel.TabIndex = 1;
            // 
            // _columnsTable
            // 
            this._columnsTable.ColumnCount = 1;
            this._columnsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._columnsTable.Controls.Add(this._columnsLabel, 0, 0);
            this._columnsTable.Controls.Add(this._columnsPanel, 0, 1);
            this._columnsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this._columnsTable.Location = new System.Drawing.Point(0, 0);
            this._columnsTable.Name = "_columnsTable";
            this._columnsTable.RowCount = 2;
            this._columnsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._columnsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._columnsTable.Size = new System.Drawing.Size(464, 513);
            this._columnsTable.TabIndex = 0;
            // 
            // _optionsLabel
            // 
            this._optionsLabel.AutoSize = true;
            this._optionsLabel.Location = new System.Drawing.Point(3, 0);
            this._optionsLabel.Name = "_optionsLabel";
            this._optionsLabel.Size = new System.Drawing.Size(76, 25);
            this._optionsLabel.TabIndex = 0;
            this._optionsLabel.Text = "Options";
            // 
            // _optionsOuterTable
            // 
            this._optionsOuterTable.ColumnCount = 1;
            this._optionsOuterTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._optionsOuterTable.Controls.Add(this._optionsLabel, 0, 0);
            this._optionsOuterTable.Controls.Add(this._optionsScrollPanel, 0, 1);
            this._optionsOuterTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this._optionsOuterTable.Location = new System.Drawing.Point(0, 0);
            this._optionsOuterTable.Name = "_optionsOuterTable";
            this._optionsOuterTable.RowCount = 2;
            this._optionsOuterTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._optionsOuterTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._optionsOuterTable.Size = new System.Drawing.Size(571, 513);
            this._optionsOuterTable.TabIndex = 0;
            // 
            // _optionsScrollPanel
            // 
            this._optionsScrollPanel.AutoScroll = true;
            this._optionsScrollPanel.BackColor = System.Drawing.Color.White;
            this._optionsScrollPanel.Controls.Add(this._optionsFlow);
            this._optionsScrollPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._optionsScrollPanel.Location = new System.Drawing.Point(3, 28);
            this._optionsScrollPanel.Name = "_optionsScrollPanel";
            this._optionsScrollPanel.Size = new System.Drawing.Size(565, 482);
            this._optionsScrollPanel.TabIndex = 1;
            // 
            // _optionsFlow
            // 
            this._optionsFlow.AutoSize = true;
            this._optionsFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._optionsFlow.Controls.Add(this._sourceLabel);
            this._optionsFlow.Controls.Add(this._srcTable);
            this._optionsFlow.Controls.Add(this._columnNamesCheck);
            this._optionsFlow.Controls.Add(this._stopAtFirstBlankRowCheck);
            this._optionsFlow.Controls.Add(this._targetLabel);
            this._optionsFlow.Controls.Add(this._dstFlow1);
            this._optionsFlow.Controls.Add(this._dstFlow2);
            this._optionsFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._optionsFlow.Location = new System.Drawing.Point(0, 0);
            this._optionsFlow.Name = "_optionsFlow";
            this._optionsFlow.Size = new System.Drawing.Size(481, 377);
            this._optionsFlow.TabIndex = 2;
            this._optionsFlow.WrapContents = false;
            // 
            // _sourceLabel
            // 
            this._sourceLabel.AutoSize = true;
            this._sourceLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._sourceLabel.Location = new System.Drawing.Point(3, 0);
            this._sourceLabel.Name = "_sourceLabel";
            this._sourceLabel.Size = new System.Drawing.Size(70, 25);
            this._sourceLabel.TabIndex = 4;
            this._sourceLabel.Text = "Source";
            // 
            // _srcTable
            // 
            this._srcTable.AutoSize = true;
            this._srcTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._srcTable.ColumnCount = 5;
            this._srcTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._srcTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._srcTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._srcTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._srcTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._srcTable.Controls.Add(this._rowRangeLabel, 4, 1);
            this._srcTable.Controls.Add(this._rowEndText, 3, 1);
            this._srcTable.Controls.Add(this._useSelectionLink, 1, 2);
            this._srcTable.Controls.Add(this._rowToLabel, 2, 1);
            this._srcTable.Controls.Add(this._rowStartText, 1, 1);
            this._srcTable.Controls.Add(this._columnRangeLabel, 4, 0);
            this._srcTable.Controls.Add(this._specificRowsCheck, 0, 1);
            this._srcTable.Controls.Add(this._columnEndText, 3, 0);
            this._srcTable.Controls.Add(this._columnToLabel, 2, 0);
            this._srcTable.Controls.Add(this._columnStartText, 1, 0);
            this._srcTable.Controls.Add(this._specificColumnsCheck, 0, 0);
            this._srcTable.Location = new System.Drawing.Point(3, 28);
            this._srcTable.Name = "_srcTable";
            this._srcTable.RowCount = 3;
            this._srcTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._srcTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._srcTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._srcTable.Size = new System.Drawing.Size(475, 99);
            this._srcTable.TabIndex = 5;
            // 
            // _useSelectionLink
            // 
            this._useSelectionLink.AutoSize = true;
            this._srcTable.SetColumnSpan(this._useSelectionLink, 4);
            this._useSelectionLink.Location = new System.Drawing.Point(222, 74);
            this._useSelectionLink.Name = "_useSelectionLink";
            this._useSelectionLink.Size = new System.Drawing.Size(250, 25);
            this._useSelectionLink.TabIndex = 37;
            this._useSelectionLink.TabStop = true;
            this._useSelectionLink.Text = "Use selection from grid above";
            this._useSelectionLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.UseSelectionLink_LinkClicked);
            // 
            // _rowRangeLabel
            // 
            this._rowRangeLabel.AutoSize = true;
            this._rowRangeLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this._rowRangeLabel.Location = new System.Drawing.Point(365, 37);
            this._rowRangeLabel.Name = "_rowRangeLabel";
            this._rowRangeLabel.Size = new System.Drawing.Size(59, 37);
            this._rowRangeLabel.TabIndex = 3;
            this._rowRangeLabel.Text = "(1-10)";
            this._rowRangeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _rowEndText
            // 
            this._rowEndText.Location = new System.Drawing.Point(320, 40);
            this._rowEndText.Name = "_rowEndText";
            this._rowEndText.Size = new System.Drawing.Size(39, 31);
            this._rowEndText.TabIndex = 2;
            this._rowEndText.TextChanged += new System.EventHandler(this.SpecificRowColumnText_TextChanged);
            // 
            // _rowToLabel
            // 
            this._rowToLabel.AutoSize = true;
            this._rowToLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this._rowToLabel.Location = new System.Drawing.Point(285, 37);
            this._rowToLabel.Name = "_rowToLabel";
            this._rowToLabel.Size = new System.Drawing.Size(29, 37);
            this._rowToLabel.TabIndex = 1;
            this._rowToLabel.Text = "to";
            this._rowToLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _rowStartText
            // 
            this._rowStartText.Location = new System.Drawing.Point(222, 40);
            this._rowStartText.Name = "_rowStartText";
            this._rowStartText.Size = new System.Drawing.Size(57, 31);
            this._rowStartText.TabIndex = 0;
            this._rowStartText.TextChanged += new System.EventHandler(this.SpecificRowColumnText_TextChanged);
            // 
            // _columnRangeLabel
            // 
            this._columnRangeLabel.AutoSize = true;
            this._columnRangeLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this._columnRangeLabel.Location = new System.Drawing.Point(365, 0);
            this._columnRangeLabel.Name = "_columnRangeLabel";
            this._columnRangeLabel.Size = new System.Drawing.Size(51, 37);
            this._columnRangeLabel.TabIndex = 3;
            this._columnRangeLabel.Text = "(A-Z)";
            this._columnRangeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _specificRowsCheck
            // 
            this._specificRowsCheck.AutoSize = true;
            this._specificRowsCheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this._specificRowsCheck.Location = new System.Drawing.Point(3, 40);
            this._specificRowsCheck.Name = "_specificRowsCheck";
            this._specificRowsCheck.Size = new System.Drawing.Size(213, 31);
            this._specificRowsCheck.TabIndex = 2;
            this._specificRowsCheck.Text = "Specific &rows only:";
            this._specificRowsCheck.UseVisualStyleBackColor = true;
            this._specificRowsCheck.CheckedChanged += new System.EventHandler(this.SpecificRowsCheck_CheckedChanged);
            // 
            // _columnEndText
            // 
            this._columnEndText.Location = new System.Drawing.Point(320, 3);
            this._columnEndText.Name = "_columnEndText";
            this._columnEndText.Size = new System.Drawing.Size(39, 31);
            this._columnEndText.TabIndex = 2;
            this._columnEndText.TextChanged += new System.EventHandler(this.SpecificRowColumnText_TextChanged);
            // 
            // _columnToLabel
            // 
            this._columnToLabel.AutoSize = true;
            this._columnToLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this._columnToLabel.Location = new System.Drawing.Point(285, 0);
            this._columnToLabel.Name = "_columnToLabel";
            this._columnToLabel.Size = new System.Drawing.Size(29, 37);
            this._columnToLabel.TabIndex = 1;
            this._columnToLabel.Text = "to";
            this._columnToLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _columnStartText
            // 
            this._columnStartText.Location = new System.Drawing.Point(222, 3);
            this._columnStartText.Name = "_columnStartText";
            this._columnStartText.Size = new System.Drawing.Size(57, 31);
            this._columnStartText.TabIndex = 0;
            this._columnStartText.TextChanged += new System.EventHandler(this.SpecificRowColumnText_TextChanged);
            // 
            // _specificColumnsCheck
            // 
            this._specificColumnsCheck.AutoSize = true;
            this._specificColumnsCheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this._specificColumnsCheck.Location = new System.Drawing.Point(3, 3);
            this._specificColumnsCheck.Name = "_specificColumnsCheck";
            this._specificColumnsCheck.Size = new System.Drawing.Size(213, 31);
            this._specificColumnsCheck.TabIndex = 0;
            this._specificColumnsCheck.Text = "Specific &columns only:";
            this._specificColumnsCheck.UseVisualStyleBackColor = true;
            this._specificColumnsCheck.CheckedChanged += new System.EventHandler(this.SpecificColumnsCheck_CheckedChanged);
            // 
            // _columnNamesCheck
            // 
            this._columnNamesCheck.AutoSize = true;
            this._columnNamesCheck.Checked = true;
            this._columnNamesCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this._columnNamesCheck.Location = new System.Drawing.Point(3, 133);
            this._columnNamesCheck.Name = "_columnNamesCheck";
            this._columnNamesCheck.Size = new System.Drawing.Size(309, 29);
            this._columnNamesCheck.TabIndex = 5;
            this._columnNamesCheck.Text = "Sheet includes column &header row";
            this._columnNamesCheck.UseVisualStyleBackColor = true;
            this._columnNamesCheck.CheckedChanged += new System.EventHandler(this.ColumnNamesCheck_CheckedChanged);
            // 
            // _stopAtFirstBlankRowCheck
            // 
            this._stopAtFirstBlankRowCheck.AutoSize = true;
            this._stopAtFirstBlankRowCheck.Checked = true;
            this._stopAtFirstBlankRowCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this._stopAtFirstBlankRowCheck.Location = new System.Drawing.Point(3, 168);
            this._stopAtFirstBlankRowCheck.Name = "_stopAtFirstBlankRowCheck";
            this._stopAtFirstBlankRowCheck.Size = new System.Drawing.Size(213, 29);
            this._stopAtFirstBlankRowCheck.TabIndex = 38;
            this._stopAtFirstBlankRowCheck.Text = "Stop at first blank row";
            this._stopAtFirstBlankRowCheck.UseVisualStyleBackColor = true;
            // 
            // _targetLabel
            // 
            this._targetLabel.AutoSize = true;
            this._targetLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._targetLabel.Location = new System.Drawing.Point(3, 200);
            this._targetLabel.Name = "_targetLabel";
            this._targetLabel.Size = new System.Drawing.Size(110, 25);
            this._targetLabel.TabIndex = 6;
            this._targetLabel.Text = "Destination";
            // 
            // _dstFlow1
            // 
            this._dstFlow1.AutoSize = true;
            this._dstFlow1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._dstFlow1.Controls.Add(this._tableNameFlow);
            this._dstFlow1.Controls.Add(this._ifTableExistsFlow);
            this._dstFlow1.Location = new System.Drawing.Point(3, 228);
            this._dstFlow1.Name = "_dstFlow1";
            this._dstFlow1.Size = new System.Drawing.Size(443, 70);
            this._dstFlow1.TabIndex = 39;
            this._dstFlow1.WrapContents = false;
            // 
            // _tableNameFlow
            // 
            this._tableNameFlow.AutoSize = true;
            this._tableNameFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._tableNameFlow.Controls.Add(this._tableNameLabel);
            this._tableNameFlow.Controls.Add(this._tableNameCombo);
            this._tableNameFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._tableNameFlow.Location = new System.Drawing.Point(3, 3);
            this._tableNameFlow.Name = "_tableNameFlow";
            this._tableNameFlow.Size = new System.Drawing.Size(244, 64);
            this._tableNameFlow.TabIndex = 0;
            this._tableNameFlow.WrapContents = false;
            // 
            // _tableNameLabel
            // 
            this._tableNameLabel.AutoSize = true;
            this._tableNameLabel.Location = new System.Drawing.Point(3, 0);
            this._tableNameLabel.Name = "_tableNameLabel";
            this._tableNameLabel.Size = new System.Drawing.Size(151, 25);
            this._tableNameLabel.TabIndex = 7;
            this._tableNameLabel.Text = "Import into &table:";
            this._tableNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _tableNameCombo
            // 
            this._tableNameCombo.FormattingEnabled = true;
            this._tableNameCombo.Location = new System.Drawing.Point(3, 28);
            this._tableNameCombo.Name = "_tableNameCombo";
            this._tableNameCombo.Size = new System.Drawing.Size(238, 33);
            this._tableNameCombo.TabIndex = 8;
            // 
            // _ifTableExistsFlow
            // 
            this._ifTableExistsFlow.AutoSize = true;
            this._ifTableExistsFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._ifTableExistsFlow.Controls.Add(this._ifTableExistsLabel);
            this._ifTableExistsFlow.Controls.Add(this._ifExistsCombo);
            this._ifTableExistsFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._ifTableExistsFlow.Location = new System.Drawing.Point(253, 3);
            this._ifTableExistsFlow.Name = "_ifTableExistsFlow";
            this._ifTableExistsFlow.Size = new System.Drawing.Size(187, 64);
            this._ifTableExistsFlow.TabIndex = 1;
            this._ifTableExistsFlow.WrapContents = false;
            // 
            // _ifTableExistsLabel
            // 
            this._ifTableExistsLabel.AutoSize = true;
            this._ifTableExistsLabel.Location = new System.Drawing.Point(3, 0);
            this._ifTableExistsLabel.Name = "_ifTableExistsLabel";
            this._ifTableExistsLabel.Size = new System.Drawing.Size(149, 25);
            this._ifTableExistsLabel.TabIndex = 24;
            this._ifTableExistsLabel.Text = "If the table e&xists:";
            this._ifTableExistsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _ifExistsCombo
            // 
            this._ifExistsCombo.DisplayMember = "Item2";
            this._ifExistsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._ifExistsCombo.FormattingEnabled = true;
            this._ifExistsCombo.Location = new System.Drawing.Point(3, 28);
            this._ifExistsCombo.Name = "_ifExistsCombo";
            this._ifExistsCombo.Size = new System.Drawing.Size(181, 33);
            this._ifExistsCombo.TabIndex = 34;
            this._ifExistsCombo.ValueMember = "Item1";
            // 
            // _dstFlow2
            // 
            this._dstFlow2.AutoSize = true;
            this._dstFlow2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._dstFlow2.Controls.Add(this._convertFailFlow);
            this._dstFlow2.Controls.Add(this._blankValuesFlow);
            this._dstFlow2.Location = new System.Drawing.Point(3, 304);
            this._dstFlow2.Name = "_dstFlow2";
            this._dstFlow2.Size = new System.Drawing.Size(413, 70);
            this._dstFlow2.TabIndex = 40;
            this._dstFlow2.WrapContents = false;
            // 
            // _convertFailFlow
            // 
            this._convertFailFlow.AutoSize = true;
            this._convertFailFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._convertFailFlow.Controls.Add(this._ifConversionFailsLabel);
            this._convertFailFlow.Controls.Add(this._convertFailCombo);
            this._convertFailFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._convertFailFlow.Location = new System.Drawing.Point(3, 3);
            this._convertFailFlow.Name = "_convertFailFlow";
            this._convertFailFlow.Size = new System.Drawing.Size(200, 64);
            this._convertFailFlow.TabIndex = 0;
            this._convertFailFlow.WrapContents = false;
            // 
            // _ifConversionFailsLabel
            // 
            this._ifConversionFailsLabel.AutoSize = true;
            this._ifConversionFailsLabel.Location = new System.Drawing.Point(3, 0);
            this._ifConversionFailsLabel.Name = "_ifConversionFailsLabel";
            this._ifConversionFailsLabel.Size = new System.Drawing.Size(194, 25);
            this._ifConversionFailsLabel.TabIndex = 35;
            this._ifConversionFailsLabel.Text = "If data conversion &fails:";
            this._ifConversionFailsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _convertFailCombo
            // 
            this._convertFailCombo.DisplayMember = "Item2";
            this._convertFailCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._convertFailCombo.FormattingEnabled = true;
            this._convertFailCombo.Location = new System.Drawing.Point(3, 28);
            this._convertFailCombo.Name = "_convertFailCombo";
            this._convertFailCombo.Size = new System.Drawing.Size(179, 33);
            this._convertFailCombo.TabIndex = 36;
            this._convertFailCombo.ValueMember = "Item1";
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
            this._blankValuesFlow.TabIndex = 1;
            this._blankValuesFlow.WrapContents = false;
            // 
            // _blankValuesLabel
            // 
            this._blankValuesLabel.AutoSize = true;
            this._blankValuesLabel.Location = new System.Drawing.Point(3, 0);
            this._blankValuesLabel.Name = "_blankValuesLabel";
            this._blankValuesLabel.Size = new System.Drawing.Size(195, 25);
            this._blankValuesLabel.TabIndex = 39;
            this._blankValuesLabel.Text = "Import blank values as:";
            // 
            // _blankValuesCombo
            // 
            this._blankValuesCombo.DisplayMember = "Item2";
            this._blankValuesCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._blankValuesCombo.FormattingEnabled = true;
            this._blankValuesCombo.Location = new System.Drawing.Point(3, 28);
            this._blankValuesCombo.Name = "_blankValuesCombo";
            this._blankValuesCombo.Size = new System.Drawing.Size(182, 33);
            this._blankValuesCombo.TabIndex = 40;
            this._blankValuesCombo.ValueMember = "Item1";
            // 
            // _buttonFlow1
            // 
            this._buttonFlow1.AutoSize = true;
            this._buttonFlow1.Controls.Add(this._previewButton);
            this._buttonFlow1.Location = new System.Drawing.Point(3, 769);
            this._buttonFlow1.Name = "_buttonFlow1";
            this._buttonFlow1.Size = new System.Drawing.Size(118, 41);
            this._buttonFlow1.TabIndex = 2;
            // 
            // _lowerSplit
            // 
            this._lowerSplit.Cursor = System.Windows.Forms.Cursors.VSplit;
            this._lowerSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lowerSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._lowerSplit.Location = new System.Drawing.Point(0, 0);
            this._lowerSplit.Name = "_lowerSplit";
            // 
            // _lowerSplit.Panel1
            // 
            this._lowerSplit.Panel1.Controls.Add(this._optionsOuterTable);
            // 
            // _lowerSplit.Panel2
            // 
            this._lowerSplit.Panel2.Controls.Add(this._columnsTable);
            this._lowerSplit.Size = new System.Drawing.Size(1039, 513);
            this._lowerSplit.SplitterDistance = 571;
            this._lowerSplit.TabIndex = 0;
            // 
            // _originalFileLabel
            // 
            this._originalFileLabel.AutoSize = true;
            this._originalFileLabel.Location = new System.Drawing.Point(3, 0);
            this._originalFileLabel.Name = "_originalFileLabel";
            this._originalFileLabel.Size = new System.Drawing.Size(105, 25);
            this._originalFileLabel.TabIndex = 0;
            this._originalFileLabel.Text = "Original File";
            // 
            // _originalFileTable
            // 
            this._originalFileTable.ColumnCount = 1;
            this._originalFileTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._originalFileTable.Controls.Add(this._originalFileLabel, 0, 0);
            this._originalFileTable.Controls.Add(this._originalFilePanel, 0, 2);
            this._originalFileTable.Controls.Add(this._sheetTable, 0, 1);
            this._originalFileTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this._originalFileTable.Location = new System.Drawing.Point(0, 0);
            this._originalFileTable.Name = "_originalFileTable";
            this._originalFileTable.RowCount = 3;
            this._originalFileTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._originalFileTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._originalFileTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._originalFileTable.Size = new System.Drawing.Size(1039, 243);
            this._originalFileTable.TabIndex = 1;
            // 
            // _originalFilePanel
            // 
            this._originalFilePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._originalFilePanel.Location = new System.Drawing.Point(0, 70);
            this._originalFilePanel.Margin = new System.Windows.Forms.Padding(0);
            this._originalFilePanel.Name = "_originalFilePanel";
            this._originalFilePanel.Size = new System.Drawing.Size(1039, 173);
            this._originalFilePanel.TabIndex = 1;
            // 
            // _sheetTable
            // 
            this._sheetTable.AutoSize = true;
            this._sheetTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._sheetTable.BackColor = System.Drawing.Color.White;
            this._sheetTable.ColumnCount = 2;
            this._sheetTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._sheetTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._sheetTable.Controls.Add(this._sheetLabel, 0, 0);
            this._sheetTable.Controls.Add(this._sheetCombo, 1, 0);
            this._sheetTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this._sheetTable.Location = new System.Drawing.Point(3, 28);
            this._sheetTable.Name = "_sheetTable";
            this._sheetTable.RowCount = 1;
            this._sheetTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._sheetTable.Size = new System.Drawing.Size(1033, 39);
            this._sheetTable.TabIndex = 2;
            // 
            // _sheetLabel
            // 
            this._sheetLabel.AutoSize = true;
            this._sheetLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._sheetLabel.Location = new System.Drawing.Point(3, 0);
            this._sheetLabel.Name = "_sheetLabel";
            this._sheetLabel.Size = new System.Drawing.Size(60, 39);
            this._sheetLabel.TabIndex = 0;
            this._sheetLabel.Text = "&Sheet:";
            this._sheetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _sheetCombo
            // 
            this._sheetCombo.DisplayMember = "Item2";
            this._sheetCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._sheetCombo.FormattingEnabled = true;
            this._sheetCombo.Location = new System.Drawing.Point(69, 3);
            this._sheetCombo.Name = "_sheetCombo";
            this._sheetCombo.Size = new System.Drawing.Size(182, 33);
            this._sheetCombo.TabIndex = 1;
            this._sheetCombo.ValueMember = "Item1";
            this._sheetCombo.SelectedIndexChanged += new System.EventHandler(this.SheetCombo_SelectedIndexChanged);
            // 
            // _outerSplit
            // 
            this._table.SetColumnSpan(this._outerSplit, 2);
            this._outerSplit.Cursor = System.Windows.Forms.Cursors.HSplit;
            this._outerSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this._outerSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this._outerSplit.Location = new System.Drawing.Point(3, 3);
            this._outerSplit.Name = "_outerSplit";
            this._outerSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _outerSplit.Panel1
            // 
            this._outerSplit.Panel1.Controls.Add(this._originalFileTable);
            // 
            // _outerSplit.Panel2
            // 
            this._outerSplit.Panel2.Controls.Add(this._lowerSplit);
            this._outerSplit.Size = new System.Drawing.Size(1039, 760);
            this._outerSplit.SplitterDistance = 243;
            this._outerSplit.TabIndex = 1;
            // 
            // _table
            // 
            this._table.ColumnCount = 2;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._table.Controls.Add(this._buttonFlow2, 1, 1);
            this._table.Controls.Add(this._outerSplit, 0, 0);
            this._table.Controls.Add(this._buttonFlow1, 0, 1);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 2;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.Size = new System.Drawing.Size(1045, 813);
            this._table.TabIndex = 12;
            // 
            // _buttonFlow2
            // 
            this._buttonFlow2.AutoSize = true;
            this._buttonFlow2.Controls.Add(this._okButton);
            this._buttonFlow2.Controls.Add(this._cancelButton);
            this._buttonFlow2.Dock = System.Windows.Forms.DockStyle.Right;
            this._buttonFlow2.Location = new System.Drawing.Point(854, 769);
            this._buttonFlow2.Name = "_buttonFlow2";
            this._buttonFlow2.Size = new System.Drawing.Size(188, 41);
            this._buttonFlow2.TabIndex = 0;
            this._buttonFlow2.WrapContents = false;
            // 
            // _okButton
            // 
            this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._okButton.AutoSize = true;
            this._okButton.Location = new System.Drawing.Point(3, 3);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(88, 35);
            this._okButton.TabIndex = 5;
            this._okButton.Text = "Import";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.AutoSize = true;
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(97, 3);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(88, 35);
            this._cancelButton.TabIndex = 6;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _updateTimer
            // 
            this._updateTimer.Interval = 25;
            this._updateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // ImportXlsForm
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(1045, 813);
            this.Controls.Add(this._table);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportXlsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import";
            this._columnsTable.ResumeLayout(false);
            this._columnsTable.PerformLayout();
            this._optionsOuterTable.ResumeLayout(false);
            this._optionsOuterTable.PerformLayout();
            this._optionsScrollPanel.ResumeLayout(false);
            this._optionsScrollPanel.PerformLayout();
            this._optionsFlow.ResumeLayout(false);
            this._optionsFlow.PerformLayout();
            this._srcTable.ResumeLayout(false);
            this._srcTable.PerformLayout();
            this._dstFlow1.ResumeLayout(false);
            this._dstFlow1.PerformLayout();
            this._tableNameFlow.ResumeLayout(false);
            this._tableNameFlow.PerformLayout();
            this._ifTableExistsFlow.ResumeLayout(false);
            this._ifTableExistsFlow.PerformLayout();
            this._dstFlow2.ResumeLayout(false);
            this._dstFlow2.PerformLayout();
            this._convertFailFlow.ResumeLayout(false);
            this._convertFailFlow.PerformLayout();
            this._blankValuesFlow.ResumeLayout(false);
            this._blankValuesFlow.PerformLayout();
            this._buttonFlow1.ResumeLayout(false);
            this._buttonFlow1.PerformLayout();
            this._lowerSplit.Panel1.ResumeLayout(false);
            this._lowerSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._lowerSplit)).EndInit();
            this._lowerSplit.ResumeLayout(false);
            this._originalFileTable.ResumeLayout(false);
            this._originalFileTable.PerformLayout();
            this._sheetTable.ResumeLayout(false);
            this._sheetTable.PerformLayout();
            this._outerSplit.Panel1.ResumeLayout(false);
            this._outerSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._outerSplit)).EndInit();
            this._outerSplit.ResumeLayout(false);
            this._table.ResumeLayout(false);
            this._table.PerformLayout();
            this._buttonFlow2.ResumeLayout(false);
            this._buttonFlow2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _previewButton;
        private System.Windows.Forms.Label _columnsLabel;
        private System.Windows.Forms.Panel _columnsPanel;
        private System.Windows.Forms.TableLayoutPanel _columnsTable;
        private System.Windows.Forms.Label _optionsLabel;
        private System.Windows.Forms.TableLayoutPanel _optionsOuterTable;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow1;
        private System.Windows.Forms.SplitContainer _lowerSplit;
        private System.Windows.Forms.Label _originalFileLabel;
        private System.Windows.Forms.TableLayoutPanel _originalFileTable;
        private System.Windows.Forms.Panel _originalFilePanel;
        private System.Windows.Forms.SplitContainer _outerSplit;
        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow2;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.TableLayoutPanel _sheetTable;
        private System.Windows.Forms.Label _sheetLabel;
        private System.Windows.Forms.ComboBox _sheetCombo;
        private System.Windows.Forms.Panel _optionsScrollPanel;
        private System.Windows.Forms.CheckBox _specificColumnsCheck;
        private System.Windows.Forms.TextBox _columnStartText;
        private System.Windows.Forms.Label _columnToLabel;
        private System.Windows.Forms.TextBox _columnEndText;
        private System.Windows.Forms.Label _columnRangeLabel;
        private System.Windows.Forms.CheckBox _specificRowsCheck;
        private System.Windows.Forms.TextBox _rowStartText;
        private System.Windows.Forms.Label _rowToLabel;
        private System.Windows.Forms.TextBox _rowEndText;
        private System.Windows.Forms.Label _rowRangeLabel;
        private System.Windows.Forms.Label _sourceLabel;
        private System.Windows.Forms.CheckBox _columnNamesCheck;
        private System.Windows.Forms.Label _targetLabel;
        private System.Windows.Forms.Label _tableNameLabel;
        private System.Windows.Forms.ComboBox _tableNameCombo;
        private System.Windows.Forms.Label _ifTableExistsLabel;
        private System.Windows.Forms.ComboBox _ifExistsCombo;
        private System.Windows.Forms.Label _ifConversionFailsLabel;
        private System.Windows.Forms.ComboBox _convertFailCombo;
        private System.Windows.Forms.LinkLabel _useSelectionLink;
        private System.Windows.Forms.Timer _updateTimer;
        private System.Windows.Forms.CheckBox _stopAtFirstBlankRowCheck;
        private System.Windows.Forms.Label _blankValuesLabel;
        private System.Windows.Forms.ComboBox _blankValuesCombo;
        private System.Windows.Forms.FlowLayoutPanel _optionsFlow;
        private System.Windows.Forms.TableLayoutPanel _srcTable;
        private System.Windows.Forms.FlowLayoutPanel _dstFlow1;
        private System.Windows.Forms.FlowLayoutPanel _tableNameFlow;
        private System.Windows.Forms.FlowLayoutPanel _ifTableExistsFlow;
        private System.Windows.Forms.FlowLayoutPanel _dstFlow2;
        private System.Windows.Forms.FlowLayoutPanel _convertFailFlow;
        private System.Windows.Forms.FlowLayoutPanel _blankValuesFlow;
    }
}