using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public class Outlet : IComponent, IOutlet
{
    private readonly RenderFragment _renderFragment;
    
    private RenderHandle _renderHandle;
    private bool _hasPendingQueuedRender;
    
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Inject] public IHostContext HostContext { get; set; } = null!;

    protected Outlet()
    {
        _renderFragment = builder =>
        {
            _hasPendingQueuedRender = false;
            
            ChildContent?.Invoke(builder);
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

    public Task SetParametersAsync(ParameterView parameters)
    {
        if (HostContext.IsPrerendering) return Task.CompletedTask;
        
        Update();
        
        return Task.CompletedTask;
    }

    public void Update(bool async = false)
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
        if (_hasPendingQueuedRender || !_renderHandle.IsInitialized) return;

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
}
