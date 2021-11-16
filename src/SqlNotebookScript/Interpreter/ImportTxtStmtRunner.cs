using System;
using System.IO;
using System.Linq;
using System.Text;
using SqlNotebookScript.Core;
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.Interpreter;

public sealed class ImportTxtStmtRunner {
    private readonly Notebook _notebook;
    private readonly ScriptEnv _env;
    private readonly ScriptRunner _runner;
    private readonly Ast.ImportTxtStmt _stmt;

    // arguments
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
    public static void Run(Notebook notebook, ScriptEnv env, ScriptRunner runner, Ast.ImportTxtStmt stmt) {
        var importer = new ImportTxtStmtRunner(notebook, env, runner, stmt);
        SqlUtil.WithTransaction(notebook, importer.Import);
    }

    private ImportTxtStmtRunner(Notebook notebook, ScriptEnv env, ScriptRunner runner, Ast.ImportTxtStmt stmt) {
        _notebook = notebook;
        _env = env;
        _runner = runner;
        _stmt = stmt;

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
                    if (_takeLines == -1) {
                        _takeLines = null;
                    }
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
        using var stream = File.OpenRead(GetFilePath());
        using var reader = NewReader(stream);

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
            args[0] = i + 1;
            args[1] = reader.ReadLine();
            _notebook.Execute(insertSql, args);
        }
    }

    private StreamReader NewReader(Stream fileStream) {
        var encoding = _fileEncoding;

        // Detect the presence of a UTF-8 BOM and arrange to handle it if this is an automatic or UTF-8 encoding.
        var isUtf8 =
            (encoding?.CodePage ?? 0) switch {
                0 => true,
                65001 => true,
                _ => false,
            };
        if (isUtf8) {
            return new (fileStream, Encoding.UTF8, true);
        }

        return _fileEncoding == null
            ? new(fileStream)
            : new(fileStream, _fileEncoding, false);
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
