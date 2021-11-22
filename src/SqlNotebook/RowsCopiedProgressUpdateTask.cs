using System;
using System.Threading;

namespace SqlNotebook;

public sealed class RowsCopiedProgressUpdateTask : IDisposable {
    private readonly CancellationTokenSource _cts = new();
    private readonly Thread _thread;

    public RowsCopiedProgressUpdateTask(string targetTable, Func<long> rowsCopiedFunc) {
        var targetTableTruncated =
            targetTable.Length > 50
            ? $"{targetTable[..50]}..."
            : targetTable;
        _thread = new(new ThreadStart(() => {
            try {
                var interval = TimeSpan.FromMilliseconds(100);
                while (!_cts.IsCancellationRequested) {
                    var n = rowsCopiedFunc();
                    WaitForm.ProgressText = $"{targetTableTruncated}\r\n{n:#,##0} rows copied";
                    _cts.Token.WaitHandle.WaitOne(interval);
                }
            } catch { }
        }));
        _thread.Start();
    }

    public void Dispose() {
        _cts.Cancel();
        _thread.Join();
        _cts.Dispose();
        WaitForm.ProgressText = null;
    }
}
