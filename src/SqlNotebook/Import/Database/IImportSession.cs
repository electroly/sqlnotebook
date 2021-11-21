using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using SqlNotebookScript.Core;

namespace SqlNotebook.Import.Database;

public interface IImportSession {
    bool FromConnectForm(IWin32Window owner);
    string GetCreateVirtualTableStatement(string sourceTableName, string notebookTableName);
    IReadOnlyList<string> TableNames { get; }
    void ImportTableByCopyingData(string quotedSourceTable, string targetTable, Notebook notebook,
        CancellationToken cancel);
}
