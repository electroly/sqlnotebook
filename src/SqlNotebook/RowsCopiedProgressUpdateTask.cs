using System;
using System.Threading;

namespace SqlNotebook;

public sealed class RowsCopiedProgressUpdateTask : IDisposable {
    private readonly CancellationTokenSource _cts = new();
    private readonly Thread _thread;
    private bool disposedValue;

    public RowsCopiedProgressUpdateTask(string targetTable, Func<long> rowsCopiedFunc) {
        var targetTableTruncated =
            targetTable.Length > 50
            ? $"{targetTable[..50]}..."
            : targetTable;
        _thread = new(new ThreadStart(() => {
            try {
                var interval = TimeSpan.FromMilliseconds(100);
                long previous = 0;
                while (!_cts.IsCancellationRequested) {
                    var n = rowsCopiedFunc();
                    if (n > previous) {
                        WaitForm.ProgressText = $"{targetTableTruncated}\r\n{n:#,##0} rows copied";
                        previous = n;
                    }
                    _cts.Token.WaitHandle.WaitOne(interval);
                }
            } catch { }
        }));
        _thread.Start();
    }

    private void Dispose(bool disposing) {
        if (!disposedValue) {
            if (disposing) {
                // dispose managed state (managed objects)
            }

            // free unmanaged resources (unmanaged objects) and override finalizer
            // set large fields to null
            _cts.Cancel();
            _thread.Join();
            _cts.Dispose();
            WaitForm.ProgressText = null;

            disposedValue = true;
        }
    }

    // override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~RowsCopiedProgressUpdateTask() {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose() {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
