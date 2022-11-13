using System.Collections.Generic;

namespace SqlNotebookScript.Interpreter.Ast;

public sealed class ExecuteStmt : Stmt
{
    public string ReturnVariableName { get; set; } // may be null
    public string ScriptName { get; set; }
    public List<ArgumentPair> Arguments { get; set; } = new List<ArgumentPair>();
    protected override bool IsLeaf { get; } = true;
}
