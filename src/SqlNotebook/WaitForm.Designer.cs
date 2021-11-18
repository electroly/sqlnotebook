namespace SqlNotebook {
    partial class WaitForm {
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
            this._infoTxt = new System.Windows.Forms.Label();
            this._table = new System.Windows.Forms.TableLayoutPanel();
            this._spinner = new System.Windows.Forms.Panel();
            this._buttonFlow = new System.Windows.Forms.FlowLayoutPanel();
            this._cancelButton = new System.Windows.Forms.Button();
            this._spinnerTimer = new System.Windows.Forms.Timer(this.components);
            this._table.SuspendLayout();
            this._buttonFlow.SuspendLayout();
            this.SuspendLayout();
            // 
            // _infoTxt
            // 
            this._infoTxt.AutoEllipsis = true;
            this._infoTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this._infoTxt.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._infoTxt.Location = new System.Drawing.Point(59, 0);
            this._infoTxt.Name = "_infoTxt";
            this._infoTxt.Size = new System.Drawing.Size(519, 312);
            this._infoTxt.TabIndex = 0;
            this._infoTxt.Text = "Running your SQL query...";
            // 
            // _table
            // 
            this._table.ColumnCount = 2;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._table.Controls.Add(this._infoTxt, 1, 0);
            this._table.Controls.Add(this._spinner, 0, 0);
            this._table.Controls.Add(this._buttonFlow, 1, 1);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 2;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.Size = new System.Drawing.Size(581, 358);
            this._table.TabIndex = 5;
            // 
            // _spinner
            // 
            this._spinner.Location = new System.Drawing.Point(3, 3);
            this._spinner.Name = "_spinner";
            this._spinner.Size = new System.Drawing.Size(50, 50);
            this._spinner.TabIndex = 5;
            this._spinner.Paint += new System.Windows.Forms.PaintEventHandler(this.Spinner_Paint);
            // 
            // _buttonFlow
            // 
            this._buttonFlow.AutoSize = true;
            this._buttonFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._buttonFlow.Controls.Add(this._cancelButton);
            this._buttonFlow.Dock = System.Windows.Forms.DockStyle.Right;
            this._buttonFlow.Location = new System.Drawing.Point(460, 315);
            this._buttonFlow.Name = "_buttonFlow";
            this._buttonFlow.Size = new System.Drawing.Size(118, 40);
            this._buttonFlow.TabIndex = 6;
            // 
            // _cancelButton
            // 
            this._cancelButton.Location = new System.Drawing.Point(3, 3);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(112, 34);
            this._cancelButton.TabIndex = 0;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // _spinnerTimer
            // 
            this._spinnerTimer.Enabled = true;
            this._spinnerTimer.Interval = 16;
            this._spinnerTimer.Tick += new System.EventHandler(this.SpinnerTimer_Tick);
            // 
            // WaitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(581, 358);
            this.ControlBox = false;
            this.Controls.Add(this._table);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WaitForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SQL Query";
            this._table.ResumeLayout(false);
            this._table.PerformLayout();
            this._buttonFlow.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _infoTxt;
        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.Panel _spinner;
        private System.Windows.Forms.Timer _spinnerTimer;
        private System.Windows.Forms.FlowLayoutPanel _buttonFlow;
        private System.Windows.Forms.Button _cancelButton;
    }
}