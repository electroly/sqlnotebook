using System.Collections.Generic;

namespace SqlNotebookScript.Interpreter.Ast;

public sealed class Block : Node
{
    public List<Stmt> Statements = new List<Stmt>();

    protected override IEnumerable<Node> GetChildren() => Statements;
}
