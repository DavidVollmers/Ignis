using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components;

public abstract class IgnisComponentBase : IComponent
{
    private readonly RenderFragment _renderFragment;

    private bool _isInitialized;
    private RenderHandle _renderHandle;
    private bool _hasPendingQueuedRender;

    protected virtual bool ShouldRender => true;

    [Inject] public IHostContext HostContext { get; set; } = null!;

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
        if (_renderHandle.IsInitialized)
        {
            throw new InvalidOperationException("Render handle already initialized.");
        }

        _renderHandle = renderHandle;
    }

    public async Task SetParametersAsync(ParameterView parameters)
    {
        if (HostContext.IsPrerendering) return;

        parameters.SetParameterProperties(this);

        if (!_isInitialized)
        {
            await OnInitializeCoreAsync();
        }
        else
        {
            await OnUpdateCoreAsync();
        }

        Update();
    }

    protected void Update(bool async = false)
    {
        if (async)
        {
            _renderHandle.Dispatcher.InvokeAsync(QueueRender);
            return;
        }

        QueueRender();
    }

    private void QueueRender()
    {
        if (_hasPendingQueuedRender || !_renderHandle.IsInitialized || !ShouldRender) return;

        _hasPendingQueuedRender = true;

        try
        {
            _renderHandle.Render(_renderFragment);
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

    protected virtual void OnUpdated()
    {
    }

    internal virtual Task OnInitializeCoreAsync()
    {
        _isInitialized = true;

        OnInitialized();

        return Task.CompletedTask;
    }

    internal virtual Task OnUpdateCoreAsync()
    {
        OnUpdated();

        return Task.CompletedTask;
    }
}
