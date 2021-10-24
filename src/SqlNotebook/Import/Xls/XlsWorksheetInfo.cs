using NPOI.SS.UserModel;
using NPOI.SS.Util;
using SqlNotebookScript.Utils;
using System;
using System.Data;
using System.Linq;

namespace SqlNotebook.Import.Xls {
    public sealed class XlsWorksheetInfo {
        public int Index;
        public string Name;
        public DataTable DataTable;

        public static XlsWorksheetInfo Load(IWorkbook workbook, int worksheetIndex) {
            var sheet = workbook.GetSheetAt(worksheetIndex);
            var data = XlsUtil.ReadSheet(sheet);

            DataTable dataTable = new();
            var numColumns = data.Max(x => x.Length);
            for (var columnIndex = 0; columnIndex < numColumns; columnIndex++) {
                var columnLetter = CellReference.ConvertNumToColString(columnIndex);
                dataTable.Columns.Add(columnLetter);
            }

            foreach (var row in data) {
                var dataRow = dataTable.NewRow();
                if (row.Length == numColumns) {
                    dataRow.ItemArray = row;
                } else {
                    var extendedRow = new object[numColumns];
                    Array.Copy(row, extendedRow, row.Length);
                    dataRow.ItemArray = extendedRow;
                }
                dataTable.Rows.Add(dataRow);
            }

            return new() {
                Index = worksheetIndex,
                Name = workbook.GetSheetName(worksheetIndex),
                DataTable = dataTable
            };
        }
    }
}
