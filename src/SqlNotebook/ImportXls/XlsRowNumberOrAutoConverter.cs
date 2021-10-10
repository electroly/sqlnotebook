using System;
using System.ComponentModel;
using System.Globalization;

namespace SqlNotebook.ImportXls {
    public sealed class XlsRowNumberOrAutoConverter : TypeConverter {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) =>
            destinationType == typeof(string);

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) =>
            ((int?)value)?.ToString() ?? "";

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
            sourceType == typeof(string);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) =>
            int.TryParse((string)value, out var num) && num >= 1 && num <= 1048576 ? num : default(int?);
    }
}
