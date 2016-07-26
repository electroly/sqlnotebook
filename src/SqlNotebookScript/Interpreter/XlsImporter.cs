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
using NPOI.SS.UserModel;
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.Interpreter {
    public sealed class XlsImporter {
        private readonly INotebook _notebook;
        private readonly ScriptEnv _env;
        private readonly ScriptRunner _runner;
        private readonly Ast.ImportXlsStmt _stmt;

        private readonly string _filePath;
        private readonly object _whichSheet; // 1-based number or string name

        // option values
        private readonly int _firstRowIndex; // 0-based index
        private readonly int? _lastRowIndex; // 0-based index
        private readonly int _firstColumnIndex; // 0-based index
        private readonly int? _lastColumnIndex; // 0-based index
        private readonly bool _headerRow = true;
        private readonly bool _truncateExistingTable;
        private readonly bool _temporaryTable;
        private readonly IfConversionFails _ifConversionFails;

        // must be run from the SQLite thread
        public static void Import(INotebook notebook, ScriptEnv env, ScriptRunner runner, Ast.ImportXlsStmt stmt) {
            var importer = new XlsImporter(notebook, env, runner, stmt);
            SqlUtil.WithTransaction(notebook, importer.Import);
        }

        private XlsImporter(INotebook notebook, ScriptEnv env, ScriptRunner runner, Ast.ImportXlsStmt stmt) {
            _notebook = notebook;
            _env = env;
            _runner = runner;
            _stmt = stmt;

            _filePath = _runner.EvaluateExpr<string>(_stmt.FilenameExpr, _env);
            if (!File.Exists(_filePath)) {
                throw new Exception($"The specified XLS/XLSX file was not found: \"{_filePath}\"");
            }

            if (_stmt.WhichSheetExpr != null) {
                _whichSheet = _runner.EvaluateExpr(_stmt.WhichSheetExpr, _env);
            }

            int? index;
            foreach (var option in _stmt.OptionsList.GetOptionKeys()) {
                switch (option) {
                    case "FIRST_ROW":
                        _firstRowIndex = _stmt.OptionsList.GetOptionInt(option, _runner, _env, 1, minValue: 1) - 1;
                        break;

                    case "LAST_ROW":
                        _lastRowIndex = _stmt.OptionsList.GetOptionInt(option, _runner, _env, 1, minValue: 1) - 1;
                        break;

                    case "FIRST_COLUMN":
                        index = XlsUtil.ColumnRefToIndex(_stmt.OptionsList.GetOption<object>(option, _runner, _env, null));
                        if (index.HasValue) {
                            _firstColumnIndex = index.Value;
                        } else {
                            throw new Exception($"The {option} option must be a valid column number or string.");
                        }
                        break;

                    case "LAST_COLUMN":
                        index = XlsUtil.ColumnRefToIndex(_stmt.OptionsList.GetOption<object>(option, _runner, _env, null));
                        if (index.HasValue) {
                            _lastColumnIndex = index.Value;
                        } else {
                            throw new Exception($"The {option} option must be a valid column number or string.");
                        }
                        break;

                    case "HEADER_ROW":
                        _headerRow = _stmt.OptionsList.GetOptionBool(option, _runner, _env, true);
                        break;

                    case "TRUNCATE_EXISTING_TABLE":
                        _truncateExistingTable = _stmt.OptionsList.GetOptionBool(option, _runner, _env, false);
                        break;

                    case "TEMPORARY_TABLE":
                        _temporaryTable = _stmt.OptionsList.GetOptionBool(option, _runner, _env, false);
                        break;

                    case "IF_CONVERSION_FAILS":
                        _ifConversionFails = (IfConversionFails)_stmt.OptionsList.GetOptionLong(
                            option, _runner, _env, 1, minValue: 1, maxValue: 3);
                        break;

                    default:
                        throw new Exception($"\"{option}\" is not a recognized option name.");
                }
            }
        }

        private void Import() {
            XlsUtil.WithWorkbook(_filePath, workbook => {
                var sheet = workbook.GetSheetAt(GetSheetIndex(workbook));
                var rows = XlsUtil.ReadSheet(sheet, _firstRowIndex, _lastRowIndex, _firstColumnIndex, _lastColumnIndex);

                SqlUtil.Import(
                    srcColNames: ReadColumnNames(rows), 
                    dataRows: rows.Skip(_headerRow ? 1 : 0),
                    importTable: _stmt.ImportTable, 
                    temporaryTable: _temporaryTable, 
                    truncateExistingTable: _truncateExistingTable, 
                    ifConversionFails: _ifConversionFails, 
                    notebook: _notebook, 
                    runner: _runner,
                    env: _env);
            });
        }

        private string[] ReadColumnNames(IReadOnlyList<object[]> rows) {
            string[] columnNames;
            if (rows.Count == 0) {
                columnNames = new[] { "column1" };
            } else if (_headerRow) {
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

        private int GetSheetIndex(IWorkbook workbook) {
            var whichSheet = _whichSheet ?? 1; // 1-based number or name string
            if (whichSheet is int || whichSheet is long) {
                var whichSheetNum = Convert.ToInt32(whichSheet);
                if (whichSheetNum < 1) {
                    throw new Exception($"The worksheet number must be at least 1.");
                }
                return whichSheetNum - 1;
            } else if (whichSheet is string) {
                var name = (string)whichSheet;
                var index = workbook.GetSheetIndex(name);
                if (index == -1) {
                    throw new Exception($"The worksheet \"{name}\" was not found.");
                } else {
                    return index;
                }
            } else {
                throw new Exception($"The \"which-sheet\" argument must be a number or a string.");
            }
        }
    }
}
