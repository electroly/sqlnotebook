using System.Collections.Generic;

namespace SqlNotebookScript.Interpreter.Ast;

// export-txt-stmt
public sealed class ExportTxtStmt : Stmt
{
    public Expr FilenameExpr { get; set; }
    public SqlStmt SelectStmt { get; set; }
    public OptionsList OptionsList { get; set; }

    protected override IEnumerable<Node> GetChildren() => new Node[] { FilenameExpr, SelectStmt, OptionsList };
}
