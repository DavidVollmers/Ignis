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
    public virtual void OnProviderRegistered(IContentProvider provider)
    {
        if (provider == null) throw new ArgumentNullException(nameof(provider));

        if (_components.Contains(provider)) return;

        _components.Add(provider);

        // needs to be set here, because the provider might be rendered before the outlet is
        provider.HostedBy(this);

        base.Update();
    }

    /// <inheritdoc />
    public void OnProviderUnregistered(IContentProvider provider)
    {
        if (provider == null) throw new ArgumentNullException(nameof(provider));

        if (!_components.Contains(provider)) return;

        _components.Remove(provider);

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
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
