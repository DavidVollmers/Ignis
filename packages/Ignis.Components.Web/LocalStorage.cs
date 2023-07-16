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

    public async Task SetItemAsync<T>(string key, T value)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
    }

    public async Task<T?> GetItemAsync<T>(string key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);

        return json == null ? default : JsonSerializer.Deserialize<T>(json);
    }

    public async Task RemoveItemAsync(string key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }

    public async Task ClearAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.clear");
    }

    public async Task<int> GetLengthAsync()
    {
        return await _jsRuntime.InvokeAsync<int>("localStorage.length");
    }

    public async Task<string?> GetKeyAsync(int index)
    {
        return await _jsRuntime.InvokeAsync<string?>("localStorage.key", index);
    }
}
