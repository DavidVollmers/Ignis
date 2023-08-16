﻿using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class ListboxButton : IgnisComponentBase, IListboxButton
{
    private readonly AttributeCollection _attributes;

    private Type? _asComponent;
    private string? _asElement;

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
    public string? Id { get; set; }

    [CascadingParameter] public IListbox Listbox { get; set; } = null!;

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<IListboxButton>? _ { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes
    {
        get => _attributes.AdditionalAttributes;
        set => _attributes.AdditionalAttributes = value;
    }

    /// <inheritdoc cref="IDynamicParentComponent{T}.Element" />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>> Attributes => _attributes;

    public ListboxButton()
    {
        AsElement = "button";

        //TODO aria-controls
        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("id", Id ?? Listbox.Id + "-button"),
            () => new KeyValuePair<string, object?>("aria-haspopup", "listbox"),
            () => new KeyValuePair<string, object?>("onclick", EventCallback.Factory.Create(this, () => Listbox.Open())),
            () => new KeyValuePair<string, object?>("__internal_preventDefault_onkeydown", Listbox.IsOpen),
#pragma warning disable CS0618
            () => new KeyValuePair<string, object?>("onkeydown", EventCallback.Factory.Create(this, OnKeyDown)),
#pragma warning restore CS0618
            () => new KeyValuePair<string, object?>("aria-expanded", Listbox.IsOpen.ToString().ToLowerInvariant()),
            () => new KeyValuePair<string, object?>("type", AsElement == "button" ? "button" : null),
            () => new KeyValuePair<string, object?>("aria-labelledby",
                Listbox.Label == null ? null : Listbox.Label.Id ?? Listbox.Id + "-label")
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Listbox == null)
        {
            throw new InvalidOperationException(
                $"{nameof(ListboxButton)} must be used inside a {nameof(Listbox<object>)}.");
        }

        Listbox.SetButton(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (AsElement != null) builder.AddElementReferenceCapture(2, e => Element = e);
        builder.AddChildContentFor<IListboxButton, ListboxButton>(3, this, ChildContent);
        if (AsComponent != null && AsComponent != typeof(Fragment))
            builder.AddComponentReferenceCapture(4, c => Component = c);

        builder.CloseAs(this);
    }

    private void OnKeyDown(KeyboardEventArgs eventArgs)
    {
        switch (eventArgs.Code)
        {
            case "Escape":
                Listbox.Close();
                break;
            case "Space" or "Enter":
                if (Listbox.IsOpen)
                {
                    Listbox.ActiveOption?.Select();
                    Listbox.Close();
                }
                else
                {
                    Listbox.Open();
                }

                break;
            case "ArrowUp" when Listbox.ActiveOption == null:
            case "ArrowDown" when Listbox.ActiveOption == null:
                if (Listbox.Options.Any()) Listbox.SetOptionActive(Listbox.Options[0], true);
                else if (!Listbox.IsOpen) Listbox.Open();
                break;
            case "ArrowDown":
            {
                var index = Array.IndexOf(Listbox.Options, Listbox.ActiveOption) + 1;
                if (index < Listbox.Options.Length) Listbox.SetOptionActive(Listbox.Options[index], true);
                else if (!Listbox.IsOpen) Listbox.Open();
                break;
            }
            case "ArrowUp":
            {
                var index = Array.IndexOf(Listbox.Options, Listbox.ActiveOption) - 1;
                if (index >= 0) Listbox.SetOptionActive(Listbox.Options[index], true);
                else if (!Listbox.IsOpen) Listbox.Open();
                break;
            }
        }
    }
}
