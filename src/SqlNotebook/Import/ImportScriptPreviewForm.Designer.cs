
namespace SqlNotebook.Import {
    partial class ImportScriptPreviewForm {
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
            this._split = new System.Windows.Forms.SplitContainer();
            this._scriptTable = new System.Windows.Forms.TableLayoutPanel();
            this._scriptLabel = new System.Windows.Forms.Label();
            this._scriptPanel = new System.Windows.Forms.Panel();
            this._previewTable = new System.Windows.Forms.TableLayoutPanel();
            this._previewLabel = new System.Windows.Forms.Label();
            this._previewPanel = new System.Windows.Forms.Panel();
            this._table.SuspendLayout();
            this._buttonFlow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._split)).BeginInit();
            this._split.Panel1.SuspendLayout();
            this._split.Panel2.SuspendLayout();
            this._split.SuspendLayout();
            this._scriptTable.SuspendLayout();
            this._previewTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // _table
            // 
            this._table.ColumnCount = 1;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.Controls.Add(this._buttonFlow, 0, 1);
            this._table.Controls.Add(this._split, 0, 0);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 2;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.Size = new System.Drawing.Size(912, 653);
            this._table.TabIndex = 0;
            // 
            // _buttonFlow
            // 
            this._buttonFlow.AutoSize = true;
            this._buttonFlow.Controls.Add(this._okButton);
            this._buttonFlow.Dock = System.Windows.Forms.DockStyle.Right;
            this._buttonFlow.Location = new System.Drawing.Point(791, 609);
            this._buttonFlow.Name = "_buttonFlow";
            this._buttonFlow.Size = new System.Drawing.Size(118, 41);
            this._buttonFlow.TabIndex = 1;
            // 
            // _okButton
            // 
            this._okButton.AutoSize = true;
            this._okButton.Location = new System.Drawing.Point(3, 3);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(112, 35);
            this._okButton.TabIndex = 0;
            this._okButton.Text = "Close";
            this._okButton.UseVisualStyleBackColor = true;
            // 
            // _split
            // 
            this._split.Dock = System.Windows.Forms.DockStyle.Fill;
            this._split.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._split.Location = new System.Drawing.Point(3, 3);
            this._split.Name = "_split";
            this._split.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _split.Panel1
            // 
            this._split.Panel1.Controls.Add(this._scriptTable);
            // 
            // _split.Panel2
            // 
            this._split.Panel2.Controls.Add(this._previewTable);
            this._split.Size = new System.Drawing.Size(906, 600);
            this._split.SplitterDistance = 302;
            this._split.TabIndex = 2;
            // 
            // _scriptTable
            // 
            this._scriptTable.ColumnCount = 1;
            this._scriptTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._scriptTable.Controls.Add(this._scriptLabel, 0, 0);
            this._scriptTable.Controls.Add(this._scriptPanel, 0, 1);
            this._scriptTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this._scriptTable.Location = new System.Drawing.Point(0, 0);
            this._scriptTable.Name = "_scriptTable";
            this._scriptTable.RowCount = 2;
            this._scriptTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._scriptTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._scriptTable.Size = new System.Drawing.Size(906, 302);
            this._scriptTable.TabIndex = 0;
            // 
            // _scriptLabel
            // 
            this._scriptLabel.AutoSize = true;
            this._scriptLabel.Location = new System.Drawing.Point(3, 0);
            this._scriptLabel.Name = "_scriptLabel";
            this._scriptLabel.Size = new System.Drawing.Size(57, 25);
            this._scriptLabel.TabIndex = 0;
            this._scriptLabel.Text = "Script";
            // 
            // _scriptPanel
            // 
            this._scriptPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._scriptPanel.Location = new System.Drawing.Point(0, 25);
            this._scriptPanel.Margin = new System.Windows.Forms.Padding(0);
            this._scriptPanel.Name = "_scriptPanel";
            this._scriptPanel.Size = new System.Drawing.Size(906, 277);
            this._scriptPanel.TabIndex = 1;
            // 
            // _previewTable
            // 
            this._previewTable.ColumnCount = 1;
            this._previewTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._previewTable.Controls.Add(this._previewLabel, 0, 0);
            this._previewTable.Controls.Add(this._previewPanel, 0, 1);
            this._previewTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this._previewTable.Location = new System.Drawing.Point(0, 0);
            this._previewTable.Name = "_previewTable";
            this._previewTable.RowCount = 2;
            this._previewTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._previewTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._previewTable.Size = new System.Drawing.Size(906, 294);
            this._previewTable.TabIndex = 1;
            // 
            // _previewLabel
            // 
            this._previewLabel.AutoSize = true;
            this._previewLabel.Location = new System.Drawing.Point(3, 0);
            this._previewLabel.Name = "_previewLabel";
            this._previewLabel.Size = new System.Drawing.Size(52, 25);
            this._previewLabel.TabIndex = 0;
            this._previewLabel.Text = "Table";
            // 
            // _previewPanel
            // 
            this._previewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._previewPanel.Location = new System.Drawing.Point(0, 25);
            this._previewPanel.Margin = new System.Windows.Forms.Padding(0);
            this._previewPanel.Name = "_previewPanel";
            this._previewPanel.Size = new System.Drawing.Size(906, 269);
            this._previewPanel.TabIndex = 1;
            // 
            // ImportScriptForm
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._okButton;
            this.ClientSize = new System.Drawing.Size(912, 653);
            this.Controls.Add(this._table);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportScriptForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import Preview";
            this._table.ResumeLayout(false);
            this._table.PerformLayout();
            this._buttonFlow.ResumeLayout(false);
            this._buttonFlow.PerformLayout();
            this._split.Panel1.ResumeLayout(false);
            this._split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._split)).EndInit();
            this._split.ResumeLayout(false);
            this._scriptTable.ResumeLayout(false);
            this._scriptTable.PerformLayout();
            this._previewTable.ResumeLayout(false);
            this._previewTable.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.SplitContainer _split;
        private System.Windows.Forms.TableLayoutPanel _scriptTable;
        private System.Windows.Forms.Label _scriptLabel;
        private System.Windows.Forms.Panel _scriptPanel;
        private System.Windows.Forms.TableLayoutPanel _previewTable;
        private System.Windows.Forms.Label _previewLabel;
        private System.Windows.Forms.Panel _previewPanel;
    }
}