using System.Collections.Generic;
using System.Linq;

namespace SqlNotebookScript.Interpreter.Ast;

// import-table
public sealed class ImportTable : Node
{
    public IdentifierOrExpr TableName { get; set; }
    public List<ImportColumn> ImportColumns { get; set; } = new List<ImportColumn>();

    protected override IEnumerable<Node> GetChildren() => new Node[] { TableName }.Concat(ImportColumns);
}
