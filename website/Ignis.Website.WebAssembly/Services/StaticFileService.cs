using System.Net;
using System.Net.Http.Json;
using System.Xml.XPath;
using Ignis.Website.Services;

namespace Ignis.Website.WebAssembly.Services;

internal class StaticFileService : IStaticFileService
{
    private readonly HttpClient _httpClient;

    public StaticFileService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<string?> GetFileContentAsync(string path, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _httpClient.GetStringAsync(BuildPath(path), cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException exception)
        {
            if (exception.StatusCode == HttpStatusCode.NotFound) return null;

            throw;
        }
    }

    public Task<T?> GetFileContentAsJsonAsync<T>(string path, CancellationToken cancellationToken = default)
    {
        return _httpClient.GetFromJsonAsync<T>(BuildPath(path), cancellationToken);
    }

    public async Task<XPathDocument?> GetFileContentAsXmlAsync(string path,
        CancellationToken cancellationToken = default)
    {
        var content = await GetFileContentAsync(path, cancellationToken).ConfigureAwait(false);

        return content == null ? null : new XPathDocument(new StringReader(content));
    }

    private static string BuildPath(string path)
    {
        return path.Replace("/docs/", "/_docs/");
    }
}
