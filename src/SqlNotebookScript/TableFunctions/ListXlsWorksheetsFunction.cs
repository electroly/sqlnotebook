using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.TableFunctions {
    public sealed class ListXlsWorksheetsFunction : CustomTableFunction {
        public override string Name => "list_xls_worksheets";

        public override string CreateTableSql =>
            @"CREATE TABLE list_xls_worksheets (_file_path HIDDEN, number INTEGER, name)";

        public override int HiddenColumnCount => 1;

        public override IEnumerable<object[]> Execute(object[] hiddenValues) {
            var filePath = ArgUtil.GetStrArg(hiddenValues[0], "file-path", Name);
            if (!File.Exists(filePath)) {
                throw new Exception($"{Name.ToUpper()}: File not found.");
            }

            try {
                return XlsUtil.ReadWorksheetNames(filePath).Select((name, i) => new object[] {
                    filePath, i + 1, name
                }).ToList();
            } catch (Exception ex) {
                throw new Exception($"{Name.ToUpper()}: {ex.Message}");
            }
        }
    }
}
