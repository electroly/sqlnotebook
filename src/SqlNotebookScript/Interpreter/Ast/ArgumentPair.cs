namespace SqlNotebookScript.Interpreter.Ast;

public sealed class ArgumentPair
{
    public string Name { get; set; }
    public Expr Value { get; set; } // may be null to indicate 'DEFAULT'
}
