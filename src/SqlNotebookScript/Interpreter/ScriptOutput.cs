using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using SqlNotebookScript.DataTables;
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.Interpreter;

public sealed class ScriptOutput : IDisposable {
    public List<SimpleDataTable> DataTables { get; } = new();
    public List<string> TextOutput { get; } = new();
    public object ScalarResult { get; set; }

    public void Append(ScriptOutput x) {
        DataTables.AddRange(x.DataTables);
        x.DataTables.Clear();
        TextOutput.AddRange(x.TextOutput);
        x.TextOutput.Clear();
    }

    public void WriteCsv(StreamWriter s, Action onRow, CancellationToken cancel) {
        foreach (var dt in DataTables) {
            cancel.ThrowIfCancellationRequested();
            s.WriteLine(string.Join(",", dt.Columns.Select(CsvUtil.EscapeCsv)));
            CsvUtil.WriteCsv(dt.Rows, s, onRow);
        }
    }

    public void Serialize(BinaryWriter writer) {
        // DataTables
        writer.Write(DataTables.Count);
        foreach (var sdt in DataTables) {
            sdt.Serialize(writer);
        }

        // TextOutput
        writer.Write(TextOutput.Count);
        foreach (var x in TextOutput) {
            writer.Write(x);
        }

        // ScalarResult
        if (ScalarResult == null) {
            writer.Write((byte)0);
        } else {
            writer.Write((byte)1);
            writer.WriteScalar(ScalarResult);
        }
    }

    public static ScriptOutput Deserialize(BinaryReader reader) {
        ScriptOutput scriptOutput = new();

        // DataTables
        var dataTableCount = reader.ReadInt32();
        for (var i = 0; i < dataTableCount; i++) {
            scriptOutput.DataTables.Add(SimpleDataTable.Deserialize(reader));
        }

        // TextOutput
        var textOutputCount = reader.ReadInt32();
        for (var i = 0; i < textOutputCount; i++) {
            scriptOutput.TextOutput.Add(reader.ReadString());
        }

        // ScalarResult
        var hasScalarResult = reader.ReadByte();
        if (hasScalarResult == 1) {
            scriptOutput.ScalarResult = reader.ReadScalar();
        } else if (hasScalarResult != 0) {
            throw new Exception("The file is corrupt.");
        }

        return scriptOutput;
    }

    public void Dispose() {
        foreach (var sdt in DataTables) {
            sdt.Dispose();
        }
    }
}
