using System.Collections.Generic;

namespace SqlNotebookScript.Interpreter.Ast;

// import-column
public sealed class ImportColumn : Node {
    public IdentifierOrExpr ColumnName { get; set; }
    public IdentifierOrExpr AsName { get; set; } // may be null
    public TypeConversion? TypeConversion { get; set; }
    protected override IEnumerable<Node> GetChildren() => new[] { ColumnName, AsName };
}
