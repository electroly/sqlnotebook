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

using SqlNotebookScript.Interpreter;
using Ast = SqlNotebookScript.Interpreter.Ast;

namespace SqlNotebookScript {
    public abstract class CustomMacro {
        public INotebook Notebook { get; set; }

        // return true if a macro expansion was made, false if not
        public abstract bool Apply(Ast.SqlStmt stmt);

        protected Ast.Expr NewExpr(string text) {
            var tokens = Notebook.Tokenize(text);
            var q = new TokenQueue(tokens, Notebook);
            Ast.SqliteSyntaxProduction ast;
            var result = SqliteParser.ReadExpr(q, out ast);
            if (result.IsValid && q.Eof()) {
                return new Ast.Expr { Sql = text, SqliteSyntax = ast };
            } else {
                throw new MacroProcessorException($"\"{text}\" is not a valid SQL expression.");
            }
        }

        protected Ast.SqlStmt NewSqlStmt(string text) {
            var ast = ParseSqlStmt(text);
            return new Ast.SqlStmt { Sql = text, SqliteSyntax = ast };
        }

        protected Ast.SqliteSyntaxProduction ParseSqlStmt(string text) {
            var tokens = Notebook.Tokenize(text);
            var q = new TokenQueue(tokens, Notebook);
            Ast.SqliteSyntaxProduction ast;
            var result = SqliteParser.ReadStmt(q, out ast);
            if (result.IsValid && q.Eof()) {
                return ast;
            } else {
                throw new MacroProcessorException($"\"{text}\" is not a valid SQL statement.");
            }
        }
    }
}
