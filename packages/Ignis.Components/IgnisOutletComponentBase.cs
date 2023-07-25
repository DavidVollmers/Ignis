using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public abstract class IgnisOutletComponentBase : IgnisComponentBase, IOutletComponent, IDisposable
{
    public bool IsAdopted => Outlet != null;

    [Parameter] public bool IgnoreOutlet { get; set; }

    [CascadingParameter] public IOutlet? Outlet { get; set; }
    
    public RenderFragment OutletContent => BuildRenderTree;

    protected override bool ShouldRender => IgnoreOutlet || !IsAdopted;

    [Inject] public IOutletRegistry OutletRegistry { get; set; } = null!;

    protected override void OnInitialized()
    {
        OutletRegistry.RegisterComponent(this);
    }

    protected new void ForceUpdate(bool async = false)
    {
        if (Outlet != null) Outlet.Update();
        else base.ForceUpdate(async);
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
