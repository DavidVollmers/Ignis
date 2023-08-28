using Ignis.Components;

namespace Ignis.Tests.Components.HeadlessUI;

public class TestHostContext : IHostContext
{
    public bool IsPrerendering => false;

    public bool IsServerSide => true;
}
