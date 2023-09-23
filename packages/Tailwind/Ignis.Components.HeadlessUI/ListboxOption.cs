using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class ListboxOption<TValue> : IgnisComponentBase, IListboxOption, IDisposable
{
    private readonly AttributeCollection _attributes;

    private Type? _asComponent;
    private string? _asElement;

    /// <inheritdoc />
    [Parameter]
    public string? AsElement
    {
        get => _asElement;
        set
        {
            _asElement = value;
            _asComponent = null;
        }
    }

    /// <inheritdoc />
    [Parameter]
    public Type? AsComponent
    {
        get => _asComponent;
        set
        {
            _asComponent = value;
            _asElement = null;
        }
    }

    /// <inheritdoc />
    [Parameter]
    public EventCallback<IComponentEvent> OnClick { get; set; }

    [CascadingParameter] public IListbox Listbox { get; set; } = null!;

    [Parameter, EditorRequired] public TValue? Value { get; set; }

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<IListboxOption>? _ { get; set; }

    [Parameter] public RenderFragment<IListboxOption>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes
    {
        get => _attributes.AdditionalAttributes;
        set => _attributes.AdditionalAttributes = value;
    }

    /// <inheritdoc />
    public bool IsActive => Listbox.ActiveOption == this;

    /// <inheritdoc />
    public bool IsSelected => Listbox.IsValueSelected(Value);

    /// <inheritdoc cref="IElementReferenceProvider.Element" />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>> Attributes => _attributes;

    public ListboxOption()
    {
        AsElement = "li";

        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("tabindex", -1),
            () => new KeyValuePair<string, object?>("role", "option"),
            () => new KeyValuePair<string, object?>("aria-selected", IsSelected.ToString().ToLowerInvariant()),
            () => new KeyValuePair<string, object?>("onclick", EventCallback.Factory.Create(this, Click)), () =>
                new KeyValuePair<string, object?>("onmouseenter",
                    EventCallback.Factory.Create(this, OnMouseEnter)),
            () => new KeyValuePair<string, object?>("onmouseleave",
                EventCallback.Factory.Create(this, OnMouseLeave)),
            () => new KeyValuePair<string, object?>("id",
                Listbox.Id + "-option-" + Array.IndexOf(Listbox.Options, this).ToString(CultureInfo.InvariantCulture)),
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

        Listbox.AddOption(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (AsElement != null) builder.AddElementReferenceCapture(2, e => Element = e);
        builder.AddChildContentFor<IListboxOption, ListboxOption<TValue>>(3, this, ChildContent?.Invoke(this));
        if (AsComponent != null && AsComponent != typeof(Fragment))
            builder.AddComponentReferenceCapture(4, c => Component = c);

        builder.CloseAs(this);
    }

    /// <inheritdoc />
    public void Click()
    {
        var @event = new ComponentEvent();

        var __ = OnClick.InvokeAsync(@event);

        if (@event.CancellationToken.IsCancellationRequested) return;

        Listbox.SelectValue(Value);

        Listbox.Close();
    }

    private void OnMouseEnter()
    {
        Listbox.SetOptionActive(this, true);
    }

    private void OnMouseLeave()
    {
        Listbox.SetOptionActive(this, false);
    }

    public void Dispose()
    {
        Listbox.RemoveOption(this);
    }
}
