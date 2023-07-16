using Microsoft.Extensions.DependencyInjection;

namespace Ignis.Components.Web;

public static class IgnisWebComponentExtensions
{
    public static IServiceCollection AddIgnisWebServices(this IServiceCollection serviceCollection)
    {
        if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));
        
        serviceCollection.AddScoped<ILocalStorage, LocalStorage>();
        
        return serviceCollection;
    }
}
