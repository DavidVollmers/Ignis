using System.Globalization;
using Ignis.Components.HeadlessUI.Aria;
using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class ListboxButton : DynamicComponentBase<ListboxButton>, IAriaComponentPart
{
    /// <inheritdoc />
    [Parameter]
    public string? Id { get; set; }

    [Parameter] public EventCallback<IComponentEvent> OnClick { get; set; }

    [CascadingParameter(Name = nameof(Listbox<object>))]
    public IAriaPopup Listbox { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public ListboxButton() : base("button")
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("id", Listbox.GetId(this)),
            () => new KeyValuePair<string, object?>("aria-haspopup", "listbox"),
            () => new KeyValuePair<string, object?>("onclick", EventCallback.Factory.Create(this, Click)),
            () => new KeyValuePair<string, object?>("aria-expanded", Listbox.IsOpen.ToString().ToLowerInvariant()),
            () => new KeyValuePair<string, object?>("type",
                string.Equals(AsElement, "button", StringComparison.OrdinalIgnoreCase) ? "button" : null),
            () => new KeyValuePair<string, object?>("aria-labelledby", Listbox.GetId(Listbox.Label)), () =>
                new KeyValuePair<string, object?>("aria-controls",
                    Listbox.IsOpen ? Listbox.GetId(Listbox.Controlled) : null),
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Listbox == null)
        {
            throw new InvalidOperationException(
                $"{nameof(ListboxButton)} must be used inside a {nameof(Listbox<object>)}.");
        }

        Listbox.Button = this;
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

    private void Click()
    {
        var @event = new ComponentEvent();

        var __ = OnClick.InvokeAsync(@event);

        if (@event.DefaultPrevented) return;

        if (Listbox.IsOpen) Listbox.Close();
        else Listbox.Open();
    }
}
