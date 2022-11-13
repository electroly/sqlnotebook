using System.Collections.Generic;

namespace SqlNotebookScript.Interpreter.Ast;

public sealed class WhileStmt : Stmt
{
    public Expr Condition { get; set; }
    public Block Block { get; set; }

    protected override IEnumerable<Node> GetChildren() => new Node[] { Condition, Block };
}
