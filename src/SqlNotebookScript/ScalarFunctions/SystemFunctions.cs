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
using System.IO;
using System.Text;
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.ScalarFunctions {
    public sealed class ChooseFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "choose";
        public override int ParamCount => -1;
        public override object Execute(IReadOnlyList<object> args) {
            if (args.Count < 3) {
                throw new Exception($"{Name.ToUpper()}: At least 3 arguments are required.");
            }
            var index = ArgUtil.GetInt32Arg(args[0], "index", Name);
            return index >= 1 && index < args.Count ? args[index] : DBNull.Value;
        }
    }

    public sealed class IsNumericFunction : CustomScalarFunction {
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

    public sealed class NewIdFunction : CustomScalarFunction {
        public override bool IsDeterministic => false;
        public override string Name => "newid";
        public override int ParamCount => 0;
        public override object Execute(IReadOnlyList<object> args) => Guid.NewGuid().ToString().ToUpper();
    }

    public sealed class HostNameFunction : CustomScalarFunction {
        public override bool IsDeterministic => false;
        public override string Name => "host_name";
        public override int ParamCount => 0;
        public override object Execute(IReadOnlyList<object> args) => Environment.MachineName;
    }

    public sealed class UserNameFunction : CustomScalarFunction {
        public override bool IsDeterministic => false;
        public override string Name => "user_name";
        public override int ParamCount => 0;
        public override object Execute(IReadOnlyList<object> args) => Environment.UserName;
    }

    public sealed class ReadFileText1Function : CustomScalarFunction {
        public override bool IsDeterministic => false;
        public override string Name => "read_file_text";
        public override int ParamCount => 1;
        public override object Execute(IReadOnlyList<object> args) {
            var filePath = ArgUtil.GetStrArg(args[0], "file-path", Name);
            try {
                return File.ReadAllText(filePath);
            } catch (Exception ex) {
                throw new Exception($"{Name.ToUpper()}: {ex.Message}");
            }
        }
    }

    public sealed class ReadFileText2Function : CustomScalarFunction {
        public override bool IsDeterministic => false;
        public override string Name => "read_file_text";
        public override int ParamCount => 2;
        public override object Execute(IReadOnlyList<object> args) {
            var filePath = ArgUtil.GetStrArg(args[0], "file-path", Name);
            var encodingNum = ArgUtil.GetInt32Arg(args[0], "file-encoding", Name);
            
            try {
                if (encodingNum < 0 || encodingNum > 65535) {
                    throw new Exception($"The \"file-encoding\" argument must be between 0 and 65535.");
                }
                var encoding = Encoding.GetEncoding(encodingNum);
                return File.ReadAllText(filePath, encoding);
            } catch (Exception ex) {
                throw new Exception($"{Name.ToUpper()}: {ex.Message}");
            }
        }
    }

    public sealed class Split2Function : CustomScalarFunction {
        public override bool IsDeterministic => false;
        public override string Name => "split";
        public override int ParamCount => 2;
        public override object Execute(IReadOnlyList<object> args) {
            var text = ArgUtil.GetStrArg(args[0], "text", Name);
            var separator = ArgUtil.GetStrArg(args[1], "separator", Name);
            if (separator.Length == 0) {
                throw new Exception($"{Name.ToUpper()}: The argument \"separator\" must not be an empty string.");
            }
            var splitted = text.Split(new[] { separator }, StringSplitOptions.None);
            return ArrayUtil.ConvertToSqlArray(splitted);
        }
    }

    public sealed class Split3Function : CustomScalarFunction {
        public override bool IsDeterministic => false;
        public override string Name => "split";
        public override int ParamCount => 3;
        public override object Execute(IReadOnlyList<object> args) {
            var text = ArgUtil.GetStrArg(args[0], "text", Name);
            var separator = ArgUtil.GetStrArg(args[1], "separator", Name);
            var whichSubstring = ArgUtil.GetInt32Arg(args[2], "which-substring", Name);
            if (separator.Length == 0) {
                throw new Exception($"{Name.ToUpper()}: The argument \"separator\" must not be an empty string.");
            }
            if (whichSubstring < 0) {
                throw new Exception($"{Name.ToUpper()}: The argument \"which-substring\" must not be negative.");
            }
            var splitted = text.Split(new[] { separator }, StringSplitOptions.None);
            return whichSubstring < splitted.Length ? ArrayUtil.ConvertToSqlArray(splitted) : null;
        }
    }

}
