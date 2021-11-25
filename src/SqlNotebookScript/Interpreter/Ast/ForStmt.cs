using System.Collections.Generic;

namespace SqlNotebookScript.Interpreter.Ast;

public sealed class ForStmt : Stmt {
    public string VariableName { get; set; }
    public Expr FirstNumberExpr { get; set; }
    public Expr LastNumberExpr { get; set; }
    public Expr StepExpr { get; set; } // may be null
    public Block Block { get; set; }
    protected override IEnumerable<Node> GetChildren() => 
        new Node[] { FirstNumberExpr, LastNumberExpr, StepExpr, Block };
}
