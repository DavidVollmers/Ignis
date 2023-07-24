﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Dialog : IgnisComponentBase, IDialog, IHandleAfterRender, IDisposable
{
    private readonly AttributeCollection _attributes;

    private IDialogDescription? _description;
    private Action? _continueWith;
    private IDialogTitle? _title;
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
    public string Id { get; } = "ignis-hui-dialog-" + Guid.NewGuid().ToString("N");

    /// <inheritdoc />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>> Attributes => _attributes;

    public Dialog()
    {
        AsElement = "div";

        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("id", Id), () => new KeyValuePair<string, object>("role", "dialog"),
            () => new KeyValuePair<string, object>("aria-modal", "true"), () => new KeyValuePair<string, object?>(
                "aria-labelledby",
                _title == null ? null : _title.Id ?? Id + "-title"),
            () => new KeyValuePair<string, object?>("aria-describedby",
                _description == null ? null : _description.Id ?? Id + "-description")
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        Transition?.AddDialog(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddContentFor(2, this, builder =>
        {
            builder.OpenComponent<CascadingValue<IDialog>>(3);
            builder.AddAttribute(4, nameof(CascadingValue<IDialog>.IsFixed), true);
            builder.AddAttribute(5, nameof(CascadingValue<IDialog>.Value), this);
            if (_isOpen)
                builder.AddAttribute(6, nameof(CascadingValue<IDialog>.ChildContent),
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

        _continueWith = continueWith;

        ForceUpdate(async);
    }

    /// <inheritdoc />
    public void SetTitle(IDialogTitle title)
    {
        if (_title != null && _title != title)
            throw new InvalidOperationException(
                $"{nameof(Dialog)} cannot contain multiple {nameof(DialogTitle)} components.");

        _title = title;
    }

    /// <inheritdoc />
    public void SetDescription(IDialogDescription description)
    {
        if (_description != null && _description != description)
            throw new InvalidOperationException(
                $"{nameof(Dialog)} cannot contain multiple {nameof(DialogDescription)} components.");

        _description = description;
    }

    /// <inheritdoc />
    public void CloseFromTransition(Action? continueWith = null)
    {
        CloseCore(continueWith, true);
    }

    public void Dispose()
    {
        Transition?.RemoveDialog(this);
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
