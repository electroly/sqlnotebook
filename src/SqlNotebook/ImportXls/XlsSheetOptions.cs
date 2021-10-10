using System.ComponentModel;

namespace SqlNotebook.ImportXls {
    public sealed class XlsSheetOptions {
        [DisplayName("First row (optional)")]
        [DefaultValue(null)]
        [TypeConverter(typeof(XlsRowNumberOrAutoConverter))]
        public int? FirstRowNumber { get; set; }

        [DisplayName("Last row (optional)")]
        [DefaultValue(null)]
        [TypeConverter(typeof(XlsRowNumberOrAutoConverter))]
        public int? LastRowNumber { get; set; }

        [DisplayName("First column (optional)")]
        [DefaultValue(null)]
        [TypeConverter(typeof(XlsColumnLetterOrAutoConverter))]
        public string FirstColumnLetter { get; set; }

        [DisplayName("Last column (optional)")]
        [DefaultValue(null)]
        [TypeConverter(typeof(XlsColumnLetterOrAutoConverter))]
        public string LastColumnLetter { get; set; }

        [DisplayName("Column headers")]
        [DefaultValue(ColumnHeadersOption.Present)]
        [TypeConverter(typeof(DescriptionEnumConverter))]
        public ColumnHeadersOption ColumnHeaders { get; set; } = ColumnHeadersOption.Present;

        [DisplayName("Empty row behavior")]
        [DefaultValue(XlsEmptyRowBehavior.EndAtFirstEmptyRow)]
        [TypeConverter(typeof(DescriptionEnumConverter))]
        public XlsEmptyRowBehavior EmptyRowBehavior { get; set; } = XlsEmptyRowBehavior.EndAtFirstEmptyRow;
    }
}
