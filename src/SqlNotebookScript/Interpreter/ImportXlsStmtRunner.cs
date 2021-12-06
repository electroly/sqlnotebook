using System;
using System.IO;
using System.Linq;
using SqlNotebookScript.Core;
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.Interpreter;

public sealed class ImportXlsStmtRunner {
    private readonly Notebook _notebook;
    private readonly ScriptEnv _env;
    private readonly ScriptRunner _runner;
    private readonly Ast.ImportXlsStmt _stmt;

    private readonly string _filePath;
    private readonly object _whichSheet; // 1-based number or string name

    // option values
    private readonly int _firstRowIndex = 0; // 0-based index
    private readonly int? _lastRowIndex = null; // 0-based index
    private readonly int _firstColumnIndex = 0; // 0-based index
    private readonly int? _lastColumnIndex = null; // 0-based index
    private readonly bool _headerRow = true;
    private readonly bool _truncateExistingTable = false;
    private readonly bool _temporaryTable = false;
    private readonly bool _stopAtFirstBlankRow = true;
    private readonly IfConversionFails _ifConversionFails = IfConversionFails.ImportAsText;

    public static void Run(Notebook notebook, ScriptEnv env, ScriptRunner runner, Ast.ImportXlsStmt stmt) {
        var importer = new ImportXlsStmtRunner(notebook, env, runner, stmt);
        SqlUtil.WithTransaction(notebook, importer.Import);
    }

    private ImportXlsStmtRunner(Notebook notebook, ScriptEnv env, ScriptRunner runner, Ast.ImportXlsStmt stmt) {
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
                    var lastRowNum = _stmt.OptionsList.GetOptionInt(option, _runner, _env, 0, minValue: 0);
                    if (lastRowNum == 0) {
                        _lastRowIndex = null;
                    } else {
                        _lastRowIndex = lastRowNum - 1;
                    }
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
                    var lastColumnValue = _stmt.OptionsList.GetOption<object>(option, _runner, _env, null);
                    if (lastColumnValue is long b && b == 0) {
                        _lastColumnIndex = null;
                        break;
                    }
                    index = XlsUtil.ColumnRefToIndex(lastColumnValue);
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

                case "STOP_AT_FIRST_BLANK_ROW":
                    _stopAtFirstBlankRow = _stmt.OptionsList.GetOptionBool(option, _runner, _env, true);
                    break;

                default:
                    throw new Exception($"\"{option}\" is not a recognized option name.");
            }
        }
    }

    private void Import() {
        XlsUtil.WithWorkbook(_filePath, workbook => {
            workbook.SeekToWorksheet(GetSheetIndex(workbook));
            var rows = workbook.ReadSheet(_firstRowIndex, _lastRowIndex, _firstColumnIndex, _lastColumnIndex);

            SqlUtil.Import(
                srcColNames: XlsUtil.ReadColumnNames(rows, _headerRow), 
                dataRows: rows.Skip(_headerRow ? 1 : 0),
                importTable: _stmt.ImportTable, 
                temporaryTable: _temporaryTable, 
                truncateExistingTable: _truncateExistingTable, 
                stopAtFirstBlankRow: _stopAtFirstBlankRow,
                ifConversionFails: _ifConversionFails, 
                notebook: _notebook, 
                runner: _runner,
                env: _env);
        });
    }

    private int GetSheetIndex(XlsUtil.IWorkbook workbook) {
        var whichSheet = _whichSheet ?? 1; // 1-based number or name string
        if (whichSheet is int || whichSheet is long) {
            var whichSheetNum = Convert.ToInt32(whichSheet);
            if (whichSheetNum < 1) {
                throw new Exception($"The worksheet number must be at least 1.");
            }
            return whichSheetNum - 1;
        } else if (whichSheet is string name) {
            var names = workbook.ReadWorksheetNames();
            var index = names.IndexOf(name);
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
