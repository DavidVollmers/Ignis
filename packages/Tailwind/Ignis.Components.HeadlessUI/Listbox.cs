using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Listbox : IgnisDynamicComponentBase, IListbox
{
    private ListboxLabel? _label;
    private ListboxButton? _button;

    public string Id { get; } = "hui-listbox-" + Guid.NewGuid().ToString("N");

    public bool IsOpen { get; private set; }

    [Parameter] public object? Value { get; set; }

    [Parameter] public EventCallback<object?> ValueChanged { get; set; }

    [Parameter] public RenderFragment<IListbox>? ChildContent { get; set; }

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

    public void SetLabel(ListboxLabel label)
    {
        if (_label != null) throw new InvalidOperationException("ListboxLabel already set.");

        _label = label ?? throw new ArgumentNullException(nameof(label));
    }

    public void SetButton(ListboxButton button)
    {
        if (_button != null) throw new InvalidOperationException("ListboxButton already set.");

        _button = button ?? throw new ArgumentNullException(nameof(button));
    }
}
