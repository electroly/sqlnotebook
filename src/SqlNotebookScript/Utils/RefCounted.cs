//#define DEBUG_REFCOUNTS
using System;
using System.Threading;

namespace SqlNotebookScript.Utils;

public abstract class RefCounted : IDisposable
{
#if DEBUG_REFCOUNTS
    private readonly Guid _id = Guid.NewGuid();
#endif
    private long _refCount = 1;

    public void Dispose()
    {
        var decremented = Interlocked.Decrement(ref _refCount);
        if (decremented == 0)
        {
#if DEBUG_REFCOUNTS
            System.Diagnostics.Debug.WriteLine($"{GetType().Name}.Dispose(): refcount = 0; disposing... ({_id})");
#endif
            OnDispose();
        }
        else
        {
#if DEBUG_REFCOUNTS
            System.Diagnostics.Debug.WriteLine($"{GetType().Name}.Dispose(): refcount = {decremented} ({_id})");
#endif
        }
        GC.SuppressFinalize(this);
    }

    protected abstract void OnDispose();

    protected void ThrowIfDisposed()
    {
        if (Interlocked.Read(ref _refCount) <= 0)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }

    public void AddRef()
    {
        var incremented = Interlocked.Increment(ref _refCount);
#if DEBUG_REFCOUNTS
        System.Diagnostics.Debug.WriteLine($"{GetType().Name}.AddRef(): refcount = {incremented} ({_id})");
#endif
        if (incremented <= 1)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }
}
