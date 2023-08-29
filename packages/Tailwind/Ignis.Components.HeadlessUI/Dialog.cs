using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Dialog : IgnisOutletComponentBase, IDialog, IHandleAfterRender
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

    [CascadingParameter] public ITransition? Transition { get; set; }

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<IDialog>? _ { get; set; }

    [Parameter] public RenderFragment<IDialog>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes
    {
        get => _attributes.AdditionalAttributes;
        set => _attributes.AdditionalAttributes = value;
    }

    /// <inheritdoc />
    public IDialogDescription? Description { get; private set; }

    /// <inheritdoc />
    public IDialogTitle? Title { get; private set; }

    /// <inheritdoc />
    public string Id { get; } = "ignis-hui-dialog-" + Guid.NewGuid().ToString("N");

    /// <inheritdoc cref="IDynamicParentComponent{T}.Element" />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>> Attributes => _attributes;

    /// <inheritdoc />
    public override RenderFragment OutletContent => Transition?.RenderFragment ?? base.OutletContent;

    /// <inheritdoc />
    public override object Identifier => Transition ?? (object)this;

    [Inject] internal FrameTracker FrameTracker { get; set; } = null!;

    public Dialog()
    {
        AsElement = "div";

        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("id", Id),
            () => new KeyValuePair<string, object?>("role", "dialog"),
            () => new KeyValuePair<string, object?>("aria-modal", "true"), () => new KeyValuePair<string, object?>(
                "aria-labelledby", Title == null ? null : Title.Id ?? Id + "-title"),
            () => new KeyValuePair<string, object?>("aria-describedby",
                Description == null ? null : Description.Id ?? Id + "-description")
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        Transition?.AddDialog(this);

        base.OnInitialized();
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (!_isOpen) return;

        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddContentFor(2, this, builder =>
        {
            builder.OpenComponent<CascadingValue<IDialog>>(3);
            builder.AddAttribute(4, nameof(CascadingValue<IDialog>.IsFixed), true);
            builder.AddAttribute(5, nameof(CascadingValue<IDialog>.Value), this);
            builder.AddAttribute(6, nameof(CascadingValue<IDialog>.ChildContent), this.GetChildContent(ChildContent));

            builder.CloseComponent();
        });

        builder.CloseAs(this);
    }

    /// <inheritdoc />
    public void Open(Action? continueWith = null)
    {
        if (_isOpen || FrameTracker.IsPending) return;

        IsOpenChanged.InvokeAsync(_isOpen = true);

        if (continueWith != null) FrameTracker.ExecuteOnNextFrame(continueWith, Update);

        Update();
    }

    /// <inheritdoc />
    public void Close(Action? continueWith = null)
    {
        if (!_isOpen || FrameTracker.IsPending) return;

        if (Transition != null)
        {
            Transition.Hide(() => CloseCore(continueWith, true));
            return;
        }

        CloseCore(continueWith);
    }

    private void CloseCore(Action? continueWith, bool async = false)
    {
        IsOpenChanged.InvokeAsync(_isOpen = false);

        if (continueWith != null) FrameTracker.ExecuteOnNextFrame(continueWith, Update);

        Update(async);
    }

    /// <inheritdoc />
    public void SetTitle(IDialogTitle title)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
    }

    /// <inheritdoc />
    public void SetDescription(IDialogDescription description)
    {
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }

    /// <inheritdoc />
    public void CloseFromTransition(Action? continueWith = null)
    {
        CloseCore(continueWith, true);
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
