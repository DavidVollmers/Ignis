using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Tab : IgnisRigidComponentBase, ITab, IDynamicComponent
{
    private Type? _asComponent;
    private string? _asElement;

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    [Parameter] public RenderFragment<ITab>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    //TODO
    public bool IsSelected => false;

    public Tab()
    {
        AsElement = "button";
    }

    //TODO aria-controls
    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        if (AsElement == "button") builder.AddAttribute(1, "type", "button");
        builder.AddAttribute(2, "tabindex", -1);
        builder.AddAttribute(3, "role", "tab");
        builder.AddAttribute(4, "aria-selected", IsSelected);
        builder.AddMultipleAttributes(5, AdditionalAttributes!);
        builder.AddContentFor(6, this, ChildContent?.Invoke(this));

        builder.CloseAs(this);
    }
}
