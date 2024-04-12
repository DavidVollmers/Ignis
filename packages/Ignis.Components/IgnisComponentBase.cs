using System.Reflection;
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
        if (HostContext.IsPrerendering)
        {
            var prerenderAttribute = GetType().GetCustomAttribute<PrerenderAttribute>();
            if (prerenderAttribute == null) return;

            parameters.SetParameterProperties(this);

            UpdateCore(async: false);

            return;
        }

        parameters.SetParameterProperties(this);

        if (!_isInitialized)
        {
            await OnInitializedCoreAsync();
        }

        await OnUpdateCoreAsync();

        UpdateCore(async: false);
    }

    protected internal virtual void Update(bool async = false) => UpdateCore(async);

    private void UpdateCore(bool async)
    {
        if (async)
        {
            _ = _renderHandle.Dispatcher.InvokeAsync(QueueRender);
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

    protected virtual void OnUpdate()
    {
    }

    internal virtual Task OnInitializedCoreAsync()
    {
        _isInitialized = true;

        OnInitialized();

        return Task.CompletedTask;
    }

    internal virtual Task OnUpdateCoreAsync()
    {
        OnUpdate();

        HostContext.OnComponentUpdate(this);

        return Task.CompletedTask;
    }
}
