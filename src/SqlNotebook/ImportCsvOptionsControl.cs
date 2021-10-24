using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SqlNotebookScript.Utils;

namespace SqlNotebook {
    public partial class ImportCsvOptionsControl : UserControl {
        private readonly List<Tuple<int, string>> _encodings = new List<Tuple<int, string>> {
            Tuple.Create(0, "Unicode")
            // populated in constructor
        };

        private readonly Tuple<ImportTableExistsOption, string>[] _ifExistsOptions = new[] {
            Tuple.Create(ImportTableExistsOption.AppendNewRows, "Append new rows"),
            Tuple.Create(ImportTableExistsOption.DeleteExistingRows, "Delete existing rows"),
            Tuple.Create(ImportTableExistsOption.DropTable, "Drop table and re-create")
        };

        private readonly Tuple<ImportConversionFailOption, string>[] _conversionFailOptions = new[] {
            Tuple.Create(ImportConversionFailOption.ImportAsText, "Import the value as text"),
            Tuple.Create(ImportConversionFailOption.SkipRow, "Skip the row"),
            Tuple.Create(ImportConversionFailOption.Abort, "Stop import with error")
        };

        public Slot<int> SkipLines { get; } = new Slot<int>();
        public Slot<bool> HasColumnHeaders { get; } = new Slot<bool>();
        public Slot<int> FileEncoding { get; } = new Slot<int>();
        public Slot<string> TargetTableName { get; } = new Slot<string>();
        public Slot<ImportTableExistsOption> IfTableExists { get; } = new Slot<ImportTableExistsOption>();
        public Slot<ImportConversionFailOption> IfConversionFails { get; } = new Slot<ImportConversionFailOption>();

        public ImportCsvOptionsControl(DatabaseSchema schema) {
            InitializeComponent();

            Ui ui = new(this);
            ui.Init(_fileInputTitle);
            ui.Init(_skipLinesLabel);
            ui.Init(_skipLinesTxt);
            ui.Init(_columnNamesLabel);
            ui.Init(_headerChk);
            ui.Init(_encodingLabel);
            ui.Init(_encodingCmb, 35);
            ui.Init(_tableOutputTitle);
            ui.MarginTop(_tableOutputTitle);
            ui.Init(_tableLabel);
            ui.Init(_tableCmb, 40);
            ui.Init(_ifTableExistsLabel);
            ui.Init(_ifExistsCmb, 30);
            ui.Init(_ifConversionFailsLabel);
            ui.Init(_convertFailCmb, 30);

            foreach (var tableName in schema.Tables.Keys) {
                _tableCmb.Items.Add(tableName);
            }

            // rename some misleadingly named system encodings
            var customEncodingNames = new Dictionary<int, string> {
                [1200] = "Unicode (UTF-16)",
                [1201] = "Unicode (UTF-16 Big-Endian)"
            };
            _encodings.AddRange(
                from encoding in Encoding.GetEncodings()
                where encoding.CodePage > 0
                let name = customEncodingNames.GetValueOrDefault(encoding.CodePage) ?? encoding.DisplayName
                orderby name
                select Tuple.Create(encoding.CodePage, name)
            );
            _encodingCmb.ValueMember = "Item1";
            _encodingCmb.DisplayMember = "Item2";
            _encodingCmb.DataSource = _encodings;

            _ifExistsCmb.ValueMember = "Item1";
            _ifExistsCmb.DisplayMember = "Item2";
            _ifExistsCmb.DataSource = _ifExistsOptions;

            _convertFailCmb.ValueMember = "Item1";
            _convertFailCmb.DisplayMember = "Item2";
            _convertFailCmb.DataSource = _conversionFailOptions;

            _skipLinesTxt.BindValue(SkipLines);
            _headerChk.BindChecked(HasColumnHeaders);
            _encodingCmb.BindSelectedValue(FileEncoding);
            _tableCmb.BindText(TargetTableName);
            _ifExistsCmb.BindSelectedValue(IfTableExists);
            _convertFailCmb.BindSelectedValue(IfConversionFails);
        }
    }

}
