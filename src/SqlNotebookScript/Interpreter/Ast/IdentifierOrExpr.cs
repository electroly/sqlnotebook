namespace SqlNotebookScript.Interpreter.Ast;

// table-name, sn-column-name
public sealed class IdentifierOrExpr : Node {
    // mutually exclusive, one will be null
    public string Identifier { get; set; }
    public Expr Expr { get; set; }
    protected override Node GetChild() => Expr;
}
