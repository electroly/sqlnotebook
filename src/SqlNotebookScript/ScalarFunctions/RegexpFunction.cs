using System.Collections.Generic;
using System.Text.RegularExpressions;
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.ScalarFunctions {
    public sealed class RegexpFunction : CustomScalarFunction {
        public override bool IsDeterministic => true;
        public override string Name => "regexp";
        public override int ParamCount => 2;

        public override object Execute(IReadOnlyList<object> args) {
            var pattern = ArgUtil.GetStrArg(args[0], "pattern", Name);
            var text = args[1].ToString();
            var regex = new Regex(pattern);
            return regex.IsMatch(text) ? 1 : 0;
        }
    }
}
