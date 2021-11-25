using System.Collections.Generic;

namespace SqlNotebookScript.Interpreter.Ast;

public sealed class TryCatchStmt : Stmt {
    public Block TryBlock { get; set; }
    public Block CatchBlock { get; set; }
    protected override IEnumerable<Node> GetChildren() => new Node[] { TryBlock, CatchBlock };
}
