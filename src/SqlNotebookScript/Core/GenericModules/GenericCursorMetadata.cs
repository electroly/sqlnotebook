using System.Collections.Generic;

namespace SqlNotebookScript.Core.GenericModules;

public record class GenericCursorMetadata {
    public GenericTableMetadata TableMetadata { get; set; }
    public IEnumerator<object[]> Enumerator { get; set; }
    public bool Eof { get; set; }
}
