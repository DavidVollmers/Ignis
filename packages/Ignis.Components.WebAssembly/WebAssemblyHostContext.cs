using Ignis.Components.Extensions;

namespace Ignis.Components.WebAssembly;

internal class WebAssemblyHostContext : HostContextBase
{
    public override bool IsPrerendering => false;

    public override bool IsServerSide => false;

    public WebAssemblyHostContext(IEnumerable<IComponentExtension> componentExtensions) : base(componentExtensions)
    {
    }
}
