﻿using SqlNotebook.Properties;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;

namespace SqlNotebook.Import.Database {
    public abstract class ImportSessionBase<TConnectionStringBuilder> : IImportSession
        where TConnectionStringBuilder : DbConnectionStringBuilder, new() {

        public abstract string ProductName { get; }
        public abstract string GetCreateVirtualTableStatement(string sourceTableName, string notebookTableName);
        protected abstract IDbConnection CreateConnection(TConnectionStringBuilder builder);
        protected abstract void ReadTableNames(IDbConnection connection);
        protected abstract TConnectionStringBuilder CreateBuilder(string connStr);
        protected abstract string GetDisplayName();
        protected abstract DatabaseConnectionForm.BasicOptions GetBasicOptions(TConnectionStringBuilder builder);
        protected abstract void SetBasicOptions(TConnectionStringBuilder builder, DatabaseConnectionForm.BasicOptions opt);
        protected abstract string GetDefaultConnectionString();
        protected abstract void SetDefaultConnectionString(string str);

        protected TConnectionStringBuilder _builder = new();

        public bool FromConnectForm(IWin32Window owner) {
            var successfulConnect = false;

            do {
                var initialConnectionString = GetDefaultConnectionString();
                if (!string.IsNullOrWhiteSpace(initialConnectionString)) {
                    try {
                        _builder.ConnectionString = initialConnectionString;
                    } catch { }
                }

                using DatabaseConnectionForm f = new(
                    $"Connect to {ProductName}", 
                    _builder,
                    b => GetBasicOptions((TConnectionStringBuilder)b),
                    (b, o) => SetBasicOptions((TConnectionStringBuilder)b, o));
                if (f.ShowDialog(owner) != DialogResult.OK) {
                    return false;
                }

                // Save the connection string for next time, even if it fails.
                SetDefaultConnectionString(_builder.ConnectionString);
                Settings.Default.Save();

                successfulConnect = DoConnect(owner);
            } while (!successfulConnect);

            return true;
        }

        private bool DoConnect(IWin32Window owner) {
            WaitForm.GoWithCancelByWalkingAway(
                owner, "Database Connection", $"Accessing {ProductName}...", out var success,
                () => {
                using var connection = CreateConnection(_builder);
                connection.Open();
                ReadTableNames(connection);
            });
            return success;
        }

        public IReadOnlyList<string> TableNames { get; protected set; } = new string[0];
    }
}