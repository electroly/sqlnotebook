namespace SqlNotebook {
    partial class TableDocumentControl {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableDocumentControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._scriptBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this._grid = new System.Windows.Forms.DataGridView();
            this._toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
            this._toolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._scriptBtn,
            this.toolStripLabel1});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this._toolStrip.ShowItemToolTips = false;
            this._toolStrip.Size = new System.Drawing.Size(745, 30);
            this._toolStrip.Stretch = true;
            this._toolStrip.TabIndex = 3;
            // 
            // _scriptBtn
            // 
            this._scriptBtn.AutoSize = false;
            this._scriptBtn.Image = ((System.Drawing.Image)(resources.GetObject("_scriptBtn.Image")));
            this._scriptBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._scriptBtn.Margin = new System.Windows.Forms.Padding(2, 1, 0, 2);
            this._scriptBtn.Name = "_scriptBtn";
            this._scriptBtn.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this._scriptBtn.Size = new System.Drawing.Size(115, 27);
            this._scriptBtn.Text = "Convert to script";
            this._scriptBtn.Click += new System.EventHandler(this.ScriptBtn_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel1.Margin = new System.Windows.Forms.Padding(0, 1, 7, 2);
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(129, 27);
            this.toolStripLabel1.Text = "Showing top 1000 rows";
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
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this._grid.Location = new System.Drawing.Point(0, 30);
            this._grid.Name = "_grid";
            this._grid.ReadOnly = true;
            this._grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this._grid.RowHeadersVisible = false;
            this._grid.RowHeadersWidth = 25;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._grid.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._grid.ShowCellErrors = false;
            this._grid.ShowCellToolTips = false;
            this._grid.ShowEditingIcon = false;
            this._grid.ShowRowErrors = false;
            this._grid.Size = new System.Drawing.Size(745, 609);
            this._grid.TabIndex = 4;
            // 
            // TableDocumentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._grid);
            this.Controls.Add(this._toolStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TableDocumentControl";
            this.Size = new System.Drawing.Size(745, 639);
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _scriptBtn;
        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    }
}
