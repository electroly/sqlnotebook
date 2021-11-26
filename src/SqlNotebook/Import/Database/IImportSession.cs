using System.Collections.Generic;
using System.Windows.Forms;

namespace SqlNotebook.Import.Database;

public record struct SelectedTable(string SourceName, string TargetName);

public interface IImportSession {
    bool FromConnectForm(IWin32Window owner);
    IReadOnlyList<string> TableNames { get; }
    string GenerateSql(IEnumerable<SelectedTable> selectedTables, bool link);
}
