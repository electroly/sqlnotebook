// SQL Notebook
// Copyright (C) 2016 Brian Luft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;

namespace SqlNotebookCoreModules {
    public abstract class MonadicMathFunction : GenericSqliteFunction {
        public override int ParamCount => 1;
        public override bool IsDeterministic => true;
        public override object Execute(IReadOnlyList<object> args) {
            var x = GetDblArg(args[0], "x");
            return Execute(x);
        }
        public abstract double Execute(double x);
    }

    public abstract class DyadicMathFunction : GenericSqliteFunction {
        public override int ParamCount => 2;
        public override bool IsDeterministic => true;
        public override object Execute(IReadOnlyList<object> args) {
            var x = GetDblArg(args[0], "x");
            var y = GetDblArg(args[1], "y");
            return Execute(x, y);
        }
        public abstract double Execute(double x, double y);
    }

    public sealed class AcosFunction : MonadicMathFunction {
        public override string Name => "acos";
        public override double Execute(double x) => Math.Acos(x);
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
