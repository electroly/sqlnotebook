namespace SqlNotebookScript.Interpreter;

public static class SqliteParser {
    public struct Result {
        public readonly bool IsValid;
        public readonly int NumValidTokens;

        // if invalid
        public readonly string InvalidMessage;

        public Result(int numTokens) {
            IsValid = true;
            NumValidTokens = numTokens;
            InvalidMessage = null;
        }

        public Result(string message, int numValidTokens) {
            IsValid = false;
            NumValidTokens = numValidTokens;
            InvalidMessage = message;
        }
    }

    public static Result ReadExpr(TokenQueue q, out Ast.SqliteSyntaxProduction ast) {
        var startingLocation = q.GetLocation();
        var matchResult = Matcher.Match("expr", q, out ast);
        var numTokens = q.GetLocation() - startingLocation;
        if (matchResult.IsMatch) {
            return new Result(numTokens);
        } else {
            return new Result(matchResult.ErrorMessage ?? "Not an expression.", numTokens);
        }
    }

    public static Result ReadStmt(TokenQueue q, out Ast.SqliteSyntaxProduction ast, string rootProdName = "sql-stmt") {
        var startingLocation = q.GetLocation();
        var matchResult = Matcher.Match(rootProdName, q, out ast);
        var numTokens = q.GetLocation() - startingLocation;
        if (matchResult.IsMatch) {
            return new Result(numTokens);
        } else {
            return new Result(matchResult.ErrorMessage ?? "Not a statement.", numTokens);
        }
    }
}
