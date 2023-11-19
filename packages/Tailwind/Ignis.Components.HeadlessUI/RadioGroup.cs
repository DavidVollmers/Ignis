using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class RadioGroup<T> : IgnisComponentBase, IDynamicParentComponent<RadioGroup<T>>
{
    private readonly IList<RadioGroupOption<T>> _options = new List<RadioGroupOption<T>>();
    private readonly AttributeCollection _attributes;

    private RadioGroupLabel? _label;
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
    public T? Value { get; set; }

    /// <summary>
    /// Occurs when the checked value changes.
    /// </summary>
    [Parameter]
    public EventCallback<T?> ValueChanged { get; set; }

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<RadioGroup<T>>? _ { get; set; }

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
    public RadioGroupOption<T>[] Options => _options.ToArray();

    /// <inheritdoc />
    public RadioGroupOption<T>? ActiveOption { get; private set; }

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
        builder.OpenComponent<CascadingValue<RadioGroup<T>>>(0);
        builder.AddAttribute(1, nameof(CascadingValue<RadioGroup<T>>.IsFixed), value: true);
        builder.AddAttribute(2, nameof(CascadingValue<RadioGroup<T>>.Value), this);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddAttribute(3, nameof(CascadingValue<RadioGroup<T>>.ChildContent), (RenderFragment)(builder =>
        {
            builder.OpenAs(4, this);
            builder.AddMultipleAttributes(5, Attributes!);
            builder.AddChildContentFor(6, this, ChildContent);

            builder.CloseAs(this);
        }));

        builder.CloseComponent();
    }

    public bool IsValueChecked<T1>(T1? value)
    {
        return value?.Equals(Value) ?? Value?.Equals(value) ?? false;
    }

    public void CheckValue<T1>(T1? value)
    {
        var __ = ValueChanged.InvokeAsync(Value = (T?)(object?)value);

        Update();
    }

    public void SetOptionActive(RadioGroupOption<T> option, bool isActive)
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

    public void AddOption(RadioGroupOption<T> option)
    {
        if (option == null) throw new ArgumentNullException(nameof(option));

        if (!_options.Contains(option)) _options.Add(option);
    }

    public void RemoveOption(RadioGroupOption<T> option)
    {
        if (option == null) throw new ArgumentNullException(nameof(option));

        _options.Remove(option);
    }

    public void SetLabel(RadioGroupLabel label)
    {
        _label = label ?? throw new ArgumentNullException(nameof(label));
    }
}
