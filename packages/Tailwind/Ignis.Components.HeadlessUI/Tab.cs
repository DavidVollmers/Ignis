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

    public IReadOnlyDictionary<string, object?> Attributes
    {
        get
        {
            var attributes = new Dictionary<string, object?>
            {
                { "tabindex", -1 },
                { "role", "tab" },
                { "aria-selected", IsSelected }
            };
            
            if (AsElement == "button") attributes.Add("type", "button");

            // ReSharper disable once InvertIf
            if (AdditionalAttributes != null)
            {
                foreach (var (key, value) in AdditionalAttributes)
                {
                    attributes[key] = value;
                }
            }

            return attributes;
        }
    }

    public Tab()
    {
        AsElement = "button";
    }

    //TODO aria-controls
    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddContentFor(2, this, ChildContent?.Invoke(this));

        builder.CloseAs(this);
    }
}
