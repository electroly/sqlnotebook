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
using System.IO;
using System.Linq;
using SqlNotebookCore;

namespace SqlNotebookScript {
    public sealed class ScriptOutput {
        public List<SimpleDataTable> DataTables { get; } = new List<SimpleDataTable>();
        public List<string> TextOutput { get; } = new List<string>();
        public object ScalarResult { get; set; }

        public void Append(ScriptOutput x) {
            DataTables.AddRange(x.DataTables);
            x.DataTables.Clear();
            TextOutput.AddRange(x.TextOutput);
            x.TextOutput.Clear();
        }

        public void WriteCsv(StreamWriter s) {
            foreach (var dt in DataTables) {
                s.WriteLine(string.Join(",", dt.Columns.Select(EscapeCsv)));
                foreach (var row in dt.Rows) {
                    s.WriteLine(string.Join(",", row.Select(EscapeCsv)));
                }
                s.WriteLine();
            }
        }

        private static string EscapeCsv(object val) {
            var str = val.ToString();
            if (StringRequiresEscape(str)) {
                return $"\"{str.Replace("\"", "\"\"")}\"";
            } else {
                return str;
            }
        }

        private static bool StringRequiresEscape(string str) {
            if (str == "") {
                return false;
            }

            return
                char.IsWhiteSpace(str.First()) ||
                char.IsWhiteSpace(str.Last()) ||
                (str.Length > 1 && str.First() == '0') ||
                str.Any(CharRequiresEscape);
        }

        private static bool CharRequiresEscape(char ch) {
            return ch == '"' || ch == '\'' || ch == ',' || ch == '\r' || ch == '\n' || ch == '\t';
        }
    }

    internal sealed class ScriptEnv {
        // local variables. keys are in lowercase.
        public Dictionary<string, object> Vars { get; } = new Dictionary<string, object>();
        // script parameters.  keys are in lowercase.
        public Dictionary<string, object> Pars { get; } = new Dictionary<string, object>();

        public ScriptOutput Output { get; private set; } = new ScriptOutput();

        public bool DidReturn { get; set; }
        public bool DidBreak { get; set; }
        public bool DidContinue { get; set; }
        public bool DidThrow { get; set; }

        public object ErrorNumber { get; set; }
        public object ErrorMessage { get; set; }
        public object ErrorState { get; set; }
    }

    public class ScriptException : Exception {
        public ScriptException(string message) : base(message) { }
    }

    public sealed class UncaughtErrorScriptException : ScriptException {
        public object ErrorNumber { get; }
        public object ErrorMessage { get; }
        public object ErrorState { get; }
        public UncaughtErrorScriptException(object errorNumber, object errorMessage, object errorState)
            : base($"Uncaught SQL error.  Message: \"{errorMessage}\", Number: {errorNumber}, State: {errorState}.") {
            ErrorNumber = errorNumber;
            ErrorMessage = errorMessage;
            ErrorState = errorState;
        }
    }

    public sealed class ScriptRunner {
        private readonly Notebook _notebook;
        private readonly IReadOnlyDictionary<Type, Action<Ast.Stmt, ScriptEnv>> _stmtRunners;

        public ScriptRunner(Notebook notebook) {
            _notebook = notebook;
            _stmtRunners = new Dictionary<Type, Action<Ast.Stmt, ScriptEnv>> {
                [typeof(Ast.SqlStmt)] = (s, e) => ExecuteSqlStmt((Ast.SqlStmt)s, e),
                [typeof(Ast.DeclareStmt)] = (s, e) => ExecuteDeclareStmt((Ast.DeclareStmt)s, e),
                [typeof(Ast.SetStmt)] = (s, e) => ExecuteSetStmt((Ast.SetStmt)s, e),
                [typeof(Ast.IfStmt)] = (s, e) => ExecuteIfStmt((Ast.IfStmt)s, e),
                [typeof(Ast.WhileStmt)] = (s, e) => ExecuteWhileStmt((Ast.WhileStmt)s, e),
                [typeof(Ast.BreakStmt)] = (s, e) => ExecuteBreakStmt((Ast.BreakStmt)s, e),
                [typeof(Ast.ContinueStmt)] = (s, e) => ExecuteContinueStmt((Ast.ContinueStmt)s, e),
                [typeof(Ast.PrintStmt)] = (s, e) => ExecutePrintStmt((Ast.PrintStmt)s, e),
                [typeof(Ast.ExecuteStmt)] = (s, e) => ExecuteExecuteStmt((Ast.ExecuteStmt)s, e),
                [typeof(Ast.ReturnStmt)] = (s, e) => ExecuteReturnStmt((Ast.ReturnStmt)s, e),
                [typeof(Ast.ThrowStmt)] = (s, e) => ExecuteThrowStmt((Ast.ThrowStmt)s, e),
                [typeof(Ast.RethrowStmt)] = (s, e) => ExecuteRethrowStmt((Ast.RethrowStmt)s, e),
                [typeof(Ast.TryCatchStmt)] = (s, e) => ExecuteTryCatchStmt((Ast.TryCatchStmt)s, e)
            };
        }

        private string GetItemData(string name) {
            string data = null;
            var dt = _notebook.Query("SELECT data FROM sqlnotebook_items WHERE name = @name",
                new Dictionary<string, object> { ["@name"] = name });
            if (dt.Rows.Count == 1) {
                data = (string)dt.Get(0, "data");
            }
            return data;
        }

        // must be run from the SQLite thread
        public ScriptOutput Execute(Ast.Script script, IReadOnlyDictionary<string, object> args) {
            var env = new ScriptEnv();
            foreach (var arg in args) {
                env.Pars[arg.Key.ToLower()] = arg.Value;
            }
            Execute(script, env);
            return env.Output;
        }

        private void Execute(Ast.Script script, ScriptEnv env) {
            ExecuteBlock(script.Block, env);

            if (env.DidBreak) {
                throw new ScriptException($"Attempted to BREAK outside of a WHILE loop.");
            } else if (env.DidContinue) {
                throw new ScriptException($"Attempted to CONTINUE outside of a WHILE loop.");
            } else if (env.DidThrow) {
                throw new UncaughtErrorScriptException(env.ErrorNumber, env.ErrorMessage, env.ErrorState);
            }
        }

        private void ExecuteBlock(Ast.Block block, ScriptEnv env) {
            foreach (var stmt in block.Statements) {
                ExecuteStmt(stmt, env);
                if (env.DidReturn || env.DidBreak || env.DidContinue || env.DidThrow) {
                    return;
                }
            }
        }

        private void ExecuteStmt(Ast.Stmt stmt, ScriptEnv env) {
            var runner = _stmtRunners[stmt.GetType()];
            runner(stmt, env);
        }

        private void ExecuteSqlStmt(Ast.SqlStmt stmt, ScriptEnv env) {
            var dt = _notebook.Query(stmt.Sql, env.Vars);
            if (dt.Columns.Any()) {
                env.Output.DataTables.Add(dt);
            }
        }

        private void ExecuteDeclareStmt(Ast.DeclareStmt stmt, ScriptEnv env) {
            var name = stmt.VariableName.ToLower();
            var declarationExists = env.Vars.ContainsKey(name);
            if (declarationExists) {
                throw new ScriptException($"Duplicate DECLARE for variable \"{stmt.VariableName}\".");
            } else if (stmt.IsParameter) {
                object value;
                if (env.Pars.TryGetValue(name, out value)) {
                    env.Vars[name] = value;
                } else if (stmt.InitialValue != null) {
                    env.Vars[name] = EvaluateExpr(stmt.InitialValue, env);
                } else {
                    throw new ScriptException(
                        $"An argument value was not provided for parameter \"{stmt.VariableName}\".");
                }
            } else {
                if (stmt.InitialValue != null) {
                    env.Vars[name] = EvaluateExpr(stmt.InitialValue, env);
                } else {
                    env.Vars[name] = 0L;
                }
            }
        }

        private void ExecuteSetStmt(Ast.SetStmt stmt, ScriptEnv env) {
            var name = stmt.VariableName.ToLower();
            if (env.Vars.ContainsKey(name)) {
                env.Vars[name] = EvaluateExpr(stmt.InitialValue, env);
            } else {
                throw new ScriptException($"Attempted to SET the undeclared variable \"{stmt.VariableName}\".");
            }
        }

        private void ExecuteIfStmt(Ast.IfStmt stmt, ScriptEnv env) {
            var condition = EvaluateExpr<long>(stmt.Condition, env);
            if (condition == 0) {
                if (stmt.ElseBlock != null) {
                    ExecuteBlock(stmt.ElseBlock, env);
                }
            } else if (condition == 1) {
                ExecuteBlock(stmt.Block, env);
            } else {
                throw new ScriptException(
                    $"Evaluation of IF condition expression \"{stmt.Condition.Sql}\" " +
                    $"produced a value of {condition} instead of the expected 0 or 1.");
            }
        }

        private void ExecuteWhileStmt(Ast.WhileStmt stmt, ScriptEnv env) {
            long condition;
            bool skipConditionCheck = false; // "continue" skips the condition check
            do {
                condition = skipConditionCheck ? 1 : EvaluateExpr<long>(stmt.Condition, env);
                skipConditionCheck = false;
                if (condition == 1) {
                    ExecuteBlock(stmt.Block, env);
                    if (env.DidReturn || env.DidThrow) {
                        return;
                    } else if (env.DidBreak) {
                        env.DidBreak = false;
                        break;
                    } else if (env.DidContinue) {
                        env.DidContinue = false;
                        skipConditionCheck = true;
                    }
                } else if (condition != 0) {
                    throw new ScriptException(
                        $"Evaluation of WHILE condition expression \"{stmt.Condition.Sql}\" " +
                        $"produced a value of {condition} instead of the expected 0 or 1.");
                }
            } while (condition == 1);
        }

        private void ExecuteBreakStmt(Ast.BreakStmt stmt, ScriptEnv env) {
            env.DidBreak = true;
        }

        private void ExecuteContinueStmt(Ast.ContinueStmt stmt, ScriptEnv env) {
            env.DidContinue = true;
        }

        private void ExecutePrintStmt(Ast.PrintStmt stmt, ScriptEnv env) {
            var value = EvaluateExpr(stmt.Value, env).ToString();
            env.Output.TextOutput.Add(value);
        }

        private void ExecuteExecuteStmt(Ast.ExecuteStmt stmt, ScriptEnv env) {
            var parser = new ScriptParser(_notebook);
            var runner = new ScriptRunner(_notebook);
            var script = parser.Parse(GetItemData(stmt.ScriptName));
            var subEnv = new ScriptEnv();
            foreach (var arg in stmt.Arguments) {
                subEnv.Pars[arg.Name.ToLower()] = EvaluateExpr(arg.Value, env);
            }
            try {
                runner.Execute(script, subEnv);
            } catch (UncaughtErrorScriptException ex) {
                env.ErrorNumber = ex.ErrorNumber;
                env.ErrorMessage = ex.ErrorMessage;
                env.ErrorState = ex.ErrorState;
                env.DidThrow = true;
                return;
            }
            env.Output.Append(subEnv.Output);
            var returnCode = subEnv.Output.ScalarResult;
            if (stmt.ReturnVariableName != null) {
                var name = stmt.ReturnVariableName.ToLower();
                env.Vars[name] = returnCode ?? 0L;
            }
        }

        private void ExecuteReturnStmt(Ast.ReturnStmt stmt, ScriptEnv env) {
            if (stmt.Value != null) {
                env.Output.ScalarResult = EvaluateExpr(stmt.Value, env);
            }
            env.DidReturn = true;
        }

        private void ExecuteThrowStmt(Ast.ThrowStmt stmt, ScriptEnv env) {
            env.ErrorNumber = EvaluateExpr(stmt.ErrorNumber, env);
            env.ErrorMessage = EvaluateExpr(stmt.Message, env);
            env.ErrorState = EvaluateExpr(stmt.State, env);
            env.DidThrow = true;
        }

        private void ExecuteRethrowStmt(Ast.RethrowStmt stmt, ScriptEnv env) {
            env.DidThrow = true;
        }

        private void ExecuteTryCatchStmt(Ast.TryCatchStmt stmt, ScriptEnv env) {
            ExecuteBlock(stmt.TryBlock, env);
            if (env.DidThrow) {
                env.DidThrow = false;
                ExecuteBlock(stmt.CatchBlock, env);
            }
        }

        private T EvaluateExpr<T>(Ast.Expr expr, ScriptEnv env) {
            var value = EvaluateExpr(expr, env);
            if (typeof(T).IsAssignableFrom(value.GetType())) {
                return (T)value;
            } else {
                throw new ScriptException(
                    $"Evaluation of expression \"{expr.Sql}\" produced a value of type " +
                    $"\"{value.GetType().Name}\" instead of the expected \"{typeof(T).Name}\".");
            }
        }

        private object EvaluateExpr(Ast.Expr expr, ScriptEnv env) {
            var dt = _notebook.Query($"SELECT ({expr.Sql})", env.Vars);
            if (dt.Columns.Count == 1 && dt.Rows.Count == 1) {
                return dt.Rows[0][0];
            } else {
                throw new ScriptException(
                    $"Evaluation of expression \"{expr.Sql}\" did not produce a value.");
            }
        }
    }
}
