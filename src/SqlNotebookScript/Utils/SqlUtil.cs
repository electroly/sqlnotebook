using System;
using System.Collections.Generic;
using System.Linq;
using SqlNotebookScript.Interpreter;
using Ast = SqlNotebookScript.Interpreter.Ast;

namespace SqlNotebookScript.Utils {
    public static class SqlUtil {
        public static string EscapeAmpersand(this string str) => str.Replace("&", "&&");

        public static string DoubleQuote(this string str) {
            return $"\"{str.Replace("\"", "\"\"")}\"";
        }

        public static string SingleQuote(this string str) {
            return $"'{str.Replace("'", "''")}'";
        }

        public static int? IndexOf(this IReadOnlyList<string> haystack, string needle) {
            var n = haystack.Count;
            for (int i = 0; i < n; i++) {
                if (haystack[i] == needle) {
                    return i;
                }
            }
            return null;
        }

        public static void Import(string[] srcColNames, IEnumerable<object[]> dataRows, Ast.ImportTable importTable,
        bool temporaryTable, bool truncateExistingTable, IfConversionFails ifConversionFails, INotebook notebook,
        ScriptRunner runner, ScriptEnv env) {
            Ast.ImportColumn[] dstColNodes;
            string[] dstColNames;

            var dstTableName = runner.EvaluateIdentifierOrExpr(importTable.TableName, env);
            GetDestinationColumns(importTable.ImportColumns, runner, env, srcColNames,
                out dstColNodes, out dstColNames);
            CreateOrTruncateTable(srcColNames, dstColNodes, dstColNames, dstTableName, temporaryTable,
                truncateExistingTable, notebook);
            VerifyColumnsExist(dstColNames, dstTableName, notebook);
            InsertDataRows(dataRows, dstColNames, dstColNodes, dstTableName, ifConversionFails, notebook);
        }

        public static string GetInsertSql(string tableName, int numValues, int numRows = 1) {
            var row = $"({string.Join(", ", Enumerable.Range(1, numValues).Select(x => $"?"))})";
            return $"INSERT INTO {tableName.DoubleQuote()} VALUES " + 
                string.Join(", ", Enumerable.Range(0, numRows).Select(x => row));
        }

        public static void VerifyColumnsExist(string[] colNames, string tableName, INotebook notebook) {
            var tableInfo = notebook.Query($"PRAGMA TABLE_INFO ({tableName.DoubleQuote()})");
            var nameColIndex = tableInfo.GetIndex("name");
            var actualColNames = tableInfo.Rows.Select(x => x[nameColIndex].ToString().ToLower()).ToList();
            foreach (var name in colNames.Where(x => x != null)) {
                if (!actualColNames.Contains(name.ToLower())) {
                    throw new Exception($"The table \"{tableName}\" does not contain the column \"{name}\".");
                }
            }
        }

        public static string GetErrorMessage(this Exception self) {
            var uncaught = self as UncaughtErrorScriptException;
            if (uncaught != null) {
                return uncaught.ErrorMessage.ToString();
            } else {
                return self.Message;
            }
        }

        public static bool TryParseValue(object rawValue, Ast.TypeConversion conversion, out object parsedValue) {
            parsedValue = null;
            if (rawValue == null) {
                return true;
            }
            switch (conversion) {
                case Ast.TypeConversion.Text:
                    parsedValue = rawValue.ToString();
                    return true;

                case Ast.TypeConversion.Integer:
                    if (rawValue is int || rawValue is long || rawValue is string) {
                        parsedValue = Convert.ToInt64(rawValue);
                        return true;
                    } else {
                        return false;
                    }

                case Ast.TypeConversion.Real:
                    if (rawValue is int || rawValue is long || rawValue is double || rawValue is string) {
                        parsedValue = Convert.ToDouble(rawValue);
                        return true;
                    } else {
                        return false;
                    }

                case Ast.TypeConversion.Date:
                    DateTime dateValue;
                    if (DateTime.TryParse(rawValue.ToString(), out dateValue)) {
                        parsedValue = DateTimeUtil.FormatDate(dateValue.Date);
                        return true;
                    } else {
                        return false;
                    }

                case Ast.TypeConversion.DateTime:
                    DateTime dateTimeValue;
                    if (DateTime.TryParse(rawValue.ToString(), out dateTimeValue)) {
                        parsedValue = DateTimeUtil.FormatDateTime(dateTimeValue);
                        return true;
                    } else {
                        return false;
                    }

                case Ast.TypeConversion.DateTimeOffset:
                    DateTimeOffset dateTimeOffsetValue;
                    if (DateTimeOffset.TryParse(rawValue.ToString(), out dateTimeOffsetValue)) {
                        parsedValue = DateTimeUtil.FormatDateTimeOffset(dateTimeOffsetValue);
                        return true;
                    } else {
                        return false;
                    }

                default:
                    throw new Exception($"Internal error: unknown type conversion \"{conversion}\".");

            }
        }

        public static void GetDestinationColumns(IEnumerable<Ast.ImportColumn> importColumns, ScriptRunner runner,
        ScriptEnv env, IReadOnlyList<string> srcColNames, out Ast.ImportColumn[] dstColNodes,
        out string[] dstColNames) {
            // if there is no column list specified, then all columns are imported with default settings.
            if (!importColumns.Any()) {
                importColumns =
                    srcColNames.Select(x => new Ast.ImportColumn {
                        ColumnName = new Ast.IdentifierOrExpr { Identifier = x },
                        TypeConversion = Ast.TypeConversion.Text
                    }).ToList();
            }

            var lowercaseSrcColNames = srcColNames.Select(x => x.ToLower()).ToArray();
            var lowercaseDstColNames = new HashSet<string>(); // to ensure we don't have duplicate column names
            dstColNodes = new Ast.ImportColumn[srcColNames.Count];
            dstColNames = new string[srcColNames.Count];
            foreach (var importCol in importColumns) {
                var name = runner.EvaluateIdentifierOrExpr(importCol.ColumnName, env);

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
                        $"The columns that were found are: {string.Join(", ", srcColNames.Select(DoubleQuote))}");
                }

                // apply the user's rename if specified
                string dstName;
                if (importCol.AsName != null) {
                    dstName = runner.EvaluateIdentifierOrExpr(importCol.AsName, env);
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

        public static void CreateOrTruncateTable(IReadOnlyList<string> srcColNames, Ast.ImportColumn[] dstColNodes,
        IReadOnlyList<string> dstColNames, string dstTableName, bool temporaryTable, bool truncateExistingTable, 
        INotebook notebook) {
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
            var sql = $"CREATE {(temporaryTable ? "TEMPORARY" : "")} TABLE IF NOT EXISTS " +
                $"{dstTableName.DoubleQuote()} ({columnDefsList})";
            notebook.Execute(sql);

            if (truncateExistingTable) {
                notebook.Execute($"DELETE FROM {dstTableName.DoubleQuote()}");
            }
        }

        public static void InsertDataRows(IEnumerable<object[]> rows, string[] dstColNames,
        Ast.ImportColumn[] dstColNodes, string dstTableName, IfConversionFails ifConversionFails, INotebook notebook) {
            var dstColCount = dstColNames.Where(x => x != null).Count();
            var insertSql = GetInsertSql(dstTableName, dstColCount, numRows: 1);
            var insertArgs = new List<object>();
            foreach (var row in rows) {
                insertArgs.Clear();
                bool skipRow = false;
                for (int j = 0; j < Math.Min(row.Length, dstColNames.Length); j++) {
                    if (dstColNames[j] == null) {
                        continue; // column not chosen to be imported
                    }
                    var originalValue = row[j];
                    var typeConversion = dstColNodes[j].TypeConversion ?? Ast.TypeConversion.Text;
                    object converted;
                    bool error = !TryParseValue(originalValue, typeConversion, out converted);

                    if (!error) {
                        insertArgs.Add(converted);
                    } else if (ifConversionFails == IfConversionFails.ImportAsText) {
                        insertArgs.Add(originalValue.ToString());
                    } else if (ifConversionFails == IfConversionFails.SkipRow) {
                        skipRow = true;
                    } else if (ifConversionFails == IfConversionFails.Abort) {
                        throw new Exception($"Failed to parse input value as type \"{typeConversion}\". Value: \"{originalValue}\".");
                    } else {
                        throw new Exception($"Internal error: unknown value for IF_CONVERSION_FAILS: \"{ifConversionFails}\".");
                    }
                }
                if (!skipRow) {
                    while (insertArgs.Count < dstColCount) {
                        insertArgs.Add(null);
                    }
                    notebook.Execute(insertSql, insertArgs);
                }
            }
        }

        public static void WithTransaction(INotebook notebook, Action action) =>
            WithTransactionCore(notebook, action, rollback: false);
 
        public static void WithRollbackTransaction(INotebook notebook, Action action) =>
            WithTransactionCore(notebook, action, rollback: true);

        private static void WithTransactionCore(INotebook notebook, Action action, bool rollback) {
            var didBeginTransaction = false;
            if (!notebook.IsTransactionActive()) {
                notebook.Execute("BEGIN");
                didBeginTransaction = true;
            }
            try {
                action();
                if (didBeginTransaction) {
                    notebook.Execute(rollback ? "ROLLBACK" : "COMMIT");
                }
            } catch {
                if (didBeginTransaction) {
                    notebook.Execute("ROLLBACK");
                }
                throw;
            }
        }

        public static T WithTransaction<T>(INotebook notebook, Func<T> func) =>
            WithTransactionCore<T>(notebook, func, rollback: false);

        public static T WithRollbackTransaction<T>(INotebook notebook, Func<T> func) =>
            WithTransactionCore<T>(notebook, func, rollback: true);

        private static T WithTransactionCore<T>(INotebook notebook, Func<T> func, bool rollback) {
            var value = default(T);
            WithTransactionCore(notebook, () => {
                value = func();
            }, rollback);
            return value;
        }
    }

    public enum IfConversionFails {
        ImportAsText = 1,
        SkipRow = 2,
        Abort = 3
    }
}
