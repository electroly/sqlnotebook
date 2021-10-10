using System;
using System.Collections.Generic;
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.ScalarFunctions {
    public sealed class ArrayFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "array";
        public override int ParamCount => -1;
        public override object Execute(IReadOnlyList<object> args) => ArrayUtil.ConvertToSqlArray(args);
    }

    public sealed class ArrayGetFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "array_get";
        public override int ParamCount => 2;
        public override object Execute(IReadOnlyList<object> args) {
            var blob = ArgUtil.GetBlobArg(args[0], "array", Name);
            var index = ArgUtil.GetInt32Arg(args[1], "element-index", Name);
            var count = ArrayUtil.GetArrayCount(blob);
            return index < 0 || index >= count ? null : ArrayUtil.GetArrayElement(blob, index);
        }
    }

    public sealed class ArraySetFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "array_set";
        public override int ParamCount => 3;
        public override object Execute(IReadOnlyList<object> args) {
            var blob = ArgUtil.GetBlobArg(args[0], "array", Name);
            var index = ArgUtil.GetInt32Arg(args[1], "element-index", Name);
            var value = args[2];
            var count = ArrayUtil.GetArrayCount(blob);
            if (index < 0 || index >= count) {
                throw new Exception($"{Name.ToUpper()}: Argument \"element-index\" is out of range.");
            } else {
                return ArrayUtil.SliceArrayElements(blob, index, 1, new[] { value });
            }
        }
    }

    public sealed class ArrayInsertFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "array_insert";
        public override int ParamCount => -1;
        public override object Execute(IReadOnlyList<object> args) {
            if (args.Count < 3) {
                throw new Exception($"{Name.ToUpper()}: At least 3 argument are required.");
            }
            var blob = ArgUtil.GetBlobArg(args[0], "array", Name);
            var index = ArgUtil.GetInt32Arg(args[1], "element-index", Name);
            var values = new List<object>();
            for (int i = 2; i < args.Count; i++) {
                values.Add(args[i]);
            }
            var count = ArrayUtil.GetArrayCount(blob);
            if (index < 0 || index > count) {
                throw new Exception($"{Name.ToUpper()}: Argument \"element-index\" is out of range.");
            } else {
                return ArrayUtil.SliceArrayElements(blob, index, 0, values);
            }
        }
    }

    public sealed class ArrayAppendFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "array_append";
        public override int ParamCount => -1;
        public override object Execute(IReadOnlyList<object> args) {
            if (args.Count < 2) {
                throw new Exception($"{Name.ToUpper()}: At least 2 arguments are required.");
            }
            var blob = ArgUtil.GetBlobArg(args[0], "array", Name);
            var values = new List<object>();
            for (int i = 1; i < args.Count; i++) {
                values.Add(args[i]);
            }
            var count = ArrayUtil.GetArrayCount(blob);
            return ArrayUtil.SliceArrayElements(blob, count, 0, values);
        }
    }

    public sealed class ArrayCountFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "array_count";
        public override int ParamCount => 1;
        public override object Execute(IReadOnlyList<object> args) {
            var blob = ArgUtil.GetBlobArg(args[0], "array", Name);
            return ArrayUtil.GetArrayCount(blob);
        }
    }

    public sealed class ArrayConcat1Function : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "array_concat";
        public override int ParamCount => 1;
        public override object Execute(IReadOnlyList<object> args) {
            var blob = ArgUtil.GetBlobArg(args[0], "array", Name);
            var elements = ArrayUtil.GetArrayElements(blob);
            return string.Join("", elements);
        }
    }

    public sealed class ArrayConcat2Function : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "array_concat";
        public override int ParamCount => 2;
        public override object Execute(IReadOnlyList<object> args) {
            var blob = ArgUtil.GetBlobArg(args[0], "array", Name);
            var separator = ArgUtil.GetStrArg(args[1], "separator", Name);
            var elements = ArrayUtil.GetArrayElements(blob);
            return string.Join(separator, elements);
        }
    }

    public sealed class ArrayMergeFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "array_merge";
        public override int ParamCount => -1;
        public override object Execute(IReadOnlyList<object> args) {
            if (args.Count < 2) {
                throw new Exception($"{Name.ToUpper()}: At least 2 arguments are required.");
            }
            var values = new List<object>();
            for (int i = 0; i < args.Count; i++) {
                var blob = ArgUtil.GetBlobArg(args[i], $"array #{i+1}", Name);
                var blobValues = ArrayUtil.GetArrayElements(blob);
                values.AddRange(blobValues);
            }
            return ArrayUtil.ConvertToSqlArray(values);
        }
    }
}
