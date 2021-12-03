using System;
using System.Collections.Generic;
using System.IO;
using SqlNotebookScript.DataTables;
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.Interpreter;

public sealed class ScriptOutput : RefCounted {
    public List<SimpleDataTable> DataTables { get; } = new();
    public List<string> TextOutput { get; } = new();
    public object ScalarResult { get; set; }

    public void Append(ScriptOutput x) {
        ThrowIfDisposed();

        // move SDT references; no change to their ref counts
        foreach (var dt in x.DataTables) {
            DataTables.Add(dt);
        }
        x.DataTables.Clear();

        TextOutput.AddRange(x.TextOutput);
        x.TextOutput.Clear();
    }

    public byte[] GetBytes() {
        using MemoryStream stream = new();
        using (BinaryWriter writer = new(stream)) {
            Serialize(writer);
        }
        return stream.ToArray();
    }

    private void Serialize(BinaryWriter writer) {
        ThrowIfDisposed();

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

    public static ScriptOutput FromBytes(byte[] bytes) {
        using MemoryStream stream = new(bytes);
        using BinaryReader reader = new(stream);
        return Deserialize(reader);
    }

    private static ScriptOutput Deserialize(BinaryReader reader) {
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

    public ScriptOutput TakeRef() {
        AddRef();
        return this;
    }

    protected override void OnDispose() {
        foreach (var sdt in DataTables) {
            sdt.Dispose();
        }
        DataTables.Clear();
        TextOutput.Clear();
        ScalarResult = null;
    }
}
