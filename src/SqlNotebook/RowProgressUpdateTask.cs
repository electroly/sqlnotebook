using System;
using System.Threading;

namespace SqlNotebook;

public sealed class RowProgressUpdateTask : IDisposable {
    private readonly CancellationTokenSource _cts = new();
    private readonly Thread _thread;
    private string _targetTruncated;
    private bool disposedValue;

    public RowProgressUpdateTask(Func<long> countFunc) : this(null, countFunc) {}

    public RowProgressUpdateTask(string target, Func<long> countFunc) {
        SetTarget(target);
        _thread = new(new ThreadStart(() => {
            try {
                var interval = TimeSpan.FromMilliseconds(100);
                long previous = -1;
                while (!_cts.IsCancellationRequested) {
                    var n = countFunc();
                    if (n != previous) {
                        WaitForm.ProgressText =
                            n == 0 ? null :
                            _targetTruncated == null ? $"{n:#,##0} rows"
                            : $"{_targetTruncated}\r\n{n:#,##0} rows";
                        previous = n;
                    }
                    _cts.Token.WaitHandle.WaitOne(interval);
                }
            } catch { }
        }));
        _thread.Start();
    }

    public void SetTarget(string target) {
        _targetTruncated =
            target == null ? null :
            target.Length > 50 ? $"{target[..50]}..."
            : target;
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
