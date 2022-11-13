namespace SqlNotebookScript.Interpreter.Ast;

public sealed class BlockStmt : Stmt
{
    public Block Block { get; set; }

    protected override Node GetChild() => Block;
}
