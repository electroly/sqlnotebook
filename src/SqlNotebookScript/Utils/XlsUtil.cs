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

        public static IReadOnlyList<object[]> ReadWorksheet(string filePath, int worksheetIndex) {
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
                return ReadSheet(sheet);
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

        public static IReadOnlyList<object[]> ReadSheet(ISheet sheet, int firstRowIndex = 0, int? lastRowIndex = null,
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
            } else if (val is string) {
                var columnString = val.ToString().ToUpper();
                return CellReference.ConvertColStringToIndex(columnString);
            } else if (val is int || val is long) {
                var index = Convert.ToInt32(val) - 1;
                return index >= 0 ? (int?)index : null;
            } else {
                return null;
            }
        }
    }
}
