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

namespace SqlNotebookScript.Interpreter {
    public sealed class XlsImporter {
        private readonly INotebook _notebook;
        private readonly ScriptEnv _env;
        private readonly ScriptRunner _runner;
        private readonly Ast.ImportXlsStmt _stmt;

        private readonly string _filePath;

        // option values
        private readonly long _skipRows;
        private readonly long? _takeRows;
        private readonly long _skipColumns;
        private readonly long? _takeColumns;
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
        public static void Import(INotebook notebook, ScriptEnv env, ScriptRunner runner, Ast.ImportXlsStmt stmt) {
            var importer = new XlsImporter(notebook, env, runner, stmt);
            importer.Import();
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

            foreach (var option in _stmt.OptionsList.GetOptionKeys()) {
                switch (option) {
                    case "FIRST_ROW":
                        break;

                    case "LAST_ROW":
                        break;

                    case "FIRST_COLUMN":
                        break;

                    case "LAST_COLUMN":
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
        }
    }
}
