using Ignis.Components.HeadlessUI.Aria;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

/// <summary>
/// Renders a dialog which can be opened and closed.
/// </summary>
public sealed class Dialog : ContentProviderBase, IDynamicParentComponent<Dialog>, IAriaModal,
    IHandleAfterRender
{
    private readonly AttributeCollection _attributes;

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

    /// <summary>
    /// Gets or sets the transition which is used to animate the dialog.
    /// </summary>
    [CascadingParameter]
    public Transition? Transition { get; set; }

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<Dialog>? _ { get; set; }

    /// <summary>
    /// Gets or sets the content of the dialog.
    /// </summary>
    [Parameter]
    public RenderFragment<Dialog>? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets additional attributes that will be applied to the dialog.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes
    {
        get => _attributes.AdditionalAttributes;
        set => _attributes.AdditionalAttributes = value;
    }

    public IAriaComponentPart? Description { get; set; }

    public IAriaComponentPart? Label { get; set; }

    public string Id { get; } = "ignis-hui-dialog-" + Guid.NewGuid().ToString("N");

    /// <inheritdoc cref="IElementReferenceProvider.Element" />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>> Attributes => _attributes;

    [Inject] private IFrameTracker FrameTracker { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dialog" /> class.
    /// </summary>
    public Dialog()
    {
        AsElement = "div";

        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("id", GetId(this)),
            () => new KeyValuePair<string, object?>("role", "dialog"),
            () => new KeyValuePair<string, object?>("aria-modal", "true"), 
            () => new KeyValuePair<string, object?>("aria-labelledby", GetId(Label)),
            () => new KeyValuePair<string, object?>("aria-describedby", GetId(Description)),
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        Transition?.AddDialog(this);

        base.OnInitialized();
    }

    /// <inheritdoc />
    public string? GetId(IAriaComponentPart? componentPart)
    {
        if (componentPart == null) return null;

        if (componentPart.Id != null) return componentPart.Id;
        
        if (componentPart == Label) return Id + "-label";

        if (componentPart == Description) return Id + "-description";
        
        return null;
    }

    /// <inheritdoc />
    protected override void RegisterAsContentProvider()
    {
        if (Transition != null) return;

        base.RegisterAsContentProvider();
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        // Always render the dialog if it's in a transition since the transition will be handled by DialogOutlet
        if (Transition != null)
        {
            BuildContentRenderTree(builder);
            return;
        }

        base.BuildRenderTree(builder);
    }

    /// <inheritdoc />
    protected override void BuildContentRenderTree(RenderTreeBuilder builder)
    {
        if (!_isOpen) return;

        builder.OpenComponent<CascadingValue<Dialog>>(0);
        builder.AddAttribute(1, nameof(CascadingValue<Dialog>.IsFixed), value: true);
        builder.AddAttribute(2, nameof(CascadingValue<Dialog>.Value), this);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddAttribute(3, nameof(CascadingValue<Dialog>.ChildContent), (RenderFragment)(builder =>
        {
            builder.OpenAs(4, this);
            builder.AddMultipleAttributes(5, Attributes!);
            builder.AddChildContentFor(6, this, ChildContent);

            builder.CloseAs(this);
        }));

        builder.CloseComponent();
    }

    /// <inheritdoc />
    public void Open(Action? continueWith = null)
    {
        if (_isOpen || FrameTracker.IsPending)
        {
            if (_isOpen) continueWith?.Invoke();
            return;
        }

        var __ = IsOpenChanged.InvokeAsync(_isOpen = true);

        if (continueWith != null) FrameTracker.ExecuteOnNextFrame(this, continueWith);

        Update();
    }

    /// <inheritdoc />
    public void Close(Action? continueWith = null)
    {
        if (!_isOpen || FrameTracker.IsPending)
        {
            if (!_isOpen) continueWith?.Invoke();
            return;
        }

        if (Transition != null)
        {
            Transition.Leave(() => CloseCore(continueWith, async: true));
            return;
        }

        CloseCore(continueWith);
    }

    private void CloseCore(Action? continueWith, bool async = false)
    {
        var __ = IsOpenChanged.InvokeAsync(_isOpen = false);

        if (continueWith != null) FrameTracker.ExecuteOnNextFrame(this, continueWith);

        Update(async);
    }

    internal void CloseFromTransition(Action? continueWith = null)
    {
        CloseCore(continueWith, async: true);
    }

    /// <inheritdoc />
    public Task OnAfterRenderAsync()
    {
        FrameTracker.OnAfterRender();

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Transition?.RemoveDialog(this);
        }

        base.Dispose(disposing);
    }
}
