using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class ListboxLabel : IgnisRigidComponentBase, IDynamicParentComponent
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

    /// <inheritdoc />
    [Parameter] public RenderFragment<IDynamicComponent>? ChildContent { get; set; }

    [CascadingParameter] public IListbox Listbox { get; set; } = null!;

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object?>? Attributes
    {
        get
        {
            var attributes = new Dictionary<string, object?>
            {
                { "id", Listbox.Id + "-label" },
                { "onclick", EventCallback.Factory.Create(this, Listbox.FocusAsync) }
            };
            
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

    public ListboxLabel()
    {
        AsElement = "label";
    }

    /// <inheritdoc />
    protected override void OnRender()
    {
        if (Listbox == null)
        {
            throw new InvalidOperationException(
                $"{nameof(ListboxLabel)} must be used inside a {nameof(Listbox<object>)}.");
        }
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor(2, this);

        builder.CloseAs(this);
    }
}
