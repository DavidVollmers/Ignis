using Microsoft.AspNetCore.Http;

namespace Ignis.Components;

internal class Server : IServer
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public bool IsPrerendering => !_httpContextAccessor.HttpContext!.Response.HasStarted;

    public Server(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }
}
