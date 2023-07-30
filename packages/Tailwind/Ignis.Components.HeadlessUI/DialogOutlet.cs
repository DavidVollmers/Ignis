using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class DialogOutlet : IgnisComponentBase, IDynamicComponent, IOutletRegistrySubscriber, IOutlet,
    IDisposable
{
    private readonly IList<IDialog> _dialogs = new List<IDialog>();

    private IOutletRegistry? _outletRegistry;
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

    [Inject]
    public IOutletRegistry? OutletRegistry
    {
        get => _outletRegistry;
        set
        {
            _outletRegistry?.Unsubscribe(this);

            _outletRegistry = value;
            _outletRegistry?.Subscribe(this);
        }
    }

    public DialogOutlet()
    {
        AsComponent = typeof(Fragment);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddContentFor(1, this, builder =>
        {
            foreach (var dialog in _dialogs)
            {
                builder.AddContent(2, dialog.OutletContent);
            }
        });

        builder.CloseAs(this);
    }

    /// <inheritdoc />
    public void OnComponentRegistered(IOutletComponent component)
    {
        if (component is not IDialog dialog) return;

        _dialogs.Add(dialog);

        dialog.SetOutlet(this);

        Update();
    }

    /// <inheritdoc />
    public void OnComponentUnregistered(IOutletComponent component)
    {
        if (component is not IDialog dialog) return;

        if (!_dialogs.Contains(dialog)) return;

        _dialogs.Remove(dialog);

        dialog.SetFree();

        Update();
    }

    /// <inheritdoc />
    public void Update(bool async = false)
    {
        Update(async);
    }

    public void Dispose()
    {
        _outletRegistry?.Unsubscribe(this);
        _outletRegistry = null;

        foreach (var dialog in _dialogs)
        {
            dialog.SetFree();
        }

        _dialogs.Clear();
    }
}
