using Ignis.Components.HeadlessUI.Aria;
using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class RadioGroupOption<T> : FocusComponentBase, IDynamicParentComponent<RadioGroupOption<T>>,
    IAriaCheckGroupOption
{
    private readonly AttributeCollection _attributes;

    private Type? _asComponent;
    private string? _asElement;

    /// <inheritdoc />
    protected override IEnumerable<object> Targets
    {
        get
        {
            yield return this;

            if (Label != null) yield return Label;

            if (Description != null) yield return Description;
        }
    }

    /// <inheritdoc />
    protected override IEnumerable<string> KeysToCapture { get; } = new[] { "ArrowUp", "ArrowDown" };

    /// <inheritdoc />
    [Parameter]
    public string? Id { get; set; }

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

    [Parameter] public T? Value { get; set; }

    [Parameter] public EventCallback<IComponentEvent> OnClick { get; set; }

    [CascadingParameter(Name = nameof(RadioGroup<object>))]
    public RadioGroup<T> RadioGroup { get; set; } = null!;

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<RadioGroupOption<T>>? _ { get; set; }

    [Parameter] public RenderFragment<RadioGroupOption<T>>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes
    {
        get => _attributes.AdditionalAttributes;
        set => _attributes.AdditionalAttributes = value;
    }

    public bool IsActive => RadioGroup.ActiveOption == this;

    public bool IsChecked => RadioGroup.IsValueChecked(Value);

    /// <inheritdoc />
    public IAriaComponentPart? Label { get; set; }

    /// <inheritdoc />
    public IAriaComponentPart? Description { get; set; }

    /// <inheritdoc cref="IElementReferenceProvider.Element" />
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
            () => new KeyValuePair<string, object?>("id", RadioGroup.GetId(this)),
            () => new KeyValuePair<string, object?>("role", "radio"),
            () => new KeyValuePair<string, object?>("tabindex", IsChecked ? 0 : -1),
            () => new KeyValuePair<string, object?>("onclick", EventCallback.Factory.Create(this, Click)),
            () => new KeyValuePair<string, object?>("aria-checked", IsChecked.ToString().ToLowerInvariant()),
            () => new KeyValuePair<string, object?>("aria-labelledby", RadioGroup.GetId(Label)),
            () => new KeyValuePair<string, object?>("aria-describedby", RadioGroup.GetId(Description)),
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (RadioGroup == null)
        {
            throw new InvalidOperationException(
                $"{nameof(RadioGroupOption<T>)} must be used inside a {nameof(RadioGroup<T>)}.");
        }

        RadioGroup.AddOption(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<CascadingValue<RadioGroupOption<T>>>(0);
        builder.AddAttribute(1, nameof(CascadingValue<RadioGroupOption<T>>.IsFixed), value: true);
        builder.AddAttribute(2, nameof(CascadingValue<RadioGroupOption<T>>.Value), this);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddAttribute(3, nameof(CascadingValue<RadioGroupOption<T>>.ChildContent), (RenderFragment)(builder =>
        {
            builder.OpenAs(4, this);
            builder.AddMultipleAttributes(5, Attributes!);
            if (AsElement != null) builder.AddElementReferenceCapture(6, e => Element = e);
            builder.AddChildContentFor(7, this, ChildContent);

            if (AsComponent != null && AsComponent != typeof(Fragment))
                builder.AddComponentReferenceCapture(8, c => Component = c);

            builder.CloseAs(this);
        }));

        builder.CloseComponent();
    }

    /// <inheritdoc />
    protected override void OnKeyDown(KeyboardEventArgs eventArgs)
    {
        if (!RadioGroup.Options.Any()) return;

        var options = RadioGroup.Options.ToArray();

        switch (eventArgs.Code)
        {
            case "ArrowUp" when RadioGroup.ActiveOption == null:
            case "ArrowDown" when RadioGroup.ActiveOption == null:
                options[0].Check();
                break;
            case "ArrowDown":
                {
                    var index = Array.IndexOf(options, RadioGroup.ActiveOption) + 1;
                    if (index < options.Length) options[index].Check();
                    else options[0].Check();
                    break;
                }
            case "ArrowUp":
                {
                    var index = Array.IndexOf(options, RadioGroup.ActiveOption) - 1;
                    if (index >= 0) options[index].Check();
                    else options[^1].Check();
                    break;
                }
        }
    }

    public void Check()
    {
        RadioGroup.CheckValue(Value);

        RadioGroup.ActiveOption = this;

        var __ = FocusAsync();
    }

    private void Click()
    {
        var @event = new ComponentEvent();

        var __ = OnClick.InvokeAsync(@event);

        if (@event.DefaultPrevented) return;

        Check();
    }

    /// <inheritdoc />
    protected override void OnTargetFocus()
    {
        RadioGroup.ActiveOption = this;
    }

    /// <inheritdoc />
    protected override void OnTargetBlur()
    {
        if (RadioGroup.ActiveOption == this) RadioGroup.ActiveOption = null;
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            RadioGroup.RemoveOption(this);
        }

        base.Dispose(disposing);
    }
}
