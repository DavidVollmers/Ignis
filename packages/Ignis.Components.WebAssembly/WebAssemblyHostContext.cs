namespace Ignis.Components.WebAssembly;

internal class WebAssemblyHostContext : IHostContext
{
    public bool IsPrerendering => false;

    public bool IsServerSide => false;
}
