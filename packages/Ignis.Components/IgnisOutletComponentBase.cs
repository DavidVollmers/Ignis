using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public abstract class IgnisOutletComponentBase : IgnisComponentBase, IOutletComponent, IDisposable
{
    private bool _ignoreOutlet;
    private IOutlet? _outlet;

    [Parameter]
#pragma warning disable BL0007
    public bool IgnoreOutlet
#pragma warning restore BL0007
    {
        get => _ignoreOutlet;
        set
        {
            OutletRegistry.UnregisterComponent(this);

            _ignoreOutlet = value;

            if (!_ignoreOutlet) OutletRegistry.RegisterComponent(this);
        }
    }

    public virtual object Identifier => this;

    public virtual RenderFragment OutletContent => BuildRenderTree;

    protected override bool ShouldRender => IgnoreOutlet || _outlet == null;

    [Inject] public IOutletRegistry OutletRegistry { get; set; } = null!;

    protected override void OnInitialized()
    {
        if (!IgnoreOutlet) OutletRegistry.RegisterComponent(this);
    }

    protected new void Update(bool async = false)
    {
        if (_outlet != null) _outlet.Update(async);
        else base.Update(async);
    }

    public void SetOutlet(IOutlet outlet)
    {
        if (outlet == null) throw new ArgumentNullException(nameof(outlet));

        if (IgnoreOutlet) return;

        if (_outlet != null && outlet != _outlet) throw new InvalidOperationException("Component is already adopted.");

        _outlet = outlet;
        
        //TODO rerender the original component to not have the same content rendered twice
    }

    public void SetFree()
    {
        _outlet = null;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;

        OutletRegistry.UnregisterComponent(this);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~IgnisOutletComponentBase()
    {
        Dispose(false);
    }
}
