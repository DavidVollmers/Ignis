using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class DialogOutlet : IgnisRigidComponentBase, IDynamicComponent, IOutlet, IDisposable
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

    [Inject] internal IOutletRegistry OutletRegistry { get; set; } = null!;
    
    public DialogOutlet()
    {
        AsComponent = typeof(Fragment);
    }

    /// <inheritdoc />
    protected override void OnRender()
    {
        OutletRegistry.AddOutlet(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        
        
        builder.CloseAs(this);
    }

    public void Dispose()
    {
        OutletRegistry.RemoveOutlet(this);
    }

    /// <inheritdoc />
    public bool CanOutlet(IOutletComponent outletComponent)
    {
        return outletComponent is IDialog;
    }
}
