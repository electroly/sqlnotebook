using System;
using System.Linq;

namespace SqlNotebookScript.Utils {
    public static class Extensions {
        public static string GetCombinedMessage(this Exception self) =>
            self is AggregateException a
            ? string.Join(Environment.NewLine, a.InnerExceptions.Select(x => x.GetCombinedMessage()))
            : self.Message;
    }
}
