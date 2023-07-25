using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class DialogOutlet : IgnisRigidComponentBase, IDynamicComponent
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

    [Inject] public IOutletRegistry OutletRegistry { get; set; } = null!;

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
            foreach (var component in OutletRegistry.GetFreeComponents())
            {
                if (component is not IDialog dialog) continue;
                
                builder.OpenComponent<Outlet>(2);
                builder.SetKey(dialog.Id);
                builder.AddAttribute(3, nameof(Outlet.ChildContent), dialog.OutletContent);
                
                builder.CloseComponent();
            }
        });

        builder.CloseAs(this);
    }
}
