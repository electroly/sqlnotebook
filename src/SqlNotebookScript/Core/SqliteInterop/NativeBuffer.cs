using System;
using System.Runtime.InteropServices;

namespace SqlNotebookScript.Core.SqliteInterop;

public sealed class NativeBuffer : IDisposable
{
    private bool _disposedValue;

    public IntPtr Ptr { get; }

    public NativeBuffer(int size)
    {
        Ptr = Marshal.AllocHGlobal(size);
        NativeMethods.ZeroMemory(Ptr, (IntPtr)size);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // dispose managed state (managed objects)
            }

            // free unmanaged resources (unmanaged objects) and override finalizer
            // set large fields to null
            if (Ptr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(Ptr);
            }
            _disposedValue = true;
        }
    }

    // override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~NativeBuffer()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
