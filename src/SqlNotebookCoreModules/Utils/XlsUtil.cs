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
using NPOI.XSSF.UserModel;

namespace SqlNotebookCoreModules.Utils {
    public static class XlsUtil {
        public static IReadOnlyList<string> ReadWorksheetNames(string filePath) {
            if (!File.Exists(filePath)) {
                throw new FileNotFoundException();
            }

            var list = new List<string>();

            using (var stream = File.OpenRead(filePath)) {
                var workbook =
                    Path.GetExtension(filePath).ToLower() == ".xls"
                    ? (IWorkbook)new HSSFWorkbook(stream) : new XSSFWorkbook(stream);
                for (int i = 0; i < workbook.NumberOfSheets; i++) {
                    var name = workbook.GetSheetName(i);
                    list.Add(name);
                }
            }

            return list;
        }

        public static IReadOnlyList<object[]> ReadWorksheet(string filePath, int worksheetIndex) {
            if (!File.Exists(filePath)) {
                throw new FileNotFoundException();
            }
            if (worksheetIndex < 0) {
                throw new Exception("Argument \"worksheet-index\" must not be negative.");
            }

            using (var stream = File.OpenRead(filePath)) {
                var workbook =
                    Path.GetExtension(filePath).ToLower() == ".xls"
                    ? (IWorkbook)new HSSFWorkbook(stream) : new XSSFWorkbook(stream);

                if (worksheetIndex >= workbook.NumberOfSheets) {
                    throw new Exception(
                        $"The worksheet index is out of range. The workbook has {workbook.NumberOfSheets} worksheets.");
                }

                var sheet = workbook.GetSheetAt(worksheetIndex);
                return ReadSheet(sheet).ToList();
            }
        }

        private static IEnumerable<object[]> ReadSheet(ISheet sheet) {
            for (int i = 0; i <= sheet.LastRowNum; i++) {
                var row = sheet.GetRow(i);
                var rowValues = new object[row.LastCellNum];
                if (row != null) {
                    for (int j = 0; j < row.LastCellNum; j++) {
                        var cell = row.GetCell(j);
                        if (cell != null) {
                            switch (cell.CellType == CellType.Formula ? cell.CachedFormulaResultType : cell.CellType) {
                                case CellType.Boolean: rowValues[j] = cell.BooleanCellValue ? 1 : 0; break;
                                case CellType.Numeric: rowValues[j] = cell.NumericCellValue; break;
                                case CellType.Blank: case CellType.Error: case CellType.Unknown:
                                    rowValues[j] = null; break;
                                default: rowValues[j] = cell.StringCellValue; break;
                            }
                        }
                    }
                }
                yield return rowValues;
            }
        }
    }
}
