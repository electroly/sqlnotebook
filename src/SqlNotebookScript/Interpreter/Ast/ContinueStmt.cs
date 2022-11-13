namespace SqlNotebookScript.Interpreter.Ast;

public sealed class ContinueStmt : Stmt
{
    protected override bool IsLeaf { get; } = true;
}
