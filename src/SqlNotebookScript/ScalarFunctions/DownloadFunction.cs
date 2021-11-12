using System.Collections.Generic;

namespace SqlNotebookScript.ScalarFunctions {
    public sealed class DownloadFunction : CustomScalarFunction {
        public override string Name => "download";

        public override int ParamCount => 1;

        public override bool IsDeterministic => false;

        public override object Execute(IReadOnlyList<object> args) {
            var url = args[0].ToString();
            return SharedHttp.Client.GetStringAsync(url).GetAwaiter().GetResult();
        }
    }
}
