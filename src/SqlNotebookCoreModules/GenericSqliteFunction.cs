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
    public abstract class GenericSqliteFunction {
        public abstract string Name { get; }
        public abstract int ParamCount { get; }
        public abstract bool IsDeterministic { get; }
        public abstract object Execute(IReadOnlyList<object> args);

        protected string GetStrArg(object arg, string name) {
            if (arg is string) {
                return (string)arg;
            } else {
                throw new Exception($"{Name.ToUpper()}: The argument \"{name}\" must be a TEXT value.");
            }
        }

        protected double GetDblArg(object arg, string name) {
            if (arg is double || arg is Int64) {
                return Convert.ToDouble(arg);
            } else {
                throw new Exception($"{Name.ToUpper()}: The argument \"{name}\" must be a FLOAT value.");
            }
        }
    }
}
