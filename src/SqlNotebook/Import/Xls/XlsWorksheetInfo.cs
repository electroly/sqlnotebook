using System;
using System.Data;
using System.Linq;
using SqlNotebookScript.Utils;

namespace SqlNotebook.Import.Xls;

public sealed class XlsWorksheetInfo
{
    public int Index;
    public string Name;
    public DataTable DataTable;
    public int FullCount;

    public static XlsWorksheetInfo Load(XlsUtil.IWorkbook workbook, int worksheetIndex)
    {
        workbook.SeekToWorksheet(worksheetIndex);
        var fullCount = workbook.GetRowCount();

        workbook.SeekToWorksheet(worksheetIndex);
        var data = workbook.ReadSheet(maxRows: 1000);

        DataTable dataTable = new();
        int numColumns;
        if (data.Count > 0)
        {
            numColumns = data.Max(x => x.Length);
            for (var columnIndex = 0; columnIndex < numColumns; columnIndex++)
            {
                var columnLetter = XlsUtil.ConvertNumToColString(columnIndex);
                dataTable.Columns.Add(columnLetter);
            }
        }
        else
        {
            numColumns = 1;
            dataTable.Columns.Add("A");
        }

        dataTable.BeginLoadData();
        foreach (var row in data)
        {
            // Format numbers.
            for (var i = 0; i < row.Length; i++)
            {
                if (row[i] is int intValue)
                {
                    row[i] = $"{intValue:#,##0}";
                }
                else if (row[i] is long longValue)
                {
                    row[i] = $"{longValue:#,##0}";
                }
                else if (row[i] is double doubleValue)
                {
                    row[i] = $"{doubleValue:#,##0.####}";
                }
            }

            if (row.Length == numColumns)
            {
                dataTable.LoadDataRow(row, true);
            }
            else
            {
                var extendedRow = new object[numColumns];
                Array.Copy(row, extendedRow, row.Length);
                dataTable.LoadDataRow(extendedRow, true);
            }
        }
        dataTable.EndLoadData();

        return new()
        {
            Index = worksheetIndex,
            Name = workbook.GetSheetName(worksheetIndex),
            DataTable = dataTable,
            FullCount = fullCount,
        };
    }
}
