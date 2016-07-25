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
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlNotebookCore;
using SqlNotebookCoreModules.Script;

namespace SqlNotebookTest {
    [TestClass]
    public sealed class SqliteGrammarTest {
        [TestMethod]
        public void TestSqlStatements() {
            var filePath = Path.Combine(Path.GetTempPath(), "SqliteGrammarTestDb");
            File.WriteAllBytes(filePath, new byte[0]);

            // these are other SQLite DBs we will attach to, to test schema names
            var otherFilePath = Path.Combine(Path.GetTempPath(), "SqliteGrammarOtherDb");
            File.WriteAllBytes(otherFilePath, Resources.OtherDb);

            var other2FilePath = Path.Combine(Path.GetTempPath(), "SqliteGrammarOtherDb2");
            File.WriteAllBytes(other2FilePath, Resources.OtherDb);

            try {
                using (var notebook = new Notebook(filePath, true)) {
                    notebook.Execute("ATTACH DATABASE ? AS other", new[] { filePath });

                    var stmts =
                        from line in Resources.SqlTests.Replace('\r', '\n').Split('\n')
                        let shouldFail = line.StartsWith("<ERR>")
                        let trimLine = line.Replace("<ERR>", "").Trim()
                        where trimLine.Any() && !trimLine.StartsWith("--")
                        select new { Cmd = trimLine, ShouldFail = shouldFail };

                    var errors = new List<string>();

                    foreach (var stmt in stmts) {
                        // try first in SQLite directly to validate the test
                        try {
                            notebook.Execute(stmt.Cmd, new Dictionary<string, object> {
                                ["@other2_path"] = other2FilePath,
                                ["@int"] = 1,
                                ["@str"] = "hello"
                            });
                            if (stmt.ShouldFail) {
                                errors.Add($"Should have failed, but SQLite accepted.\n{stmt.Cmd}");
                                continue;
                            }
                        } catch (Exception ex) {
                            if (!stmt.ShouldFail) {
                                errors.Add($"Should have passed, but SQLite rejected.\n{stmt.Cmd}\n" +
                                    $"SQLite error: " + ex.Message);
                                continue;
                            }
                        }

                        // then try with our parser to see if we match SQLite
                        var tokens = notebook.Tokenize(stmt.Cmd);
                        var q = new TokenQueue(tokens, notebook);
                        var result = SqlValidator.ReadStmt(q);
                        var success = result.InvalidMessage == null && result.IsValid &&
                            tokens.Count == result.NumValidTokens;
                        if (stmt.ShouldFail && success) {
                            errors.Add($"Should have failed, but we accepted.\n{stmt.Cmd}");
                            continue;
                        } else if (!stmt.ShouldFail && !success) {
                            errors.Add($"Should have passed, but we rejected.\n{stmt.Cmd}\n" +
                                $"InvalidMessage: {result.InvalidMessage}\nIsValid: {result.IsValid}\n" +
                                $"NumValidTokens: {result.NumValidTokens}");
                            continue;
                        }
                    }

                    if (errors.Any()) {
                        Assert.Fail(string.Join("\n\n", errors));
                    }
                }
            } finally {
                File.Delete(filePath);
                File.Delete(otherFilePath);
                File.Delete(other2FilePath);
            }
        }
    }
}
