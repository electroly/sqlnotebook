using System.Data;

namespace SqlNotebookScript.Core.AdoModules;

public record class AdoCursorMetadata {
    public AdoTableMetadata TableMetadata { get; set; }
    public IDbConnection Connection { get; set; }
    public IDbCommand Command { get; set; }
    public IDataReader Reader { get; set; }
    public string ReaderSql { get; set; }
    public bool IsEof { get; set; }
}
