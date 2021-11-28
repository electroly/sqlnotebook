using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using SqlNotebookScript.Core;
using SqlNotebookScript.DataTables;
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.Interpreter;

public sealed class ExportCsvStmtRunner {
    private readonly Notebook _notebook;
    private readonly ScriptEnv _env;
    private readonly ScriptRunner _runner;
    private readonly Ast.ExportCsvStmt _stmt;
    private readonly CancellationToken _cancel;

    // arguments
    private readonly string _filePath;

    // options
    private readonly bool _headerRow = true;
    private readonly char _separator = ',';
    private readonly bool _truncateExistingFile;
    private readonly Encoding _fileEncoding;

    // must be run from the SQLite thread
    public static void Run(Notebook notebook, ScriptEnv env, ScriptRunner runner, Ast.ExportCsvStmt stmt,
        CancellationToken cancel
        ) {
        ExportCsvStmtRunner exporter = new(notebook, env, runner, stmt, cancel);
        exporter.Export();
    }

    private ExportCsvStmtRunner(Notebook notebook, ScriptEnv env, ScriptRunner runner, Ast.ExportCsvStmt stmt,
        CancellationToken cancel
        ) {
        _notebook = notebook;
        _env = env;
        _runner = runner;
        _stmt = stmt;
        _cancel = cancel;
        _filePath = GetFilePath();

        foreach (var option in _stmt.OptionsList.GetOptionKeys()) {
            switch (option) {
                case "HEADER_ROW":
                    _headerRow = _stmt.OptionsList.GetOptionBool(option, _runner, _env, true);
                    break;

                case "SEPARATOR":
                    {
                        var separator = _stmt.OptionsList.GetOption(option, _runner, _env, ",");
                        if (separator.Length != 1) {
                            throw new Exception("EXPORT CSV: The separator must be a single character.");
                        }
                        _separator = separator[0];
                        break;
                    }

                case "TRUNCATE_EXISTING_FILE":
                    _truncateExistingFile = _stmt.OptionsList.GetOptionBool(option, _runner, _env, false);
                    break;

                case "FILE_ENCODING":
                    _fileEncoding = _stmt.OptionsList.GetOptionEncoding(option, _runner, _env);
                    break;

                default:
                    throw new Exception($"EXPORT CSV: \"{option}\" is not a recognized option name.");
            }
        }

        if (_fileEncoding == null) {
            _fileEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
        }
    }

    private void Export() {
        if (_stmt.TableNameOrNull != null) {
            var tableName = _runner.EvaluateIdentifierOrExpr(_stmt.TableNameOrNull, _env);
            WriteTable("SELECT * FROM " + tableName.DoubleQuote());
        } else if (_stmt.ScriptNameOrNull != null) {
            var scriptName = _runner.EvaluateIdentifierOrExpr(_stmt.ScriptNameOrNull, _env);
            using var output = _runner.ExecuteSubScript(scriptName, new(), _env);
            if (output.DataTables.Count == 0) {
                throw new Exception($"EXPORT CSV: The script \"{scriptName}\" did not produce a result set.");
            }
            WriteTable(output.DataTables[0]);
        } else if (_stmt.SelectStmtOrNull != null) {
            WriteTable(_stmt.SelectStmtOrNull.Sql);
        } else {
            throw new Exception("EXPORT CSV: Internal error. No input provided.");
        }
    }

    private void WriteTable(SimpleDataTable sdt) {
        using var status = WaitStatus.StartRows(Path.GetFileName(_filePath));
        var fileMode = _truncateExistingFile ? FileMode.Create : FileMode.Append;
        using var stream = File.Open(_filePath, fileMode, FileAccess.Write, FileShare.None);
        using var writer = new StreamWriter(stream, _fileEncoding);
        if (_headerRow) {
            writer.WriteLine(string.Join(_separator, sdt.Columns.Select(c => CsvUtil.QuoteCsv(c, _separator))));
        }
        CsvUtil.WriteCsv(sdt.Rows, writer, status.IncrementRows, _separator, _cancel);
    }

    private void WriteTable(string sql) {
        using var status = WaitStatus.StartRows(Path.GetFileName(_filePath));
        var fileMode = _truncateExistingFile ? FileMode.Create : FileMode.Append;
        using var stream = File.Open(_filePath, fileMode, FileAccess.Write, FileShare.None);
        using var writer = new StreamWriter(stream, _fileEncoding);

        using var statement = _notebook.Prepare(sql);
        var args = statement.GetArgs(_env.Vars);
        statement.ExecuteStream(args, OnHeader, OnRow, CancellationToken.None);

        void OnHeader(List<string> columnNames) {
            if (_headerRow) {
                writer.WriteLine(string.Join(_separator, columnNames.Select(c => CsvUtil.QuoteCsv(c, _separator))));
            }
        }

        void OnRow(object[] row) {
            _cancel.ThrowIfCancellationRequested();
            CsvUtil.WriteCsvLine(writer, _separator, row);
            status.IncrementRows();
        }
    }

    private string GetFilePath() {
        var filePath = _runner.EvaluateExpr<string>(_stmt.FilenameExpr, _env);
        var dirPath = Path.GetDirectoryName(filePath);
        if (Directory.Exists(dirPath)) {
            return filePath;
        } else {
            throw new Exception($"EXPORT CSV: The output folder was not found: \"{dirPath}\"");
        }
    }
}
