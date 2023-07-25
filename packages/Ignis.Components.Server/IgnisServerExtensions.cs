using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ignis.Components.Server;

public static class IgnisServerExtensions
{
    public static IServiceCollection AddIgnisServerServices(this IServiceCollection serviceCollection)
    {
        if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));

        serviceCollection.AddHttpContextAccessor();

        serviceCollection.TryAddSingleton<IHostContext, ServerHostContext>();

        return serviceCollection;
    }
}
