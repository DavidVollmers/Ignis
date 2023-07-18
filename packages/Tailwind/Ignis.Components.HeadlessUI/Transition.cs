using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public sealed class Transition : IgnisRigidComponentBase, IDynamicComponent
{
    private Type? _asComponent;
    private string? _asElement;

    [Parameter]
    public string? AsElement
    {
        get => _asElement;
        set
        {
            _asElement = value;
            _asComponent = null;
        }
    }

    [Parameter]
    public Type? AsComponent
    {
        get => _asComponent;
        set
        {
            _asComponent = value;
            _asElement = null;
        }
    }

    [Parameter] public string? Enter { get; set; }

    public Transition()
    {
        AsElement = "div";
    }
}
