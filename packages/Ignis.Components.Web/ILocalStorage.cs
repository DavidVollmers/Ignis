namespace Ignis.Components.Web;

public interface ILocalStorage
{
    Task SetItemAsync<T>(string key, T value, CancellationToken cancellationToken = default);

    Task<T?> GetItemAsync<T>(string key, CancellationToken cancellationToken = default);

    Task RemoveItemAsync(string key, CancellationToken cancellationToken = default);

    Task ClearAsync(CancellationToken cancellationToken = default);

    Task<int> GetLengthAsync(CancellationToken cancellationToken = default);

    Task<string?> GetKeyAsync(int index, CancellationToken cancellationToken = default);
}
