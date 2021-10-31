using System;
using System.Linq;

namespace SqlNotebookScript.Utils {
    public static class Extensions {
        public static string GetExceptionMessage(this Exception self) {
            if (self is AggregateException agg) {
                return string.Join(Environment.NewLine, agg.InnerExceptions.Select(x => x.GetExceptionMessage()));
            } else {
                return self.Message;
            }
        }
    }
}
