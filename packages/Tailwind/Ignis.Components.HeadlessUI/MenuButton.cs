using Ignis.Components.HeadlessUI.Aria;
using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class MenuButton : DynamicComponentBase<MenuButton>, IAriaComponentPart
{
    /// <inheritdoc />
    [Parameter] public string? Id { get; set; }

    [Parameter] public EventCallback<IComponentEvent> OnClick { get; set; }

    [CascadingParameter] public Menu Menu { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public MenuButton() : base("button")
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("id", Menu.GetId(this)),
            () => new KeyValuePair<string, object?>("aria-haspopup", "true"),
            () => new KeyValuePair<string, object?>("onclick", EventCallback.Factory.Create(this, Click)),
            () => new KeyValuePair<string, object?>("aria-expanded", Menu.IsOpen.ToString().ToLowerInvariant()),
            () => new KeyValuePair<string, object?>("type",
                string.Equals(AsElement, "button", StringComparison.OrdinalIgnoreCase) ? "button" : null),
            () => new KeyValuePair<string, object?>("aria-controls", Menu.IsOpen ? Menu.GetId(Menu.Controlled) : null),
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Menu == null)
        {
            throw new InvalidOperationException(
                $"{nameof(MenuButton)} must be used inside a {nameof(HeadlessUI.Menu)}.");
        }

        Menu.Button = this;
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

        if (Menu.IsOpen) Menu.Close();
        else Menu.Open();
    }
}
