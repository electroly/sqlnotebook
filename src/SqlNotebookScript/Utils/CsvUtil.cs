// SQL Notebook
// Copyright (C) 2018 Brian Luft
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

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SqlNotebookScript.Utils {
    public static class CsvUtil {
        public static void WriteCsv(IEnumerable<object[]> rows, StreamWriter writer) {
            foreach (var row in rows) {
                var first = true;
                foreach (var value in row) {
                    if (first) {
                        first = false;
                    } else {
                        writer.Write(",");
                    }
                    writer.Write(EscapeCsv(value));
                }
                writer.WriteLine("");
            }
        }

        public static string EscapeCsv(object val) {
            var str = val?.ToString() ?? "";
            if (StringRequiresEscape(str)) {
                return $"\"{str.Replace("\"", "\"\"")}\"";
            } else {
                return str;
            }
        }

        public static bool StringRequiresEscape(string str) {
            if (str == "") {
                return false;
            }

            return
                char.IsWhiteSpace(str[0]) ||
                char.IsWhiteSpace(str[str.Length - 1]) ||
                (str.Length > 1 && str[0] == '0') ||
                str.Any(CharRequiresEscape);
        }

        public static bool CharRequiresEscape(char ch) =>
            ch == '"' || ch == '\'' || ch == ',' || ch == '\r' || ch == '\n' || ch == '\t';
    }
}
