namespace SqlNotebook {
    partial class ImportColumnsControl {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this._grid = new System.Windows.Forms.DataGridView();
            this._importColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this._sourceNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._targetNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._conversionColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.SuspendLayout();
            // 
            // _grid
            // 
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AllowUserToResizeRows = false;
            this._grid.BackgroundColor = System.Drawing.SystemColors.Window;
            this._grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._importColumn,
            this._sourceNameColumn,
            this._targetNameColumn,
            this._conversionColumn});
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this._grid.Location = new System.Drawing.Point(0, 0);
            this._grid.Name = "_grid";
            this._grid.RowHeadersVisible = false;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._grid.ShowCellToolTips = false;
            this._grid.ShowEditingIcon = false;
            this._grid.ShowRowErrors = false;
            this._grid.Size = new System.Drawing.Size(540, 260);
            this._grid.TabIndex = 1;
            // 
            // _importColumn
            // 
            this._importColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this._importColumn.DataPropertyName = "import";
            this._importColumn.HeaderText = "Import?";
            this._importColumn.Name = "_importColumn";
            this._importColumn.Width = 60;
            // 
            // _sourceNameColumn
            // 
            this._sourceNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._sourceNameColumn.DataPropertyName = "source_name";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this._sourceNameColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this._sourceNameColumn.HeaderText = "Original name";
            this._sourceNameColumn.Name = "_sourceNameColumn";
            this._sourceNameColumn.ReadOnly = true;
            this._sourceNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _targetNameColumn
            // 
            this._targetNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._targetNameColumn.DataPropertyName = "target_name";
            this._targetNameColumn.HeaderText = "New column name";
            this._targetNameColumn.Name = "_targetNameColumn";
            this._targetNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _conversionColumn
            // 
            this._conversionColumn.DataPropertyName = "conversion";
            this._conversionColumn.FillWeight = 130F;
            this._conversionColumn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this._conversionColumn.HeaderText = "Conversion";
            this._conversionColumn.Items.AddRange(new object[] {
            "TEXT",
            "INTEGER",
            "REAL",
            "DATE",
            "DATETIME",
            "DATETIMEOFFSET"});
            this._conversionColumn.Name = "_conversionColumn";
            this._conversionColumn.Width = 130;
            // 
            // ImportColumnsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._grid);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ImportColumnsControl";
            this.Size = new System.Drawing.Size(540, 260);
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.DataGridViewCheckBoxColumn _importColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _sourceNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _targetNameColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn _conversionColumn;
    }
}
