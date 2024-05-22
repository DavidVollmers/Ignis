namespace Ignis.Website.Services;

public interface ISearchService
{
    IAsyncEnumerable<SearchResult> SearchAsync(string query, CancellationToken cancellationToken = default);
}
