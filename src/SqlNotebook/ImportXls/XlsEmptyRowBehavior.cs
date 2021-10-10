using System.ComponentModel;

namespace SqlNotebook.ImportXls {
    public enum XlsEmptyRowBehavior {
        [Description("End at first empty row")]
        EndAtFirstEmptyRow,

        [Description("Continue past empty rows")]
        ContinuePastEmptyRows
    }
}
