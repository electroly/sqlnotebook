using System;
using System.Collections.Generic;

namespace SqlNotebookScript.TableFunctions {
    public sealed class RangeFunction : CustomTableFunction {
        public override string Name => "range";

        public override string CreateTableSql =>
            "CREATE TABLE range (_start HIDDEN, _count HIDDEN, _step HIDDEN, number PRIMARY KEY)";

        public override int HiddenColumnCount => 3;

        public override IEnumerable<object[]> Execute(object[] hiddenValues) {
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
