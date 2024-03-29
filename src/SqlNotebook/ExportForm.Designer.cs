﻿namespace SqlNotebook {
    partial class ExportForm {
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
            System.Windows.Forms.ColumnHeader _columnHeader;
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Scripts", System.Windows.Forms.HorizontalAlignment.Center);
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("Tables", System.Windows.Forms.HorizontalAlignment.Center);
            System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("Views", System.Windows.Forms.HorizontalAlignment.Center);
            this._helpLabel = new System.Windows.Forms.Label();
            this._cancelButton = new System.Windows.Forms.Button();
            this._exportButton = new System.Windows.Forms.Button();
            this._list = new System.Windows.Forms.ListView();
            this._table = new System.Windows.Forms.TableLayoutPanel();
            this._buttonFlow = new System.Windows.Forms.FlowLayoutPanel();
            _columnHeader = new System.Windows.Forms.ColumnHeader();
            this._table.SuspendLayout();
            this._buttonFlow.SuspendLayout();
            this.SuspendLayout();
            // 
            // _helpLabel
            // 
            this._helpLabel.AutoSize = true;
            this._helpLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._helpLabel.Location = new System.Drawing.Point(3, 0);
            this._helpLabel.Name = "_helpLabel";
            this._helpLabel.Size = new System.Drawing.Size(291, 32);
            this._helpLabel.TabIndex = 0;
            this._helpLabel.Text = "&Choose an item to export.";
            // 
            // _cancelBtn
            // 
            this._cancelButton.AutoSize = true;
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(97, 3);
            this._cancelButton.Name = "_cancelBtn";
            this._cancelButton.Size = new System.Drawing.Size(88, 35);
            this._cancelButton.TabIndex = 12;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _saveBtn
            // 
            this._exportButton.AutoSize = true;
            this._exportButton.Enabled = false;
            this._exportButton.Location = new System.Drawing.Point(3, 3);
            this._exportButton.Name = "_saveBtn";
            this._exportButton.Size = new System.Drawing.Size(88, 35);
            this._exportButton.TabIndex = 11;
            this._exportButton.Text = "Export";
            this._exportButton.UseVisualStyleBackColor = true;
            this._exportButton.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // _list
            // 
            this._list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            _columnHeader});
            this._list.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup4.Header = "Scripts";
            listViewGroup4.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup4.Name = "Script";
            listViewGroup5.Header = "Tables";
            listViewGroup5.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup5.Name = "Table";
            listViewGroup6.Header = "Views";
            listViewGroup6.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup6.Name = "View";
            this._list.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup4,
            listViewGroup5,
            listViewGroup6});
            this._list.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this._list.LabelWrap = false;
            this._list.Location = new System.Drawing.Point(3, 35);
            this._list.MultiSelect = false;
            this._list.Name = "_list";
            this._list.Size = new System.Drawing.Size(643, 553);
            this._list.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this._list.TabIndex = 1;
            this._list.UseCompatibleStateImageBehavior = false;
            this._list.View = System.Windows.Forms.View.Details;
            this._list.SelectedIndexChanged += new System.EventHandler(this.List_SelectedIndexChanged);
            // 
            // _table
            // 
            this._table.ColumnCount = 1;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.Controls.Add(this._buttonFlow, 0, 2);
            this._table.Controls.Add(this._helpLabel, 0, 0);
            this._table.Controls.Add(this._list, 0, 1);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 3;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.Size = new System.Drawing.Size(649, 638);
            this._table.TabIndex = 13;
            // 
            // _buttonFlow
            // 
            this._buttonFlow.AutoSize = true;
            this._buttonFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._buttonFlow.Controls.Add(this._exportButton);
            this._buttonFlow.Controls.Add(this._cancelButton);
            this._buttonFlow.Dock = System.Windows.Forms.DockStyle.Right;
            this._buttonFlow.Location = new System.Drawing.Point(458, 594);
            this._buttonFlow.Name = "_buttonFlow";
            this._buttonFlow.Size = new System.Drawing.Size(188, 41);
            this._buttonFlow.TabIndex = 0;
            // 
            // ExportForm
            // 
            this.AcceptButton = this._exportButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(649, 638);
            this.Controls.Add(this._table);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CSV Export";
            this._table.ResumeLayout(false);
            this._table.PerformLayout();
            this._buttonFlow.ResumeLayout(false);
            this._buttonFlow.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _exportButton;
        private System.Windows.Forms.ListView _list;
        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow;
        private System.Windows.Forms.Label _helpLabel;
    }
}