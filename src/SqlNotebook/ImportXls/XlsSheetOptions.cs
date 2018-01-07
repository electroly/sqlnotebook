// SQL Notebook
// Copyright (C) 2018 Brian Luft
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
