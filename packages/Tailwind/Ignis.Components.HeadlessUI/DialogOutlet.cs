using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class DialogOutlet : ContentHostBase, IDynamicComponent
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

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    /// <inheritdoc cref="IElementReferenceProvider.Element" />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    public DialogOutlet()
    {
        AsComponent = typeof(Fragment);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<CascadingValue<IContentHost>>(0);
        builder.AddAttribute(1, nameof(CascadingValue<IContentHost>.IsFixed), value: true);
        builder.AddAttribute(2, nameof(CascadingValue<IContentHost>.Value), this);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddAttribute(3, nameof(CascadingValue<IContentHost>.ChildContent), (RenderFragment)(builder =>
        {
            builder.OpenAs(4, this);
            builder.AddMultipleAttributes(5, AdditionalAttributes!);
            // ReSharper disable once VariableHidesOuterVariable
            builder.AddContentFor(6, this, builder =>
            {
                foreach (var dialog in Components)
                {
                    builder.AddContent(7, dialog.Content);
                }
            });

            builder.CloseAs(this);
        }));

        builder.CloseComponent();
    }

    /// <inheritdoc />
    public override void OnProviderRegistered(IContentProvider provider)
    {
        // ReSharper disable once ConvertIfStatementToSwitchStatement
        if (provider == null) throw new ArgumentNullException(nameof(provider));

        if (provider is not Dialog and not Transition { HasOutletDialogs: true }) return;

        base.OnProviderRegistered(provider);
    }
}
