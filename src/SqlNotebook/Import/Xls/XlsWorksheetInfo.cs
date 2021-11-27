using System;
using System.Data;
using System.Linq;
using SqlNotebookScript.Utils;

namespace SqlNotebook.Import.Xls;

public sealed class XlsWorksheetInfo {
    public int Index;
    public string Name;
    public DataTable DataTable;

    public static XlsWorksheetInfo Load(XlsUtil.IWorkbook workbook, int worksheetIndex) {
        workbook.SeekToWorksheet(worksheetIndex);
        var data = workbook.ReadSheet(maxRows: 1000);

        DataTable dataTable = new();
        int numColumns;
        if (data.Count > 0) {
            numColumns = data.Max(x => x.Length);
            for (var columnIndex = 0; columnIndex < numColumns; columnIndex++) {
                var columnLetter = XlsUtil.ConvertNumToColString(columnIndex);
                dataTable.Columns.Add(columnLetter);
            }
        } else {
            numColumns = 1;
            dataTable.Columns.Add("A");
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
