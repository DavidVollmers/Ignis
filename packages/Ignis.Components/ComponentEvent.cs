namespace Ignis.Components;

public class ComponentEvent : IComponentEvent, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public bool DefaultPrevented => this._cancellationTokenSource.IsCancellationRequested;

    public void PreventDefault()
    {
        this._cancellationTokenSource.Cancel();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
