using Ignis.Components.Extensions;

namespace Ignis.Components;

public abstract class HostContextBase : IHostContext
{
    public abstract bool IsPrerendering { get; }

    public abstract bool IsServerSide { get; }

    public IEnumerable<IComponentExtension> ComponentExtensions { get; }

    protected HostContextBase(IEnumerable<IComponentExtension> componentExtensions)
    {
        ComponentExtensions = componentExtensions ?? throw new ArgumentNullException(nameof(componentExtensions));
    }

    public void OnComponentUpdate(IgnisComponentBase component)
    {
        if (component == null) throw new ArgumentNullException(nameof(component));

        foreach (var extension in ComponentExtensions)
        {
            extension.OnUpdate(component);
        }
    }
}
