using MySql.Data.MySqlClient;
using SqlNotebookScript.Utils;
using System.Collections.Generic;
using System.Data;

namespace SqlNotebook.Import.Database {
    public sealed class MySqlImportSession : ImportSessionBase<MySqlConnectionStringBuilder> {
        public override string ProductName { get; } = "MySQL";

        public override string GetCreateVirtualTableStatement(string sourceTableName, string notebookTableName) {
            return $"CREATE VIRTUAL TABLE {notebookTableName.DoubleQuote()} USING mysql ('{_builder.ConnectionString.Replace("'", "''")}', '{sourceTableName.Replace("'", "''")}')";
        }

        protected override IDbConnection CreateConnection(MySqlConnectionStringBuilder builder) {
            return new MySqlConnection(builder.ConnectionString);
        }

        protected override void ReadTableNames(IDbConnection connection) {
            var tableNames = new List<string>();
            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = "SELECT table_name FROM information_schema.tables ORDER BY table_name";
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        tableNames.Add(reader.GetString(0));
                    }
                }
            }
            TableNames = tableNames;
        }

        protected override MySqlConnectionStringBuilder CreateBuilder(string connStr) {
            return new MySqlConnectionStringBuilder(connStr);
        }

        protected override string GetDisplayName() {
            return $"{_builder.Database} on {_builder.Server} (MySQL)";
        }

        protected override DatabaseConnectionForm.BasicOptions GetBasicOptions(MySqlConnectionStringBuilder builder) {
            return new DatabaseConnectionForm.BasicOptions {
                Server = builder.Server,
                Database = builder.Database,
                Username = builder.UserID,
                Password = builder.Password
            };
        }

        protected override void SetBasicOptions(MySqlConnectionStringBuilder builder, DatabaseConnectionForm.BasicOptions opt) {
            builder.Server = opt.Server;
            builder.Database = opt.Database;
            builder.UserID = opt.Username;
            builder.Password = opt.Password;
        }
    }
}
