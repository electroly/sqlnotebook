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
using SqlNotebookScript;

namespace SqlNotebookScript.Interpreter {
    public sealed class TxtExporter {
        private readonly INotebook _notebook;
        private readonly ScriptEnv _env;
        private readonly ScriptRunner _runner;
        private readonly Ast.ExportTxtStmt _stmt;

        // arguments
        private readonly string _filePath;

        // options
        private readonly bool _truncateExistingFile;
        private readonly Encoding _fileEncoding;

        // must be run from the SQLite thread
        public static void Export(INotebook notebook, ScriptEnv env, ScriptRunner runner, Ast.ExportTxtStmt stmt) {
            var exporter = new TxtExporter(notebook, env, runner, stmt);
            exporter.Export();
        }

        private TxtExporter(INotebook notebook, ScriptEnv env, ScriptRunner runner, Ast.ExportTxtStmt stmt) {
            _notebook = notebook;
            _env = env;
            _runner = runner;
            _stmt = stmt;

            _filePath = GetFilePath();

            foreach (var option in _stmt.OptionsList.GetOptionKeys()) {
                switch (option) {
                    case "TRUNCATE_EXISTING_FILE ":
                        _truncateExistingFile = _stmt.OptionsList.GetOptionBool(option, _runner, _env, false);
                        break;

                    case "FILE_ENCODING":
                        _fileEncoding =
                            _stmt.OptionsList.GetOptionEncoding(option, _runner, _env);
                        break;

                    default:
                        throw new Exception($"\"{option}\" is not a recognized option name.");
                }
            }

            if (_fileEncoding == null) {
                _fileEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
            }
        }

        private void Export() {
            var fileMode = _truncateExistingFile ? FileMode.Create : FileMode.Append;
            using (var stream = File.Open(_filePath, fileMode, FileAccess.Write, FileShare.None)) 
            using (var writer = new StreamWriter(stream, _fileEncoding)) {
                var sdt = _notebook.Query(_stmt.SelectStmt.Sql, _env.Vars);
                foreach (var row in sdt.Rows) {
                    writer.WriteLine(string.Join("", row));
                }
            }
        }

        private string GetFilePath() {
            var filePath = _runner.EvaluateExpr<string>(_stmt.FilenameExpr, _env);
            var dirPath = Path.GetDirectoryName(filePath);
            if (Directory.Exists(dirPath)) {
                return filePath;
            } else {
                throw new Exception($"The output folder was not found: \"{dirPath}\"");
            }
        }
    }
}
