using System.Text.Json;
using Ignis.Website.Services;

namespace Ignis.Website.Server.Services;

internal class StaticFileService : IStaticFileService
{
    public async Task<string?> GetFileContentAsync(string path, CancellationToken cancellationToken = default)
    {
        var file = new FileInfo(BuildPath(path));

        if (!file.Exists)  return null;

        return await File.ReadAllTextAsync(file.FullName, cancellationToken);
    }

    public async Task<T?> GetFileContentAsync<T>(string path, CancellationToken cancellationToken = default)
    {
        var json = await GetFileContentAsync(path, cancellationToken);

        return json == null ? default : JsonSerializer.Deserialize<T>(json);
    }

    private static string BuildPath(string path)
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Path.Combine(path.Split('/')));
    }
}
