namespace SqlNotebook {
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ColumnHeader _columnHeader;
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Scripts", System.Windows.Forms.HorizontalAlignment.Center);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Tables", System.Windows.Forms.HorizontalAlignment.Center);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Views", System.Windows.Forms.HorizontalAlignment.Center);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportForm));
            this._helpLabel = new System.Windows.Forms.Label();
            this._openBtn = new System.Windows.Forms.Button();
            this._cancelBtn = new System.Windows.Forms.Button();
            this._saveBtn = new System.Windows.Forms.Button();
            this._list = new System.Windows.Forms.ListView();
            this._imageList = new System.Windows.Forms.ImageList(this.components);
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
            // _openBtn
            // 
            this._openBtn.AutoSize = true;
            this._openBtn.Enabled = false;
            this._openBtn.Location = new System.Drawing.Point(3, 3);
            this._openBtn.Name = "_openBtn";
            this._openBtn.Size = new System.Drawing.Size(88, 35);
            this._openBtn.TabIndex = 10;
            this._openBtn.Text = "Open";
            this._openBtn.UseVisualStyleBackColor = true;
            this._openBtn.Click += new System.EventHandler(this.OpenBtn_Click);
            // 
            // _cancelBtn
            // 
            this._cancelBtn.AutoSize = true;
            this._cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelBtn.Location = new System.Drawing.Point(191, 3);
            this._cancelBtn.Name = "_cancelBtn";
            this._cancelBtn.Size = new System.Drawing.Size(88, 35);
            this._cancelBtn.TabIndex = 12;
            this._cancelBtn.Text = "Cancel";
            this._cancelBtn.UseVisualStyleBackColor = true;
            // 
            // _saveBtn
            // 
            this._saveBtn.AutoSize = true;
            this._saveBtn.Enabled = false;
            this._saveBtn.Location = new System.Drawing.Point(97, 3);
            this._saveBtn.Name = "_saveBtn";
            this._saveBtn.Size = new System.Drawing.Size(88, 35);
            this._saveBtn.TabIndex = 11;
            this._saveBtn.Text = "Save as";
            this._saveBtn.UseVisualStyleBackColor = true;
            this._saveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // _list
            // 
            this._list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            _columnHeader});
            this._list.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup1.Header = "Scripts";
            listViewGroup1.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup1.Name = "Script";
            listViewGroup2.Header = "Tables";
            listViewGroup2.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup2.Name = "Table";
            listViewGroup3.Header = "Views";
            listViewGroup3.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup3.Name = "View";
            this._list.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this._list.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this._list.HideSelection = false;
            this._list.LabelWrap = false;
            this._list.Location = new System.Drawing.Point(3, 35);
            this._list.MultiSelect = false;
            this._list.Name = "_list";
            this._list.Size = new System.Drawing.Size(643, 444);
            this._list.SmallImageList = this._imageList;
            this._list.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this._list.TabIndex = 1;
            this._list.UseCompatibleStateImageBehavior = false;
            this._list.View = System.Windows.Forms.View.Details;
            this._list.SelectedIndexChanged += new System.EventHandler(this.List_SelectedIndexChanged);
            // 
            // _imageList
            // 
            this._imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this._imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_imageList.ImageStream")));
            this._imageList.TransparentColor = System.Drawing.Color.Transparent;
            this._imageList.Images.SetKeyName(0, "script.png");
            this._imageList.Images.SetKeyName(1, "table.png");
            this._imageList.Images.SetKeyName(2, "filter.png");
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
            this._buttonFlow.Controls.Add(this._openBtn);
            this._buttonFlow.Controls.Add(this._saveBtn);
            this._buttonFlow.Controls.Add(this._cancelBtn);
            this._buttonFlow.Dock = System.Windows.Forms.DockStyle.Right;
            this._buttonFlow.Location = new System.Drawing.Point(346, 485);
            this._buttonFlow.Name = "_buttonFlow";
            this._buttonFlow.Size = new System.Drawing.Size(300, 150);
            this._buttonFlow.TabIndex = 0;
            // 
            // ExportForm
            // 
            this.AcceptButton = this._openBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelBtn;
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
        private System.Windows.Forms.Button _openBtn;
        private System.Windows.Forms.Button _cancelBtn;
        private System.Windows.Forms.Button _saveBtn;
        private System.Windows.Forms.ListView _list;
        private System.Windows.Forms.ImageList _imageList;
        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow;
        private System.Windows.Forms.Label _helpLabel;
    }
}