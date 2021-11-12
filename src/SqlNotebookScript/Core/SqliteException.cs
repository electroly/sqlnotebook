using System;

namespace SqlNotebookScript.Core;

public sealed class SqliteException : Exception {
    public SqliteException(string message) : base(message) { }
}
