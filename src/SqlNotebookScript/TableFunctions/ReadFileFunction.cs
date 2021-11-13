using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SqlNotebookScript.TableFunctions;

public sealed class ReadFileFunction : CustomTableFunction {
    public override string Name => "read_file";

    public override int HiddenColumnCount => 2;

    public override string CreateTableSql =>
        "CREATE TABLE read_file (_file_path HIDDEN, _encoding HIDDEN, number PRIMARY KEY, line)";

    public override IEnumerable<object[]> Execute(object[] hiddenValues) {
        var filePathObj = hiddenValues[0];
        var encodingObj = hiddenValues[1] ?? 0L;

        if (filePathObj == null) {
            throw new Exception($"{Name.ToUpper()}: The \"file-path\" argument is required.");
        }
        if (!(filePathObj is string)) {
            throw new Exception($"{Name.ToUpper()}: The \"file-path\" argument must be a string.");
        }
        if (!(encodingObj is Int64)) {
            throw new Exception($"{Name.ToUpper()}: The \"encoding\" argument must be an integer.");
        }

        var filePath = (string)filePathObj;
        var encoding = (Int64)encodingObj;

        if (encoding < 0 && encoding > 65535) {
            throw new Exception($"{Name.ToUpper()}: The \"encoding\" argument must be between 0 and 65535.");
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
