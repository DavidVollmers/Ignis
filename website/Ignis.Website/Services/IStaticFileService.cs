namespace Ignis.Website.Services;

public interface IStaticFileService
{
    Task<string?> GetFileContentAsync(string path, CancellationToken cancellationToken = default);
    
    Task<T?> GetFileContentAsync<T>(string path, CancellationToken cancellationToken = default);
}
