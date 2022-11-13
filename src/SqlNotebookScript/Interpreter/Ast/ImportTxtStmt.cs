using System.Collections.Generic;

namespace SqlNotebookScript.Interpreter.Ast;

// import-txt-stmt
public sealed class ImportTxtStmt : Stmt
{
    public Expr FilenameExpr { get; set; }
    public IdentifierOrExpr TableName { get; set; }
    public IdentifierOrExpr LineNumberColumnName { get; set; } // may be null
    public IdentifierOrExpr TextColumnName { get; set; } // may be null
    public OptionsList OptionsList { get; set; }

    protected override IEnumerable<Node> GetChildren() =>
        new Node[] { FilenameExpr, TableName, LineNumberColumnName, TextColumnName, OptionsList };
}
