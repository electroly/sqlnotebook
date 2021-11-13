using System.Collections.Generic;
using System.Windows.Forms;

namespace SqlNotebook.Import.Database;

public interface IImportSession {
    bool FromConnectForm(IWin32Window owner);
    string GetCreateVirtualTableStatement(string sourceTableName, string notebookTableName);
    IReadOnlyList<string> TableNames { get; }
}
