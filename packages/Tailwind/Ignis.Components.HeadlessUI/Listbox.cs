using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Listbox<TValue> : IgnisComponentBase, IDynamicComponent, IListbox
{
    private readonly IList<IListboxOption> _options = new List<IListboxOption>();

    private ITransition? _transition;
    private Type? _asComponent;
    private string? _asElement;
    private IFocus? _button;

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

    public IListboxOption[] Options => _options.ToArray();
    
    public IListboxOption? ActiveOption { get; private set; }

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
            builder.AddAttribute(5, nameof(FocusDetector.Strict), false);
            builder.AddAttribute(6, nameof(FocusDetector.OnBlur), EventCallback.Factory.Create(this, Close));
            // ReSharper disable once VariableHidesOuterVariable
            builder.AddAttribute(7, nameof(FocusDetector.ChildContent), (RenderFragment)(builder =>
            {
                builder.OpenComponent<CascadingValue<IListbox>>(8);
                builder.AddAttribute(9, nameof(CascadingValue<IListbox>.IsFixed), true);
                builder.AddAttribute(10, nameof(CascadingValue<IListbox>.Value), this);
                builder.AddAttribute(11, nameof(CascadingValue<IListbox>.ChildContent),
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

        _transition?.Show();
    }

    public void Close()
    {
        if (!IsOpen) return;

        if (_transition != null)
        {
            _transition.Hide(() => CloseCore(true));
            return;
        }

        CloseCore();
    }

    private void CloseCore(bool async = false)
    {
        IsOpen = false;

        ForceUpdate(async);
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

        ForceUpdate();
    }

    public void SetOptionActive(IListboxOption option, bool isActive)
    {
        if (isActive)
        {
            ActiveOption = option;
        }
        else if (ActiveOption == option)
        {
            ActiveOption = null;
        }
        
        ForceUpdate();
    }

    public void AddOption(IListboxOption option)
    {
        if (option == null) throw new ArgumentNullException(nameof(option));

        if (!_options.Contains(option)) _options.Add(option);
    }

    public void RemoveOption(IListboxOption option)
    {
        if (option == null) throw new ArgumentNullException(nameof(option));
        
        _options.Remove(option);
    }

    public void SetButton(IFocus button)
    {
        if (_button != null && _button != button)
            throw new InvalidOperationException("Listbox cannot contain multiple buttons.");

        _button = button ?? throw new ArgumentNullException(nameof(button));
    }

    public void SetTransition(ITransition transition)
    {
        if (_transition != null && _transition != transition)
            throw new InvalidOperationException("Listbox cannot contain multiple transitions.");

        _transition = transition ?? throw new ArgumentNullException(nameof(transition));
    }
}
