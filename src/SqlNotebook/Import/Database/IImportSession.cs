using System.Collections.Generic;
using System.Data.Common;
using System.Windows.Forms;

namespace SqlNotebook.Import.Database;

public interface IImportSession
{
    bool FromConnectForm(IWin32Window owner);
    IReadOnlyList<(string Schema, string Table)> TableNames { get; }
    string GenerateSql(IEnumerable<SourceTable> selectedTables, bool link);
    DbConnection CreateConnection();
    DatabaseConnectionForm.BasicOptions GetBasicOptions(DbConnectionStringBuilder builder);
    void SetBasicOptions(DbConnectionStringBuilder builder, DatabaseConnectionForm.BasicOptions opt);
    void Clear(DbConnectionStringBuilder builder);
}
