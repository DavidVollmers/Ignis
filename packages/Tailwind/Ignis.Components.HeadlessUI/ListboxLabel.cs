using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class ListboxLabel : IgnisRigidComponentBase, IDynamicComponent
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

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [CascadingParameter] public IListbox Listbox { get; set; } = null!;

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

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
        builder.AddAttribute(1, "id", Listbox.Id + "-label");
        builder.AddAttribute(2, "onclick", EventCallback.Factory.Create(this, Listbox.FocusAsync));
        builder.AddMultipleAttributes(3, AdditionalAttributes!);
        builder.AddContentFor(4, this, ChildContent);

        builder.CloseAs(this);
    }
}
