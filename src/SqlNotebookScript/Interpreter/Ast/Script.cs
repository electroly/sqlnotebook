namespace SqlNotebookScript.Interpreter.Ast;

public sealed class Script : Node
{
    public Block Block { get; set; }

    protected override Node GetChild() => Block;
}
