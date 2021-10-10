using System;
using System.Collections.Generic;
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.ScalarFunctions {
    public abstract class MonadicMathFunction : CustomScalarFunction {
        public override int ParamCount => 1;
        public override bool IsDeterministic => true;
        public override object Execute(IReadOnlyList<object> args) {
            var x = ArgUtil.GetFloatArg(args[0], "x", Name);
            return Execute(x);
        }
        public abstract double Execute(double x);
    }

    public abstract class DyadicMathFunction : CustomScalarFunction {
        public override int ParamCount => 2;
        public override bool IsDeterministic => true;
        public override object Execute(IReadOnlyList<object> args) {
            var x = ArgUtil.GetFloatArg(args[0], "x", Name);
            var y = ArgUtil.GetFloatArg(args[1], "y", Name);
            return Execute(x, y);
        }
        public abstract double Execute(double x, double y);
    }

    public sealed class AcosFunction : MonadicMathFunction {
        public override string Name => "acos";
        public override double Execute(double x) => Math.Acos(x);
    }

    public sealed class AsinFunction : MonadicMathFunction {
        public override string Name => "asin";
        public override double Execute(double x) => Math.Asin(x);
    }

    public sealed class AtanFunction : MonadicMathFunction {
        public override string Name => "atan";
        public override double Execute(double x) => Math.Atan(x);
    }

    public sealed class Atan2Function : DyadicMathFunction {
        public override string Name => "atan2";
        public override double Execute(double x, double y) => Math.Atan2(x, y);
    }

    public sealed class CeilingFunction : MonadicMathFunction {
        public override string Name => "ceiling";
        public override double Execute(double x) => Math.Ceiling(x);
    }

    public sealed class CosFunction : MonadicMathFunction {
        public override string Name => "cos";
        public override double Execute(double x) => Math.Cos(x);
    }

    public sealed class CoshFunction : MonadicMathFunction {
        public override string Name => "cosh";
        public override double Execute(double x) => Math.Cosh(x);
    }

    public sealed class ExpFunction : MonadicMathFunction {
        public override string Name => "exp";
        public override double Execute(double x) => Math.Exp(x);
    }

    public sealed class FloorFunction : MonadicMathFunction {
        public override string Name => "floor";
        public override double Execute(double x) => Math.Floor(x);
    }

    public sealed class LogFunction : MonadicMathFunction {
        public override string Name => "log";
        public override double Execute(double x) => Math.Log(x);
    }

    public sealed class Log10Function : MonadicMathFunction {
        public override string Name => "log10";
        public override double Execute(double x) => Math.Log10(x);
    }

    public sealed class PowFunction : DyadicMathFunction {
        public override string Name => "pow";
        public override double Execute(double x, double y) => Math.Pow(x, y);
    }

    public sealed class RoundFunction : MonadicMathFunction {
        public override string Name => "round";
        public override double Execute(double x) => Math.Round(x);
    }

    public sealed class SignFunction : MonadicMathFunction {
        public override string Name => "sign";
        public override double Execute(double x) => Math.Sign(x);
    }

    public sealed class SinFunction : MonadicMathFunction {
        public override string Name => "sin";
        public override double Execute(double x) => Math.Sin(x);
    }

    public sealed class SinhFunction : MonadicMathFunction {
        public override string Name => "sinh";
        public override double Execute(double x) => Math.Sinh(x);
    }

    public sealed class SqrtFunction : MonadicMathFunction {
        public override string Name => "sqrt";
        public override double Execute(double x) => Math.Sqrt(x);
    }

    public sealed class TanFunction : MonadicMathFunction {
        public override string Name => "tan";
        public override double Execute(double x) => Math.Tan(x);
    }

    public sealed class TanhFunction : MonadicMathFunction {
        public override string Name => "tanh";
        public override double Execute(double x) => Math.Tanh(x);
    }
}
