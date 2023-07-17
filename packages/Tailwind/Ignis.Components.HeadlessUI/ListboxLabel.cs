using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class ListboxLabel : IgnisDynamicComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [CascadingParameter] public IListbox Listbox { get; set; } = null!;

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    public ListboxLabel()
    {
        AsElement = "label";
    }

    protected override void OnInitialized()
    {
        if (Listbox == null)
        {
            throw new InvalidOperationException("ListboxLabel must be used inside a Listbox.");
        }
    }

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
