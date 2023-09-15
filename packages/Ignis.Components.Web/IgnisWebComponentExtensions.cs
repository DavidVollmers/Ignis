using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ignis.Components.Web;

public static class IgnisWebComponentExtensions
{
    public static IServiceCollection AddIgnisWeb(this IServiceCollection serviceCollection)
    {
        if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));

        serviceCollection.TryAddScoped<ILocalStorage, LocalStorage>();

        return serviceCollection;
    }
}
