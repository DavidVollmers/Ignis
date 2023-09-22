namespace Ignis.Components;

public abstract class IgnisAsyncComponentBase : IgnisComponentBase, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    protected CancellationToken CancellationToken => _cancellationTokenSource.Token;

    protected virtual Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnUpdateAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    internal override async Task OnInitializedCoreAsync()
    {
        await base.OnInitializedCoreAsync();

        if (_cancellationTokenSource.IsCancellationRequested) return;

        await OnInitializedAsync(_cancellationTokenSource.Token);
    }

    internal override async Task OnUpdateCoreAsync()
    {
        await base.OnUpdateCoreAsync();

        if (_cancellationTokenSource.IsCancellationRequested) return;

        await OnUpdateAsync(_cancellationTokenSource.Token);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
