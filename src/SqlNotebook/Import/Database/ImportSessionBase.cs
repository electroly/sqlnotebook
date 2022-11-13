using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Windows.Forms;
using SqlNotebook.Properties;
using SqlNotebookScript.Utils;

namespace SqlNotebook.Import.Database;

public abstract partial class ImportSessionBase<TConnectionStringBuilder> : IImportSession
    where TConnectionStringBuilder : DbConnectionStringBuilder, new()
{
    public abstract string ProductName { get; }
    public IReadOnlyList<(string Schema, string Table)> TableNames { get; protected set; } =
        Array.Empty<(string Schema, string Table)>();
    public abstract DbConnection CreateConnection();
    protected abstract void ReadTableNames(IDbConnection connection);
    protected abstract TConnectionStringBuilder CreateBuilder(string connStr);
    protected abstract string GetDisplayName();
    protected abstract DatabaseConnectionForm.BasicOptions GetBasicOptions(TConnectionStringBuilder builder);
    protected abstract void SetBasicOptions(TConnectionStringBuilder builder, DatabaseConnectionForm.BasicOptions opt);
    protected abstract string GetDefaultConnectionString();
    protected abstract void SetDefaultConnectionString(string str);
    protected abstract string SqliteModuleName { get; }

    protected TConnectionStringBuilder _builder = new();

    public ImportSessionBase()
    {
        Clear(_builder);
    }

    public bool FromConnectForm(IWin32Window owner)
    {
        var successfulConnect = false;

        do
        {
            var initialConnectionString = GetDefaultConnectionString();
            if (!string.IsNullOrWhiteSpace(initialConnectionString))
            {
                try
                {
                    _builder.ConnectionString = initialConnectionString;
                }
                catch { }
            }
            else
            {
                Clear(_builder);
            }

            using DatabaseConnectionForm f = new($"Connect to {ProductName}", _builder, this);
            if (f.ShowDialog(owner) != DialogResult.OK)
            {
                return false;
            }

            // Save the connection string for next time, even if it fails.
            SetDefaultConnectionString(_builder.ConnectionString);
            Settings.Default.Save();

            successfulConnect = DoConnect(owner);
        } while (!successfulConnect);

        return true;
    }

    private bool DoConnect(IWin32Window owner)
    {
        WaitForm.GoWithCancelByWalkingAway(
            owner,
            "Database Connection",
            $"Accessing {ProductName}...",
            out var success,
            () =>
            {
                using var connection = CreateConnection();
                connection.Open();
                ReadTableNames(connection);
            }
        );
        return success;
    }

    public string GenerateSql(IEnumerable<SourceTable> selectedTables, bool link)
    {
        StringBuilder sb = new();
        sb.Append("DECLARE @cs = ");
        sb.Append(_builder.ConnectionString.SingleQuote());
        sb.Append(";\r\n\r\n");
        foreach (var sourceTable in selectedTables)
        {
            if (sourceTable.SourceIsSql && link)
            {
                throw new ExceptionEx(
                    "Live links to custom queries are not supported.",
                    "Choose the \"Copy source data into notebook\" method instead."
                );
            }
            sb.Append("DROP TABLE IF EXISTS ");
            sb.Append(sourceTable.TargetTableName.DoubleQuote());
            sb.Append(";\r\n");
            sb.Append("IMPORT DATABASE ");
            sb.Append(SqliteModuleName.SingleQuote());
            sb.Append(" CONNECTION @cs ");
            if (sourceTable.SourceIsTable)
            {
                if (sourceTable.SourceSchemaName != null)
                {
                    sb.Append("SCHEMA ");
                    sb.Append(sourceTable.SourceSchemaName.SingleQuote());
                    sb.Append(' ');
                }
                sb.Append("TABLE ");
                sb.Append(sourceTable.SourceTableName.SingleQuote());
            }
            else if (sourceTable.SourceIsSql)
            {
                sb.Append("QUERY ");
                sb.Append(sourceTable.SourceSql.SingleQuote());
            }
            else
            {
                throw new Exception("Unexpected source table; neither table nor SQL?");
            }
            if (sourceTable.SourceIsSql || sourceTable.SourceTableName != sourceTable.TargetTableName)
            {
                sb.Append(" INTO ");
                sb.Append(sourceTable.TargetTableName.DoubleQuote());
            }
            if (link)
            {
                sb.Append(" OPTIONS (LINK: 1)");
            }
            sb.Append(";\r\n\r\n");
        }
        return sb.ToString();
    }

    DatabaseConnectionForm.BasicOptions IImportSession.GetBasicOptions(DbConnectionStringBuilder builder)
    {
        return GetBasicOptions((TConnectionStringBuilder)builder);
    }

    void IImportSession.SetBasicOptions(DbConnectionStringBuilder builder, DatabaseConnectionForm.BasicOptions opt)
    {
        SetBasicOptions((TConnectionStringBuilder)builder, opt);
    }

    public virtual void Clear(DbConnectionStringBuilder builder)
    {
        builder.Clear();
    }
}
