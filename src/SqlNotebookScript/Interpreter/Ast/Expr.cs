namespace SqlNotebookScript.Interpreter.Ast;

public sealed class Expr : Node
{
    public string Sql { get; set; }
    public SqliteSyntaxProduction SqliteSyntax { get; set; }

    protected override Node GetChild() => SqliteSyntax;
}
