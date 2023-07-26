using Ignis.Markdown.Processor.Contracts;

namespace Ignis.Website.Services;

public interface IPageService
{
    Task<Section?> GetSectionByLinkAsync(string link, CancellationToken cancellationToken = default);
    
    Task<Section[]?> GetSectionsAsync(CancellationToken cancellationToken = default);
    
    Task<string?> GetPageContentAsync(Page page, CancellationToken cancellationToken = default);
}
