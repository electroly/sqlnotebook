using System.ComponentModel;

namespace SqlNotebook {
    public enum ImportTableExistsOption {
        [Description("Append new rows")]
        AppendNewRows,

        [Description("Delete existing rows")]
        DeleteExistingRows,

        [Description("Drop table and re-create")]
        DropTable
    }

    public enum ColumnHeadersOption {
        [Description("Cell range includes headers")]
        Present,

        [Description("No column headers")]
        NotPresent
    }

    public enum ImportConversionFailOption {
        [Description("Import the value as text")]
        ImportAsText = 1,

        [Description("Skip the row")]
        SkipRow = 2,

        [Description("Stop import with error")]
        Abort = 3
    }

}
