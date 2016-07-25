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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlNotebookScript;

namespace SqlNotebookScript.Interpreter {
    public static class Util {
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

        public static IEnumerable<string> GetOptionKeys(this Ast.OptionsList optionsList) {
            if (optionsList != null) {
                return optionsList.Options.Keys.Select(x => x.ToUpper());
            } else {
                return new string[0];
            }
        }

        public static T GetOption<T>(this Ast.OptionsList optionsList, string name, ScriptRunner runner, ScriptEnv env,
        T defaultValue) {
            Ast.Expr valueExpr;
            if (optionsList != null && optionsList.Options.TryGetValue(name.ToLower(), out valueExpr)) {
                return runner.EvaluateExpr<T>(valueExpr, env);
            } else {
                return defaultValue;
            }
        }

        public static bool GetOptionBool(this Ast.OptionsList optionsList, string name, ScriptRunner runner,
        ScriptEnv env, bool defaultValue) {
            long num = optionsList.GetOption<long>(name, runner, env, defaultValue ? 1 : 0);
            if (num != 0 && num != 1) {
                throw new Exception($"{name} must be 0 or 1.");
            } else {
                return num == 1;
            }
        }

        public static Encoding GetOptionEncoding(this Ast.OptionsList optionsList, string name, ScriptRunner runner,
        ScriptEnv env) {
            var encodingNum = optionsList.GetOption<long>(name, runner, env, 0);
            if (encodingNum == 0) {
                return null;
            } else if (encodingNum < 0 || encodingNum > 65535) {
                throw new Exception($"{name} must be between 0 and 65535.");
            } else {
                Encoding encoding = null;
                try {
                    encoding = Encoding.GetEncoding((int)encodingNum);
                } catch (ArgumentOutOfRangeException) {
                    // invalid codepage
                } catch (ArgumentException) {
                    // invalid codepage
                } catch (NotSupportedException) {
                    // invalid codepage
                }

                if (encoding == null) {
                    throw new Exception($"The code page {encodingNum} is invalid or is not supported on this system.");
                } else {
                    return encoding;
                }
            }
        }

        public static long GetOptionLong(this Ast.OptionsList optionsList, string name, ScriptRunner runner,
        ScriptEnv env, long defaultValue, long? minValue = null, long? maxValue = null) {
            var num = optionsList.GetOption<long>(name, runner, env, defaultValue);
            bool outOfRange =
                (minValue.HasValue && num < minValue.Value) ||
                (maxValue.HasValue && num > maxValue.Value);
            if (outOfRange) {
                if (minValue.HasValue && !maxValue.HasValue) {
                    throw new Exception($"{name} must be an integer ≥ {minValue.Value}.");
                } else if (minValue.HasValue && maxValue.HasValue) {
                    throw new Exception($"{name} must be an integer between {minValue.Value} and {maxValue.Value}.");
                } else if (!minValue.HasValue && maxValue.HasValue) {
                    throw new Exception($"{name} must be an integer ≤ {maxValue.Value}.");
                }
            }
            return num;
        }

        public static string GetInsertSql(string tableName, int numValues, int numRows = 1) {
            var row = $"({string.Join(", ", Enumerable.Range(1, numValues).Select(x => $"?"))})";
            return $"INSERT INTO {tableName.DoubleQuote()} VALUES " + string.Join(", ", Enumerable.Range(0, numRows).Select(x => row));
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
    }
}
