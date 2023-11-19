using Ignis.Components.HeadlessUI.Aria;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class ListboxLabel : DynamicComponentBase<ListboxLabel>, IAriaComponentPart
{
    /// <inheritdoc />
    [Parameter]
    public string? Id { get; set; }

    [CascadingParameter(Name = nameof(Listbox<object>))]
    public IAriaPopup Listbox { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public ListboxLabel() : base("label")
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("id", Listbox.GetId(this)), () =>
                new KeyValuePair<string, object?>("onclick",
                    EventCallback.Factory.Create(this, Listbox.FocusAsync)),
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Listbox == null)
        {
            throw new InvalidOperationException(
                $"{nameof(ListboxLabel)} must be used inside a {nameof(Listbox<object>)}.");
        }

        Listbox.Label = this;
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (AsElement != null) builder.AddElementReferenceCapture(2, e => Element = e);
        builder.AddChildContentFor(3, this, ChildContent);
        if (AsComponent != null && AsComponent != typeof(Fragment))
            builder.AddComponentReferenceCapture(4, c => Component = c);

        builder.CloseAs(this);
    }
}
