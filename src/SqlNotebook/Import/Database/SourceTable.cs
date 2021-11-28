using System.Text.RegularExpressions;

namespace SqlNotebook.Import.Database;

public sealed class SourceTable {
    private static readonly Regex _whitespaceRegex = new(@"\s+", RegexOptions.Compiled);

    public bool SourceIsTable { get; set; }
    public string SourceSchemaName { get; set; }
    public string SourceTableName { get; set; }
    public bool SourceIsSql { get; set; }
    public string SourceSql { get; set; }
    public string TargetTableName { get; set; }

    private SourceTable() { }

    public static SourceTable FromTable(string schema, string table) =>
        new() {
            SourceIsTable = true,
            SourceSchemaName = schema,
            SourceTableName = table,
            TargetTableName =
                schema != null && schema != "dbo"
                ? $"{schema}.{table}"
                : table,
        };

    public static SourceTable FromSql(string sql, string targetTableName) =>
        new() {
            SourceIsSql = true,
            SourceSql = sql,
            TargetTableName = targetTableName,
        };

    private string SourceDisplayText {
        get {
            if (SourceIsTable) {
                if (SourceSchemaName != null && SourceSchemaName != "dbo") {
                    return $"{SourceSchemaName}.{SourceTableName}";
                }
                return SourceTableName;
            }
            var text = _whitespaceRegex.Replace(SourceSql, " ");
            if (text.Length > 50) {
                text = text[..50];
            }
            return text;
        }
    }

    public string DisplayText =>
        SourceIsTable && SourceDisplayText == TargetTableName
        ? SourceDisplayText
        : $"{SourceDisplayText} → {TargetTableName}";
}
