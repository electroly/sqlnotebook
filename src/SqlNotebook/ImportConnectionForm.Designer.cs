namespace SqlNotebook {
    partial class ImportConnectionForm {
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
            this._passwordLabel = new System.Windows.Forms.Label();
            this._usernameLabel = new System.Windows.Forms.Label();
            this._databaseLabel = new System.Windows.Forms.Label();
            this._addressLabel = new System.Windows.Forms.Label();
            this._propertyGrid = new System.Windows.Forms.PropertyGrid();
            this._okBtn = new System.Windows.Forms.Button();
            this._cancelBtn = new System.Windows.Forms.Button();
            this._passwordTxt = new System.Windows.Forms.TextBox();
            this._usernameTxt = new System.Windows.Forms.TextBox();
            this._databaseTxt = new System.Windows.Forms.TextBox();
            this._serverTxt = new System.Windows.Forms.TextBox();
            this._windowsAuthChk = new System.Windows.Forms.CheckBox();
            this._basicTable = new System.Windows.Forms.TableLayoutPanel();
            this._tabs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this._table = new System.Windows.Forms.TableLayoutPanel();
            this._buttonFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._basicTable.SuspendLayout();
            this._tabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this._table.SuspendLayout();
            this._buttonFlow.SuspendLayout();
            this.SuspendLayout();
            // 
            // _passwordLabel
            // 
            this._passwordLabel.AutoSize = true;
            this._passwordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._passwordLabel.Location = new System.Drawing.Point(3, 186);
            this._passwordLabel.Name = "_passwordLabel";
            this._passwordLabel.Size = new System.Drawing.Size(739, 25);
            this._passwordLabel.TabIndex = 5;
            this._passwordLabel.Text = "&Password:";
            this._passwordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _usernameLabel
            // 
            this._usernameLabel.AutoSize = true;
            this._usernameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._usernameLabel.Location = new System.Drawing.Point(3, 124);
            this._usernameLabel.Name = "_usernameLabel";
            this._usernameLabel.Size = new System.Drawing.Size(739, 25);
            this._usernameLabel.TabIndex = 2;
            this._usernameLabel.Text = "&Username:";
            this._usernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _databaseLabel
            // 
            this._databaseLabel.AutoSize = true;
            this._databaseLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._databaseLabel.Location = new System.Drawing.Point(3, 62);
            this._databaseLabel.Name = "_databaseLabel";
            this._databaseLabel.Size = new System.Drawing.Size(739, 25);
            this._databaseLabel.TabIndex = 2;
            this._databaseLabel.Text = "&Database name:";
            this._databaseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _addressLabel
            // 
            this._addressLabel.AutoSize = true;
            this._addressLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._addressLabel.Location = new System.Drawing.Point(3, 0);
            this._addressLabel.Name = "_addressLabel";
            this._addressLabel.Size = new System.Drawing.Size(739, 25);
            this._addressLabel.TabIndex = 0;
            this._addressLabel.Text = "Server &address:";
            this._addressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _propertyGrid
            // 
            this._propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._propertyGrid.Location = new System.Drawing.Point(3, 3);
            this._propertyGrid.Name = "_propertyGrid";
            this._propertyGrid.Size = new System.Drawing.Size(745, 701);
            this._propertyGrid.TabIndex = 9;
            this._propertyGrid.ToolbarVisible = false;
            // 
            // _okBtn
            // 
            this._okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._okBtn.AutoSize = true;
            this._okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okBtn.Location = new System.Drawing.Point(3, 3);
            this._okBtn.Name = "_okBtn";
            this._okBtn.Size = new System.Drawing.Size(88, 35);
            this._okBtn.TabIndex = 10;
            this._okBtn.Text = "Connect";
            this._okBtn.UseVisualStyleBackColor = true;
            this._okBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // _cancelBtn
            // 
            this._cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelBtn.AutoSize = true;
            this._cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelBtn.Location = new System.Drawing.Point(97, 3);
            this._cancelBtn.Name = "_cancelBtn";
            this._cancelBtn.Size = new System.Drawing.Size(88, 35);
            this._cancelBtn.TabIndex = 11;
            this._cancelBtn.Text = "Cancel";
            this._cancelBtn.UseVisualStyleBackColor = true;
            // 
            // _passwordTxt
            // 
            this._passwordTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this._passwordTxt.Location = new System.Drawing.Point(3, 214);
            this._passwordTxt.Name = "_passwordTxt";
            this._passwordTxt.PasswordChar = '*';
            this._passwordTxt.Size = new System.Drawing.Size(739, 31);
            this._passwordTxt.TabIndex = 7;
            this._passwordTxt.UseSystemPasswordChar = true;
            // 
            // _usernameTxt
            // 
            this._usernameTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this._usernameTxt.Location = new System.Drawing.Point(3, 152);
            this._usernameTxt.Name = "_usernameTxt";
            this._usernameTxt.Size = new System.Drawing.Size(739, 31);
            this._usernameTxt.TabIndex = 5;
            // 
            // _databaseTxt
            // 
            this._databaseTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this._databaseTxt.Location = new System.Drawing.Point(3, 90);
            this._databaseTxt.Name = "_databaseTxt";
            this._databaseTxt.Size = new System.Drawing.Size(739, 31);
            this._databaseTxt.TabIndex = 3;
            // 
            // _serverTxt
            // 
            this._serverTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this._serverTxt.Location = new System.Drawing.Point(3, 28);
            this._serverTxt.Name = "_serverTxt";
            this._serverTxt.Size = new System.Drawing.Size(739, 31);
            this._serverTxt.TabIndex = 1;
            // 
            // _windowsAuthChk
            // 
            this._windowsAuthChk.AutoSize = true;
            this._windowsAuthChk.Location = new System.Drawing.Point(3, 251);
            this._windowsAuthChk.Name = "_windowsAuthChk";
            this._windowsAuthChk.Size = new System.Drawing.Size(263, 29);
            this._windowsAuthChk.TabIndex = 8;
            this._windowsAuthChk.Text = "Use &Windows authentication";
            this._windowsAuthChk.UseVisualStyleBackColor = true;
            this._windowsAuthChk.CheckedChanged += new System.EventHandler(this.WindowsAuthChk_CheckedChanged);
            // 
            // _basicTable
            // 
            this._basicTable.AutoSize = true;
            this._basicTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._basicTable.ColumnCount = 1;
            this._basicTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._basicTable.Controls.Add(this._windowsAuthChk, 0, 8);
            this._basicTable.Controls.Add(this._addressLabel, 0, 0);
            this._basicTable.Controls.Add(this._passwordTxt, 0, 7);
            this._basicTable.Controls.Add(this._serverTxt, 0, 1);
            this._basicTable.Controls.Add(this._passwordLabel, 0, 6);
            this._basicTable.Controls.Add(this._databaseLabel, 0, 2);
            this._basicTable.Controls.Add(this._usernameTxt, 0, 5);
            this._basicTable.Controls.Add(this._databaseTxt, 0, 3);
            this._basicTable.Controls.Add(this._usernameLabel, 0, 4);
            this._basicTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this._basicTable.Location = new System.Drawing.Point(3, 3);
            this._basicTable.Name = "_basicTable";
            this._basicTable.RowCount = 9;
            this._basicTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._basicTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._basicTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._basicTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._basicTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._basicTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._basicTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._basicTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._basicTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._basicTable.Size = new System.Drawing.Size(745, 701);
            this._basicTable.TabIndex = 13;
            // 
            // _tabs
            // 
            this._tabs.Controls.Add(this.tabPage1);
            this._tabs.Controls.Add(this.tabPage2);
            this._tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabs.Location = new System.Drawing.Point(3, 3);
            this._tabs.Name = "_tabs";
            this._tabs.SelectedIndex = 0;
            this._tabs.Size = new System.Drawing.Size(759, 745);
            this._tabs.TabIndex = 14;
            this._tabs.SelectedIndexChanged += new System.EventHandler(this.Tabs_TabIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this._basicTable);
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(751, 707);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basic";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this._propertyGrid);
            this.tabPage2.Location = new System.Drawing.Point(4, 34);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(751, 707);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Advanced";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // _table
            // 
            this._table.ColumnCount = 1;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.Controls.Add(this._buttonFlow, 0, 1);
            this._table.Controls.Add(this._tabs, 0, 0);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 2;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.Size = new System.Drawing.Size(765, 907);
            this._table.TabIndex = 15;
            // 
            // _buttonFlow
            // 
            this._buttonFlow.Controls.Add(this._okBtn);
            this._buttonFlow.Controls.Add(this._cancelBtn);
            this._buttonFlow.Dock = System.Windows.Forms.DockStyle.Right;
            this._buttonFlow.Location = new System.Drawing.Point(462, 754);
            this._buttonFlow.Name = "_buttonFlow";
            this._buttonFlow.Size = new System.Drawing.Size(300, 150);
            this._buttonFlow.TabIndex = 0;
            // 
            // ImportConnectionForm
            // 
            this.AcceptButton = this._okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelBtn;
            this.ClientSize = new System.Drawing.Size(765, 907);
            this.Controls.Add(this._table);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportConnectionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect to [...]";
            this._basicTable.ResumeLayout(false);
            this._basicTable.PerformLayout();
            this._tabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this._table.ResumeLayout(false);
            this._buttonFlow.ResumeLayout(false);
            this._buttonFlow.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid _propertyGrid;
        private System.Windows.Forms.Button _okBtn;
        private System.Windows.Forms.Button _cancelBtn;
        private System.Windows.Forms.TextBox _passwordTxt;
        private System.Windows.Forms.TextBox _usernameTxt;
        private System.Windows.Forms.TextBox _databaseTxt;
        private System.Windows.Forms.TextBox _serverTxt;
        private System.Windows.Forms.CheckBox _windowsAuthChk;
        private System.Windows.Forms.TableLayoutPanel _basicTable;
        private System.Windows.Forms.TabControl _tabs;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow;
        private System.Windows.Forms.Label _passwordLabel;
        private System.Windows.Forms.Label _usernameLabel;
        private System.Windows.Forms.Label _databaseLabel;
        private System.Windows.Forms.Label _addressLabel;
    }
}