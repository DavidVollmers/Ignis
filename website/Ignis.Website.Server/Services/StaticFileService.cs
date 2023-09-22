using System.Text.Json;
using System.Xml.XPath;
using Ignis.Website.Services;

namespace Ignis.Website.Server.Services;

internal class StaticFileService : IStaticFileService
{
    public async Task<string?> GetFileContentAsync(string path, CancellationToken cancellationToken = default)
    {
        var file = new FileInfo(BuildPath(path));

        if (!file.Exists) return null;

        return await File.ReadAllTextAsync(file.FullName, cancellationToken).ConfigureAwait(false);
    }

    public async Task<T?> GetFileContentAsJsonAsync<T>(string path, CancellationToken cancellationToken = default)
    {
        var json = await GetFileContentAsync(path, cancellationToken).ConfigureAwait(false);

        return json == null ? default : JsonSerializer.Deserialize<T>(json);
    }

    public Task<XPathDocument?> GetFileContentAsXmlAsync(string path, CancellationToken cancellationToken = default)
    {
        var file = new FileInfo(BuildPath(path));

        return Task.FromResult(!file.Exists ? null : new XPathDocument(file.FullName));
    }

    private static string BuildPath(string path)
    {
        if (new FileInfo(path).Exists) return path;

        return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
            Path.Combine(path.Replace("/docs/", "/_docs/").Split('/')));
    }
}
