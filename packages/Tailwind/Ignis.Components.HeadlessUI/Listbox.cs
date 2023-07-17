using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Listbox<TValue> : IgnisComponentBase, IDynamicComponent, IListbox
{
    private ListboxButton? _button;
    private Type? _asComponent;
    private string? _asElement;

    [Parameter]
#pragma warning disable BL0007
    public string? AsElement
#pragma warning restore BL0007
    {
        get => _asElement;
        set
        {
            _asElement = value;
            _asComponent = null;
        }
    }

    [Parameter]
#pragma warning disable BL0007
    public Type? AsComponent
#pragma warning restore BL0007
    {
        get => _asComponent;
        set
        {
            _asComponent = value;
            _asElement = null;
        }
    }

    public string Id { get; } = "hui-listbox-" + Guid.NewGuid().ToString("N");

    public bool IsOpen { get; private set; }

    [Parameter] public TValue? Value { get; set; }

    [Parameter] public EventCallback<TValue?> ValueChanged { get; set; }

    [Parameter] public RenderFragment<IListbox>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    public Listbox()
    {
        AsComponent = typeof(Fragment);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, AdditionalAttributes!);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddContentFor(2, this, builder =>
        {
            builder.OpenComponent<FocusDetector>(3);
            builder.AddAttribute(4, nameof(FocusDetector.Id), Id);
            builder.AddAttribute(5, nameof(FocusDetector.OnBlur), EventCallback.Factory.Create(this, Close));
            // ReSharper disable once VariableHidesOuterVariable
            builder.AddAttribute(6, nameof(FocusDetector.ChildContent), (RenderFragment)(builder =>
            {
                builder.OpenComponent<CascadingValue<IListbox>>(7);
                builder.AddAttribute(8, nameof(CascadingValue<IListbox>.IsFixed), true);
                builder.AddAttribute(9, nameof(CascadingValue<IListbox>.Value), this);
                builder.AddAttribute(10, nameof(CascadingValue<IListbox>.ChildContent),
                    ChildContent?.Invoke(this));

                builder.CloseComponent();
            }));

            builder.CloseComponent();
        });

        builder.CloseAs(this);
    }

    public void Open()
    {
        if (IsOpen) return;

        IsOpen = true;

        ForceUpdate();
    }

    public void Close()
    {
        if (!IsOpen) return;

        IsOpen = false;

        ForceUpdate();
    }

    public async Task FocusAsync()
    {
        if (_button == null) return;

        await _button.FocusAsync();
    }

    public bool IsValueSelected<TValue1>(TValue1? value)
    {
        return value?.Equals(Value) ?? Value?.Equals(value) ?? false;
    }

    public void SelectValue<TValue1>(TValue1? value)
    {
        Value = (TValue?)(object?)value;
        ValueChanged.InvokeAsync(Value);
    }

    public void SetButton(ListboxButton button)
    {
        if (_button != null && _button != button) throw new InvalidOperationException("ListboxButton already set.");

        _button = button ?? throw new ArgumentNullException(nameof(button));
    }
}
