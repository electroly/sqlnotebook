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
using SqlNotebookCore;

namespace SqlNotebookScript {
    public sealed class SyntaxException : Exception {
        public SyntaxException(IEnumerable<string> expecteds, Queue<Token> tokens)
            : base($"Syntax error at \"{string.Join(" ", tokens.Select(x => x.Text).Take(5)).Trim()}\".  Expected: {string.Join(", ", expecteds)}") { }
        public SyntaxException(Queue<Token> tokens) : base($"Syntax error at \"{string.Join(" ", tokens.Select(x => x.Text).Take(5)).Trim()}\"") { }
        public SyntaxException(string message) : base(message) { }
    }

    internal sealed class TokenQueue {
        private readonly Queue<Token> _queue;

        public Notebook Notebook { get; }

        public TokenQueue(IEnumerable<Token> input, Notebook notebook) {
            Notebook = notebook;
            _queue = new Queue<Token>(input);
        }

        public Token Take() {
            if (_queue.Any()) {
                return _queue.Dequeue();
            } else {
                throw new SyntaxException("Unexpected end of input.");
            }
        }

        public Token Take(params string[] expectedTexts) {
            var text = Peek();
            if (expectedTexts.Any(x => x.ToLower() == text)) {
                return Take();
            } else {
                throw new SyntaxException(expectedTexts, _queue);
            }
        }

        public string Peek(int skip = 0) {
            return _queue.Skip(skip).FirstOrDefault()?.Text.ToLower() ?? "";
        }

        public Token PeekToken(int skip = 0) {
            return _queue.Skip(skip).FirstOrDefault();
        }

        public bool Eof() {
            return !_queue.Any();
        }

        public override string ToString() {
            return string.Join(" ", _queue.Select(x => x.Text));
        }

        public static implicit operator Queue<Token>(TokenQueue q) {
            return q._queue;
        }
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
        private readonly Notebook _notebook;

        public ScriptParser(Notebook notebook) {
            _notebook = notebook;
        }

        public Ast.Script Parse(string input) {
            var tokens = new TokenQueue(Notebook.Tokenize(input), _notebook);
            return ParseScript(tokens);
        }

        private static Ast.Script ParseScript(TokenQueue q) {
            var script = new Ast.Script();
            while (!q.Eof()) {
                script.Block.Statements.Add(ParseStmt(q));
            }
            return script;
        }

        private static Ast.Stmt ParseStmt(TokenQueue q) {
            switch (q.Peek(0)) {
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
                default: return ParseSqlStmt(q);
            }
        }

        private static Ast.DeclareStmt ParseDeclareStmt(TokenQueue q) {
            q.Take("declare");
            var stmt = new Ast.DeclareStmt();
            if (q.Peek() == "parameter") {
                q.Take();
                stmt.IsParameter = true;
            }
            ParseAssignmentStmtCore(q, stmt);
            return stmt;
        }

        private static Ast.Stmt ParseSetStmt(TokenQueue q) {
            q.Take("set");
            var stmt = new Ast.SetStmt();
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
            q.Take("while");
            var stmt = new Ast.WhileStmt();
            stmt.Condition = ParseExpr(q);
            stmt.Block = ParseBlock(q);
            ConsumeSemicolon(q);
            return stmt;
        }

        private static Ast.Stmt ParseBreakStmt(TokenQueue q) {
            q.Take("break");
            ConsumeSemicolon(q);
            return new Ast.BreakStmt();
        }

        private static Ast.Stmt ParseContinueStmt(TokenQueue q) {
            q.Take("continue");
            ConsumeSemicolon(q);
            return new Ast.ContinueStmt();
        }

        private static Ast.Stmt ParsePrintStmt(TokenQueue q) {
            q.Take("print");
            var stmt = new Ast.PrintStmt();
            stmt.Value = ParseExpr(q);
            ConsumeSemicolon(q);
            return stmt;
        }

        private static Ast.Stmt ParseExecuteStmt(TokenQueue q) {
            q.Take("exec", "execute");
            var stmt = new Ast.ExecuteStmt();

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
            q.Take("return");
            var stmt = new Ast.ReturnStmt();
            if (PeekExpr(q)) {
                stmt.Value = ParseExpr(q);
            }
            ConsumeSemicolon(q);
            return stmt;
        }

        private static Ast.Stmt ParseThrowStmt(TokenQueue q) {
            q.Take("throw");
            var stmt = new Ast.ThrowStmt();
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
            q.Take("if");
            var stmt = new Ast.IfStmt();
            stmt.Condition = ParseExpr(q);
            stmt.Block = ParseBlock(q);
            if (q.Peek() == "else") {
                q.Take("else");
                stmt.ElseBlock = ParseBlock(q);
            }
            return stmt;
        }

        private static Ast.Stmt ParseTryCatchStmt(TokenQueue q) {
            var stmt = new Ast.TryCatchStmt();

            q.Take("begin");
            q.Take("try");
            stmt.TryBlock = new Ast.Block();
            while (q.Peek() != "end") {
                stmt.TryBlock.Statements.Add(ParseStmt(q));
            }
            q.Take("end");
            q.Take("try");

            q.Take("begin");
            q.Take("catch");
            stmt.CatchBlock = new Ast.Block();
            while (q.Peek() != "end") {
                stmt.CatchBlock.Statements.Add(ParseStmt(q));
            }
            q.Take("end");
            q.Take("catch");
            return stmt;
        }

        private static Ast.Stmt ParseSqlStmt(TokenQueue q) {
            var n = q.Notebook;
            var prefix = TrimOurKeywordsFromEnd(n.FindLongestValidStatementPrefix(q.ToString()));

            if (prefix.Any()) {
                // consume the corresponding valid tokens from the queue
                int len = 0;
                while (len < prefix.Length) {
                    len += q.Take().Text.Length + 1;
                }

                return new Ast.SqlStmt { Sql = prefix.Trim() };
            } else {
                throw new SyntaxException(q);
            }
        }

        private static string ParseVariableName(TokenQueue q) {
            var t = q.PeekToken();
            if (t != null && t.Type == TokenType.Variable && IsVariableName(t.GetUnescapedText())) {
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
            var stmt = new Ast.Block();
            if (q.Peek() == "begin") {
                q.Take("begin");
                while (q.Peek() != "end") {
                    stmt.Statements.Add(ParseStmt(q));
                }
                q.Take("end");
            } else {
                stmt.Statements.Add(ParseStmt(q));
            }
            return stmt;
        }

        private static Ast.Expr ParseExpr(TokenQueue q) {
            var n = q.Notebook;
            var selectStr = "SELECT ( ";
            var prefix = TrimOurKeywordsFromEnd(n.FindLongestValidStatementPrefix(selectStr + q.ToString()));

            if (prefix.Length > selectStr.Length) {
                // consume the corresponding valid tokens from the queue
                int len = selectStr.Length;
                while (len < prefix.Length) {
                    len += q.Take().Text.Length + 1;
                }

                return new Ast.Expr { Sql = prefix.Substring(selectStr.Length).Trim() };
            } else {
                throw new SyntaxException(q);
            }
        }

        private static string TrimOurKeywordsFromEnd(string sql) {
            var lcSql = sql.ToLower().Trim();
            foreach (var x in new[] { "declare", "set", "if", "while", "break", "continue", "print", "exec", "execute", "return", "throw" }) {
                if (lcSql.EndsWith($" {x}")) {
                    return sql.Substring(0, lcSql.Length - x.Length - 1);
                }
            }
            return sql;
        }

        private static bool PeekExpr(TokenQueue q) {
            var n = q.Notebook;
            var selectStr = "SELECT ( ";
            var prefix = n.FindLongestValidStatementPrefix(selectStr + q.ToString());

            if (prefix.Length > selectStr.Length) {
                return true;
            } else {
                throw new SyntaxException(q);
            }
        }

        private static void ConsumeSemicolon(TokenQueue q) {
            if (q.Peek() == ";") {
                q.Take();
            }
        }

    }
}
