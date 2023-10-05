using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ignis.Components.Server;

public static class IgnisServerExtensions
{
    public static IServiceCollection AddIgnisServer(this IServiceCollection serviceCollection)
    {
        if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));

        serviceCollection.AddIgnis();

        serviceCollection.AddHttpContextAccessor();

        serviceCollection.TryAddScoped<IHostContext, ServerHostContext>();

        return serviceCollection;
    }
}
