using System.Collections.Generic;

namespace SqlNotebookScript;

public sealed class ScriptParameterRecord {
    public string ScriptName { get; set; }
    public List<string> ParamNames { get; set; } = new();
}
