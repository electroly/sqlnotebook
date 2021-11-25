
namespace SqlNotebook {
    partial class ConsoleControl {
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
            this._table = new System.Windows.Forms.TableLayoutPanel();
            this._outputPanel = new System.Windows.Forms.Panel();
            this._inputBorderPanel = new System.Windows.Forms.Panel();
            this._inputTable = new System.Windows.Forms.TableLayoutPanel();
            this._inputPanel = new System.Windows.Forms.Panel();
            this._executeButton = new System.Windows.Forms.Button();
            this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._clearHistoryMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._table.SuspendLayout();
            this._inputBorderPanel.SuspendLayout();
            this._inputTable.SuspendLayout();
            this._contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _table
            // 
            this._table.ColumnCount = 1;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._table.Controls.Add(this._outputPanel, 0, 0);
            this._table.Controls.Add(this._inputBorderPanel, 0, 1);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 2;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this._table.Size = new System.Drawing.Size(1269, 426);
            this._table.TabIndex = 0;
            // 
            // _outputPanel
            // 
            this._outputPanel.AutoScroll = true;
            this._outputPanel.BackColor = System.Drawing.SystemColors.Window;
            this._outputPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._outputPanel.Location = new System.Drawing.Point(0, 0);
            this._outputPanel.Margin = new System.Windows.Forms.Padding(0);
            this._outputPanel.Name = "_outputPanel";
            this._outputPanel.Size = new System.Drawing.Size(1269, 326);
            this._outputPanel.TabIndex = 2;
            // 
            // _inputBorderPanel
            // 
            this._inputBorderPanel.BackColor = System.Drawing.Color.White;
            this._inputBorderPanel.Controls.Add(this._inputTable);
            this._inputBorderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._inputBorderPanel.Location = new System.Drawing.Point(0, 327);
            this._inputBorderPanel.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this._inputBorderPanel.Name = "_inputBorderPanel";
            this._inputBorderPanel.Size = new System.Drawing.Size(1269, 99);
            this._inputBorderPanel.TabIndex = 2;
            // 
            // _inputTable
            // 
            this._inputTable.ColumnCount = 2;
            this._inputTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._inputTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._inputTable.Controls.Add(this._inputPanel, 0, 0);
            this._inputTable.Controls.Add(this._executeButton, 1, 0);
            this._inputTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this._inputTable.Location = new System.Drawing.Point(0, 0);
            this._inputTable.Name = "_inputTable";
            this._inputTable.RowCount = 1;
            this._inputTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._inputTable.Size = new System.Drawing.Size(1269, 99);
            this._inputTable.TabIndex = 1;
            // 
            // _inputPanel
            // 
            this._inputPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._inputPanel.Location = new System.Drawing.Point(0, 0);
            this._inputPanel.Margin = new System.Windows.Forms.Padding(0);
            this._inputPanel.Name = "_inputPanel";
            this._inputPanel.Size = new System.Drawing.Size(1154, 99);
            this._inputPanel.TabIndex = 1;
            // 
            // _executeButton
            // 
            this._executeButton.AutoSize = true;
            this._executeButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._executeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this._executeButton.FlatAppearance.BorderSize = 0;
            this._executeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._executeButton.Location = new System.Drawing.Point(1154, 0);
            this._executeButton.Margin = new System.Windows.Forms.Padding(0);
            this._executeButton.Name = "_executeButton";
            this._executeButton.Size = new System.Drawing.Size(115, 99);
            this._executeButton.TabIndex = 0;
            this._executeButton.Text = "Execute (F5)";
            this._executeButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this._executeButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this._executeButton.UseVisualStyleBackColor = true;
            this._executeButton.Click += new System.EventHandler(this.ExecuteButton_Click);
            // 
            // _contextMenuStrip
            // 
            this._contextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this._contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._clearHistoryMenu});
            this._contextMenuStrip.Name = "_contextMenuStrip";
            this._contextMenuStrip.Size = new System.Drawing.Size(183, 36);
            // 
            // _clearHistoryMenu
            // 
            this._clearHistoryMenu.Name = "_clearHistoryMenu";
            this._clearHistoryMenu.Size = new System.Drawing.Size(182, 32);
            this._clearHistoryMenu.Text = "Clear history";
            this._clearHistoryMenu.Click += new System.EventHandler(this.ClearHistoryMenu_Click);
            // 
            // ConsoleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._table);
            this.Name = "ConsoleControl";
            this.Size = new System.Drawing.Size(1269, 426);
            this._table.ResumeLayout(false);
            this._inputBorderPanel.ResumeLayout(false);
            this._inputTable.ResumeLayout(false);
            this._inputTable.PerformLayout();
            this._contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.Button _executeButton;
        private System.Windows.Forms.Panel _inputPanel;
        private System.Windows.Forms.Panel _outputPanel;
        private System.Windows.Forms.Panel _inputBorderPanel;
        private System.Windows.Forms.TableLayoutPanel _inputTable;
        private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _clearHistoryMenu;
    }
}
