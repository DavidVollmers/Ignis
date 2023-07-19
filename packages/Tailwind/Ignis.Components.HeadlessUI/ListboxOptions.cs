using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class ListboxOptions : IgnisRigidComponentBase, IDynamicComponent
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
    
    [CascadingParameter] public IListbox Listbox { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    public ListboxOptions()
    {
        AsElement = "ul";
    }

    protected override void OnRender()
    {
        if (Listbox == null)
        {
            throw new InvalidOperationException("ListboxOptions must be used inside a Listbox.");
        }
    }
    
    //TODO aria-active-descendant
    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (!Listbox.IsOpen) return;
        
        builder.OpenAs(0, this);
        builder.AddAttribute(1, "tabindex", -1);
        builder.AddAttribute(2, "role", "listbox");
        builder.AddAttribute(3, "aria-activedescendant");
        builder.AddAttribute(4, "aria-orientation", "vertical");
        builder.AddAttribute(5, "aria-labelledby", Listbox.Id + "-label");
        builder.AddMultipleAttributes(6, AdditionalAttributes!);
        builder.AddContentFor(7, this, ChildContent);

        builder.CloseAs(this);
    }
}
