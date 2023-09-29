using Ignis.Components;

namespace Ignis.Tests.Common;

internal class TestHostContext : IHostContext
{
    public bool IsPrerendering => false;

    public bool IsServerSide => true;
}
