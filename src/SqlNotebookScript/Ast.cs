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

using System.Collections.Generic;

namespace SqlNotebookScript.Ast {
    public sealed class Script {
        public Block Block { get; set; } = new Block();
    }

    public abstract class Node { }
    
    public sealed class Expr : Node {
        public string Sql { get; set; }
    }

    public abstract class Stmt : Node { }

    public sealed class Block : Node {
        public List<Stmt> Statements = new List<Stmt>();
    }

    public sealed class SqlStmt : Stmt {
        public string Sql { get; set; }
    }

    public abstract class AssignmentStmt : Stmt {
        public string VariableName { get; set; }
        public Expr InitialValue { get; set; } // may be null
    }

    public sealed class DeclareStmt : AssignmentStmt {
        public bool IsParameter { get; set; }
    }

    public sealed class SetStmt : AssignmentStmt { }

    public sealed class IfStmt : Stmt {
        public Expr Condition { get; set; }
        public Block Block { get; set; }
        public Block ElseBlock { get; set; } // may be null
    }

    public sealed class WhileStmt : Stmt {
        public Expr Condition { get; set; }
        public Block Block { get; set; }
    }

    public sealed class BreakStmt : Stmt { }

    public sealed class ContinueStmt : Stmt { }

    public sealed class PrintStmt : Stmt {
        public Expr Value { get; set; }
    }

    public sealed class ArgumentPair {
        public string Name { get; set; }
        public Expr Value { get; set; } // may be null to indicate 'DEFAULT'
    }

    public sealed class ExecuteStmt : Stmt {
        public string ReturnVariableName { get; set; } // may be null
        public string ScriptName { get; set; }
        public List<ArgumentPair> Arguments { get; set; } = new List<ArgumentPair>();
    }

    public sealed class ReturnStmt : Stmt {
        public Expr Value { get; set; }
    }

    public sealed class ThrowStmt : Stmt {
        public Expr ErrorNumber { get; set; }
        public Expr Message { get; set; }
        public Expr State { get; set; }
    }

    public sealed class RethrowStmt : Stmt { }

    public sealed class TryCatchStmt : Stmt {
        public Block TryBlock { get; set; }
        public Block CatchBlock { get; set; }
    }
}
