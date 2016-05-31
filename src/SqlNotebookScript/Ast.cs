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
using System.Linq;
using SqlNotebookCore;

namespace SqlNotebookScript.Ast {
    public abstract class Node {
        public Token SourceToken { get; set; }

        // each node will implement only one of the following three:
        protected virtual bool IsLeaf { get; } = false; // if the node has no children
        protected virtual Node GetChild() { return null; }  // if the node only has one child
        protected virtual IEnumerable<Node> GetChildren() { return _empty; } // if the node has multiple children

        private static Node[] _empty = new Node[0];
        public IEnumerable<Node> Children {
            get {
                if (IsLeaf) {
                    return _empty;
                }
                var child = GetChild();
                if (child != null) {
                    return new Node[] { child };
                } else {
                    return GetChildren();
                }
            }
        }

        public IEnumerable<Node> Traverse() {
            var stack = new Stack<Node>();
            stack.Push(this);

            while (stack.Any()) {
                var n = stack.Pop();
                if (!n.IsLeaf) {
                    var onlyChild = n.GetChild();
                    if (onlyChild != null) {
                        stack.Push(onlyChild);
                    } else {
                        foreach (var child in n.GetChildren().Reverse()) {
                            if (child != null) {
                                stack.Push(child);
                            }
                        }
                    }
                }
                yield return n;
            }
        }
    }

    //
    // SQL Notebook stuff
    //

    public sealed class Script : Node {
        public Block Block { get; set; }
        protected override Node GetChild() { return Block; }
    }

    public sealed class Expr : Node {
        public string Sql { get; set; }
    }

    public abstract class Stmt : Node { }

    public sealed class Block : Node {
        public List<Stmt> Statements = new List<Stmt>();
        protected override IEnumerable<Node> GetChildren() { return Statements; }
    }

    public sealed class SqlStmt : Stmt {
        public string Sql { get; set; }
        protected override bool IsLeaf { get; } = true;
    }

    public abstract class AssignmentStmt : Stmt {
        public string VariableName { get; set; }
        public Expr InitialValue { get; set; } // may be null
        protected override Node GetChild() { return InitialValue; }
    }

    public sealed class DeclareStmt : AssignmentStmt {
        public bool IsParameter { get; set; }
    }

    public sealed class SetStmt : AssignmentStmt { }

    public sealed class IfStmt : Stmt {
        public Expr Condition { get; set; }
        public Block Block { get; set; }
        public Block ElseBlock { get; set; } // may be null
        protected override IEnumerable<Node> GetChildren() { return new Node[] { Condition, Block, ElseBlock }; }
    }

    public sealed class WhileStmt : Stmt {
        public Expr Condition { get; set; }
        public Block Block { get; set; }
        protected override IEnumerable<Node> GetChildren() { return new Node[] { Condition, Block }; }
    }

    public sealed class BreakStmt : Stmt {
        protected override bool IsLeaf { get; } = true;
    }

    public sealed class ContinueStmt : Stmt {
        protected override bool IsLeaf { get; } = true;
    }

    public sealed class PrintStmt : Stmt {
        public Expr Value { get; set; }
        protected override Node GetChild() { return Value; }
    }

    public sealed class ArgumentPair {
        public string Name { get; set; }
        public Expr Value { get; set; } // may be null to indicate 'DEFAULT'
    }

    public sealed class ExecuteStmt : Stmt {
        public string ReturnVariableName { get; set; } // may be null
        public string ScriptName { get; set; }
        public List<ArgumentPair> Arguments { get; set; } = new List<ArgumentPair>();
        protected override bool IsLeaf { get; } = true;
    }

    public sealed class ReturnStmt : Stmt {
        public Expr Value { get; set; }
        protected override Node GetChild() { return Value; }
    }

    public sealed class ThrowStmt : Stmt {
        public Expr ErrorNumber { get; set; }
        public Expr Message { get; set; }
        public Expr State { get; set; }
        protected override IEnumerable<Node> GetChildren() { yield return ErrorNumber; yield return Message; yield return State; }
    }

    public sealed class RethrowStmt : Stmt {
        protected override bool IsLeaf { get; } = true;
    }

    public sealed class TryCatchStmt : Stmt {
        public Block TryBlock { get; set; }
        public Block CatchBlock { get; set; }
        protected override IEnumerable<Node> GetChildren() { yield return TryBlock; yield return CatchBlock; }
    }

    //
    // SQLite stuff
    //

    // explain-stmt ::= EXPLAIN [QUERY PLAN] <sql-stmt>

    // alter-table-stmt ::= 
    //      ALTER TABLE [ schema-name "." ] table-name 
    //      ( RENAME TO new-table-name ) | ( ADD [COLUMN] <column-def> )

    // column-def ::= column-name [<type-name>] [<column-constraint>]*

    // type-name ::=
    //      name [ 
    //          "("
    //              <signed-number>
    //              [ "," <signed-number> ] 
    //          ")" 
    //      ]

    // column-constraint ::= 
    //      [ CONSTRAINT name ]
    //      ( PRIMARY KEY [ASC | DESC] <conflict-clause> [AUTOINCREMENT] )
    //      |   ( NOT NULL <conflict-clause> )
    //      |   ( UNIQUE <conflict-clause> )
    //      |   ( CHECK "(" <expr> ")" )
    //      |   ( DEFAULT ( <signed-number> | <literal-value> | "(" <expr> ")" ) )
    //      |   ( COLLATE collation-name )
    //      |   ( <foreign-key-clause> )

    // foreign-key-clause ::= 
    //      REFERENCES foreign-table 
    //      [ "(" column-name [ "," column-name ]* ")" ]
    //      [
    //          (
    //              ON ( DELETE | UPDATE )
    //              ( SET NULL | SET DEFAULT | CASCADE | RESTRICT | NO ACTION )
    //          ) | (
    //              MATCH name
    //          )
    //      ]
    //      [ [NOT] DEFERRABLE [ INITIALLY DEFERRED | INITIALLY IMMEDIATE ] ]

    // conflict-clause ::= [ ON CONFLICT ( ROLLBACK | ABORT | FAIL | IGNORE | REPLACE ) ]

    // expr ::= <literal-value>
    // expr ::= <bind-parameter>
    // expr ::= [ [ database-name "." ] table-name "." ] column-name
    // expr ::= unary-operator <expr>
    // expr ::= <expr> binary-operator <expr>
    // expr ::= function-name "(" [ [DISTINCT] <expr> [ "," <expr> ]* | "*" ] ")"
    // expr ::= "(" <expr> ")"
    // expr ::= CAST "(" <expr> AS <type-name> ")"
    // expr ::= <expr> COLLATE collation-name
    // expr ::= <expr> [ NOT ] ( LIKE | GLOB | REGEXP | MATCH ) <expr> [ ESCAPE <expr> ]
    // expr ::= <expr> ( ISNULL | NOTNULL | NOT NULL )
    // expr ::= <expr> IS [ NOT ] <expr>
    // expr ::= <expr> [ NOT ] BETWEEN <expr> AND <expr>
    // expr ::= <expr> [ NOT ] IN
    //      (
    //          "(" [ <select-stmt> | <expr> [ , <expr> ]* ] ")" |
    //          [ database-name "." ] table-name
    //      )
    // expr ::= [ [ NOT ] EXISTS ] "(" <select-stmt> ")"
    // expr ::= CASE [ <expr> ] WHEN <expr> THEN <expr> [ ELSE <expr> ] END
    // expr ::= <raise-function>

}
