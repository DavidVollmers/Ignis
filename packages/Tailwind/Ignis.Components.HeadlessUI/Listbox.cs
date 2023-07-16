﻿using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Listbox<TValue> : IgnisDynamicComponentBase, IListbox<TValue>
{
    private readonly string _id = "hui-listbox-" + Guid.NewGuid().ToString("N");

    public bool IsOpen { get; private set; }

    [Parameter] public TValue? Value { get; set; }

    [Parameter] public EventCallback<TValue?> ValueChanged { get; set; }

    [Parameter] public RenderFragment<IListbox<TValue>>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? Attributes { get; set; }

    public Listbox()
    {
        AsComponent = typeof(Fragment);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddChildContentFor(2, this, builder =>
        {
            builder.OpenComponent<FocusDetector>(3);
            builder.AddAttribute(4, nameof(FocusDetector.Id), _id);
            builder.AddAttribute(5, nameof(FocusDetector.OnBlur), EventCallback.Factory.Create(this, Close));
            // ReSharper disable once VariableHidesOuterVariable
            builder.AddAttribute(6, nameof(FocusDetector.ChildContent), (RenderFragment)(builder =>
            {
                builder.OpenComponent<CascadingValue<IListbox<TValue>>>(7);
                builder.AddAttribute(8, nameof(CascadingValue<IListbox<TValue>>.IsFixed), true);
                builder.AddAttribute(9, nameof(CascadingValue<IListbox<TValue>>.Value), this);
                builder.AddAttribute(10, nameof(CascadingValue<IListbox<TValue>>.ChildContent), ChildContent?.Invoke(this));

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
}
