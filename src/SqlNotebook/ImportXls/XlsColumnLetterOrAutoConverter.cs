using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using SqlNotebookScript.Utils;

namespace SqlNotebook.ImportXls {
    public sealed class XlsColumnLetterOrAutoConverter : TypeConverter {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) =>
            destinationType == typeof(string);

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            var columnIndex = XlsUtil.ColumnRefToIndex(value); // zero-based
            return columnIndex.HasValue && columnIndex >= 0 && columnIndex < 16384
                ? XlsUtil.GetColumnRefString(value)
                : "";
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
            sourceType == typeof(string);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            var str = XlsUtil.GetColumnRefString(value);
            return (str?.Any() ?? false) ? str : null;
        }
    }
}
