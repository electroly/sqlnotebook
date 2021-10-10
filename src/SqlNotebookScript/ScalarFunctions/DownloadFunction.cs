using System.Collections.Generic;
using System.Net;

namespace SqlNotebookScript.ScalarFunctions {
    public sealed class DownloadFunction : CustomScalarFunction {
        public override string Name => "download";

        public override int ParamCount => 1;

        public override bool IsDeterministic => false;

        public override object Execute(IReadOnlyList<object> args) {
            var url = args[0].ToString();
            using (var client = new WebClient()) {
                return client.DownloadString(url);
            }
        }
    }
}
