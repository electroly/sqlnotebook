using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SqlNotebookScript.Utils;

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
