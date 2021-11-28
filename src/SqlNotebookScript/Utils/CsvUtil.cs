using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace SqlNotebookScript.Utils;

public static class CsvUtil {
    public static void WriteCsv(
        IEnumerable<object[]> rows, StreamWriter writer, Action onRow = null, char separator = ',',
        CancellationToken cancel = default
        ) {
        foreach (var row in rows) {
            cancel.ThrowIfCancellationRequested();
            WriteCsvLine(writer, separator, row);
            onRow?.Invoke();
        }
    }

    public static void WriteCsvLine(StreamWriter writer, char separator, object[] row) {
        var first = true;
        foreach (var value in row) {
            if (first) {
                first = false;
            } else {
                writer.Write(separator);
            }
            writer.Write(QuoteCsv(value, separator));
        }
        writer.WriteLine("");
    }

    public static string QuoteCsv(object val, char separator) {
        var str = val?.ToString() ?? "";
        if (StringRequiresEscape(str, separator)) {
            return $"\"{str.Replace("\"", "\"\"")}\"";
        } else {
            return str;
        }
    }

    private static bool StringRequiresEscape(string str, char separator) {
        var len = str.Length;
        if (len == 0) {
            return false;
        }
        if (char.IsWhiteSpace(str[0])) {
            return true;
        }
        if (char.IsWhiteSpace(str[^1])) {
            return true;
        }
        if (str[0] == '0') {
            return true;
        }

        for (var i = 0; i < len; i++) {
            var ch = str[i];
            if (ch == '"' || ch == '\'' || ch == separator || ch == '\r' || ch == '\n' || ch == '\t') {
                return true;
            }
        }

        return false;
    }
}
