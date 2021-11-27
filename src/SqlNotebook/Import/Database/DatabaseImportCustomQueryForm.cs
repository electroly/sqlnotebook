using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlNotebook.Properties;

namespace SqlNotebook.Import.Database;

public partial class DatabaseImportCustomQueryForm : ZForm {
    private readonly SqlTextControl _sqlTextControl;
    private readonly DataGridView _grid;
    private readonly IImportSession _session;

    public string TargetName { get; private set; }
    public string Sql { get; private set; }

    public DatabaseImportCustomQueryForm(IImportSession session, string targetName, string sql) {
        InitializeComponent();
        _session = session;

        _sqlTextControl = new(false) { Dock = DockStyle.Fill };
        _sqlPanel.Controls.Add(_sqlTextControl);
        _sqlTextControl.SqlText = sql;

        _grid = DataGridViewUtil.NewDataGridView();
        _grid.Dock = DockStyle.Fill;
        _previewPanel.Controls.Add(_grid);

        Ui ui = new(this, 120, 40);
        ui.Init(_table);
        ui.InitHeader(_importLabel);
        ui.Init(_importFlow);
        ui.Pad(_importFlow);
        ui.MarginBottom(_importFlow);
        ui.Init(_targetNameLabel);
        ui.Init(_targetNameText, 50);
        ui.Init(_splitter, 0.5);
        ui.Init(_sqlTable);
        ui.InitHeader(_sqlLabel);
        ui.Init(_previewToolStrip);
        ui.Init(_executeButton, Resources.ControlPlayBlue, Resources.control_play_blue32);
        ui.Init(_sqlPanel);
        ui.Init(_previewTable);
        ui.InitHeader(_previewLabel);
        ui.Init(_previewPanel);
        ui.Init(_buttonFlow);
        ui.MarginTop(_buttonFlow);
        ui.Init(_okButton);
        ui.Init(_cancelButton);

        _targetNameText.Text = targetName;
        _sqlTextControl.F5KeyPress += delegate { Execute(); };
        Shown += delegate { _sqlTextControl.SqlFocus(); };
    }

    private void PreviewButton_Click(object sender, EventArgs e) {
        Execute();
    }

    private void Execute() {
        var sql = _sqlTextControl.SqlText;
        if (string.IsNullOrWhiteSpace(sql)) {
            Ui.ShowError(this, "Error", "Please enter an SQL query.");
            return;
        }
        if (string.IsNullOrWhiteSpace(_targetNameText.Text)) {
            Ui.ShowError(this, "Error", "Please enter an imported table name.");
            return;
        }

        DataTable dt = null;
        WaitForm.GoWithCancel(this, "Preview", "Executing remote query...", out var success, cancel => {
            DbConnection connection = null;
            DbCommand command = null;
            DbDataReader reader = null;
            try {
                connection = _session.CreateConnection();
                connection.OpenAsync(cancel).ConfigureAwait(false).GetAwaiter().GetResult();
                command = connection.CreateCommand();
                command.CommandText = sql;
                reader = command.ExecuteReaderAsync(cancel).ConfigureAwait(false).GetAwaiter().GetResult();
                dt = new();
                for (var i = 0; i < reader.FieldCount; i++) {
                    dt.Columns.Add(reader.GetName(i));
                }
                dt.BeginLoadData();
                while (dt.Rows.Count < 1000 && reader.ReadAsync(cancel).ConfigureAwait(false).GetAwaiter().GetResult()) {
                    cancel.ThrowIfCancellationRequested();
                    var row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    dt.LoadDataRow(row, true);
                }
                dt.EndLoadData();
            } finally {
                _ = Task.Run(() => {
                    reader?.DisposeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    command?.Dispose();
                    connection?.Dispose();
                }, CancellationToken.None);
            }
        });
        if (success) {
            _splitter.Panel2Collapsed = false;
            _grid.DataSource = dt;
            _grid.AutoSizeColumns(this.Scaled(400));
        }
    }

    private void OkButton_Click(object sender, EventArgs e) {
        TargetName = _targetNameText.Text;
        Sql = _sqlTextControl.SqlText;
        DialogResult = DialogResult.OK;
        Close();
    }
}
