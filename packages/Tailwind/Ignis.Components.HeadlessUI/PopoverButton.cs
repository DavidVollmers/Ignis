using Ignis.Components.HeadlessUI.Aria;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class PopoverButton : DynamicComponentBase<PopoverButton>, IAriaComponentPart
{
    /// <inheritdoc />
    [Parameter]
    public string? Id { get; set; }

    [Parameter] public EventCallback<IComponentEvent> OnClick { get; set; }

    [CascadingParameter] public Popover Popover { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public PopoverButton() : base("button")
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("id", Popover.GetId(this)),
            () => new KeyValuePair<string, object?>("onclick", EventCallback.Factory.Create(this, Click)),
            () => new KeyValuePair<string, object?>("aria-expanded", Popover.IsOpen.ToString().ToLowerInvariant()),
            () => new KeyValuePair<string, object?>("type",
                string.Equals(AsElement, "button", StringComparison.OrdinalIgnoreCase) ? "button" : null),
            () => new KeyValuePair<string, object?>("aria-controls",
                Popover.IsOpen ? Popover.GetId(Popover.Controlled) : null),
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Popover == null)
        {
            throw new InvalidOperationException(
                $"{nameof(PopoverButton)} must be used inside a {nameof(HeadlessUI.Popover)}.");
        }

        Popover.Button = this;
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

        if (Popover.IsOpen) Popover.Close();
        else Popover.Open();
    }
}
