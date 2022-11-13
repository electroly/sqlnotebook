using System;
using System.Data;

namespace SqlNotebookScript.Core.AdoModules;

public record class AdoCreateInfo
{
    public Func<string, IDbConnection> ConnectionCreator { get; init; }

    /// <remarks>
    /// Should contain {0} as a placeholder for the escaped table name including surrounding quotes.
    /// </remarks>
    public string SelectRandomSampleSql { get; init; }

    // For SQL Server we need to fall back to a non-TABLESAMPLE for views
    public string SelectRandomSampleSqlFallback { get; init; }

    // " or `
    public char EscapeChar;
}
