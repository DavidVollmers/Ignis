using System.Xml.XPath;

namespace Ignis.Website.Services;

public interface IStaticFileService
{
    Task<string?> GetFileContentAsync(string path, CancellationToken cancellationToken = default);

    Task<T?> GetFileContentAsJsonAsync<T>(string path, CancellationToken cancellationToken = default);

    Task<XPathDocument?> GetFileContentAsXmlAsync(string path, CancellationToken cancellationToken = default);
}
