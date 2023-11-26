using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components;

public abstract class ContentProviderBase : IgnisComponentBase, IContentProvider, IDisposable
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

    public RenderFragment Content => BuildContentRenderTree;

    [Inject] public IContentRegistry ContentRegistry { get; set; } = null!;

    protected override void OnInitialized()
    {
        RegisterAsContentProvider();
    }

    protected virtual void RegisterAsContentProvider()
    {
        if (!IgnoreOutlet && Outlet == null) ContentRegistry.RegisterContentProvider(this);
    }

    public void HostedBy(IContentHost host)
    {
        Outlet = host ?? throw new ArgumentNullException(nameof(host));

        base.Update();
    }

    protected internal override void Update(bool async = false)
    {
        Outlet?.Update(async);

        base.Update(async);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (Outlet != null) return;

        BuildContentRenderTree(builder);
    }

    protected abstract void BuildContentRenderTree(RenderTreeBuilder builder);

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;

        ContentRegistry.UnregisterContentProvider(this);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
