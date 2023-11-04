using Ignis.Components;
using Ignis.Components.Extensions;

namespace Ignis.Tests.Common;

internal class TestHostContext : HostContextBase
{
    public override bool IsPrerendering => false;

    public override bool IsServerSide => true;

    public TestHostContext(IEnumerable<IComponentExtension> componentExtensions) : base(componentExtensions)
    {
    }
}
