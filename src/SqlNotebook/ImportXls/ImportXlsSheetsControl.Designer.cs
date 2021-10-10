namespace SqlNotebook.ImportXls {
    partial class ImportXlsSheetsControl {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this._grid = new System.Windows.Forms.DataGridView();
            this._toBeImportedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this._originalNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._newNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._importTableExistsColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this._onErrorColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._toBeImportedColumn,
            this._originalNameColumn,
            this._newNameColumn,
            this._importTableExistsColumn,
            this._onErrorColumn});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._grid.DefaultCellStyle = dataGridViewCellStyle3;
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this._grid.Location = new System.Drawing.Point(0, 0);
            this._grid.Name = "_grid";
            this._grid.RowHeadersVisible = false;
            this._grid.RowHeadersWidth = 62;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._grid.ShowCellToolTips = false;
            this._grid.ShowEditingIcon = false;
            this._grid.ShowRowErrors = false;
            this._grid.Size = new System.Drawing.Size(1122, 440);
            this._grid.TabIndex = 3;
            this._grid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_CellValueChanged);
            // 
            // _toBeImportedColumn
            // 
            this._toBeImportedColumn.DataPropertyName = "ToBeImported";
            this._toBeImportedColumn.Frozen = true;
            this._toBeImportedColumn.HeaderText = "Import?";
            this._toBeImportedColumn.MinimumWidth = 8;
            this._toBeImportedColumn.Name = "_toBeImportedColumn";
            this._toBeImportedColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this._toBeImportedColumn.Width = 60;
            // 
            // _originalNameColumn
            // 
            this._originalNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._originalNameColumn.DataPropertyName = "OriginalName";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this._originalNameColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this._originalNameColumn.HeaderText = "Original worksheet name";
            this._originalNameColumn.MinimumWidth = 8;
            this._originalNameColumn.Name = "_originalNameColumn";
            this._originalNameColumn.ReadOnly = true;
            this._originalNameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this._originalNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _newNameColumn
            // 
            this._newNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._newNameColumn.DataPropertyName = "NewName";
            this._newNameColumn.HeaderText = "Imported name";
            this._newNameColumn.MinimumWidth = 8;
            this._newNameColumn.Name = "_newNameColumn";
            this._newNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _importTableExistsColumn
            // 
            this._importTableExistsColumn.DataPropertyName = "ImportTableExistsString";
            this._importTableExistsColumn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this._importTableExistsColumn.HeaderText = "If table exists...";
            this._importTableExistsColumn.MinimumWidth = 8;
            this._importTableExistsColumn.Name = "_importTableExistsColumn";
            this._importTableExistsColumn.Width = 200;
            // 
            // _onErrorColumn
            // 
            this._onErrorColumn.DataPropertyName = "ImportConversionFailString";
            this._onErrorColumn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this._onErrorColumn.HeaderText = "If data conversion fails...";
            this._onErrorColumn.MinimumWidth = 8;
            this._onErrorColumn.Name = "_onErrorColumn";
            this._onErrorColumn.Width = 200;
            // 
            // ImportXlsSheetsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this._grid);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "ImportXlsSheetsControl";
            this.Size = new System.Drawing.Size(1122, 440);
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.DataGridViewCheckBoxColumn _toBeImportedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _originalNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _newNameColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn _importTableExistsColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn _onErrorColumn;
    }
}
