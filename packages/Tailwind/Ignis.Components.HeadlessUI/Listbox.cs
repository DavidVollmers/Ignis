using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

/// <summary>
/// Renders a listbox which can be used to select one or more values.
/// </summary>
/// <typeparam name="TValue">The value type.</typeparam>
public sealed class Listbox<TValue> : IgnisComponentBase, IListbox, IHandleAfterRender
{
    private readonly IList<IListboxOption> _options = new List<IListboxOption>();

    private ITransition? _transition;
    private Type? _asComponent;
    private string? _asElement;
    private IFocus? _button;
    private bool _isOpen;

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
    /// The selected value.
    /// </summary>
    [Parameter]
    public TValue? Value { get; set; }

    /// <summary>
    /// Occurs when the selected value changes.
    /// </summary>
    [Parameter]
    public EventCallback<TValue?> ValueChanged { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool IsOpen
    {
        get => _isOpen;
        set
        {
            if (value) Open();
            else Close();
        }
    }

    /// <inheritdoc />
    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<IListbox>? _ { get; set; }

    [Parameter] public RenderFragment<IListbox>? ChildContent { get; set; }

    /// <summary>
    /// Additional attributes to be applied to the listbox.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    public IListboxOption[] Options => _options.ToArray();

    /// <inheritdoc />
    public IListboxOption? ActiveOption { get; private set; }

    /// <inheritdoc />
    public IListboxLabel? Label { get; private set; }

    /// <inheritdoc />
    public string Id { get; } = "ignis-hui-listbox-" + Guid.NewGuid().ToString("N");

    /// <inheritdoc />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>>? Attributes => AdditionalAttributes;

    [Inject] internal FrameTracker FrameTracker { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Listbox{TValue}"/> class.
    /// </summary>
    public Listbox()
    {
        AsComponent = typeof(Fragment);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddContentFor(2, this, builder =>
        {
            builder.OpenComponent<FocusDetector>(3);
            builder.AddAttribute(4, nameof(FocusDetector.Id), Id);
            builder.AddAttribute(6, nameof(FocusDetector.OnBlur), EventCallback.Factory.Create(this, () => Close()));
            // ReSharper disable once VariableHidesOuterVariable
            builder.AddAttribute(7, nameof(FocusDetector.ChildContent), (RenderFragment)(builder =>
            {
                builder.OpenComponent<CascadingValue<IListbox>>(8);
                builder.AddAttribute(9, nameof(CascadingValue<IListbox>.IsFixed), true);
                builder.AddAttribute(10, nameof(CascadingValue<IListbox>.Value), this);
                builder.AddAttribute(11, nameof(CascadingValue<IListbox>.ChildContent),
                    this.GetChildContent(ChildContent));

                builder.CloseComponent();
            }));

            builder.CloseComponent();
        });

        builder.CloseAs(this);
    }

    /// <inheritdoc />
    public void Open(Action? continueWith = null)
    {
        if (_isOpen || FrameTracker.IsPending) return;

        IsOpenChanged.InvokeAsync(_isOpen = true);

        FrameTracker.ExecuteOnNextFrame(() =>
        {
            var selectedOption = Options.FirstOrDefault(x => x.IsSelected);
            if (selectedOption != null) SetOptionActive(selectedOption, true);

            if (_transition != null) _transition.Show(continueWith);
            else continueWith?.Invoke();
        }, ForceUpdate);
        
        ForceUpdate();
    }

    /// <inheritdoc />
    public void Close(Action? continueWith = null)
    {
        if (!_isOpen || FrameTracker.IsPending) return;

        if (_transition != null)
        {
            _transition.Hide(() => CloseCore(continueWith, true));
            return;
        }

        CloseCore(continueWith);
    }

    private void CloseCore(Action? continueWith, bool async = false)
    {
        IsOpenChanged.InvokeAsync(_isOpen = false);

        if (continueWith != null) FrameTracker.ExecuteOnNextFrame(continueWith, ForceUpdate);

        ForceUpdate(async);
    }

    /// <inheritdoc />
    public async Task FocusAsync()
    {
        if (_button == null) return;

        await _button.FocusAsync();
    }

    /// <inheritdoc />
    public bool IsValueSelected<TValue1>(TValue1? value)
    {
        return value?.Equals(Value) ?? Value?.Equals(value) ?? false;
    }

    /// <inheritdoc />
    public void SelectValue<TValue1>(TValue1? value)
    {
        ValueChanged.InvokeAsync(Value = (TValue?)(object?)value);

        ForceUpdate();
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void AddOption(IListboxOption option)
    {
        if (option == null) throw new ArgumentNullException(nameof(option));

        if (!_options.Contains(option)) _options.Add(option);
    }

    /// <inheritdoc />
    public void RemoveOption(IListboxOption option)
    {
        if (option == null) throw new ArgumentNullException(nameof(option));

        _options.Remove(option);
    }

    /// <inheritdoc />
    public void SetButton(IFocus button)
    {
        _button = button ?? throw new ArgumentNullException(nameof(button));
    }

    /// <inheritdoc />
    public void SetLabel(IListboxLabel label)
    {
        Label = label ?? throw new ArgumentNullException(nameof(label));
    }

    /// <inheritdoc />
    public void SetTransition(ITransition transition)
    {
        _transition = transition ?? throw new ArgumentNullException(nameof(transition));
    }

    /// <inheritdoc />
    public Task OnAfterRenderAsync()
    {
        FrameTracker.OnAfterRender();
        
        return Task.CompletedTask;
    }
}
