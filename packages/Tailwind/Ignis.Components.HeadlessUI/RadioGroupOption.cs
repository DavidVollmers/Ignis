using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class RadioGroupOption<TValue> : IgnisComponentBase, IRadioGroupOption, IDisposable
{
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

    [Parameter] public TValue? Value { get; set; }

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
    public bool IsActive => RadioGroup.ActiveOption == this;

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
            () => new KeyValuePair<string, object?>("tabindex", IsChecked ? 0 : -1),
            () => new KeyValuePair<string, object?>("aria-checked", IsChecked.ToString().ToLowerInvariant()),
            () => new KeyValuePair<string, object?>("aria-labelledby", _label?.Id)
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (RadioGroup == null)
        {
            throw new InvalidOperationException(
                $"{nameof(RadioGroupOption<TValue>)} must be used inside a {nameof(RadioGroup<object>)}.");
        }

        RadioGroup.AddOption(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor<IRadioGroupOption, RadioGroupOption<TValue>>(2, this, ChildContent?.Invoke(this));

        builder.CloseAs(this);
    }

    /// <inheritdoc />
    public void SetLabel(IRadioGroupLabel label)
    {
        _label = label ?? throw new ArgumentNullException(nameof(label));
    }

    /// <inheritdoc />
    public async Task FocusAsync()
    {
        if (Element.HasValue)
        {
            await Element.Value.FocusAsync();
        }
        else if (Component is IFocus focus)
        {
            await focus.FocusAsync();
        }

        Update();
    }

    public void Dispose()
    {
        RadioGroup.RemoveOption(this);
    }
}
