using System.Collections.Generic;

namespace SqlNotebookScript.Interpreter.Ast;

// import-csv-stmt
public sealed class ImportCsvStmt : Stmt {
    public Expr FilenameExpr { get; set; }
    public ImportTable ImportTable { get; set; }
    public OptionsList OptionsList { get; set; }
    public Expr SeparatorExpr { get; set; }
    protected override IEnumerable<Node> GetChildren() => new Node[] { FilenameExpr, SeparatorExpr, ImportTable, OptionsList };
}
