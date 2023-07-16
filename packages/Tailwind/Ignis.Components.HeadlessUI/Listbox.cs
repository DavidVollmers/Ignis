using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public sealed class Listbox<TValue> : IgnisComponentBase, IDynamicComponent, IListbox<TValue>
{
    public bool IsOpen { get; private set; }

    [Parameter] public string? AsElement { get; set; }

    [Parameter] public Type? AsComponent { get; set; } = typeof(Fragment);

    [Parameter] public TValue? Value { get; set; }

    [Parameter] public EventCallback<TValue?>? ValueChanged { get; set; }

    [Parameter] public RenderFragment<IListbox<TValue>>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? Attributes { get; set; }

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
