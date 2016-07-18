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
using System.Text;

namespace SqlNotebookCoreModules {
    public sealed class ReadFileLinesModule : GenericSqliteModule {
        public override string GetName() => "read_file_lines";

        public override int GetHiddenColumnCount() => 2;

        public override string GetCreateTableSql() =>
            "CREATE TABLE read_file_lines (_file_path HIDDEN, _encoding HIDDEN, number PRIMARY KEY, line)";

        public override IEnumerable<object[]> GetCursor(object[] hiddenValues) {
            var filePathObj = hiddenValues[0];
            var encodingObj = hiddenValues[1] ?? 0L;

            if (filePathObj == null) {
                throw new Exception("READ_FILE_LINES: The \"file-path\" argument is required.");
            }
            if (!(filePathObj is string)) {
                throw new Exception("READ_FILE_LINES: The \"file-path\" argument must be a string.");
            }
            if (!(encodingObj is Int64)) {
                throw new Exception("READ_FILE_LINES: The \"encoding\" argument must be an integer.");
            }

            var filePath = (string)filePathObj;
            var encoding = (Int64)encodingObj;

            if (encoding < 0 && encoding > 65535) {
                throw new Exception("READ_FILE_LINES: The \"encoding\" argument must be between 0 and 65535.");
            }

            string[] lines;
            if (encoding == 0) {
                lines = File.ReadAllLines(filePath);
            } else {
                lines = File.ReadAllLines(filePath, Encoding.GetEncoding((int)encoding));
            }
            return lines.Select((x, i) => new object[] { filePath, encoding, i, x });
        }
    }
}
