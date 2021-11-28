using System.Linq;

namespace SqlNotebookScript.Interpreter;

public static class ScriptParserExtensions {
    public static string GetUnescapedText(this Token token) {
        var x = token.Text;
        if (x == "") {
            return x;
        } else if (x.First() == '"' && x.Last() == '"') {
            return x.Substring(1, x.Length - 2).Replace("\"\"", "\"");
        } else if (x.First() == '\'' && x.Last() == '\'') {
            return x.Substring(1, x.Length - 2).Replace("''", "'");
        } else if (x.First() == '`' && x.Last() == '`') {
            return x.Substring(1, x.Length - 2).Replace("``", "`");
        } else if (x.First() == '[' && x.Last() == ']') {
            return x.Substring(1, x.Length - 2).Replace("]]", "]");
        } else {
            return x;
        }
    }
}
