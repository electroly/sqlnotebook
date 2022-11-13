namespace SqlNotebookScript.Interpreter.Ast;

public sealed class BreakStmt : Stmt
{
    protected override bool IsLeaf { get; } = true;
}
