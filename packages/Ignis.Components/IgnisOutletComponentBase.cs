using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public abstract class IgnisOutletComponentBase : IgnisComponentBase, IOutletComponent, IDisposable
{
    private bool _ignoreOutlet;
    
    [Parameter]
    public bool IgnoreOutlet
    {
        get => _ignoreOutlet;
        set
        {
            OutletRegistry.UnregisterComponent(this);

            _ignoreOutlet = value;

            if (!_ignoreOutlet && Outlet == null) OutletRegistry.RegisterComponent(this);
        }
    }

    [CascadingParameter] public IOutlet? Outlet { get; set; }
    
    public virtual RenderFragment Content => BuildRenderTree;

    protected override bool ShouldRender => IgnoreOutlet;

    [Inject] public IOutletRegistry OutletRegistry { get; set; } = null!;

    protected override void OnInitialized()
    {
        if (!IgnoreOutlet && Outlet == null) OutletRegistry.RegisterComponent(this);
    }

    protected new void Update(bool async = false)
    {
        if (Outlet != null) Outlet.Update(async);
        else base.Update(async);
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
