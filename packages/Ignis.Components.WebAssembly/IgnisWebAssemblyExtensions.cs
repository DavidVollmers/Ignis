using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ignis.Components.WebAssembly;

public static class IgnisWebAssemblyExtensions
{
    public static IServiceCollection AddIgnisWebAssembly(this IServiceCollection serviceCollection)
    {
        if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));

        serviceCollection.AddIgnis();

        serviceCollection.TryAddScoped<IHostContext, WebAssemblyHostContext>();

        return serviceCollection;
    }
}
