using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Tab : IgnisComponentBase, ITab, IDisposable
{
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

    //TODO aria-controls
    /// <inheritdoc />
    public IReadOnlyDictionary<string, object?> Attributes
    {
        get
        {
            var attributes = new Dictionary<string, object?>
            {
                { "tabindex", IsSelected ? 0 : -1 },
                { "role", "tab" },
                { "aria-selected", IsSelected },
                { "onclick", EventCallback.Factory.Create(this, OnClick) }
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
        builder.AddChildContentFor<ITab, Tab>(2, this, ChildContent?.Invoke(this));

        builder.CloseAs(this);
    }

    private void OnClick()
    {
        TabGroup.SelectTab(this);
    }

    public void Dispose()
    {
        TabGroup.RemoveTab(this);
    }
}
