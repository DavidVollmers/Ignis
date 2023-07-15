using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public sealed class Fragment : IComponent
{
    private readonly RenderFragment _renderFragment;

    private RenderHandle? _renderHandle;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public Fragment()
    {
        _renderFragment = builder =>
        {
            builder.AddContent(0, ChildContent);
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

    public Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);
        
        _renderHandle?.Render(_renderFragment);

        return Task.CompletedTask;
    }
}
