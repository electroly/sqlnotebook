using System;
using System.Collections.Generic;
using System.IO;
using ExcelDataReader;

namespace SqlNotebookScript.Utils;

public static class XlsUtil {
    public interface IWorkbook {
        List<string> ReadWorksheetNames();
        void SeekToWorksheet(int index);
        List<object[]> ReadSheet(
            int firstRowIndex = 0,
            int? lastRowIndex = null,
            int firstColumnIndex = 0,
            int? lastColumnIndex = null,
            int maxRows = int.MaxValue);
        string GetSheetName(int index);
    }

    private sealed class Workbook : IWorkbook, IDisposable {
        private readonly Stream _stream;
        private readonly IExcelDataReader _reader;
        private bool _disposedValue;

        public Workbook(string filePath) {
            try {
                _stream = File.OpenRead(filePath);
                ExcelReaderConfiguration config = new();
                _reader = ExcelReaderFactory.CreateReader(_stream, config);
            } catch {
                _stream?.Dispose();
                _reader?.Dispose();
                throw;
            }
        }

        private void Dispose(bool disposing) {
            if (!_disposedValue) {
                if (disposing) {
                    // dispose managed state (managed objects)
                    _reader.Dispose();
                    _stream.Dispose();
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                _disposedValue = true;
            }
        }

        public void Dispose() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public List<string> ReadWorksheetNames() {
            List<string> list = new();
            _reader.Reset();
            do {
                list.Add(_reader.Name);
            } while (_reader.NextResult());
            return list;
        }

        public string GetSheetName(int index) {
            SeekToWorksheet(index);
            return _reader.Name;
        }

        public void SeekToWorksheet(int index) {
            _reader.Reset();
            for (var i = 0; i < index; i++) {
                if (!_reader.NextResult()) {
                    throw new Exception($"Worksheet {index + 1} does not exist in this workbook.");
                }
            }
        }

        public List<object[]> ReadSheet(
            int firstRowIndex = 0,
            int? lastRowIndex = null,
            int firstColumnIndex = 0,
            int? lastColumnIndex = null,
            int maxRows = int.MaxValue
            ) {
            List<object[]> list = new();

            for (var i = 0; i < firstRowIndex; i++) {
                if (!_reader.Read()) {
                    return new();
                }
            }

            lastColumnIndex = Math.Min(
                lastColumnIndex ?? int.MaxValue,
                _reader.FieldCount - 1);

            var columnCount = lastColumnIndex.Value - firstColumnIndex + 1;
            if (columnCount < 0) {
                columnCount = 0;
            }

            for (var i = firstRowIndex;
                (!lastRowIndex.HasValue || i <= lastRowIndex.Value) && list.Count < maxRows;
                i++
                ) {
                if (!_reader.Read()) {
                    break;
                }

                var row = new object[columnCount];
                for (var j = firstColumnIndex;
                    j <= (lastColumnIndex ?? (_reader.FieldCount - 1));
                    j++
                    ) {
                    if (j < _reader.FieldCount) {
                        row[j - firstColumnIndex] = _reader.GetValue(j);
                    }
                }
                
                list.Add(row);
            }

            return list;
        }
    }

    public static void WithWorkbook(string filePath, Action<IWorkbook> action) {
        using Workbook workbook = new(filePath);
        action(workbook);
    }

    public static IReadOnlyList<string> ReadWorksheetNames(string filePath) {
        using Workbook workbook = new(filePath);
        return workbook.ReadWorksheetNames();
    }

    public static List<object[]> ReadWorksheet(string filePath, int worksheetIndex, int? lastRowIndex = null) {
        if (worksheetIndex < 0) {
            throw new Exception("The worksheet index must not be negative.");
        }

        using Workbook workbook = new(filePath);
        workbook.SeekToWorksheet(worksheetIndex);
        return workbook.ReadSheet(lastRowIndex: lastRowIndex);
    }

    public static int? ColumnRefToIndex(object val) { // returns 0-based column index
        // val may be numeric (1-based column number) or string (A, B, C, ..., XFC, XFD)
        if (val == null) {
            return null;
        } else if (val is string str) {
            if (int.TryParse(str, out var num)) {
                return num >= 1 ? (int?)(num - 1) : null;
            } else {
                return ConvertColStringToIndex(str.ToUpperInvariant());
            }
        } else if (val is int || val is long) {
            var num = Convert.ToInt32(val);
            return num >= 1 ? (int?)(num - 1) : null;
        } else {
            return null;
        }
    }

    public static int ConvertColStringToIndex(string s) {
        int index = 0;
        foreach (var ch in s) {
            if (ch < 'A' || ch > 'Z') {
                throw new Exception($"\"{s}\" is not a valid Excel column name.");
            }
            index *= 26;
            index += ch - 'A';
        }
        return index;
    }

    public static string ConvertNumToColString(int n) {
        if (n == 0) {
            return "A";
        }
        var s = "";
        while (n > 0) {
            var ch = (char)('A' + (n % 26));
            s = ch + s;
            n /= 26;
        }
        return s;
    }

    public static List<string> ReadColumnNames(IReadOnlyList<object[]> rows, bool headerRow) {
        List<string> columnNames = new();
        if (rows.Count == 0) {
            columnNames.Add("column1");
        } else if (headerRow) {
            foreach (var originalName in rows[0]) {
                var isNull = originalName is DBNull || originalName == null;
                if (isNull) {
                    columnNames.Add("");
                } else {
                    columnNames.Add(originalName.ToString());
                }
            }
        } else {
            for (int i = 0; i < rows[0].Length; i++) {
                columnNames.Add($"column{i + 1}");
            }
        }

        return columnNames;
    }
}
