using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SqlNotebookScript.Core.SqliteInterop;

public sealed class NativeString : IDisposable {
    private static readonly UTF8Encoding _utf8Encoding = new(encoderShouldEmitUTF8Identifier: false);
    private static readonly UnicodeEncoding _utf16Encoding = new(bigEndian: false, byteOrderMark: false);
    private bool _disposedValue;

    public IntPtr Ptr { get; private set; }
    
    /// <remarks>Excludes the NUL terminator.</remarks>
    public int ByteCount { get; }

    public NativeString(string str, bool utf16 = false) {
        byte[] bytes;
        if (utf16) {
            bytes = _utf16Encoding.GetBytes(str);
        } else {
            bytes = _utf8Encoding.GetBytes(str);
        }
        ByteCount = bytes.Length;
        Ptr = Marshal.AllocHGlobal(ByteCount + (utf16 ? 2 : 1));
        Marshal.Copy(bytes, 0, Ptr, bytes.Length);
        if (utf16) {
            Marshal.WriteInt16(Ptr + bytes.Length, 0);
        } else {
            Marshal.WriteByte(Ptr + bytes.Length, 0);
        }
    }

    private void Dispose(bool disposing) {
        if (!_disposedValue) {
            if (disposing) {
                // dispose managed state (managed objects)
            }

            // free unmanaged resources (unmanaged objects) and override finalizer
            // set large fields to null
            if (Ptr != IntPtr.Zero) {
                Marshal.FreeHGlobal(Ptr);
                Ptr = IntPtr.Zero;
            }
            _disposedValue = true;
        }
    }

    // override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~NativeString() {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose() {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public static IntPtr NewUnmanagedUtf8StringWithSqliteAllocator(string str) {
        var utf8Bytes = _utf8Encoding.GetBytes(str);
        var unmanaged = NativeMethods.sqlite3_malloc(utf8Bytes.Length + 1);
        NativeMethods.ZeroMemory(unmanaged, (IntPtr)(utf8Bytes.Length + 1));
        Marshal.Copy(utf8Bytes, 0, unmanaged, utf8Bytes.Length);
        return unmanaged;
    }
}
