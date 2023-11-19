using System.Globalization;
using Ignis.Components.HeadlessUI.Aria;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class ListboxOptions : DynamicComponentBase<ListboxOptions>, IAriaComponentPart
{
    /// <inheritdoc />
    [Parameter]
    public string? Id { get; set; }

    [CascadingParameter(Name = nameof(Listbox<object>))]
    public IAriaPopup Listbox { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public ListboxOptions() : base("ul")
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("id", Listbox.GetId(this)),
            () => new KeyValuePair<string, object?>("tabindex", -1),
            () => new KeyValuePair<string, object?>("role", "listbox"),
            () => new KeyValuePair<string, object?>("aria-orientation", "vertical"), () =>
                new KeyValuePair<string, object?>("aria-labelledby", Listbox.GetId(Listbox.Button)),
            () => new KeyValuePair<string, object?>("aria-activedescendant", Listbox.GetId(Listbox.ActiveDescendant)),
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Listbox == null)
        {
            throw new InvalidOperationException(
                $"{nameof(ListboxOptions)} must be used inside a {nameof(Listbox<object>)}.");
        }

        Listbox.Controlled = this;
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (!Listbox.IsOpen) return;

        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (AsElement != null) builder.AddElementReferenceCapture(2, e => Element = e);
        builder.AddChildContentFor(3, this, ChildContent);
        if (AsComponent != null && AsComponent != typeof(Fragment))
            builder.AddComponentReferenceCapture(4, c => Component = c);

        builder.CloseAs(this);
    }
}
