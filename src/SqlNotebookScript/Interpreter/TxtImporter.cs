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
using System.IO;
using System.Text;
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.Interpreter {
    public sealed class TxtImporter {
        private readonly INotebook _notebook;
        private readonly ScriptEnv _env;
        private readonly ScriptRunner _runner;
        private readonly Ast.ImportTxtStmt _stmt;

        // arguments
        private readonly string _filePath;
        private readonly string _tableName;
        private readonly string _lineNumberColumnName; // may be null
        private readonly string _textColumnName; // may be null

        // options
        private readonly long _skipLines;
        private readonly long? _takeLines;
        private readonly bool _truncateExistingTable;
        private readonly bool _temporaryTable;
        private readonly Encoding _fileEncoding; // or null for automatic

        // must be run from the SQLite thread
        public static void Import(INotebook notebook, ScriptEnv env, ScriptRunner runner, Ast.ImportTxtStmt stmt) {
            var importer = new TxtImporter(notebook, env, runner, stmt);
            SqlUtil.WithTransaction(notebook, importer.Import);
        }

        private TxtImporter(INotebook notebook, ScriptEnv env, ScriptRunner runner, Ast.ImportTxtStmt stmt) {
            _notebook = notebook;
            _env = env;
            _runner = runner;
            _stmt = stmt;

            _filePath = GetFilePath();
            _tableName = _runner.EvaluateIdentifierOrExpr(_stmt.TableName, _env);
            _lineNumberColumnName = _stmt.LineNumberColumnName == null ? null : _runner.EvaluateIdentifierOrExpr(_stmt.LineNumberColumnName, _env);
            _textColumnName = _stmt.TextColumnName == null ? null : _runner.EvaluateIdentifierOrExpr(_stmt.TextColumnName, _env);

            foreach (var option in _stmt.OptionsList.GetOptionKeys()) {
                switch (option) {
                    case "SKIP_LINES":
                        _skipLines = _stmt.OptionsList.GetOptionLong(option, _runner, _env, 0, minValue: 0);
                        break;

                    case "TAKE_LINES":
                        _takeLines = _stmt.OptionsList.GetOptionLong(option, _runner, _env, -1, minValue: -1);
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

                    default:
                        throw new Exception($"\"{option}\" is not a recognized option name.");
                }
            }
        }

        private void Import() {
            using (var reader = NewReader(GetFilePath())) {
                // skip the specified number of initial file lines
                for (int i = 0; i < _skipLines && !reader.EndOfStream; i++) {
                    reader.ReadLine();
                }

                CreateOrTruncateTable();

                if (_textColumnName != null) {
                    SqlUtil.VerifyColumnsExist(new[] { _lineNumberColumnName, _textColumnName }, _tableName, _notebook);
                }

                var insertSql = GetInsertSql();
                var args = new object[2];
                for (long i = 0; (!_takeLines.HasValue || i < _takeLines.Value) && !reader.EndOfStream; i++) {
                    args[0] = i;
                    args[1] = reader.ReadLine();
                    _notebook.Execute(insertSql, args);
                }
            }
        }

        private StreamReader NewReader(string filePath) {
            return _fileEncoding == null
                ? new StreamReader(filePath)
                : new StreamReader(filePath, _fileEncoding, false);
        }

        private string GetFilePath() {
            var filePath = _runner.EvaluateExpr<string>(_stmt.FilenameExpr, _env);
            if (File.Exists(filePath)) {
                return filePath;
            } else {
                throw new Exception($"The specified file was not found: \"{filePath}\"");
            }
        }

        private void CreateOrTruncateTable() {
            _notebook.Execute($"CREATE {(_temporaryTable ? "TEMPORARY" : "")} TABLE IF NOT EXISTS " +
                $"{_tableName.DoubleQuote()} ({(_lineNumberColumnName ?? "number").DoubleQuote()} INTEGER, " +
                $"{(_textColumnName ?? "line").DoubleQuote()} TEXT)");

            if (_truncateExistingTable) {
                _notebook.Execute($"DELETE FROM {_tableName.DoubleQuote()}");
            }
        }

        private string GetInsertSql() {
            var columnList = _textColumnName == null ? "" : $"({_lineNumberColumnName}, {_textColumnName})";
            return $"INSERT INTO {_tableName.DoubleQuote()} {columnList} VALUES (?, ?)";
        }
    }
}
