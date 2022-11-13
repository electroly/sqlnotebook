namespace SqlNotebookScript.Interpreter.Ast;

public sealed class DeclareStmt : AssignmentStmt
{
    public bool IsParameter { get; set; }
}
