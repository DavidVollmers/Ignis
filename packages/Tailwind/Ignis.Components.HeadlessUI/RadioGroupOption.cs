using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public sealed class RadioGroupOption : IgnisComponentBase, IRadioGroupOption
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

    [CascadingParameter] public IRadioGroup RadioGroup { get; set; } = null!;

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<IRadioGroupOption>? _ { get; set; }

    [Parameter] public RenderFragment<IRadioGroupOption>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes
    {
        get => _attributes.AdditionalAttributes;
        set => _attributes.AdditionalAttributes = value;
    }

    /// <inheritdoc />
    public bool IsChecked => RadioGroup.IsValueChecked(Value);

    /// <inheritdoc />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>> Attributes => _attributes;

    public RadioGroupOption()
    {
        AsElement = "div";

        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("role", "radio"),
            () => new KeyValuePair<string, object?>("aria-checked", IsChecked.ToString().ToLowerInvariant())
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (RadioGroup == null)
        {
            throw new InvalidOperationException(
                $"{nameof(RadioGroupOption)} must be used inside a {nameof(RadioGroup<object>)}.");
        }

        RadioGroup.AddOption(this);
    }
}
