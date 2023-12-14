using Ignis.Components.HeadlessUI.Aria;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class MenuItem : DynamicComponentBase<MenuItem>, IAriaDescendant, IDisposable
{
    /// <inheritdoc />
    [Parameter] public string? Id { get; set; }

    [Parameter]
    public EventCallback<IComponentEvent> OnClick { get; set; }

    [CascadingParameter] public Menu Menu { get; set; } = null!;

    [Parameter] public RenderFragment<MenuItem>? ChildContent { get; set; }

    /// <inheritdoc />
    public bool IsActive => Menu.ActiveDescendant == this;

    public MenuItem() : base(typeof(Fragment))
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("id", Menu.GetId(this)),
            () => new KeyValuePair<string, object?>("tabindex", -1),
            () => new KeyValuePair<string, object?>("role", "menuitem"),
            () => new KeyValuePair<string, object?>("onclick", EventCallback.Factory.Create(this, Click)), () =>
                new KeyValuePair<string, object?>("onmouseenter",
                    EventCallback.Factory.Create(this, OnMouseEnter)),
            () => new KeyValuePair<string, object?>("onmouseleave",
                EventCallback.Factory.Create(this, OnMouseLeave))
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Menu == null)
        {
            throw new InvalidOperationException($"{nameof(MenuItem)} must be used inside a {nameof(HeadlessUI.Menu)}.");
        }

        Menu.AddDescendant(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (AsElement != null) builder.AddElementReferenceCapture(2, e => Element = e);
        builder.AddChildContentFor(3, this, ChildContent?.Invoke(this));
        if (AsComponent != null && AsComponent != typeof(Fragment))
            builder.AddComponentReferenceCapture(4, c => Component = c);

        builder.CloseAs(this);
    }

    /// <inheritdoc />
    public void Click()
    {
        var @event = new ComponentEvent();

        var __ = OnClick.InvokeAsync(@event);

        if (@event.DefaultPrevented) return;

        Menu.Close();
    }

    private void OnMouseEnter()
    {
        Menu.SetActiveDescendant(this, isActive: true);
    }

    private void OnMouseLeave()
    {
        Menu.SetActiveDescendant(this, isActive: false);
    }

    public void Dispose()
    {
        Menu.RemoveDescendant(this);
    }
}
