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
            this._spinnerTimer = new System.Windows.Forms.Timer(this.components);
            this._table.SuspendLayout();
            this.SuspendLayout();
            // 
            // _infoTxt
            // 
            this._infoTxt.AutoEllipsis = true;
            this._infoTxt.AutoSize = true;
            this._infoTxt.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._infoTxt.Location = new System.Drawing.Point(59, 0);
            this._infoTxt.Name = "_infoTxt";
            this._infoTxt.Size = new System.Drawing.Size(291, 32);
            this._infoTxt.TabIndex = 0;
            this._infoTxt.Text = "Running your SQL query...";
            // 
            // _table
            // 
            this._table.ColumnCount = 2;
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._table.Controls.Add(this._infoTxt, 1, 0);
            this._table.Controls.Add(this._spinner, 0, 0);
            this._table.Dock = System.Windows.Forms.DockStyle.Fill;
            this._table.Location = new System.Drawing.Point(0, 0);
            this._table.Name = "_table";
            this._table.RowCount = 1;
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 358F));
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
            this.BackColor = System.Drawing.Color.White;
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _infoTxt;
        private System.Windows.Forms.TableLayoutPanel _table;
        private System.Windows.Forms.Panel _spinner;
        private System.Windows.Forms.Timer _spinnerTimer;
    }
}