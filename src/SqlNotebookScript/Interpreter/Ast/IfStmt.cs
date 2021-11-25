using System.Collections.Generic;

namespace SqlNotebookScript.Interpreter.Ast;

public sealed class IfStmt : Stmt {
    public Expr Condition { get; set; }
    public Block Block { get; set; }
    public Block ElseBlock { get; set; } // may be null
    protected override IEnumerable<Node> GetChildren() => new Node[] { Condition, Block, ElseBlock };
}
