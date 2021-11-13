using System;

namespace SqlNotebookScript.Interpreter;

public class ScriptException : Exception {
    public ScriptException(string message) : base(message) { }
}
