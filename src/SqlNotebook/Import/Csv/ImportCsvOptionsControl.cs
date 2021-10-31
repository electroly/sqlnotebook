using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SqlNotebookScript.Utils;

namespace SqlNotebook.Import.Csv {
    public partial class ImportCsvOptionsControl : UserControl {
        private readonly List<Tuple<int, string>> _encodings = new List<Tuple<int, string>> {
            Tuple.Create(0, "Auto detect")
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

        public Slot<int> SkipLines { get; } = new();
        public Slot<bool> HasColumnHeaders { get; } = new();
        public Slot<int> FileEncoding { get; } = new();
        public Slot<string> TargetTableName { get; } = new();
        public Slot<ImportTableExistsOption> IfTableExists { get; } = new();
        public Slot<ImportConversionFailOption> IfConversionFails { get; } = new();
        public Slot<string> Separator { get; } = new();

        public ImportCsvOptionsControl(DatabaseSchema schema) {
            InitializeComponent();

            Ui ui = new(this);
            ui.Init(_fileInputTitle);
            ui.MarginBottom(_fileInputTitle);
            ui.Init(_sourceFlow);
            ui.Init(_skipLinesFlow);
            ui.Init(_skipLinesLabel);
            ui.Init(_skipLinesTxt);
            ui.MarginRight(_skipLinesTxt);
            ui.Init(_headerChk);
            ui.MarginTop(_headerChk);
            ui.Init(_encodingFlow);
            ui.Init(_encodingLabel);
            ui.Init(_encodingCmb, 35);
            ui.MarginRight(_encodingCmb);
            ui.Init(_separatorFlow);
            ui.Init(_separatorLabel);
            ui.Init(_separatorCombo, 10);
            ui.Init(_tableOutputTitle);
            ui.MarginTop(_tableOutputTitle);
            ui.MarginBottom(_tableOutputTitle);
            ui.Init(_tableLabel);
            ui.Init(_targetFlow);
            ui.Init(_tableNameFlow);
            ui.Init(_tableCmb, 40);
            ui.MarginRight(_tableCmb);
            ui.Init(_ifTableExistsLabel);
            ui.Init(_ifExistsFlow);
            ui.Init(_ifExistsCmb, 30);
            ui.Init(_convertFailFlow);
            ui.Init(_ifConversionFailsLabel);
            ui.MarginTop(_ifConversionFailsLabel);
            ui.Init(_convertFailCmb, 30);

            foreach (var tableName in schema.Tables.Keys) {
                _tableCmb.Items.Add(tableName);
            }

            // rename some misleadingly named system encodings
            var customEncodingNames = new Dictionary<int, string> {
                [1200] = "Unicode (UTF-16)",
            };
            _encodings.AddRange(
                from encoding in Encoding.GetEncodings()
                where encoding.CodePage > 0 &&
                    // nobody wants these big endian encodings
                    encoding.CodePage != 1201 && // UTF-16 Big-Endian
                    encoding.CodePage != 12001 // UTF-32 Big-Endian
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
            _separatorCombo.BindText(Separator);

            _separatorCombo.SelectionLength = 0;
        }

        public void SelectTableCombo() {
            _tableCmb.Select();
        }
    }
}
