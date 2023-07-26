using Ignis.Markdown.Processor.Contracts;

namespace Ignis.Website.Services;

public abstract class PageServiceBase : IPageService
{
    private readonly IDictionary<string, string?> _cache = new Dictionary<string, string?>();

    private Section[]? _sections;

    public async Task<Section?> GetSectionByLinkAsync(string link, CancellationToken cancellationToken = default)
    {
        if (link == null) throw new ArgumentNullException(nameof(link));

        // ReSharper disable once InvertIf
        if (_sections == null)
        {
            await ReloadSectionsAsync(cancellationToken);

            if (_sections == null) return null;
        }

        var section = _sections!.FirstOrDefault(s => s.Links.Any(l => l.Link == link));
        return section == null ? null : OrderSectionPages(section);
    }

    public async Task<Section[]?> GetSectionsAsync(CancellationToken cancellationToken = default)
    {
        if (_sections == null)
        {
            await ReloadSectionsAsync(cancellationToken);
        }

        return _sections?
            .OrderBy(s => s.Links.MinBy(l => l.Order)?.Order ?? -1)
            .Select(OrderSectionPages)
            .ToArray();
    }

    public async Task<string?> GetPageContentAsync(Page page, CancellationToken cancellationToken = default)
    {
        if (page == null) throw new ArgumentNullException(nameof(page));

        if (_cache.TryGetValue(page.Link, out var cachedContent)) return cachedContent;

        var pageContent = await LoadPageContentAsync(page, cancellationToken);
        
        return _cache[page.Link] = pageContent;
    }

    protected abstract Task<string?> LoadPageContentAsync(Page page, CancellationToken cancellationToken);
    
    protected abstract Task<Section[]?> LoadSectionsAsync(CancellationToken cancellationToken);
    
    private async Task ReloadSectionsAsync(CancellationToken cancellationToken)
    {
        var sections = await LoadSectionsAsync(cancellationToken);
        
        if (sections == null) return;

        _sections = sections;
        
        _cache.Clear();
    }

    private static Section OrderSectionPages(Section section)
    {
        return new Section { Title = section.Title, Links = section.Links.OrderBy(l => l.Order).ToArray() };
    }
}
