using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Npgsql;
using SqlNotebook.Properties;

namespace SqlNotebook.Import.Database;

public sealed class PostgreSqlImportSession : ImportSessionBase<NpgsqlConnectionStringBuilder> {
    public override string ProductName { get; } = "PostgreSQL";
    protected override string SqliteModuleName => "pgsql";

    public override DbConnection CreateConnection() {
        return new NpgsqlConnection(_builder.ConnectionString);
    }

    protected override void ReadTableNames(IDbConnection connection) {
        List<(string Schema, string Table)> tableNames = new();
        using (var cmd = connection.CreateCommand()) {
            cmd.CommandText = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' ORDER BY table_name";
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) {
                tableNames.Add((null, reader.GetString(0)));
            }
        }
        TableNames = tableNames;
    }

    protected override NpgsqlConnectionStringBuilder CreateBuilder(string connStr) {
        return new NpgsqlConnectionStringBuilder(connStr);
    }

    protected override string GetDisplayName() {
        return $"{_builder.Database} on {_builder.Host} (PostgreSQL)";
    }

    protected override DatabaseConnectionForm.BasicOptions GetBasicOptions(NpgsqlConnectionStringBuilder builder) {
        return new DatabaseConnectionForm.BasicOptions {
            Server = builder.Host,
            Database = builder.Database,
            Username = builder.Username,
            Password = builder.Password
        };
    }

    protected override void SetBasicOptions(NpgsqlConnectionStringBuilder builder, DatabaseConnectionForm.BasicOptions opt) {
        builder.Host = opt.Server;
        builder.Database = opt.Database;
        builder.Username = opt.Username;
        builder.Password = opt.Password;
    }

    protected override string GetDefaultConnectionString() => Settings.Default.PostgreSqlLastConnectionString;
    protected override void SetDefaultConnectionString(string str) => Settings.Default.PostgreSqlLastConnectionString = str;
}
