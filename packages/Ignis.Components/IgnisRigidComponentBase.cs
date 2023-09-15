using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components;

public abstract class IgnisRigidComponentBase : IComponent
{
    private readonly RenderFragment _renderFragment;

    private RenderHandle _renderHandle;

    protected IgnisRigidComponentBase()
    {
        _renderFragment = BuildRenderTree;
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
        parameters.SetParameterProperties(this);

        OnRender();

        _renderHandle.Render(_renderFragment);

        return Task.CompletedTask;
    }

    protected virtual void BuildRenderTree(RenderTreeBuilder builder)
    {
    }

    protected virtual void OnRender()
    {
    }
}
