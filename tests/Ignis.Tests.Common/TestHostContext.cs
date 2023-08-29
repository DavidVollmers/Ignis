using Ignis.Components;

namespace Ignis.Tests.Common;

public class TestHostContext : IHostContext
{
    public bool IsPrerendering => false;

    public bool IsServerSide => true;
}
