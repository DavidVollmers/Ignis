using System.Net;
using System.Net.Http.Json;
using Ignis.Markdown.Processor.Contracts;
using Ignis.Website.Services;

namespace Ignis.Website.WebAssembly.Services;

internal class PageService : PageServiceBase
{
    private const string DocsPath = "/docs";

    private readonly HttpClient _httpClient;
    private readonly ILogger<PageService> _logger;

    public PageService(HttpClient httpClient, ILogger<PageService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<string?> LoadPageContentAsync(Page page, CancellationToken cancellationToken)
    {
        var path = DocsPath + page.Link;
        if (path == "/") path += "index";
        path += ".html";

        try
        {
            return await _httpClient.GetStringAsync(path, cancellationToken);
        }
        catch (HttpRequestException exception)
        {
            if (exception.StatusCode != HttpStatusCode.NotFound) throw;

            _logger.LogWarning("Sitemap file not found.");
            return null;
        }
    }

    protected override Task<Section[]?> LoadSectionsAsync(CancellationToken cancellationToken)
    {
        const string path = DocsPath + "/sitemap.json";

        return _httpClient.GetFromJsonAsync<Section[]>(path, cancellationToken);
    }
}
