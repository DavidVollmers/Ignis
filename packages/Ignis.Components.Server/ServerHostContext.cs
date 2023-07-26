using Microsoft.AspNetCore.Http;

namespace Ignis.Components.Server;

internal class ServerHostContext : IHostContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public bool IsPrerendering => !_httpContextAccessor.HttpContext?.Response.HasStarted ?? false;

    public bool IsServerSide => true;

    public ServerHostContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }
}
