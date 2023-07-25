using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components;

public abstract class IgnisComponentBase : IComponent
{
    private readonly RenderFragment _renderFragment;

    private bool _isInitialized;
    private RenderHandle _renderHandle;
    private bool _hasPendingQueuedRender;

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
        if (HostContext == null)
        {
            throw new InvalidOperationException(
                "Missing host context. Make sure to either install Ignis.Components.Server or Ignis.Components.WebAssembly.");
        }

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
            await InitializeAsync();
        }
        else
        {
            await UpdateAsync();
        }

        ForceUpdate();
    }

    protected void ForceUpdate(bool async = false)
    {
        if (async)
        {
            _renderHandle.Dispatcher.InvokeAsync(ForceUpdateCore);
            return;
        }

        ForceUpdateCore();
    }

    private void ForceUpdateCore()
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

    protected virtual void BuildRenderTree(RenderTreeBuilder builder)
    {
    }

    protected virtual void OnInitialized()
    {
    }

    protected virtual void OnUpdated()
    {
    }

    internal virtual Task InitializeAsync()
    {
        _isInitialized = true;

        OnInitialized();

        return Task.CompletedTask;
    }

    internal virtual Task UpdateAsync()
    {
        OnUpdated();

        return Task.CompletedTask;
    }
}
