using System;
using System.Collections.Generic;

namespace SqlNotebookScript.Interpreter;

public sealed class ScriptEnv {
    // local variables and script parameters. keys are in lowercase.
    public Dictionary<string, object> Vars { get; } = new Dictionary<string, object>();

    // the names of script parameters, in lowercase.  this is populated as DECLARE PARAMETER statements are ran,
    // to ensure that the same parameter is not declared twice.
    public HashSet<string> ParNames { get; } = new HashSet<string>();

    public ScriptOutput Output { get; private set; } = new ScriptOutput();

    public bool DidReturn { get; set; }
    public bool DidBreak { get; set; }
    public bool DidContinue { get; set; }

    public Action OnRow;
}
