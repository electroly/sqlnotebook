using System.Collections.Generic;

namespace SqlNotebookScript.Interpreter.Ast;

public sealed class ExportCsvStmt : Stmt
{
    public Expr FilenameExpr { get; set; }
    public IdentifierOrExpr TableNameOrNull { get; set; }
    public IdentifierOrExpr ScriptNameOrNull { get; set; }
    public SqlStmt SelectStmtOrNull { get; set; }
    public OptionsList OptionsList { get; set; }

    protected override IEnumerable<Node> GetChildren() =>
        new Node[] { FilenameExpr, TableNameOrNull, ScriptNameOrNull, SelectStmtOrNull, OptionsList };
}
