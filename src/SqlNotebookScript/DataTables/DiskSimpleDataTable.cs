using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using SqlNotebookScript.Utils;

namespace SqlNotebookScript.DataTables;

public sealed class DiskSimpleDataTable : SimpleDataTable
{
    private readonly Timer _inactivityTimer;
    private readonly List<object[]> _rowCache = new(100); // First 100 rows cached in memory.
    private bool _disposed = false;
    private TempFile _headerTempFile = new(".header");
    private TempFile _dataTempFile = new(".data");
    private int _dataCount = 0;

    // These are disposed and nulled when the table finishes loading.
    private BinaryWriter _headerWriter;
    private Stream _dataWriterStream;
    private BinaryWriter _dataWriter;

    // These are populated when loading is finished.
    private bool _loadingFinished = false;

    // These are created and disposed on-demand after the table finishes loading.
    private readonly object _readerLock = new();
    private Stream _headerReaderStream;
    private BinaryReader _headerReader;
    private Stream _dataReaderStream;
    private BinaryReader _dataReader;

    public DiskSimpleDataTable(IReadOnlyList<string> columns)
    {
        Columns = columns;
        var dict = new Dictionary<string, int>();
        int i = 0;
        foreach (var columnName in columns)
        {
            dict[columnName] = i++;
        }
        _columnIndices = dict;

        var headerStream = File.Create(_headerTempFile.FilePath);
        _headerWriter = new(headerStream, Encoding.UTF8, leaveOpen: false);

        _dataWriterStream = File.Create(_dataTempFile.FilePath);
        _dataWriter = new(_dataWriterStream, Encoding.UTF8, leaveOpen: false);

        _inactivityTimer = new(OnInactivityTimerTick, null, Timeout.Infinite, Timeout.Infinite);
    }

    private void OnInactivityTimerTick(object obj)
    {
        _inactivityTimer.Change(Timeout.Infinite, Timeout.Infinite);
        lock (_readerLock)
        {
            _headerReader?.Dispose();
            _headerReader = null;
            _headerReaderStream?.Dispose();
            _headerReaderStream = null;
            _dataReader?.Dispose();
            _dataReader = null;
            _dataReaderStream?.Dispose();
            _dataReaderStream = null;
        }
    }

    public void LoadRow(object[] row)
    {
        if (_loadingFinished)
        {
            throw new InvalidOperationException("Loading has finished.");
        }

        _headerWriter.Write(_dataWriterStream.Position);
        for (var i = 0; i < row.Length; i++)
        {
            _dataWriter.WriteScalar(row[i]);
        }

        if (_rowCache.Count < 100)
        {
            var rowCopy = new object[row.Length];
            Array.Copy(row, rowCopy, row.Length);
            _rowCache.Add(rowCopy);
        }

        _dataCount++;
    }

    public void FinishLoading(long? fullCount = null)
    {
        FullCount = fullCount ?? _dataCount;
        _loadingFinished = true;
        _headerWriter?.Dispose();
        _headerWriter = null;
        _dataWriter?.Dispose();
        _dataWriter = null;
        _dataWriterStream?.Dispose();
        _dataWriterStream = null;
        Rows = new DiskSimpleDataTableList(this, _dataCount);
    }

    private void CreateReaders()
    {
        if (_headerReader == null)
        {
            _headerReaderStream = File.OpenRead(_headerTempFile.FilePath);
            _headerReader = new(_headerReaderStream);
            _dataReaderStream = File.OpenRead(_dataTempFile.FilePath);
            _dataReader = new(_dataReaderStream);
        }

        _inactivityTimer.Change(2000, Timeout.Infinite);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _headerWriter?.Dispose();
            _headerWriter = null;
            _headerReader?.Dispose();
            _headerReader = null;
            _headerReaderStream?.Dispose();
            _headerReaderStream = null;
            _headerTempFile?.Dispose();
            _headerTempFile = null;

            _dataWriter?.Dispose();
            _dataWriter = null;
            _dataReader?.Dispose();
            _dataReader = null;
            _dataReaderStream?.Dispose();
            _dataReaderStream = null;
            _dataWriterStream?.Dispose();
            _dataWriterStream = null;
            _dataTempFile?.Dispose();
            _dataTempFile = null;

            _inactivityTimer.Dispose();
            _disposed = true;
        }
    }

    public void GetRow(long index, object[] row)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(DiskSimpleDataTable));
        }
        if (!_loadingFinished)
        {
            throw new InvalidOperationException("Loading has not finished.");
        }

        if (index < _rowCache.Count)
        {
            var cachedRow = _rowCache[(int)index];
            Array.Copy(cachedRow, row, cachedRow.Length);
            return;
        }

        lock (_readerLock)
        {
            CreateReaders();
            _headerReaderStream.Seek(index * sizeof(long), SeekOrigin.Begin);
            var dataOffset = _headerReader.ReadInt64();
            _dataReaderStream.Seek(dataOffset, SeekOrigin.Begin);
            for (var i = 0; i < Columns.Count; i++)
            {
                row[i] = _dataReader.ReadScalar();
            }
        }
    }
}
