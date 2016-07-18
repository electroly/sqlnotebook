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

namespace SqlNotebookCoreModules {
    public sealed class RangeModule : GenericSqliteModule {
        public override string GetName() => "range";

        public override string GetCreateTableSql() =>
            "CREATE TABLE range (_start HIDDEN, _count HIDDEN, _step HIDDEN, number PRIMARY KEY)";

        public override int GetHiddenColumnCount() => 3;

        public override IEnumerable<object[]> GetCursor(object[] hiddenValues) {
            var startObj = hiddenValues[0];
            var countObj = hiddenValues[1];
            var stepObj = hiddenValues[2] ?? 1L;

            if (startObj == null) {
                throw new Exception("RANGE: The \"start\" argument is required.");
            }
            if (countObj == null) {
                throw new Exception("RANGE: The \"count\" argument is required.");
            }

            if (!(startObj is Int64)) {
                throw new Exception("RANGE: The \"start\" argument must be an INTEGER value.");
            }
            if (!(countObj is Int64)) {
                throw new Exception("RANGE: The \"count\" argument must be an INTEGER value.");
            }
            if (!(stepObj is Int64)) {
                throw new Exception("RANGE: The \"step\" argument must be an INTEGER value.");
            }

            var start = (Int64)startObj;
            var count = (Int64)countObj;
            var step = (Int64)stepObj;

            for (long steps = 0, value = start; steps < count; steps++, value += step) {
                yield return new object[] { start, count, step, value };
            }
        }
    }
}
