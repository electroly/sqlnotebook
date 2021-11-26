using System;
using System.Threading;

namespace SqlNotebookScript.Utils;

public sealed class RowProgressUpdateTask : IDisposable {
    private readonly CancellationTokenSource _cts = new();
    private readonly Thread _thread;
    private bool disposedValue;

    public RowProgressUpdateTask(ExecutionStatus.InFlight status, Func<long> countFunc) {
        _thread = new(new ThreadStart(() => {
            try {
                var interval = TimeSpan.FromMilliseconds(100);
                long previous = -1;
                while (!_cts.IsCancellationRequested) {
                    var n = countFunc();
                    if (n != previous) {
                        status.SetStatus($"{n:#,##0} rows");
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

            disposedValue = true;
        }
    }

    // override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~RowProgressUpdateTask() {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose() {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
