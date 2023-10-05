using Ignis.Components.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Ignis.Components.Reactivity;

public static class IgnisReactivityExtensions
{
    public static IServiceCollection AddIgnisReactivity(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddSingleton<IComponentExtension, IgnisReactivityExtension>();

        return services;
    }
}
