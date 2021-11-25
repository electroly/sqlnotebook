namespace SqlNotebookScript.Interpreter.Ast;

public abstract class AssignmentStmt : Stmt {
    public string VariableName { get; set; }
    public Expr InitialValue { get; set; } // may be null
    protected override Node GetChild() => InitialValue;
}
