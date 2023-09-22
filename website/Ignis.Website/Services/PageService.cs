using Ignis.Website.Contracts;
using Microsoft.Extensions.Logging;

namespace Ignis.Website.Services;

internal class PageService : IPageService
{
    private static readonly IDictionary<string, string?> Cache = new Dictionary<string, string?>(StringComparer.Ordinal);
    private static Section[]? _sections;

    private readonly IStaticFileService _staticFileService;
    private readonly ILogger<PageService> _logger;

    public PageService(IStaticFileService staticFileService, ILogger<PageService> logger)
    {
        _staticFileService = staticFileService ?? throw new ArgumentNullException(nameof(staticFileService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Section?> GetSectionByLinkAsync(string link, CancellationToken cancellationToken = default)
    {
        if (link == null) throw new ArgumentNullException(nameof(link));

        // ReSharper disable once InvertIf
        if (_sections == null)
        {
            await ReloadSectionsAsync(cancellationToken).ConfigureAwait(false);
        }

        var section = _sections?.FirstOrDefault(s => s.Pages.Any(l => CompareLinks(l.Link, link)));
        return section == null ? null : OrderSectionPages(section);
    }

    public async Task<Section[]?> GetSectionsAsync(CancellationToken cancellationToken = default)
    {
        if (_sections == null)
        {
            await ReloadSectionsAsync(cancellationToken).ConfigureAwait(false);
        }

        return _sections?
            .OrderBy(s => s.Pages.MinBy(l => l.Order)?.Order ?? -1)
            .Select(OrderSectionPages)
            .ToArray();
    }

    public async Task<string?> GetPageContentAsync(Page page, CancellationToken cancellationToken = default)
    {
        if (page == null) throw new ArgumentNullException(nameof(page));

        if (Cache.TryGetValue(page.Link, out var cachedContent)) return cachedContent;

        var pageContent = await LoadPageContentAsync(page, cancellationToken).ConfigureAwait(false);

        return Cache[page.Link] = pageContent;
    }

    private async Task<string?> LoadPageContentAsync(Page page, CancellationToken cancellationToken)
    {
        var path = page.Link;
        if (string.Equals(path, "/", StringComparison.Ordinal)) path += "index";
        path = "/docs" + path + ".html";

        return await _staticFileService.GetFileContentAsync(path, cancellationToken).ConfigureAwait(false);
    }

    private async Task<Section[]?> LoadSectionsAsync(CancellationToken cancellationToken)
    {
        const string path = "/docs/sitemap.json";

        return await _staticFileService.GetFileContentAsJsonAsync<Section[]>(path, cancellationToken).ConfigureAwait(false);
    }

    private async Task ReloadSectionsAsync(CancellationToken cancellationToken)
    {
        var sections = await LoadSectionsAsync(cancellationToken).ConfigureAwait(false);

        if (sections == null)
        {
            _logger.LogWarning("Failed to load sections.");
            return;
        }

        _sections = sections;

        Cache.Clear();
    }

    public bool CompareLinks(string link1, string link2)
    {
        if (link1.EndsWith("/", StringComparison.Ordinal)) link1 = link1[..^1];
        if (link2.EndsWith("/", StringComparison.Ordinal)) link2 = link2[..^1];
        return string.Equals(link1, link2, StringComparison.Ordinal);
    }

    private static Section OrderSectionPages(Section section)
    {
        return new Section { Title = section.Title, Pages = section.Pages.OrderBy(l => l.Order).ToArray() };
    }
}
