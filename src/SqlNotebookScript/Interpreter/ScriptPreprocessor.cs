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
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.Interpreter {
    public sealed class ScriptPreprocessorException : Exception {
        public ScriptPreprocessorException(string message) : base(message) { }
    }

    public sealed class ScriptPreprocessor {
        private readonly INotebook _notebook;

        public ScriptPreprocessor(INotebook notebook) {
            _notebook = notebook;
        }

        public void PreprocessStmt(Ast.SqlStmt input) {
            while (ApplyReadCsvMacro(input)) { }
        }

        private bool ApplyReadCsvMacro(Ast.SqlStmt input) {
            var tableFunctionCallNodes = input.Traverse()
                .OfType<Ast.SqliteSyntaxProduction>()
                .Where(x => x.Name == "table-or-subquery.table-function-call");
            foreach (var tableFunctionCallNode in tableFunctionCallNodes) {
                var functionName = tableFunctionCallNode.Traverse()
                    .OfType<Ast.SqliteSyntaxProduction>()
                    .FirstOrDefault(x => x.Name == "table-or-subquery.table-function-name")
                    ?.Text;
                if (functionName.ToLower() != "read_csv") {
                    continue;
                }

                // read_csv(file-path, [has-header-row], [skip-rows], [file-encoding])
                var args = tableFunctionCallNode.TraverseDottedChildren()
                    .Where(x => x.Name == "table-or-subquery.arg").ToList();
                if (args.Count < 1 || args.Count > 4) {
                    throw new ScriptPreprocessorException($"READ_CSV: Between 1 and 4 arguments are required.");
                }

                var filePathExpr = args[0];
                var hasHeaderRowExpr = args.Count >= 2 ? args[1] : null;
                var skipRowsExpr = args.Count >= 3 ? args[2] : null;
                var fileEncodingExpr = args.Count >= 4 ? args[3] : null;
                var tempTableName = Guid.NewGuid().ToString();

                var importStmt = new Ast.ImportCsvStmt {
                    SourceToken = tableFunctionCallNode.SourceToken,
                    FilenameExpr = NewExpr(filePathExpr.Text),
                    ImportTable = new Ast.ImportTable {
                        TableName = new Ast.IdentifierOrExpr { Identifier = tempTableName },
                    },
                    OptionsList = new Ast.OptionsList {
                        Options = new Dictionary<string, Ast.Expr> {
                            ["temporary_table"] = NewExpr("1"),
                            ["skip_lines"] = NewExpr(skipRowsExpr?.Text ?? "0"),
                            ["header_row"] = NewExpr(hasHeaderRowExpr?.Text ?? "1"),
                            ["file_encoding"] = NewExpr(fileEncodingExpr?.Text ?? "0")
                        }
                    }
                };
                foreach (var n in importStmt.Traverse()) {
                    n.SourceToken = tableFunctionCallNode.SourceToken;
                }
                input.RunBefore.Add(importStmt);

                var dropTableStmt = NewSqlStmt($"DROP TABLE IF EXISTS {tempTableName.DoubleQuote()}");
                input.RunAfter.Add(dropTableStmt);

                // replace the READ_CSV(...) call with the table name
                var originalTokens = _notebook.Tokenize(input.Sql).Select(x => x.Text).ToList();
                for (int i = 0; i < tableFunctionCallNode.NumTokens; i++) {
                    originalTokens.RemoveAt(tableFunctionCallNode.StartToken);
                }
                originalTokens.Insert(tableFunctionCallNode.StartToken, tempTableName.DoubleQuote());
                var newSqlText = string.Join(" ", originalTokens);
                input.Sql = newSqlText;
                input.SqliteSyntax = ParseSqlStmt(newSqlText);
                return true;
            }
            return false;
        }

        private Ast.Expr NewExpr(string text) {
            var tokens = _notebook.Tokenize(text);
            var q = new TokenQueue(tokens, _notebook);
            Ast.SqliteSyntaxProduction ast;
            var result = SqliteParser.ReadExpr(q, out ast);
            if (result.IsValid && q.Eof()) {
                return new Ast.Expr { Sql = text, SqliteSyntax = ast };
            } else {
                throw new ScriptPreprocessorException($"\"{text}\" is not a valid SQL expression.");
            }
        }

        private Ast.SqlStmt NewSqlStmt(string text) {
            var ast = ParseSqlStmt(text);
            return new Ast.SqlStmt { Sql = text, SqliteSyntax = ast };
        }

        private Ast.SqliteSyntaxProduction ParseSqlStmt(string text) {
            var tokens = _notebook.Tokenize(text);
            var q = new TokenQueue(tokens, _notebook);
            Ast.SqliteSyntaxProduction ast;
            var result = SqliteParser.ReadStmt(q, out ast);
            if (result.IsValid && q.Eof()) {
                return ast;
            } else {
                throw new ScriptPreprocessorException($"\"{text}\" is not a valid SQL statement.");
            }
        }
    }
}
