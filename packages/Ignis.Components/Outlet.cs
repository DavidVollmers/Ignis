using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public sealed class Outlet : IComponent, IOutlet
{
    private readonly RenderFragment _renderFragment;
    
    private RenderHandle _renderHandle;
    private bool _hasPendingQueuedRender;
    
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Inject] public IHostContext HostContext { get; set; } = null!;

    public Outlet()
    {
        _renderFragment = builder =>
        {
            _hasPendingQueuedRender = false;

            builder.OpenComponent<CascadingValue<IOutlet>>(0);
            builder.AddAttribute(1, nameof(CascadingValue<IOutlet>.IsFixed), true);
            builder.AddAttribute(2, nameof(CascadingValue<IOutlet>.Value), this);
            builder.AddAttribute(3, nameof(CascadingValue<IOutlet>.ChildContent), ChildContent);

            builder.CloseComponent();
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
