using System.Globalization;
using Ignis.Components.HeadlessUI.Aria;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class ListboxOption<T> : DynamicComponentBase<ListboxOption<T>>, IAriaDescendant, IDisposable
{
    /// <inheritdoc />
    [Parameter]
    public string? Id { get; set; }

    [Parameter] public EventCallback<IComponentEvent> OnClick { get; set; }

    [CascadingParameter(Name = nameof(Listbox<T>))]
    public Listbox<T> Listbox { get; set; } = null!;

    [Parameter, EditorRequired] public T Value { get; set; } = default!;

    [Parameter] public RenderFragment<ListboxOption<T>>? ChildContent { get; set; }

    /// <inheritdoc />
    public bool IsActive => Listbox.ActiveDescendant == this;

    public bool IsSelected => Listbox.IsValueSelected(Value);

    public ListboxOption() : base("li")
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("id", Listbox.GetId(this)),
            () => new KeyValuePair<string, object?>("tabindex", -1),
            () => new KeyValuePair<string, object?>("role", "option"),
            () => new KeyValuePair<string, object?>("aria-selected", IsSelected.ToString().ToLowerInvariant()),
            () => new KeyValuePair<string, object?>("onclick", EventCallback.Factory.Create(this, Click)), () =>
                new KeyValuePair<string, object?>("onmouseenter",
                    EventCallback.Factory.Create(this, OnMouseEnter)),
            () => new KeyValuePair<string, object?>("onmouseleave",
                EventCallback.Factory.Create(this, OnMouseLeave)),
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Listbox == null)
        {
            throw new InvalidOperationException(
                $"{nameof(ListboxOption<object>)} must be used inside a {nameof(Listbox<object>)}.");
        }

        Listbox.AddDescendant(this);
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

        Listbox.SelectValue(Value);

        if (!Listbox.Multiple) Listbox.Close();
    }

    private void OnMouseEnter()
    {
        Listbox.SetActiveDescendant(this, isActive: true);
    }

    private void OnMouseLeave()
    {
        Listbox.SetActiveDescendant(this, isActive: false);
    }

    public void Dispose()
    {
        Listbox.RemoveDescendant(this);
    }
}
