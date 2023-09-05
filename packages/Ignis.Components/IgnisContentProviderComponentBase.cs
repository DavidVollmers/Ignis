using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components;

public abstract class IgnisContentProviderComponentBase : IgnisComponentBase, IContentProvider, IDisposable
{
    private bool _ignoreOutlet;
    
    [Parameter]
    public bool IgnoreOutlet
    {
        get => _ignoreOutlet;
        set
        {
            ContentRegistry.UnregisterContentProvider(this);

            _ignoreOutlet = value;

            RegisterAsContentProvider();
        }
    }

    [CascadingParameter] public IContentHost? Outlet { get; set; }
    
    public abstract RenderFragment Content { get; }

    [Inject] public IContentRegistry ContentRegistry { get; set; } = null!;

    protected override void OnInitialized()
    {
        RegisterAsContentProvider();
    }

    protected virtual void RegisterAsContentProvider()
    {
        if (!IgnoreOutlet && Outlet == null) ContentRegistry.RegisterContentProvider(this);
    }
    
    protected new void Update(bool async = false)
    {
        Outlet?.Update(async);
        
        base.Update(async);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (Outlet != null) return;
        
        builder.AddContent(0, Content);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;

        ContentRegistry.UnregisterContentProvider(this);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~IgnisContentProviderComponentBase()
    {
        Dispose(false);
    }
}
