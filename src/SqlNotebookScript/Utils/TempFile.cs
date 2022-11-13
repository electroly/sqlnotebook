using System;
using System.IO;
using System.Threading.Tasks;

namespace SqlNotebookScript.Utils;

public sealed class TempFile : IDisposable
{
    private bool _disposedValue;

    public string FilePath { get; }

    public TempFile(string extension)
    {
        FilePath = NotebookTempFiles.GetTempFilePath(extension);
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
            try
            {
                File.Delete(FilePath);
            }
            catch
            {
                // try again in a little bit
                Task.Delay(TimeSpan.FromSeconds(1))
                    .ContinueWith(t =>
                    {
                        try
                        {
                            File.Delete(FilePath);
                        }
                        catch
                        {
                            // oh well. we'll clean it up at the end of the process, or on the next run.
                        }
                    });
            }
            _disposedValue = true;
        }
    }

    // override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~TempFile()
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
