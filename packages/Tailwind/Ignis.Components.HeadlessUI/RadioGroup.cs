using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class RadioGroup<T> : DynamicComponentBase<RadioGroup<T>>
{
    private readonly IList<RadioGroupOption<T>> _options = new List<RadioGroupOption<T>>();

    private RadioGroupLabel? _label;

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

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public RadioGroupOption<T>[] Options => _options.ToArray();

    public RadioGroupOption<T>? ActiveOption { get; private set; }

    public string Id { get; } = "ignis-hui-radiogroup-" + Guid.NewGuid().ToString("N");

    public RadioGroup() : base("div")
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("id", Id),
            () => new KeyValuePair<string, object?>("role", "radiogroup"),
            () => new KeyValuePair<string, object?>("aria-labelledby",
                _label == null ? null : _label.Id ?? Id + "-label"),
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
