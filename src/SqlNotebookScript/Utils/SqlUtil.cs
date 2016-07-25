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
using SqlNotebookScript.Interpreter;
using Ast = SqlNotebookScript.Interpreter.Ast;

namespace SqlNotebookScript.Utils {
    public static class SqlUtil {
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
