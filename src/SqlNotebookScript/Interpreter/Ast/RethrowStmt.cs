namespace SqlNotebookScript.Interpreter.Ast;

public sealed class RethrowStmt : Stmt {
    protected override bool IsLeaf { get; } = true;
}
