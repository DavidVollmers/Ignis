using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class ListboxButton<TValue> : IgnisDynamicComponentBase, IFocus
{
    private ElementReference? _element;
    private object? _component;

    [CascadingParameter] public IListbox<TValue> Listbox { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? Attributes { get; set; }

    public ListboxButton()
    {
        AsElement = "button";
    }

    protected override void OnInitialized()
    {
        if (Listbox == null)
        {
            throw new InvalidOperationException("ListboxButton must be used inside a Listbox.");
        }

        Listbox.SetButton(this);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        if (AsElement == "button") builder.AddAttribute(1, "type", "button");
        builder.AddAttribute(2, "aria-haspopup", "listbox");
        builder.AddAttribute(3, "onclick", EventCallback.Factory.Create(this, Listbox.Open));
        builder.AddAttribute(4, "aria-labelledby", Listbox.Id + "-label");
        builder.AddAttribute(5, "aria-expanded", Listbox.IsOpen);
        builder.AddMultipleAttributes(6, Attributes!);
        builder.AddReferenceCaptureFor(7, this, e => _element = e, c => _component = c);
        builder.AddContent(8, ChildContent);

        builder.CloseAs(this);
    }

    public async Task FocusAsync()
    {
        if (_element.HasValue)
        {
            await _element.Value.FocusAsync();    
        }
        else if (_component is IFocus focus)
        {
            await focus.FocusAsync();
        }
    }
}
