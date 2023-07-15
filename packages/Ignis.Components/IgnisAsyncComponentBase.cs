namespace Ignis.Components;

public abstract class IgnisAsyncComponentBase : IgnisComponentBase, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    protected virtual Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnUpdatedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    internal override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await OnInitializedAsync(_cancellationTokenSource.Token);
    }

    internal override async Task UpdateAsync()
    {
        await base.UpdateAsync();

        await OnUpdatedAsync(_cancellationTokenSource.Token);
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

    ~IgnisAsyncComponentBase()
    {
        Dispose(false);
    }
}
