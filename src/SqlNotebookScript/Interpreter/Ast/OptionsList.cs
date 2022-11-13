using System.Collections.Generic;

namespace SqlNotebookScript.Interpreter.Ast;

// options-list
public sealed class OptionsList : Node
{
    public Dictionary<string, Expr> Options { get; set; } = new Dictionary<string, Expr>(); // lowercase key

    protected override IEnumerable<Node> GetChildren() => Options.Values;
}
