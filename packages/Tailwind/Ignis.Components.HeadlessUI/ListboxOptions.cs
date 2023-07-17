using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class ListboxOptions : IgnisDynamicComponentBase
{
    [CascadingParameter] public IListbox Listbox { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    public ListboxOptions()
    {
        AsElement = "ul";
    }

    protected override void OnInitialized()
    {
        if (Listbox == null)
        {
            throw new InvalidOperationException("ListboxOptions must be used inside a Listbox.");
        }
    }

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
