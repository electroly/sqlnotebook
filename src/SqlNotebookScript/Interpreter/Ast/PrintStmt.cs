namespace SqlNotebookScript.Interpreter.Ast;

public sealed class PrintStmt : Stmt
{
    public Expr Value { get; set; }

    protected override Node GetChild()
    {
        return Value;
    }
}
