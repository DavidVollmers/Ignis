using Ignis.Components.Extensions;

namespace Ignis.Components;

public interface IHostContext
{
    bool IsPrerendering { get; }

    bool IsServerSide { get; }

    IEnumerable<IComponentExtension> ComponentExtensions { get; }

    internal void OnComponentUpdate(IgnisComponentBase component);
}
