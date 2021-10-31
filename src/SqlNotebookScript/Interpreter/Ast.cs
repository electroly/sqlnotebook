using System.Collections.Generic;
using System.Linq;
using SqlNotebookScript;

namespace SqlNotebookScript.Interpreter.Ast {
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

    public sealed class SqliteSyntaxProduction : Node {
        public string Name { get; set; }
        public string Text { get; set; }
        public int StartToken { get; set; }
        public int NumTokens { get; set; }
        public List<SqliteSyntaxProduction> Items { get; set; } = new List<SqliteSyntaxProduction>();
        protected override IEnumerable<Node> GetChildren() => Items;

        public IEnumerable<SqliteSyntaxProduction> TraverseDottedChildren() {
            var stack = new Stack<SqliteSyntaxProduction>();
            stack.Push(this);

            while (stack.Any()) {
                var n = stack.Pop();
                if (n == null || (n.Name != null && !n.Name.Contains("."))) {
                    continue;
                }
                foreach (var item in n.Items.AsEnumerable().Reverse()) {
                    stack.Push(item);
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
        public SqliteSyntaxProduction SqliteSyntax { get; set; }
        protected override Node GetChild() => SqliteSyntax;
    }

    public abstract class Stmt : Node { }

    public sealed class Block : Node {
        public List<Stmt> Statements = new List<Stmt>();
        protected override IEnumerable<Node> GetChildren() => Statements;
    }

    public sealed class BlockStmt : Stmt {
        public Block Block { get; set; }
        protected override Node GetChild() => Block;
    }

    public sealed class SqlStmt : Stmt {
        public string Sql { get; set; }
        public SqliteSyntaxProduction SqliteSyntax { get; set; }

        // statements may be added here by the preprocessor
        public List<Stmt> RunBefore { get; set; } = new List<Stmt>();
        public List<Stmt> RunAfter { get; set; } = new List<Stmt>();

        protected override IEnumerable<Node> GetChildren() =>
            new Node[] { SqliteSyntax }.Concat(RunBefore).Concat(RunAfter);
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

    public sealed class ForStmt : Stmt {
        public string VariableName { get; set; }
        public Expr FirstNumberExpr { get; set; }
        public Expr LastNumberExpr { get; set; }
        public Expr StepExpr { get; set; } // may be null
        public Block Block { get; set; }
        protected override IEnumerable<Node> GetChildren() => 
            new Node[] { FirstNumberExpr, LastNumberExpr, StepExpr, Block };
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
        protected override Node GetChild() => Value;
    }

    public sealed class ThrowStmt : Stmt {
        public bool HasErrorValues { get; set; }
        public Expr Message { get; set; }
        protected override Node GetChild() => Message;
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
        public Expr SeparatorExpr { get; set; }
        protected override IEnumerable<Node> GetChildren() => new Node[] { FilenameExpr, SeparatorExpr, ImportTable, OptionsList };
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

    // export-txt-stmt
    public sealed class ExportTxtStmt : Stmt {
        public Expr FilenameExpr { get; set; }
        public SqlStmt SelectStmt { get; set; }
        public OptionsList OptionsList { get; set; }
        protected override IEnumerable<Node> GetChildren() =>
            new Node[] { FilenameExpr, SelectStmt, OptionsList };
    }

    // import-xls-stmt
    public sealed class ImportXlsStmt : Stmt {
        public Expr FilenameExpr { get; set; }
        public Expr WhichSheetExpr { get; set; } // may be null
        public ImportTable ImportTable { get; set; }
        public OptionsList OptionsList { get; set; }
        protected override IEnumerable<Node> GetChildren() =>
            new Node[] { FilenameExpr, WhichSheetExpr, ImportTable, OptionsList };
    }
}
