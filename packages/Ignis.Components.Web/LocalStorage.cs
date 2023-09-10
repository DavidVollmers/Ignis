using System.Text.Json;
using Microsoft.JSInterop;

namespace Ignis.Components.Web;

internal class LocalStorage : ILocalStorage
{
    private readonly IJSRuntime _jsRuntime;

    public LocalStorage(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
    }

    public async Task SetItemAsync<T>(string key, T value, CancellationToken cancellationToken = default)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", cancellationToken, key,
            JsonSerializer.Serialize(value));
    }

    public async Task<T?> GetItemAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", cancellationToken, key);

        return json == null ? default : JsonSerializer.Deserialize<T>(json);
    }

    public async Task RemoveItemAsync(string key, CancellationToken cancellationToken = default)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", cancellationToken, key);
    }

    public async Task ClearAsync(CancellationToken cancellationToken = default)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.clear", cancellationToken);
    }

    public async Task<int> GetLengthAsync(CancellationToken cancellationToken = default)
    {
        return await _jsRuntime.InvokeAsync<int>("localStorage.length", cancellationToken);
    }

    public async Task<string?> GetKeyAsync(int index, CancellationToken cancellationToken = default)
    {
        return await _jsRuntime.InvokeAsync<string?>("localStorage.key", cancellationToken, index);
    }
}
