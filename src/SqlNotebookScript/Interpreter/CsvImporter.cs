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
using System.Text;
using Microsoft.VisualBasic.FileIO;
using SqlNotebookScript;
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.Interpreter {
    public sealed class CsvImporter {
        private readonly INotebook _notebook;
        private readonly ScriptEnv _env;
        private readonly ScriptRunner _runner;
        private readonly Ast.ImportCsvStmt _stmt;

        // option values
        private readonly long _skipLines;
        private readonly long? _takeLines;
        private readonly bool _headerRow;
        private readonly bool _truncateExistingTable;
        private readonly bool _temporaryTable;
        private readonly Encoding _fileEncoding; // or null for automatic
        private readonly IfConversionFails _ifConversionFails;

        private enum IfConversionFails {
            ImportAsText = 1,
            SkipRow = 2,
            Abort = 3
        }

        // must be run from the SQLite thread
        public static void Import(INotebook notebook, ScriptEnv env, ScriptRunner runner, Ast.ImportCsvStmt stmt) {
            var importer = new CsvImporter(notebook, env, runner, stmt);
            importer.Import();
        }

        private CsvImporter(INotebook notebook, ScriptEnv env, ScriptRunner runner, Ast.ImportCsvStmt stmt) {
            _notebook = notebook;
            _env = env;
            _runner = runner;
            _stmt = stmt;

            foreach (var option in _stmt.OptionsList.GetOptionKeys()) {
                switch (option) {
                    case "SKIP_LINES":
                        _skipLines = _stmt.OptionsList.GetOptionLong(option, _runner, _env, 0, minValue: 0);
                        break;

                    case "TAKE_LINES":
                        _takeLines = _stmt.OptionsList.GetOptionLong(option, _runner, _env, -1, minValue: -1);
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

                    case "FILE_ENCODING":
                        _fileEncoding = _stmt.OptionsList.GetOptionEncoding(option, _runner, _env);
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
            var filePath = GetFilePath();
            using (var stream = File.OpenRead(filePath))
            using (var bufferedStream = new BufferedStream(stream))
            using (var parser = NewParser(bufferedStream)) {
                var parserBuffer = new TextFieldParserBuffer(parser);

                // skip the specified number of initial file lines
                for (int i = 0; i < _skipLines && !parserBuffer.EndOfData; i++) {
                    parserBuffer.SkipLine();
                }

                // consume the header row if present.  invent generic column names if not.
                var srcColNames = ReadColumnNames(parserBuffer);

                // if the user specified a column list, then check to make sure all of the specified column names exist
                // in the list we just read.  indices in dstColNodes/dstColNames match up with srcColNames, and some
                // elements of dstColNodes may be null if the user does not want to include certain CSV columns.
                // all dstColNodes elements will be null if the user specified no columns explicitly, which means they
                // want to include all columns.
                Ast.ImportColumn[] dstColNodes;
                string[] dstColNames;
                GetDestinationColumns(srcColNames, out dstColNodes, out dstColNames);

                // create or truncate the target table, if necessary.
                var dstTableName = _runner.EvaluateIdentifierOrExpr(_stmt.ImportTable.TableName, _env);
                CreateOrTruncateTable(srcColNames, dstColNodes, dstColNames, dstTableName);

                // ensure that the target table has all of the requested column names.
                SqlUtil.VerifyColumnsExist(dstColNames, dstTableName, _notebook);

                // read the data rows and insert into the target table.
                InsertData(parserBuffer, dstColNames, dstColNodes, dstTableName);
            }
        }

        private void InsertData(TextFieldParserBuffer parser, string[] dstColNames, Ast.ImportColumn[] dstColNodes, 
        string dstTableName) {
            var batchRowLimit = 10;
            var insertSqlSingle = SqlUtil.GetInsertSql(dstTableName, dstColNames.Length, numRows: 1);
            var insertSqlBatch = SqlUtil.GetInsertSql(dstTableName, dstColNames.Length, numRows: batchRowLimit);
            var batchArgs = new object[batchRowLimit * dstColNames.Length];
            int batchRowsSoFar = 0;
            for (int i = 0; (!_takeLines.HasValue || i < _takeLines.Value) && !parser.EndOfData; i++) {
                var cells = parser.ReadFields();
                var batchArgOffset = batchRowsSoFar * dstColNames.Length;

                // 'cells' contains the raw string values from the CSV line, and may not be the same length as 'args'.
                // 'args' is where we need to write the converted values. it contains the previous line's data so if
                // this line is missing some columns, we need to null out the values from last time.
                bool skipRow = false;
                for (int j = 0; !skipRow && j < Math.Min(cells.Length, dstColNames.Length); j++) {
                    var text = cells[j];
                    var typeConversion = dstColNodes[j].TypeConversion ?? Ast.TypeConversion.Text;

                    object converted = text;
                    bool error = false;
                    switch (typeConversion) {
                        case Ast.TypeConversion.Text:
                            converted = text;
                            break;

                        case Ast.TypeConversion.Integer:
                            int intValue;
                            if (int.TryParse(text, out intValue)) {
                                converted = intValue;
                            } else {
                                error = true;
                            }
                            break;

                        case Ast.TypeConversion.Real:
                            double realValue;
                            if (double.TryParse(text, out realValue)) {
                                converted = realValue;
                            } else {
                                error = true;
                            }
                            break;

                        case Ast.TypeConversion.Date:
                            DateTime dateValue;
                            if (DateTime.TryParse(text, out dateValue)) {
                                converted = dateValue.Date.ToString("yyyy-MM-dd");
                            } else {
                                error = true;
                            }
                            break;

                        case Ast.TypeConversion.DateTime:
                            DateTime dateTimeValue;
                            if (DateTime.TryParse(text, out dateTimeValue)) {
                                converted = dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            } else {
                                error = true;
                            }
                            break;

                        case Ast.TypeConversion.DateTimeOffset:
                            DateTimeOffset dateTimeOffsetValue;
                            if (DateTimeOffset.TryParse(text, out dateTimeOffsetValue)) {
                                converted = dateTimeOffsetValue.ToString("yyyy-MM-dd HH:mm:ss.fff zzz");
                            } else {
                                error = true;
                            }
                            break;

                        default:
                            throw new Exception($"Internal error: unknown type conversion \"{typeConversion}\".");
                    }

                    if (!error || _ifConversionFails == IfConversionFails.ImportAsText) {
                        batchArgs[batchArgOffset + j] = converted;
                    } else if (_ifConversionFails == IfConversionFails.SkipRow) {
                        skipRow = true;
                    } else if (_ifConversionFails == IfConversionFails.Abort) {
                        throw new Exception($"Failed to parse input value as type \"{typeConversion}\". Value: \"{text}\".");
                    } else {
                        throw new Exception($"Internal error: unknown value for IF_CONVERSION_FAILS \"{_ifConversionFails}\".");
                    }
                }

                if (skipRow) {
                    // clear out this row, throwing out what we just did
                    Array.Clear(batchArgs, batchArgOffset, dstColNames.Length);
                } else {
                    // this row is good to go, so include it in the batch
                    batchRowsSoFar++;
                }

                if (batchRowsSoFar == batchRowLimit) {
                    _notebook.Execute(insertSqlBatch, batchArgs);
                    Array.Clear(batchArgs, 0, batchArgs.Length);
                    batchRowsSoFar = 0;
                }
            }

            // if the number of rows isn't a multiple of the batch size, then we have some left over
            var singleArgs = new object[dstColNames.Length];
            for (int i = 0; i < batchRowsSoFar; i++) {
                Array.Copy(batchArgs, i * dstColNames.Length, singleArgs, 0, dstColNames.Length);
                _notebook.Execute(insertSqlSingle, singleArgs);
            }
        }

        private void CreateOrTruncateTable(IReadOnlyList<string> srcColNames, Ast.ImportColumn[] dstColNodes,
        IReadOnlyList<string> dstColNames, string dstTableName) {
            // create the table if it doesn't already exist.
            var columnDefs = new List<string>();
            if (dstColNodes.All(x => x == null)) {
                // the user did not specify a column list, so all columns will be included
                columnDefs.AddRange(srcColNames.Select(SqlUtil.DoubleQuote));
            } else {
                // the user specified which columns to include
                for (int i = 0; i < dstColNodes.Length; i++) {
                    if (dstColNodes[i] != null) {
                        var name = dstColNames[i];
                        var type = dstColNodes[i].TypeConversion?.ToString() ?? "";
                        string sqlType = "";
                        if (dstColNodes[i].TypeConversion.HasValue) {
                            switch (dstColNodes[i].TypeConversion.Value) {
                                case Ast.TypeConversion.Integer: sqlType = "integer"; break;
                                case Ast.TypeConversion.Real: sqlType = "real"; break;
                                default: sqlType = "text"; break;
                            }
                        }

                        columnDefs.Add($"{name.DoubleQuote()} {sqlType}");
                    }
                }
            }
            var columnDefsList = string.Join(", ", columnDefs);
            var sql = $"CREATE {(_temporaryTable ? "TEMPORARY" : "")} TABLE IF NOT EXISTS " +
                $"{dstTableName.DoubleQuote()} ({columnDefsList})";
            _notebook.Execute(sql);

            if (_truncateExistingTable) {
                _notebook.Execute($"DELETE FROM {dstTableName.DoubleQuote()}");
            }
        }

        private void GetDestinationColumns(IReadOnlyList<string> srcColNames, out Ast.ImportColumn[] dstColNodes, out string[] dstColNames) {
            var lowercaseSrcColNames = srcColNames.Select(x => x.ToLower()).ToArray();
            var lowercaseDstColNames = new HashSet<string>(); // to ensure we don't have duplicate column names
            dstColNodes = new Ast.ImportColumn[srcColNames.Count];
            dstColNames = new string[srcColNames.Count];
            foreach (var importCol in _stmt.ImportTable.ImportColumns) {
                var name = _runner.EvaluateIdentifierOrExpr(importCol.ColumnName, _env);

                var colIndex = lowercaseSrcColNames.IndexOf(name.ToLower());
                if (colIndex.HasValue) {
                    if (dstColNodes[colIndex.Value] == null) {
                        dstColNodes[colIndex.Value] = importCol;
                    } else {
                        throw new Exception($"The input column \"{name}\" was specified more than once in the column list.");
                    }
                } else {
                    // the user specified a column name that does not exist in the CSV file
                    throw new Exception($"The column \"{name}\" does not exist in the CSV file. " +
                        $"The columns that were found are: {string.Join(", ", srcColNames.Select(SqlUtil.DoubleQuote))}");
                }
                
                // apply the user's rename if specified
                string dstName;
                if (importCol.AsName != null) {
                    dstName = _runner.EvaluateIdentifierOrExpr(importCol.AsName, _env);
                } else {
                    dstName = name;
                }
                dstColNames[colIndex.Value] = dstName;
                
                // ensure this isn't a duplicate destination column name
                if (lowercaseDstColNames.Contains(dstName.ToLower())) {
                    throw new Exception($"The column \"{dstName}\" was specified more than once as a destination column name.");
                } else {
                    lowercaseDstColNames.Add(dstName.ToLower());
                }
            }
        }

        private List<string> ReadColumnNames(TextFieldParserBuffer parser) {
            var srcColNames = new List<string>();

            if (_headerRow) {
                // read the column header row
                var cells = parser.ReadFields();
                if (cells == null) {
                    // end of file; there is nothing here.
                    if (_skipLines == 0) {
                        throw new Exception("No column header row was found because the file is empty.");
                    } else {
                        throw new Exception("No column header row was found because all rows were skipped.");
                    }
                }

                // add a numeric suffix to each column name if necessary to make them all unique
                foreach (var cell in cells) {
                    var testName = cell;
                    var testNum = 1;
                    while (srcColNames.Contains(testName)) {
                        testNum++;
                        testName = $"{cell}{testNum}";
                    }
                    srcColNames.Add(testName);
                }
            } else {
                // no header row so use "column1", "column2", etc. based on the first row of data
                var fields = parser.PeekFields();
                if (fields == null || !fields.Any()) {
                    // treat empty file as a single column with no rows
                    srcColNames.Add("column1");
                } else {
                    for (int i = 0; i < fields.Length; i++) {
                        srcColNames.Add($"column{i + 1}");
                    }
                }
            }

            return srcColNames;
        }

        private string GetFilePath() {
            var filePath = _runner.EvaluateExpr<string>(_stmt.FilenameExpr, _env);
            if (File.Exists(filePath)) {
                return filePath;
            } else {
                throw new Exception($"The specified CSV file was not found: \"{filePath}\"");
            }
        }

        private TextFieldParser NewParser(Stream stream) {
            var x = _fileEncoding == null
                ? new TextFieldParser(stream)
                : new TextFieldParser(stream, _fileEncoding, false);
            x.HasFieldsEnclosedInQuotes = true;
            x.SetDelimiters(",");
            return x;
        }

        // allow a line to be read, un-read, and then read again
        private sealed class TextFieldParserBuffer {
            private readonly Stack<string[]> _unreadStack = new Stack<string[]>();
            private readonly TextFieldParser _parser;

            public TextFieldParserBuffer(TextFieldParser parser) {
                _parser = parser;
            }

            public bool EndOfData => !_unreadStack.Any() && _parser.EndOfData;

            public void SkipLine() {
                if (_unreadStack.Any()) {
                    _unreadStack.Pop();
                } else if (!_parser.EndOfData) {
                    _parser.ReadLine();
                }
            }

            public string[] ReadFields() { // or null
                if (_unreadStack.Any()) {
                    return _unreadStack.Pop();
                } else if (_parser.EndOfData) {
                    return null;
                } else {
                    return _parser.ReadFields();
                }
            }

            public void UnreadFields(string[] fields) {
                if (fields != null) {
                    _unreadStack.Push(fields);
                }
            }

            public string[] PeekFields() { // or null
                var fields = ReadFields();
                UnreadFields(fields);
                return fields;
            }
        }
    }
}
