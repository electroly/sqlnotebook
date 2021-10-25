using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace SqlNotebookScript.Utils {
    public static class XlsUtil {
        public static T WithWorkbook<T>(string filePath, Func<IWorkbook, T> func) {
            using (var stream = File.OpenRead(filePath)) {
                var workbook =
                    Path.GetExtension(filePath).ToLower() == ".xls"
                    ? (IWorkbook)new HSSFWorkbook(stream) : new XSSFWorkbook(stream);
                return func(workbook);
            }
        }

        public static void WithWorkbook(string filePath, Action<IWorkbook> action) {
            WithWorkbook(filePath, workbook => {
                action(workbook);
                return true;
            });
        }

        public static IReadOnlyList<string> ReadWorksheetNames(string filePath) {
            if (!File.Exists(filePath)) {
                throw new FileNotFoundException();
            }

            return WithWorkbook(filePath, workbook => {
                var list = new List<string>();
                for (int i = 0; i < workbook.NumberOfSheets; i++) {
                    var name = workbook.GetSheetName(i);
                    list.Add(name);
                }
                return list;
            });
        }

        public static List<object[]> ReadWorksheet(string filePath, int worksheetIndex, int? lastRowIndex = null) {
            if (!File.Exists(filePath)) {
                throw new FileNotFoundException();
            }
            if (worksheetIndex < 0) {
                throw new Exception("The worksheet index must not be negative.");
            }

            return WithWorkbook(filePath, workbook => {
                if (worksheetIndex >= workbook.NumberOfSheets) {
                    throw new Exception(
                        $"The worksheet index is out of range. The workbook has {workbook.NumberOfSheets} worksheets.");
                }

                var sheet = workbook.GetSheetAt(worksheetIndex);
                return ReadSheet(sheet, lastRowIndex: lastRowIndex);
            });
        }

        public static object ReadCell(ICell cell) {
            if (cell != null) {
                if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell)) {
                    return DateTimeUtil.FormatDateTime(cell.DateCellValue);
                } else {
                    switch (cell.CellType == CellType.Formula ? cell.CachedFormulaResultType : cell.CellType) {
                        case CellType.Boolean: return cell.BooleanCellValue ? 1 : 0;
                        case CellType.Numeric: return cell.NumericCellValue;
                        case CellType.Blank: case CellType.Error: case CellType.Unknown: return null;
                        default: return cell.StringCellValue;
                    }
                }
            } else {
                return null;
            }
        }

        public static List<object[]> ReadSheet(ISheet sheet, int firstRowIndex = 0, int? lastRowIndex = null,
        int firstColumnIndex = 0, int? lastColumnIndex = null) {
            var lastRow = Math.Min(sheet.LastRowNum, lastRowIndex ?? int.MaxValue);
            var rowValues = new List<object>();
            var list = new List<object[]>();
            for (int i = firstRowIndex; i <= lastRow; i++) {
                var row = sheet.GetRow(i);
                rowValues.Clear();
                if (row != null) {
                    var lastCol = Math.Min(row.LastCellNum - 1, lastColumnIndex ?? int.MaxValue);
                    for (int j = firstColumnIndex; j <= lastCol; j++) {
                        rowValues.Add(ReadCell(row.GetCell(j)));
                    }
                }
                list.Add(rowValues.ToArray());
            }
            return list;
        }

        public static int? ColumnRefToIndex(object val) { // returns 0-based column index
            // val may be numeric (1-based column number) or string (A, B, C, ..., XFC, XFD)
            if (val == null) {
                return null;
            } else if (val is string str) {
                if (int.TryParse(str, out var num)) {
                    return num >= 1 ? (int?)(num - 1) : null;
                } else {
                    return CellReference.ConvertColStringToIndex(str.ToUpper());
                }
            } else if (val is int || val is long) {
                var num = Convert.ToInt32(val);
                return num >= 1 ? (int?)(num - 1) : null;
            } else {
                return null;
            }
        }

        public static string GetColumnRefString(object val) {
            var index = ColumnRefToIndex(val);
            if (index.HasValue) {
                return CellReference.ConvertNumToColString(index.Value);
            } else {
                return null;
            }
        }

        public static string[] ReadColumnNames(string filePath, int worksheetIndex, int? firstRowNumber,
            string firstColumnLetter, string lastColumnLetter, bool headerRow
            ) {
            var firstRowIndex = firstRowNumber.HasValue ? firstRowNumber.Value - 1 : 0;
            var firstColumnIndex = firstColumnLetter == null ? 0 : ColumnRefToIndex(firstColumnLetter) ?? 0;
            var lastColumnIndex = lastColumnLetter == null ? null : ColumnRefToIndex(lastColumnLetter);
            return WithWorkbook(filePath, workbook => {
                var sheet = workbook.GetSheetAt(worksheetIndex);
                var row =
                    ReadSheet(sheet, firstRowIndex, firstRowIndex, firstColumnIndex, lastColumnIndex)
                    .SingleOrDefault();
                if (row == null) {
                    return new string[0];
                } else {
                    var strings = new string[row.Length];
                    for (var i = 0; i < row.Length; i++) {
                        strings[i] = row[i]?.ToString() ?? "";
                    }
                    return strings;
                }
            });
        }

        public static string[] ReadColumnNames(IReadOnlyList<object[]> rows, bool headerRow) {
            string[] columnNames;
            if (rows.Count == 0) {
                columnNames = new[] { "column1" };
            } else if (headerRow) {
                var originalHeader = rows[0];
                columnNames = new string[originalHeader.Length];
                var seenColumnNames = new HashSet<string>();
                for (int i = 0; i < originalHeader.Length; i++) {
                    var originalName = originalHeader[i];
                    var isNull = originalName is DBNull || originalName == null;
                    var prefix = isNull ? $"column{i + 1}" : originalName.ToString();
                    var candidate = prefix;
                    int suffix = 2;
                    while (seenColumnNames.Contains(candidate)) {
                        candidate = $"{prefix}{suffix++}";
                    }
                    seenColumnNames.Add(candidate);
                    columnNames[i] = candidate;
                }
            } else {
                columnNames = new string[rows[0].Length];
                for (int i = 0; i < rows[0].Length; i++) {
                    columnNames[i] = $"column{i + 1}";
                }
            }

            return columnNames;
        }
    }
}
