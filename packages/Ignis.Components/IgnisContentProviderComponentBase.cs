using Microsoft.AspNetCore.Components;

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
    
    public RenderFragment Content => BuildRenderTree;

    protected override bool ShouldRender => IgnoreOutlet;

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
        if (Outlet != null) Outlet.Update(async);
        else base.Update(async);
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
