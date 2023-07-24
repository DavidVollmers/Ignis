using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Disclosure : IgnisComponentBase, IDisclosure, IHandleAfterRender
{
    private Action? _continueWith;
    private Type? _asComponent;
    private string? _asElement;
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
    public RenderFragment<IDisclosure>? _ { get; set; }

    [Parameter] public RenderFragment<IDisclosure>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>>? Attributes => AdditionalAttributes;

    public Disclosure()
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
            builder.OpenComponent<CascadingValue<IDisclosure>>(3);
            builder.AddAttribute(4, nameof(CascadingValue<IDisclosure>.IsFixed), true);
            builder.AddAttribute(5, nameof(CascadingValue<IDisclosure>.Value), this);
            builder.AddAttribute(6, nameof(CascadingValue<IDisclosure>.ChildContent),
                this.GetChildContent(ChildContent));

            builder.CloseComponent();
        });

        builder.CloseAs(this);
    }

    /// <inheritdoc />
    public void Open(Action? continueWith = null)
    {
        if (_isOpen || _continueWith != null) return;
        
        IsOpenChanged.InvokeAsync(_isOpen = true);

        _continueWith = continueWith;

        ForceUpdate();
    }

    /// <inheritdoc />
    public void Close(Action? continueWith = null)
    {
        if (!_isOpen || _continueWith != null) return;

        IsOpenChanged.InvokeAsync(_isOpen = false);

        _continueWith = continueWith;

        ForceUpdate();
    }

    /// <inheritdoc />
    public Task OnAfterRenderAsync()
    {
        var continueWith = _continueWith;
        
        _continueWith = null;
        
        continueWith?.Invoke();
        
        return Task.CompletedTask;
    }
}
