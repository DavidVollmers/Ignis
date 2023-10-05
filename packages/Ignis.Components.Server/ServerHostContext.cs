using Ignis.Components.Extensions;
using Microsoft.AspNetCore.Http;

namespace Ignis.Components.Server;

internal class ServerHostContext : HostContextBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public override bool IsPrerendering => !_httpContextAccessor.HttpContext?.Response.HasStarted ?? false;

    public override bool IsServerSide => true;

    public ServerHostContext(IEnumerable<IComponentExtension> componentExtensions,
        IHttpContextAccessor httpContextAccessor) : base(componentExtensions)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }
}
