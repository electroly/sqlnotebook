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
using System.IO;
using System.Linq;

namespace SqlNotebookCoreModules {
    public sealed class ListXlsWorksheetsModule : GenericSqliteModule {
        public override string Name => "list_xls_worksheets";

        public override string CreateTableSql =>
            @"CREATE TABLE list_xls_worksheets (_file_path HIDDEN, number INTEGER, name)";

        public override int HiddenColumnCount => 1;

        public override IEnumerable<object[]> Execute(object[] hiddenValues) {
            var filePath = ModUtil.GetStrArg(hiddenValues[0], "file-path", Name);
            if (!File.Exists(filePath)) {
                throw new Exception($"{Name.ToUpper()}: File not found.");
            }

            try {
                return XlsUtil.ReadWorksheetNames(filePath).Select((name, i) => new object[] {
                    filePath, i, name
                }).ToList();
            } catch (Exception ex) {
                throw new Exception($"{Name.ToUpper()}: {ex.Message}");
            }
        }
    }
}
