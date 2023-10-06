using Ignis.Components.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Ignis.Components.Reactivity;

public static class IgnisReactivityExtensions
{
    public static IServiceCollection AddIgnisReactivity(this IServiceCollection serviceCollection)
    {
        if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
        
        serviceCollection.AddSingleton<IComponentExtension, ReactiveLinkExtension>();

        return serviceCollection;
    }
}
