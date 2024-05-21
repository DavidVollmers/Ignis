namespace Ignis.Website.Services;

public interface ISearchService
{
    Task<SearchResult[]> SearchAsync(string query, CancellationToken cancellationToken = default);
}
