using System.Collections.Generic;

namespace SqlNotebookScript;

public sealed class NotebookUserData {
    public List<NotebookItemRecord> Items { get; set; } = new();
    public List<ScriptParameterRecord> ScriptParameters { get; set; } = new();
    public LastErrorRecord LastError { get; set; } = new();
}
