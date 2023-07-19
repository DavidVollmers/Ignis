using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class Tab : IgnisComponentBase, ITab, IDisposable
{
    private bool _preventKeyDownDefault;
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

    [CascadingParameter] public ITabGroup TabGroup { get; set; } = null!;

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<ITab>? _ { get; set; }

    [Parameter] public RenderFragment<ITab>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    public bool IsSelected => TabGroup.IsTabSelected(this);

    /// <inheritdoc />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    //TODO aria-controls
    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>>? Attributes
    {
        get
        {
            var attributes = new Dictionary<string, object?>
            {
                { "tabindex", IsSelected ? 0 : -1 },
                { "role", "tab" },
                { "aria-selected", IsSelected },
                { "onclick", EventCallback.Factory.Create(this, OnClick) },
                { "__internal_preventDefault_onkeydown", _preventKeyDownDefault },
#pragma warning disable CS0618
                { "onkeydown", EventCallback.Factory.Create(this, OnKeyDown) },
#pragma warning restore CS0618
            };

            if (AsElement == "button") attributes.Add("type", "button");

            // ReSharper disable once InvertIf
            if (AdditionalAttributes != null)
            {
                foreach (var (key, value) in AdditionalAttributes)
                {
                    attributes[key] = value;
                }
            }

            return attributes;
        }
    }

    public Tab()
    {
        AsElement = "button";
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (TabGroup == null)
        {
            throw new InvalidOperationException($"{nameof(Tab)} must be used inside a {nameof(HeadlessUI.TabGroup)}.");
        }

        TabGroup.AddTab(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (AsElement != null) builder.AddElementReferenceCapture(2, e => Element = e);
        builder.AddChildContentFor<ITab, Tab>(3, this, ChildContent?.Invoke(this));
        if (AsComponent != null && AsComponent != typeof(Fragment))
            builder.AddComponentReferenceCapture(4, c => Component = c);

        builder.CloseAs(this);
    }

    private void OnKeyDown(KeyboardEventArgs eventArgs)
    {
        var oldPreventKeyDownDefault = _preventKeyDownDefault;

        _preventKeyDownDefault = true;

        switch (eventArgs.Code)
        {
            case "ArrowLeft":
                TabGroup.SelectTab(TabGroup.SelectedIndex == 0
                    ? TabGroup.Tabs[^1]
                    : TabGroup.Tabs[TabGroup.SelectedIndex - 1]);
                break;
            case "ArrowRight":
                TabGroup.SelectTab(TabGroup.SelectedIndex == TabGroup.Tabs.Length - 1
                    ? TabGroup.Tabs[0]
                    : TabGroup.Tabs[TabGroup.SelectedIndex + 1]);
                break;
            default:
                _preventKeyDownDefault = false;
                break;
        }

        if (oldPreventKeyDownDefault != _preventKeyDownDefault)
        {
            ForceUpdate();
        }
    }

    private void OnClick()
    {
        TabGroup.SelectTab(this);
    }

    /// <inheritdoc />
    public async Task FocusAsync()
    {
        if (Element.HasValue)
        {
            await Element.Value.FocusAsync();
        }
        else if (Component is IFocus focus)
        {
            await focus.FocusAsync();
        }

        ForceUpdate();
    }

    public void Dispose()
    {
        TabGroup.RemoveTab(this);
    }
}
