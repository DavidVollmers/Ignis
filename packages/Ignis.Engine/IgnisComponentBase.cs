using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Engine;

public abstract class IgnisComponentBase : IComponent, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly RenderFragment _renderFragment;

    private bool _isInitialized;
    private RenderHandle? _renderHandle;
    private bool _hasPendingQueuedRender;

    protected IgnisComponentBase()
    {
        _renderFragment = builder =>
        {
            _hasPendingQueuedRender = false;
            BuildRenderTree(builder);
        };
    }
    
    public void Attach(RenderHandle renderHandle)
    {
        if (_renderHandle is { IsInitialized: true })
        {
            throw new InvalidOperationException("Render handle already initialized.");
        }

        _renderHandle = renderHandle;

        CheckInitializedAsync(_cancellationTokenSource.Token).GetAwaiter().GetResult();
    }

    public async Task SetParametersAsync(ParameterView parameters)
    {
        await CheckInitializedAsync(_cancellationTokenSource.Token);

        parameters.SetParameterProperties(this);
        
        OnUpdated();
        
        await OnUpdatedAsync(_cancellationTokenSource.Token);

        ForceUpdate();
    }

    private async Task CheckInitializedAsync(CancellationToken cancellationToken)
    {
        if (_isInitialized || _renderHandle is not { IsInitialized: true }) return;

        _isInitialized = true;

        // ReSharper disable once MethodHasAsyncOverloadWithCancellation
        OnInitialized();

        await OnInitializedAsync(cancellationToken);
    }
    
    protected void ForceUpdate()
    {
        if (_hasPendingQueuedRender) return;
        
        _hasPendingQueuedRender = true;

        try
        {
            _renderHandle?.Render(_renderFragment);
        }
        catch
        {
            _hasPendingQueuedRender = false;
            throw;
        }
    }

    protected virtual void BuildRenderTree(RenderTreeBuilder builder)
    {
    }

    protected virtual void OnInitialized()
    {
    }

    protected virtual Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected virtual void OnUpdated()
    {
    }

    protected virtual Task OnUpdatedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
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

    ~IgnisComponentBase()
    {
        Dispose(false);
    }
}
