using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class RadioGroup<TValue> : IgnisComponentBase, IDynamicParentComponent<RadioGroup>
{
    private readonly IList<IRadioGroupOption> _options = new List<IRadioGroupOption>();
    private readonly AttributeCollection _attributes;

    private IRadioGroupLabel? _label;
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

    /// <summary>
    /// The checked value.
    /// </summary>
    [Parameter]
    public TValue? Value { get; set; }

    /// <summary>
    /// Occurs when the checked value changes.
    /// </summary>
    [Parameter]
    public EventCallback<TValue?> ValueChanged { get; set; }

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<IRadioGroup>? _ { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Additional attributes to be applied to the radio group.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes
    {
        get => _attributes.AdditionalAttributes;
        set => _attributes.AdditionalAttributes = value;
    }

    /// <inheritdoc />
    public IRadioGroupOption[] Options => _options.ToArray();

    /// <inheritdoc />
    public IRadioGroupOption? ActiveOption { get; private set; }

    /// <inheritdoc />
    public string Id { get; } = "ignis-hui-radiogroup-" + Guid.NewGuid().ToString("N");

    /// <inheritdoc cref="IElementReferenceProvider.Element" />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>> Attributes => _attributes;

    public RadioGroup()
    {
        AsElement = "div";

        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("id", Id),
            () => new KeyValuePair<string, object?>("role", "radiogroup"),
            () => new KeyValuePair<string, object?>("aria-labelledby",
                _label == null ? null : _label.Id ?? Id + "-label")
        });
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddContentFor(2, this, builder =>
        {
            builder.OpenComponent<CascadingValue<IRadioGroup>>(3);
            builder.AddAttribute(4, nameof(CascadingValue<IRadioGroup>.IsFixed), true);
            builder.AddAttribute(5, nameof(CascadingValue<IRadioGroup>.Value), this);
            builder.AddAttribute(6, nameof(CascadingValue<IRadioGroup>.ChildContent),
                this.GetChildContent<IRadioGroup, RadioGroup<TValue>>(ChildContent));

            builder.CloseComponent();
        });

        builder.CloseAs(this);
    }

    /// <inheritdoc />
    public bool IsValueChecked<TValue1>(TValue1? value)
    {
        return value?.Equals(Value) ?? Value?.Equals(value) ?? false;
    }

    /// <inheritdoc />
    public void CheckValue<TValue1>(TValue1? value)
    {
        var __ = ValueChanged.InvokeAsync(Value = (TValue?)(object?)value);

        Update();
    }

    /// <inheritdoc />
    public void SetOptionActive(IRadioGroupOption option, bool isActive)
    {
        if (option == null) throw new ArgumentNullException(nameof(option));

        if (isActive)
        {
            ActiveOption = option;
        }
        else if (ActiveOption == option)
        {
            ActiveOption = null;
        }

        Update();
    }

    /// <inheritdoc />
    public void AddOption(IRadioGroupOption option)
    {
        if (option == null) throw new ArgumentNullException(nameof(option));

        if (!_options.Contains(option)) _options.Add(option);
    }

    /// <inheritdoc />
    public void RemoveOption(IRadioGroupOption option)
    {
        if (option == null) throw new ArgumentNullException(nameof(option));

        _options.Remove(option);
    }

    /// <inheritdoc />
    public void SetLabel(IRadioGroupLabel label)
    {
        _label = label ?? throw new ArgumentNullException(nameof(label));
    }
}
