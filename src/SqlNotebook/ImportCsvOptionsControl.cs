// SQL Notebook
// Copyright (C) 2016 Brian Luft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SqlNotebookScript;

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

        public ImportCsvOptionsControl() {
            InitializeComponent();
            _fileInputTitle.MakeDivider();
            _tableOutputTitle.MakeDivider();

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

    public enum ImportTableExistsOption {
        AppendNewRows,
        DeleteExistingRows,
        DropTable
    }

    public enum ImportConversionFailOption {
        ImportAsText = 1,
        SkipRow = 2,
        Abort = 3
    }

}
