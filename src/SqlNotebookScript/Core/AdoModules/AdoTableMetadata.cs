using System;
using System.Collections.Generic;
using System.Data;

namespace SqlNotebookScript.Core.AdoModules;

public record class AdoTableMetadata
{
    public string ConnectionString { get; init; }
    public string AdoTableName { get; init; }
    public string AdoSchemaName { get; init; }
    public List<string> ColumnNames { get; init; }
    public Func<string, IDbConnection> ConnectionCreator { get; init; }
    public long InitialRowCount { get; init; }
    public char EscapeChar { get; init; }

    /// <summary>
    /// Column name => estimated rows as a fraction (0-1) of the total row count
    /// </summary>
    public Dictionary<string, double> EstimatedRowsPercentByColumn { get; init; }
}
