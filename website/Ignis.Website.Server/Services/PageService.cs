using System.Text.Json;
using Ignis.Markdown.Processor.Contracts;
using Ignis.Website.Services;

namespace Ignis.Website.Server.Services;

internal class PageService : PageServiceBase
{
    private readonly ILogger<PageService> _logger;

    public PageService(ILogger<PageService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<string?> LoadPageContentAsync(Page page, CancellationToken cancellationToken)
    {
        var path = page.Link;
        if (path == "/") path += "index";
        path += ".html";

        var pageFile = new FileInfo(BuildDocsPath(path.Split('/')));

        if (!pageFile.Exists) return null;
        
        return await File.ReadAllTextAsync(pageFile.FullName, cancellationToken);
    }

    protected override async Task<Section[]?> LoadSectionsAsync(CancellationToken cancellationToken)
    {
        var sitemapFile = new FileInfo(BuildDocsPath("sitemap.json"));

        // ReSharper disable once InvertIf
        if (!sitemapFile.Exists)
        {
            _logger.LogWarning("Sitemap file not found.");
            return null;
        }

        var json = await File.ReadAllTextAsync(sitemapFile.FullName, cancellationToken);
        
        return JsonSerializer.Deserialize<Section[]>(json);
    }

    private static string BuildDocsPath(params string[] parts)
    {
        var docsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "docs");

        return Path.Combine(docsPath, Path.Combine(parts));
    }
}
