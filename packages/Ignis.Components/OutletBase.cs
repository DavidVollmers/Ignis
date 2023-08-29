using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public abstract class OutletBase : IgnisComponentBase, IOutlet, IOutletRegistrySubscriber, IDisposable
{
    private readonly IList<IOutletComponent> _components = new List<IOutletComponent>();

    private IOutletRegistry? _outletRegistry;

    protected IEnumerable<IOutletComponent> Components => _components;

    [Inject]
    public IOutletRegistry? OutletRegistry
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
    public virtual void OnComponentRegistered(IOutletComponent component)
    {
        if (_components.Contains(component) || _components.Any(c => c.Identifier.Equals(component.Identifier))) return;

        _components.Add(component);

        component.SetOutlet(this);

        base.Update();
    }

    /// <inheritdoc />
    public void OnComponentUnregistered(IOutletComponent component)
    {
        if (!_components.Contains(component)) return;

        _components.Remove(component);

        component.SetFree();

        base.Update();
    }

    void IOutlet.Update(bool async)
    {
        base.Update(async);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;

        _outletRegistry?.Unsubscribe(this);
        _outletRegistry = null;

        foreach (var component in _components)
        {
            component.SetFree();
        }

        _components.Clear();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~OutletBase()
    {
        Dispose(false);
    }
}
