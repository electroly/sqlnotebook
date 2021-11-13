namespace SqlNotebookScript.Interpreter;

public sealed class UncaughtErrorScriptException : ScriptException {
    public object ErrorMessage { get; }
    public UncaughtErrorScriptException(object errorMessage)
        : base(errorMessage.ToString()) {
        ErrorMessage = errorMessage;
    }
}
