using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using SqlNotebook.Properties;

namespace SqlNotebook.Import.Database;

public sealed class SqlServerImportSession : ImportSessionBase<SqlConnectionStringBuilder> {
    public override string ProductName { get; } = "Microsoft SQL Server";
    protected override string SqliteModuleName => "mssql";

    public override DbConnection CreateConnection() {
        return new SqlConnection(_builder.ConnectionString);
    }

    protected override void ReadTableNames(IDbConnection connection) {
        var tableNames = new List<string>();
        using (var cmd = connection.CreateCommand()) {
            cmd.CommandText = "SELECT table_name, table_schema FROM INFORMATION_SCHEMA.TABLES ORDER BY table_name";
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) {
                var tableName = reader.GetString(0);
                var tableSchema = reader.GetString(1);
                var combinedName = tableSchema == "dbo" ? tableName : $"{tableSchema}.{tableName}";
                tableNames.Add(combinedName);
            }
        }
        TableNames = tableNames;
    }

    protected override SqlConnectionStringBuilder CreateBuilder(string connStr) {
        return new SqlConnectionStringBuilder(connStr);
    }

    protected override string GetDisplayName() {
        return $"{_builder.InitialCatalog} on {_builder.DataSource} (Microsoft SQL Server)";
    }

    protected override DatabaseConnectionForm.BasicOptions GetBasicOptions(SqlConnectionStringBuilder builder) {
        return new DatabaseConnectionForm.BasicOptions {
            Server = builder.DataSource,
            Database = builder.InitialCatalog,
            Username = builder.UserID,
            Password = builder.Password,
            UseWindowsAuth = builder.IntegratedSecurity
        };
    }

    protected override void SetBasicOptions(SqlConnectionStringBuilder builder, DatabaseConnectionForm.BasicOptions opt) {
        builder.DataSource = opt.Server;
        builder.InitialCatalog = opt.Database;
        builder.UserID = opt.Username;
        builder.Password = opt.Password;
        builder.IntegratedSecurity = opt.UseWindowsAuth;
    }

    protected override string GetDefaultConnectionString() => Settings.Default.SqlServerLastConnectionString;
    protected override void SetDefaultConnectionString(string str) => Settings.Default.SqlServerLastConnectionString = str;
}
