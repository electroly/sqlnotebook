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
    public sealed class ChooseFunction : GenericSqliteFunction {
        public override bool IsDeterministic => true;
        public override string Name => "choose";
        public override int ParamCount => -1;
        public override object Execute(IReadOnlyList<object> args) {
            if (args.Count < 3) {
                throw new Exception($"{Name.ToUpper()}: At least 3 arguments are required.");
            }
            var index = ModUtil.GetInt32Arg(args[0], "index", Name);
            return index >= 1 && index < args.Count ? args[index] : DBNull.Value;
        }
    }

    public sealed class IsNumericFunction : GenericSqliteFunction {
        public override bool IsDeterministic => true;
        public override string Name => "isnumeric";
        public override int ParamCount => 1;
        public override object Execute(IReadOnlyList<object> args) {
            var arg = args[0];
            decimal value;
            if (arg is int || arg is long || arg is float || arg is double) {
                return 1;
            } else if (decimal.TryParse(arg.ToString(), out value)) {
                return 1;
            } else {
                return 0;
            }
        }
    }

    public sealed class NewIdFunction : GenericSqliteFunction {
        public override bool IsDeterministic => false;
        public override string Name => "newid";
        public override int ParamCount => 0;
        public override object Execute(IReadOnlyList<object> args) => Guid.NewGuid().ToString().ToUpper();
    }

    public sealed class HostNameFunction : GenericSqliteFunction {
        public override bool IsDeterministic => false;
        public override string Name => "host_name";
        public override int ParamCount => 0;
        public override object Execute(IReadOnlyList<object> args) => Environment.MachineName;
    }

    public sealed class UserNameFunction : GenericSqliteFunction {
        public override bool IsDeterministic => false;
        public override string Name => "user_name";
        public override int ParamCount => 0;
        public override object Execute(IReadOnlyList<object> args) => Environment.UserName;
    }
}
