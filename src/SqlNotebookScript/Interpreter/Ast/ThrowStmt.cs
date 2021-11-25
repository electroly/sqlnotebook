namespace SqlNotebookScript.Interpreter.Ast;

public sealed class ThrowStmt : Stmt {
    public bool HasErrorValues { get; set; }
    public Expr Message { get; set; }
    protected override Node GetChild() => Message;
}
