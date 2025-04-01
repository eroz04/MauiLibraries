namespace BaseLibraries.Helpers;

public class Disposable : IDisposable
{
    private bool _isDisposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~Disposable()
    {
        Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed) return;
        if (disposing)
        {
            // dispose
        }
        _isDisposed = true;
    }
}
