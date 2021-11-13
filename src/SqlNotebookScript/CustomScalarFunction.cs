using System.Collections.Generic;

namespace SqlNotebookScript;

public abstract class CustomScalarFunction {
    public abstract string Name { get; }
    public abstract int ParamCount { get; }
    public abstract bool IsDeterministic { get; }
    public abstract object Execute(IReadOnlyList<object> args);
}
