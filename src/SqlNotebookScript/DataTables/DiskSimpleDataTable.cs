using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.DataTables;

public sealed class DiskSimpleDataTable : SimpleDataTable, IDisposable {
    private readonly TempFile _headerTempFile = new();
    private readonly TempFile _dataTempFile = new();
    private readonly Stream _headerStream;
    private readonly Stream _dataStream;
    private int _dataCount = 0;

    // These are disposed and nulled when the table finishes loading.
    private BinaryWriter _headerWriter;
    private BinaryWriter _dataWriter;
    private bool _loadingFinished = false;

    // These are created when the table finishes loading.
    private BinaryReader _headerReader;
    private BinaryReader _dataReader;

    public DiskSimpleDataTable(IReadOnlyList<string> columns) {
        Columns = columns;

        _headerStream = File.Create(_headerTempFile.FilePath);
        _headerWriter = new(_headerStream, Encoding.UTF8, leaveOpen: true);
        
        _dataStream = File.Create(_dataTempFile.FilePath);
        _dataWriter = new(_dataStream, Encoding.UTF8, leaveOpen: true);
    }

    public void LoadRow(object[] row) {
        if (_loadingFinished) {
            throw new InvalidOperationException("Loading has finished.");
        }

        _headerWriter.Write(_dataStream.Position);
        for (var i = 0; i < row.Length; i++) {
            _dataWriter.WriteScalar(row[i]);
        }

        _dataCount++;
    }

    public void FinishLoading(long? fullCount = null) {
        FullCount = fullCount ?? _dataCount;
        _loadingFinished = true;
        _headerWriter?.Dispose();
        _headerWriter = null;
        _dataWriter?.Dispose();
        _dataWriter = null;
        _headerReader = new(_headerStream);
        _dataReader = new(_dataStream);
        Rows = new DiskSimpleDataTableList(this, _dataCount);
    }

    public override void Dispose() {
        _headerWriter?.Dispose();
        _headerReader?.Dispose();
        _headerStream?.Dispose();
        _headerTempFile?.Dispose();

        _dataWriter?.Dispose();
        _dataReader?.Dispose();
        _dataStream?.Dispose();
        _dataTempFile?.Dispose();
    }

    public void GetRow(long index, object[] row) {
        if (!_loadingFinished) {
            throw new InvalidOperationException("Loading has not finished.");
        }

        _headerStream.Seek(index * sizeof(long), SeekOrigin.Begin);
        long dataOffset = _headerReader.ReadInt64();
        _dataStream.Seek(dataOffset, SeekOrigin.Begin);
        for (var i = 0; i < Columns.Count; i++) {
            row[i] = _dataReader.ReadScalar();
        }
    }
}
