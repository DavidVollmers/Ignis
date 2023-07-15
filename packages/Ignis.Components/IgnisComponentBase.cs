using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components;

public abstract class IgnisComponentBase : IComponent
{
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
    }

    public async Task SetParametersAsync(ParameterView parameters)
    {
        if (!_isInitialized)
        {
            await InitializeAsync();
        }
        
        parameters.SetParameterProperties(this);

        await UpdateAsync();

        ForceUpdate();
    }

    protected void ForceUpdate()
    {
        if (_hasPendingQueuedRender || _renderHandle is not { IsInitialized: true }) return;

        _hasPendingQueuedRender = true;

        try
        {
            _renderHandle.Value.Render(_renderFragment);
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
