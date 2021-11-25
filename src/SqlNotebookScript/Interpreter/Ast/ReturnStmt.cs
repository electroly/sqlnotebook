namespace SqlNotebookScript.Interpreter.Ast;

public sealed class ReturnStmt : Stmt {
    public Expr Value { get; set; }
    protected override Node GetChild() => Value;
}
