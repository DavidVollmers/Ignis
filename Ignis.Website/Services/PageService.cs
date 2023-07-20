using System.Text.Json;
using Ignis.Markdown.Processor.Contracts;

namespace Ignis.Website.Services;

internal class PageService : IPageService
{
    private readonly ILogger<PageService> _logger;
    private readonly IDictionary<string, string?> _cache = new Dictionary<string, string?>();

    private Section[]? _sections;

    public PageService(ILogger<PageService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Section? GetSectionByLink(string link)
    {
        if (link == null) throw new ArgumentNullException(nameof(link));

        // ReSharper disable once InvertIf
        if (_sections == null)
        {
            LoadSections();

            if (_sections == null) return null;
        }

        var section = _sections!.FirstOrDefault(s => s.Links.Any(l => l.Link == link));
        return section == null
            ? null
            : new Section { Title = section.Title, Links = section.Links.OrderBy(l => l.Order).ToArray() };
    }

    public Section[] GetSections()
    {
        if (_sections == null)
        {
            LoadSections();
        }

        return _sections?.OrderBy(s => s.Links.MinBy(l => l.Order)?.Order ?? -1).ToArray() 
               ?? Array.Empty<Section>();
    }

    public string? GetPageContent(Page page)
    {
        if (page == null) throw new ArgumentNullException(nameof(page));

        if (_cache.TryGetValue(page.Link, out var cachedContent)) return cachedContent;

        var path = page.Link;
        if (path == "/") path += "index";
        path += ".html";

        var pageFile = new FileInfo(BuildDocsPath(path.Split('/')));

        if (!pageFile.Exists) return null;

        return _cache[page.Link] = File.ReadAllText(pageFile.FullName);
    }

    private void LoadSections()
    {
        var sitemapFile = new FileInfo(BuildDocsPath("sitemap.json"));

        if (!sitemapFile.Exists)
        {
            _logger.LogWarning("Sitemap file not found.");
            return;
        }

        _sections = JsonSerializer.Deserialize<Section[]>(File.ReadAllText(sitemapFile.FullName));

        _cache.Clear();
    }

    private static string BuildDocsPath(params string[] parts)
    {
        var docsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "docs");

        return Path.Combine(docsPath, Path.Combine(parts));
    }
}
