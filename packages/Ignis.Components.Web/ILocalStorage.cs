namespace Ignis.Components.Web;

public interface ILocalStorage
{
    Task SetItemAsync<T>(string key, T value);
    
    Task<T?> GetItemAsync<T>(string key);
    
    Task RemoveItemAsync(string key);
    
    Task ClearAsync();
    
    Task<int> GetLengthAsync();
    
    Task<string?> GetKeyAsync(int index);
}
