using System.ComponentModel;

namespace SqlNotebookScript.Utils;

public enum BlankValuesOption
{
    [Description("Import as empty string")]
    EmptyString = 1,

    [Description("Import as NULL")]
    Null = 2,

    [Description("Import as NULL for non-TEXT columns only")]
    EmptyStringOrNull = 3,
}
