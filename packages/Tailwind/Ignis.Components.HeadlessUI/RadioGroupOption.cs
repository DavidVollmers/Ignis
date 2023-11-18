using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class RadioGroupOption<T> : FocusComponentBase, IDynamicParentComponent<RadioGroupOption<T>>
{
    private readonly AttributeCollection _attributes;

    private RadioGroupDescription? _description;
    private RadioGroupLabel? _label;
    private Type? _asComponent;
    private string? _asElement;

    /// <inheritdoc />
    protected override IEnumerable<object> Targets
    {
        get
        {
            yield return this;

            if (_label != null) yield return _label;

            if (_description != null) yield return _description;
        }
    }

    /// <inheritdoc />
    protected override IEnumerable<string> KeysToCapture { get; } = new[] { "ArrowUp", "ArrowDown" };

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

    /// <inheritdoc />
    [Parameter]
    public EventCallback<IComponentEvent> OnClick { get; set; }

    [CascadingParameter] public RadioGroup<T> RadioGroup { get; set; } = null!;

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

    /// <inheritdoc />
    public bool IsActive => RadioGroup.ActiveOption == this;

    /// <inheritdoc />
    public bool IsChecked => RadioGroup.IsValueChecked(Value);

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
            () => new KeyValuePair<string, object?>("role", "radio"),
            () => new KeyValuePair<string, object?>("tabindex", IsChecked ? 0 : -1),
            () => new KeyValuePair<string, object?>("onclick", EventCallback.Factory.Create(this, Click)),
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
                $"{nameof(RadioGroupOption<T>)} must be used inside a {nameof(RadioGroup<T>)}.");
        }

        RadioGroup.AddOption(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<CascadingValue<RadioGroupOption<T>>>(0);
        builder.AddAttribute(1, nameof(CascadingValue<RadioGroupOption<T>>.IsFixed), true);
        builder.AddAttribute(2, nameof(CascadingValue<RadioGroupOption<T>>.Value), this);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddAttribute(3, nameof(CascadingValue<RadioGroupOption<T>>.ChildContent), (RenderFragment)(builder =>
        {
            builder.OpenAs(4, this);
            builder.AddMultipleAttributes(5, Attributes!);
            if (AsElement != null) builder.AddElementReferenceCapture(6, e => Element = e);
            // ReSharper disable once VariableHidesOuterVariable
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

        switch (eventArgs.Code)
        {
            case "ArrowUp" when RadioGroup.ActiveOption == null:
            case "ArrowDown" when RadioGroup.ActiveOption == null:
                RadioGroup.Options[0].Check();
                break;
            case "ArrowDown":
            {
                var index = Array.IndexOf(RadioGroup.Options, RadioGroup.ActiveOption) + 1;
                if (index < RadioGroup.Options.Length) RadioGroup.Options[index].Check();
                else RadioGroup.Options[0].Check();
                break;
            }
            case "ArrowUp":
            {
                var index = Array.IndexOf(RadioGroup.Options, RadioGroup.ActiveOption) - 1;
                if (index >= 0) RadioGroup.Options[index].Check();
                else RadioGroup.Options[^1].Check();
                break;
            }
        }
    }

    /// <inheritdoc />
    public void Check()
    {
        RadioGroup.CheckValue(Value);

        RadioGroup.SetOptionActive(this, true);

        var __ = FocusAsync();
    }

    private void Click()
    {
        var @event = new ComponentEvent();

        var __ = OnClick.InvokeAsync(@event);

        if (@event.CancellationToken.IsCancellationRequested) return;

        Check();
    }

    /// <inheritdoc />
    public void SetLabel(RadioGroupLabel label)
    {
        _label = label ?? throw new ArgumentNullException(nameof(label));
    }

    /// <inheritdoc />
    public void SetDescription(RadioGroupDescription description)
    {
        _description = description ?? throw new ArgumentNullException(nameof(description));
    }

    /// <inheritdoc />
    protected override void OnFocus()
    {
        RadioGroup.SetOptionActive(this, true);
    }

    /// <inheritdoc />
    protected override void OnBlur()
    {
        RadioGroup.SetOptionActive(this, false);
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
