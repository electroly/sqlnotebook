// SQL Notebook
// Copyright (C) 2017 Brian Luft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.ComponentModel;
using System.Globalization;

namespace SqlNotebook {
    // adapted from http://stackoverflow.com/a/7422801
    public sealed class DescriptionEnumConverter : EnumConverter {
        private Type _enumType;

        public DescriptionEnumConverter(Type type) : base(type) {
            _enumType = type;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destType) => destType == typeof(string);

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType) {
            var fi = _enumType.GetField(Enum.GetName(_enumType, value));
            var dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
            return dna?.Description ?? value.ToString();
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType) => srcType == typeof(string);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            foreach (var fi in _enumType.GetFields()) {
                var dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
                if (dna != null && (string)value == dna.Description) {
                    return Enum.Parse(_enumType, fi.Name);
                }
            }
            return Enum.Parse(_enumType, (string)value);
        }
    }
}
