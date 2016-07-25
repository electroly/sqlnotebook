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
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using SqlNotebookScript;

namespace SqlNotebookScript.Interpreter {
    public sealed class SyntaxException : Exception {
        public SyntaxException(IEnumerable<string> expecteds, TokenQueue q)
            : base($"Syntax error at \"{q.GetSnippet()}\".  Expected: {string.Join(", ", expecteds)}") { }
        public SyntaxException(IEnumerable<string> expecteds, List<Token> tokens)
            : base($"Syntax error at \"{string.Join(" ", tokens.Select(x => x.Text).Take(5)).Trim()}\".  Expected: {string.Join(", ", expecteds)}") { }
        public SyntaxException(TokenQueue q) : base($"Syntax error at \"{q.GetSnippet()}\"") { }
        public SyntaxException(List<Token> tokens) : base($"Syntax error at \"{string.Join(" ", tokens.Select(x => x.Text).Take(5)).Trim()}\"") { }
        public SyntaxException(string message) : base(message) { }
    }

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

    public sealed class ScriptParser {
        private readonly INotebook _notebook;

        public ScriptParser(INotebook notebook) {
            _notebook = notebook;
        }

        public Ast.Script Parse(string input) {
            var tokens = new TokenQueue(_notebook.Tokenize(input), _notebook);
            return ParseScript(tokens);
        }

        private static Ast.Script ParseScript(TokenQueue q) {
            var script = new Ast.Script { SourceToken = q.SourceToken };
            script.Block = new Ast.Block { SourceToken = q.SourceToken };
            while (!q.Eof()) {
                var stmt = ParseStmt(q);
                if (stmt != null) {
                    script.Block.Statements.Add(stmt);
                }
            }
            return script;
        }

        private static Ast.Stmt ParseStmt(TokenQueue q) { // or null
            switch (q.Peek(0)) {
                case ";": q.Take(";"); return null;
                case "declare": return ParseDeclareStmt(q);
                case "while": return ParseWhileStmt(q);
                case "break": return ParseBreakStmt(q);
                case "continue": return ParseContinueStmt(q);
                case "print": return ParsePrintStmt(q);
                case "exec": case "execute": return ParseExecuteStmt(q);
                case "return": return ParseReturnStmt(q);
                case "throw": return ParseThrowStmt(q);
                case "set": return ParseSetStmt(q);
                case "if": return ParseIfStmt(q);
                case "begin": return q.Peek(1) == "try" ? ParseTryCatchStmt(q) : ParseSqlStmt(q);
                case "import": return ParseImportStmt(q);
                case "export": return ParseExportStmt(q);
                default: return ParseSqlStmt(q);
            }
        }

        private static Ast.DeclareStmt ParseDeclareStmt(TokenQueue q) {
            var stmt = new Ast.DeclareStmt { SourceToken = q.SourceToken };
            q.Take("declare");
            if (q.Peek() == "parameter") {
                q.Take();
                stmt.IsParameter = true;
            }
            ParseAssignmentStmtCore(q, stmt);
            return stmt;
        }

        private static Ast.Stmt ParseSetStmt(TokenQueue q) {
            var stmt = new Ast.SetStmt { SourceToken = q.SourceToken };
            q.Take("set");
            ParseAssignmentStmtCore(q, stmt);
            return stmt;
        }

        private static void ParseAssignmentStmtCore(TokenQueue q, Ast.AssignmentStmt stmt) {
            stmt.VariableName = ParseVariableName(q);
            if (q.Peek() == "=") {
                q.Take();
                stmt.InitialValue = ParseExpr(q);
            }
            ConsumeSemicolon(q);
        }

        private static Ast.Stmt ParseWhileStmt(TokenQueue q) {
            var stmt = new Ast.WhileStmt { SourceToken = q.SourceToken };
            q.Take("while");
            stmt.Condition = ParseExpr(q);
            stmt.Block = ParseBlock(q);
            ConsumeSemicolon(q);
            return stmt;
        }

        private static Ast.Stmt ParseBreakStmt(TokenQueue q) {
            var stmt = new Ast.BreakStmt { SourceToken = q.SourceToken };
            q.Take("break");
            ConsumeSemicolon(q);
            return stmt;
        }

        private static Ast.Stmt ParseContinueStmt(TokenQueue q) {
            var stmt = new Ast.ContinueStmt { SourceToken = q.SourceToken };
            q.Take("continue");
            ConsumeSemicolon(q);
            return stmt;
        }

        private static Ast.Stmt ParsePrintStmt(TokenQueue q) {
            var stmt = new Ast.PrintStmt { SourceToken = q.SourceToken };
            q.Take("print");
            stmt.Value = ParseExpr(q);
            ConsumeSemicolon(q);
            return stmt;
        }

        private static Ast.Stmt ParseExecuteStmt(TokenQueue q) {
            var stmt = new Ast.ExecuteStmt { SourceToken = q.SourceToken };
            q.Take("exec", "execute");

            if (q.Peek(1) == "=") {
                stmt.ReturnVariableName = ParseVariableName(q);
                q.Take("=");
            }

            if (q.PeekToken().Type == TokenType.String || q.PeekToken().Type == TokenType.Id) {
                stmt.ScriptName = q.Take().GetUnescapedText();
            } else {
                throw new SyntaxException(new[] { "string", "identifier" }, q);
            }

            while (IsVariableName(q.PeekToken()?.GetUnescapedText() ?? "") && q.Peek(1) == "=") {
                var arg = new Ast.ArgumentPair();
                arg.Name = ParseVariableName(q);
                q.Take("=");
                if (q.Peek() == "default") {
                    q.Take();
                } else {
                    arg.Value = ParseExpr(q);
                }
                stmt.Arguments.Add(arg);
            }

            ConsumeSemicolon(q);
            return stmt;
        }

        private static Ast.Stmt ParseReturnStmt(TokenQueue q) {
            var stmt = new Ast.ReturnStmt { SourceToken = q.SourceToken };
            q.Take("return");
            if (PeekExpr(q)) {
                stmt.Value = ParseExpr(q);
            }
            ConsumeSemicolon(q);
            return stmt;
        }

        private static Ast.Stmt ParseThrowStmt(TokenQueue q) {
            var stmt = new Ast.ThrowStmt { SourceToken = q.SourceToken };
            q.Take("throw");
            if (PeekExpr(q)) {
                stmt.ErrorNumber = ParseExpr(q);
                q.Take(",");
                stmt.Message = ParseExpr(q);
                q.Take(",");
                stmt.State = ParseExpr(q);
            }
            ConsumeSemicolon(q);
            return stmt;
        }

        private static Ast.Stmt ParseIfStmt(TokenQueue q) {
            var stmt = new Ast.IfStmt { SourceToken = q.SourceToken };
            q.Take("if");
            stmt.Condition = ParseExpr(q);
            stmt.Block = ParseBlock(q);
            if (q.Peek() == "else") {
                q.Take("else");
                stmt.ElseBlock = ParseBlock(q);
            }
            return stmt;
        }

        private static Ast.Stmt ParseTryCatchStmt(TokenQueue q) {
            var stmt = new Ast.TryCatchStmt { SourceToken = q.SourceToken };

            q.Take("begin");
            q.Take("try");
            stmt.TryBlock = new Ast.Block { SourceToken = q.SourceToken };
            while (q.Peek() != "end") {
                var tryStmt = ParseStmt(q);
                if (tryStmt != null) {
                    stmt.TryBlock.Statements.Add(tryStmt);
                }
            }
            q.Take("end");
            q.Take("try");

            q.Take("begin");
            q.Take("catch");
            stmt.CatchBlock = new Ast.Block { SourceToken = q.SourceToken };
            while (q.Peek() != "end") {
                var catchStmt = ParseStmt(q);
                if (catchStmt != null) {
                    stmt.CatchBlock.Statements.Add(catchStmt);
                }
            }
            q.Take("end");
            q.Take("catch");
            return stmt;
        }

        private static Ast.Stmt ParseImportStmt(TokenQueue q) {
            switch (q.Peek(1)) {
                case "csv": return Check(q, ParseImportCsvStmt(q));
                case "txt": case "text": return Check(q, ParseImportTxtStmt(q));
                case "xls": case "xlsx": return Check(q, ParseImportXlsStmt(q));
                default: throw new SyntaxException($"Unknown import type: \"{q.Peek(1)}\"");
            }
        }

        private static Ast.Stmt ParseExportStmt(TokenQueue q) {
            switch (q.Peek(1)) {
                case "txt": case "text": return Check(q, ParseExportTxtStmt(q));
                default: throw new SyntaxException($"Unknown export type: \"{q.Peek(1)}\"");
            }
        }

        private static Ast.ImportCsvStmt ParseImportCsvStmt(TokenQueue q) { // or null
            var stmt = new Ast.ImportCsvStmt { SourceToken = q.SourceToken };
            var start = q.GetLocation();
            if (!q.TakeMaybe("import")) { q.Jump(start); return null; }
            if (!q.TakeMaybe("csv")) { q.Jump(start); return null; }
            stmt.FilenameExpr = ParseExpr(q);
            q.Take("into");
            stmt.ImportTable = Check(q, ParseImportTable(q));
            if (q.TakeMaybe("options")) {
                stmt.OptionsList = Check(q, ParseOptionsList(q));
            }
            ConsumeSemicolon(q);
            return stmt;
        }

        private static Ast.ImportXlsStmt ParseImportXlsStmt(TokenQueue q) { // or null
            var stmt = new Ast.ImportXlsStmt { SourceToken = q.SourceToken };
            var start = q.GetLocation();
            if (!q.TakeMaybe("import")) { q.Jump(start); return null; }
            if (!q.TakeMaybe("xls", "xlsx")) { q.Jump(start); return null; }
            stmt.FilenameExpr = ParseExpr(q);
            if (q.TakeMaybe("worksheet")) {
                stmt.WhichSheetExpr = ParseExpr(q);
            }
            q.Take("into");
            stmt.ImportTable = Check(q, ParseImportTable(q));
            if (q.TakeMaybe("options")) {
                stmt.OptionsList = Check(q, ParseOptionsList(q));
            }
            ConsumeSemicolon(q);
            return stmt;
        }

        private static Ast.ImportTable ParseImportTable(TokenQueue q) {
            var n = new Ast.ImportTable { SourceToken = q.SourceToken };
            n.TableName = Check(q, ParseIdentifierOrExpr(q));
            if (q.Peek() == "(") {
                q.Take("(");
                do {
                    n.ImportColumns.Add(Check(q, ParseImportColumn(q)));
                } while (q.TakeMaybe(","));
                q.Take(")");
            }
            return n;
        }

        private static Ast.ImportColumn ParseImportColumn(TokenQueue q) {
            var n = new Ast.ImportColumn { SourceToken = q.SourceToken };
            Check(q, n.ColumnName = ParseIdentifierOrExpr(q));
            if (q.Peek() == "as") {
                q.Take("as");
                n.AsName = Check(q, ParseIdentifierOrExpr(q));
            }
            switch (q.Peek()) {
                case "text": q.Take(); n.TypeConversion = Ast.TypeConversion.Text; break;
                case "integer": q.Take(); n.TypeConversion = Ast.TypeConversion.Integer; break;
                case "real": q.Take(); n.TypeConversion = Ast.TypeConversion.Real; break;
                case "date": q.Take(); n.TypeConversion = Ast.TypeConversion.Date; break;
                case "datetime": q.Take(); n.TypeConversion = Ast.TypeConversion.DateTime; break;
                case "datetimeoffset": q.Take(); n.TypeConversion = Ast.TypeConversion.DateTimeOffset; break;
            }
            return n;
        }

        private static Ast.OptionsList ParseOptionsList(TokenQueue q) { // or null
            var n = new Ast.OptionsList { SourceToken = q.SourceToken };
            if (!q.TakeMaybe("(")) {
                return null;
            }
            do {
                var key = Check(q, ParseIdentifier(q)).ToLower();
                q.Take(":");
                var value = ParseExpr(q);
                n.Options[key] = value;
            } while (q.TakeMaybe(","));
            q.Take(")");
            return n;
        }

        private static Ast.IdentifierOrExpr ParseIdentifierOrExpr(TokenQueue q) { // or null
            var token = q.PeekToken();
            if (token.Type == TokenType.Id) {
                q.Take();
                return new Ast.IdentifierOrExpr { SourceToken = token, Identifier = token.GetUnescapedText() };
            } else if (PeekExpr(q)) {
                return new Ast.IdentifierOrExpr { SourceToken = token, Expr = ParseExpr(q) };
            } else {
                return null;
            }
        }

        private static string ParseIdentifier(TokenQueue q) { // or null
            var token = q.PeekToken();
            if (token.Type == TokenType.Id) {
                return q.Take().GetUnescapedText();
            } else {
                return null;
            }
        }

        private static Ast.ImportTxtStmt ParseImportTxtStmt(TokenQueue q) { // or null
            var stmt = new Ast.ImportTxtStmt { SourceToken = q.SourceToken };
            var start = q.GetLocation();
            if (!q.TakeMaybe("import")) { q.Jump(start); return null; }
            if (!q.TakeMaybe("txt", "text")) { q.Jump(start); return null; }
            stmt.FilenameExpr = ParseExpr(q);
            q.Take("into");
            stmt.TableName = Check(q, ParseIdentifierOrExpr(q));
            if (q.Peek() == "(") {
                q.Take("(");
                stmt.LineNumberColumnName = Check(q, ParseIdentifierOrExpr(q));
                q.Take(",");
                stmt.TextColumnName = Check(q, ParseIdentifierOrExpr(q));
                q.Take(")");
            }
            if (q.TakeMaybe("options")) {
                stmt.OptionsList = Check(q, ParseOptionsList(q));
            }
            ConsumeSemicolon(q);
            return stmt;
        }

        private static Ast.ExportTxtStmt ParseExportTxtStmt(TokenQueue q) { // or null
            var stmt = new Ast.ExportTxtStmt { SourceToken = q.SourceToken };
            var start = q.GetLocation();
            if (!q.TakeMaybe("export")) { q.Jump(start); return null; }
            if (!q.TakeMaybe("txt", "text")) { q.Jump(start); return null; }
            stmt.FilenameExpr = ParseExpr(q);
            q.Take("from");
            q.Take("(");
            stmt.SelectStmt = ParseSqlStmt(q, "select-stmt");
            q.Take(")");
            if (q.TakeMaybe("options")) {
                stmt.OptionsList = Check(q, ParseOptionsList(q));
            }
            ConsumeSemicolon(q);
            return stmt;
        }

        // ----

        private static T Check<T>(TokenQueue q, T shouldBeNonNull) {
            if (shouldBeNonNull == null) {
                throw new SyntaxException(q);
            } else {
                return shouldBeNonNull;
            }
        }

        private static string ParseVariableName(TokenQueue q) {
            var t = q.PeekToken();
            if (t.Type == TokenType.Variable && IsVariableName(t.GetUnescapedText())) {
                q.Take();
                return t.GetUnescapedText();
            } else {
                throw new SyntaxException(new[] { "variable name starting with @ $ :" }, q);
            }
        }

        private static readonly Regex _variableNameRegex = new Regex(@"^[:@$][A-Za-z_][A-Za-z0-9_]*$");
        private static bool IsVariableName(string tok) {
            return _variableNameRegex.IsMatch(tok);
        }

        private static Ast.Block ParseBlock(TokenQueue q) {
            var stmt = new Ast.Block { SourceToken = q.SourceToken };
            if (q.Peek() == "begin") {
                q.Take("begin");
                while (q.Peek() != "end") {
                    var blockStmt = ParseStmt(q);
                    if (blockStmt != null) {
                        stmt.Statements.Add(blockStmt);
                    }
                }
                q.Take("end");
            } else {
                var blockStmt = ParseStmt(q);
                if (blockStmt != null) {
                    stmt.Statements.Add(blockStmt);
                }
            }
            return stmt;
        }

        private static Ast.SqlStmt ParseSqlStmt(TokenQueue q, string rootProdName = "sql-stmt") {
            var start = q.GetLocation();
            var tok = q.SourceToken;
            var result = SqlValidator.ReadStmt(q, rootProdName);
            if (result.IsValid) {
                return new Ast.SqlStmt { Sql = q.Substring(start, result.NumValidTokens), SourceToken = tok };
            } else if (result.InvalidMessage != null) {
                throw new SyntaxException(result.InvalidMessage);
            } else {
                throw new SyntaxException(q);
            }
        }

        private static Ast.Expr ParseExpr(TokenQueue q) {
            var start = q.GetLocation();
            var tok = q.SourceToken;
            var result = SqlValidator.ReadExpr(q);
            if (result.IsValid) {
                return new Ast.Expr { Sql = q.Substring(start, result.NumValidTokens), SourceToken = tok };
            } else if (result.InvalidMessage != null) {
                throw new SyntaxException(result.InvalidMessage);
            } else {
                throw new SyntaxException(q);
            }
        }

        private static bool PeekExpr(TokenQueue q) {
            var start = q.GetLocation();
            var tok = q.SourceToken;
            var result = SqlValidator.ReadExpr(q);
            q.Jump(start);
            return result.IsValid;
        }

        private static void ConsumeSemicolon(TokenQueue q) {
            if (q.Peek() == ";") {
                q.Take();
            }
        }
    }
}
