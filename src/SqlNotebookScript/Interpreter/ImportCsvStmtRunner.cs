using Microsoft.VisualBasic.FileIO;
using SqlNotebookScript.Core;
using SqlNotebookScript.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SqlNotebookScript.Interpreter {
    public sealed class ImportCsvStmtRunner {
        private readonly Notebook _notebook;
        private readonly ScriptEnv _env;
        private readonly ScriptRunner _runner;
        private readonly Ast.ImportCsvStmt _stmt;

        // option values
        private readonly long _skipLines;
        private readonly long? _takeLines;
        private readonly bool _headerRow = true;
        private readonly bool _truncateExistingTable;
        private readonly bool _temporaryTable;
        private readonly Encoding _fileEncoding; // or null for automatic
        private readonly IfConversionFails _ifConversionFails;

        // must be run from the SQLite thread
        public static void Run(Notebook notebook, ScriptEnv env, ScriptRunner runner, Ast.ImportCsvStmt stmt) {
            var importer = new ImportCsvStmtRunner(notebook, env, runner, stmt);
            SqlUtil.WithTransaction(notebook, importer.Import);
        }

        private ImportCsvStmtRunner(Notebook notebook, ScriptEnv env, ScriptRunner runner, Ast.ImportCsvStmt stmt) {
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
            string separator = ",";
            if (_stmt.SeparatorExpr != null) {
                separator = _runner.EvaluateExpr(_stmt.SeparatorExpr, _env).ToString();
                if (separator.Equals("Tab", StringComparison.OrdinalIgnoreCase)) {
                    separator = "\t";
                }
            }

            var filePath = GetFilePath();
            using (var stream = File.OpenRead(filePath))
            using (var bufferedStream = new BufferedStream(stream))
            using (var parser = NewParser(bufferedStream, separator)) {
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
                SqlUtil.GetDestinationColumns(_stmt.ImportTable.ImportColumns, _runner, _env, srcColNames,
                    out dstColNodes, out dstColNames);

                // create or truncate the target table, if necessary.
                var dstTableName = _runner.EvaluateIdentifierOrExpr(_stmt.ImportTable.TableName, _env);
                SqlUtil.CreateOrTruncateTable(srcColNames, dstColNodes, dstColNames, dstTableName, _temporaryTable,
                    _truncateExistingTable, _notebook);

                // ensure that the target table has all of the requested column names.
                SqlUtil.VerifyColumnsExist(dstColNames, dstTableName, _notebook);

                // read the data rows and insert into the target table.
                SqlUtil.InsertDataRows(GetRows(parserBuffer), dstColNames, dstColNodes, dstTableName, _ifConversionFails,
                    _notebook);
            }
        }

        private IEnumerable<object[]> GetRows(TextFieldParserBuffer parser) {
            for (int i = 0; (!_takeLines.HasValue || i < _takeLines.Value) && !parser.EndOfData; i++) {
                yield return parser.ReadFields();
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

        private TextFieldParser NewParser(Stream stream, string separator) {
            var x = _fileEncoding == null
                ? new TextFieldParser(stream)
                : new TextFieldParser(stream, _fileEncoding, false);
            x.HasFieldsEnclosedInQuotes = true;
            if (separator == "") {
                separator = ",";
            }
            x.SetDelimiters(separator);
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
