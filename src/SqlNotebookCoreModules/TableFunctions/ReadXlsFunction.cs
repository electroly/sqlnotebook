// SQL Notebook
// Copyright (C) 2016 Brian Luft
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
using System.Collections.Generic;
using System.Linq;
using SqlNotebookCoreModules.Utils;

namespace SqlNotebookCoreModules.TableFunctions {
    public sealed class ReadXlsFunction : CustomTableFunction {
        public override string Name => "read_xls";

        public override string CreateTableSql =>
            @"CREATE TABLE read_xls (_file_path HIDDEN, _worksheet_index HIDDEN,
            row_number INTEGER PRIMARY KEY, row_array BLOB, num_columns INTEGER)";

        public override int HiddenColumnCount => 2;

        public override IEnumerable<object[]> Execute(object[] hiddenValues) {
            var filePath = ArgUtil.GetStrArg(hiddenValues[0], "file-path", Name);
            var worksheetIndex = ArgUtil.GetInt32Arg(hiddenValues[1], "worksheet-index", Name);

            try {
                return XlsUtil.ReadWorksheet(filePath, worksheetIndex).Select((x, i) => new object[] {
                    filePath,
                    worksheetIndex,
                    i + 1,
                    ArrayUtil.ConvertToSqlArray(x ?? new object[0]),
                    x?.Length ?? 0
                });
            } catch (Exception ex) {
                throw new Exception($"{Name.ToUpper()}: {ex.Message}");
            }
        }
    }
}
