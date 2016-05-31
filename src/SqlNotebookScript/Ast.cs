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

    public sealed class Script : Node {
        public Block Block { get; set; }
        protected override Node GetChild() => Block;
    }

    public sealed class Expr : Node {
        public string Sql { get; set; }
    }

    public abstract class Stmt : Node { }

    public sealed class Block : Node {
        public List<Stmt> Statements = new List<Stmt>();
        protected override IEnumerable<Node> GetChildren() => Statements;
    }

    public sealed class SqlStmt : Stmt {
        public string Sql { get; set; }
        protected override bool IsLeaf { get; } = true;
    }

    public abstract class AssignmentStmt : Stmt {
        public string VariableName { get; set; }
        public Expr InitialValue { get; set; } // may be null
        protected override Node GetChild() => InitialValue;
    }

    public sealed class DeclareStmt : AssignmentStmt {
        public bool IsParameter { get; set; }
    }

    public sealed class SetStmt : AssignmentStmt { }

    public sealed class IfStmt : Stmt {
        public Expr Condition { get; set; }
        public Block Block { get; set; }
        public Block ElseBlock { get; set; } // may be null
        protected override IEnumerable<Node> GetChildren() => new Node[] { Condition, Block, ElseBlock };
    }

    public sealed class WhileStmt : Stmt {
        public Expr Condition { get; set; }
        public Block Block { get; set; }
        protected override IEnumerable<Node> GetChildren() => new Node[] { Condition, Block };
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
        protected override IEnumerable<Node> GetChildren() => new Node[] { ErrorNumber, Message, State };
    }

    public sealed class RethrowStmt : Stmt {
        protected override bool IsLeaf { get; } = true;
    }

    public sealed class TryCatchStmt : Stmt {
        public Block TryBlock { get; set; }
        public Block CatchBlock { get; set; }
        protected override IEnumerable<Node> GetChildren() => new Node[] { TryBlock, CatchBlock };
    }

    // import-csv-stmt
    public sealed class ImportCsvStmt : Stmt {
        public Expr FilenameExpr { get; set; }
        public ImportTable ImportTable { get; set; }
        public OptionsList OptionsList { get; set; }
        protected override IEnumerable<Node> GetChildren() => new Node[] { FilenameExpr, ImportTable, OptionsList };
    }

    // options-list
    public sealed class OptionsList : Node {
        public Dictionary<string, Expr> Options { get; set; } = new Dictionary<string, Expr>(); // lowercase key
        protected override IEnumerable<Node> GetChildren() => Options.Values;
    }

    // import-table
    public sealed class ImportTable : Node {
        public IdentifierOrExpr TableName { get; set; }
        public List<ImportColumn> ImportColumns { get; set; } = new List<ImportColumn>();
        protected override IEnumerable<Node> GetChildren() => new Node[] { TableName }.Concat(ImportColumns);
    }

    // import-column
    public sealed class ImportColumn : Node {
        public IdentifierOrExpr ColumnName { get; set; }
        public IdentifierOrExpr AsName { get; set; } // may be null
        public TypeConversion? TypeConversion { get; set; }
        protected override IEnumerable<Node> GetChildren() => new[] { ColumnName, AsName };
    }

    public enum TypeConversion {
        Text,
        Integer,
        Real,
        Date,
        DateTime,
        DateTimeOffset
    }
    
    // table-name, sn-column-name
    public sealed class IdentifierOrExpr : Node {
        // mutually exclusive, one will be null
        public string Identifier { get; set; }
        public Expr Expr { get; set; }
        protected override Node GetChild() => Expr;
    }

    // import-txt-stmt
    public sealed class ImportTxtStmt : Stmt {
        public Expr FilenameExpr { get; set; }
        public IdentifierOrExpr TableName { get; set; }
        public IdentifierOrExpr LineNumberColumnName { get; set; } // may be null
        public IdentifierOrExpr TextColumnName { get; set; } // may be null
        public OptionsList OptionsList { get; set; }
        protected override IEnumerable<Node> GetChildren() => 
            new Node[] { FilenameExpr, TableName, LineNumberColumnName, TextColumnName, OptionsList };
    }


}
