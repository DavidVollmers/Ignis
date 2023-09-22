using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Switch : IgnisComponentBase, ISwitch
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

    [Parameter] public bool Checked { get; set; }

    [Parameter] public EventCallback<bool> CheckedChanged { get; set; }

    /// <inheritdoc />
    [Parameter]
    public EventCallback<IComponentEvent> OnClick { get; set; }

    [CascadingParameter] public ISwitchGroup? SwitchGroup { get; set; }

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<ISwitch>? _ { get; set; }

    [Parameter] public RenderFragment<ISwitch>? ChildContent { get; set; }

    /// <summary>
    /// Additional attributes to be applied to the switch.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes
    {
        get => _attributes.AdditionalAttributes;
        set => _attributes.AdditionalAttributes = value;
    }

    /// <inheritdoc cref="IElementReferenceProvider.Element" />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>> Attributes => _attributes;

    public Switch()
    {
        AsElement = "button";

        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("id",
                Id != null || SwitchGroup != null ? Id ?? SwitchGroup?.Id + "-button" : null),
            () => new KeyValuePair<string, object?>("tabindex", "0"),
            () => new KeyValuePair<string, object?>("role", "switch"),
            () => new KeyValuePair<string, object?>("aria-checked", Checked.ToString().ToLowerInvariant()),
            () => new KeyValuePair<string, object?>("onclick", EventCallback.Factory.Create(this, Click)),
            () => new KeyValuePair<string, object?>("type", string.Equals(AsElement, "button", StringComparison.Ordinal) ? "button" : null), () =>
                new KeyValuePair<string, object?>("aria-labelledby",
                    SwitchGroup?.Label == null ? null : SwitchGroup.Label.Id ?? SwitchGroup.Id + "-label"),
            () => new KeyValuePair<string, object?>("aria-describedby",
                SwitchGroup?.Description == null ? null : SwitchGroup.Description.Id ?? SwitchGroup.Id + "-description")
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        SwitchGroup?.SetSwitch(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor<ISwitch, Switch>(2, this, ChildContent?.Invoke(this));

        builder.CloseAs(this);
    }

    /// <inheritdoc />
    public void Toggle()
    {
        CheckedChanged.InvokeAsync(Checked = !Checked);

        Update();
    }

    private void Click()
    {
        var @event = new ComponentEvent();

        OnClick.InvokeAsync(@event);

        if (@event.CancellationToken.IsCancellationRequested) return;

        Toggle();
    }
}
