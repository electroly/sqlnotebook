using System;
using System.Collections.Generic;

namespace SqlNotebookScript.TableFunctions;

public sealed class IntegerRangeFunction : CustomTableFunction {
    public override string Name => "integer_range";

    public override string CreateTableSql =>
        "CREATE TABLE integer_range (_start HIDDEN, _count HIDDEN, _step HIDDEN, number PRIMARY KEY)";

    public override int HiddenColumnCount => 3;

    public override IEnumerable<object[]> Execute(object[] hiddenValues) {
        var startObj = hiddenValues[0];
        var countObj = hiddenValues[1];
        var stepObj = hiddenValues[2] ?? 1L;

        if (startObj == null) {
            throw new Exception("INTEGER_RANGE: The \"start\" argument is required.");
        }
        if (countObj == null) {
            throw new Exception("INTEGER_RANGE: The \"count\" argument is required.");
        }

        if (startObj is not long) {
            throw new Exception("INTEGER_RANGE: The \"start\" argument must be an INTEGER value.");
        }
        if (countObj is not long) {
            throw new Exception("INTEGER_RANGE: The \"count\" argument must be an INTEGER value.");
        }
        if (stepObj is not long) {
            throw new Exception("INTEGER_RANGE: The \"step\" argument must be an INTEGER value.");
        }

        var start = (long)startObj;
        var count = (long)countObj;
        var step = (long)stepObj;

        for (long steps = 0, value = start; steps < count; steps++, value += step) {
            yield return new object[] { start, count, step, value };
        }
    }
}
