using System;
using System.Collections.Generic;
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.ScalarFunctions;

public abstract class MonadicMathFunction : CustomScalarFunction
{
    public override int ParamCount => 1;
    public override bool IsDeterministic => true;

    public override object Execute(IReadOnlyList<object> args)
    {
        var x = ArgUtil.GetFloatArg(args[0], "x", Name);
        return Execute(x);
    }

    public abstract double Execute(double x);
}

public abstract class DyadicMathFunction : CustomScalarFunction
{
    public override int ParamCount => 2;
    public override bool IsDeterministic => true;

    public override object Execute(IReadOnlyList<object> args)
    {
        var x = ArgUtil.GetFloatArg(args[0], "x", Name);
        var y = ArgUtil.GetFloatArg(args[1], "y", Name);
        return Execute(x, y);
    }

    public abstract double Execute(double x, double y);
}

public sealed class RoundFunction : MonadicMathFunction
{
    public override string Name => "round";

    public override double Execute(double x) => Math.Round(x);
}

public sealed class SignFunction : MonadicMathFunction
{
    public override string Name => "sign";

    public override double Execute(double x) => Math.Sign(x);
}
