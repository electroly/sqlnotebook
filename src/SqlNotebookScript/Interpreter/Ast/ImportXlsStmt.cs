using System.Collections.Generic;

namespace SqlNotebookScript.Interpreter.Ast;

// import-xls-stmt
public sealed class ImportXlsStmt : Stmt
{
    public Expr FilenameExpr { get; set; }
    public Expr WhichSheetExpr { get; set; } // may be null
    public ImportTable ImportTable { get; set; }
    public OptionsList OptionsList { get; set; }

    protected override IEnumerable<Node> GetChildren() =>
        new Node[] { FilenameExpr, WhichSheetExpr, ImportTable, OptionsList };
}
