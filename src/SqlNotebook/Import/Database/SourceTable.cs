using System.Text.RegularExpressions;

namespace SqlNotebook.Import.Database;

public sealed class SourceTable {
    private static readonly Regex _whitespaceRegex = new(@"\s+", RegexOptions.Compiled);

    public bool SourceIsTable { get; set; }
    public string SourceTableName { get; set; }
    public bool SourceIsSql { get; set; }
    public string SourceSql { get; set; }
    public string TargetTableName { get; set; }

    private SourceTable() { }

    public static SourceTable FromTable(string table) =>
        new() {
            SourceIsTable = true,
            SourceTableName = table,
            TargetTableName = table,
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
        SourceIsTable && SourceTableName == TargetTableName
        ? SourceDisplayText
        : $"{SourceDisplayText} → {TargetTableName}";
}
