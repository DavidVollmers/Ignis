using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class PopoverButton : IgnisComponentBase, IPopoverButton
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
    public string? Id { get; set; }

    [CascadingParameter] public IPopover Popover { get; set; } = null!;

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<IPopoverButton>? _ { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes
    {
        get => _attributes.AdditionalAttributes;
        set => _attributes.AdditionalAttributes = value;
    }

    /// <inheritdoc />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>> Attributes => _attributes;

    public PopoverButton()
    {
        AsElement = "button";

        //TODO aria-controls
        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("id", Id ?? Popover.Id + "-button"), () =>
                new KeyValuePair<string, object?>("onclick",
                    EventCallback.Factory.Create(this, () => Popover.Open())),
#pragma warning disable CS0618
            () => new KeyValuePair<string, object?>("onkeydown", EventCallback.Factory.Create(this, OnKeyDown)),
#pragma warning restore CS0618
            () => new KeyValuePair<string, object?>("aria-expanded", Popover.IsOpen.ToString().ToLowerInvariant()),
            () => new KeyValuePair<string, object?>("type", AsElement == "button" ? "button" : null),
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

        Popover.SetButton(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor<IPopoverButton, PopoverButton>(2, this, ChildContent);

        builder.CloseAs(this);
    }

    private void OnKeyDown(KeyboardEventArgs eventArgs)
    {
        if (eventArgs.Code != "Escape") return;
        
        Popover.Close();
    }
}
