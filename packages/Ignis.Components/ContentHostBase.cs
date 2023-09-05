using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public abstract class ContentHostBase : IgnisComponentBase, IContentHost, IContentRegistrySubscriber, IDisposable
{
    private readonly IList<IContentProvider> _components = new List<IContentProvider>();

    private IContentRegistry? _outletRegistry;

    protected IEnumerable<IContentProvider> Components => _components;

    [Inject]
    public IContentRegistry? OutletRegistry
    {
        get => _outletRegistry;
        set
        {
            _outletRegistry?.Unsubscribe(this);

            _outletRegistry = value;
            _outletRegistry?.Subscribe(this);
        }
    }

    /// <inheritdoc />
    public virtual void OnProviderRegistered(IContentProvider component)
    {
        if (_components.Contains(component)) return;

        _components.Add(component);

        base.Update();
    }

    /// <inheritdoc />
    public void OnProviderUnregistered(IContentProvider component)
    {
        if (!_components.Contains(component)) return;

        _components.Remove(component);

        base.Update();
    }

    void IContentHost.Update(bool async)
    {
        base.Update(async);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;

        _outletRegistry?.Unsubscribe(this);
        _outletRegistry = null;

        _components.Clear();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~ContentHostBase()
    {
        Dispose(false);
    }
}
