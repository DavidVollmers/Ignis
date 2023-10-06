using Ignis.Components;
using Ignis.Components.Extensions;

namespace Ignis.Tests.Components;

internal class TestComponentExtension : IComponentExtension
{
    private readonly Action<IgnisComponentBase> _onUpdate;

    public TestComponentExtension(Action<IgnisComponentBase> onUpdate)
    {
        _onUpdate = onUpdate ?? throw new ArgumentNullException(nameof(onUpdate));
    }

    public void OnUpdate(IgnisComponentBase component)
    {
        _onUpdate(component);
    }
}
