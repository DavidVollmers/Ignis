using Ignis.Components;
using Ignis.Components.Web;
using Microsoft.Extensions.DependencyInjection;

namespace Ignis.Website;

public static class IgnisWebsiteExtensions
{
    public static IServiceCollection AddIgnisWebsite(this IServiceCollection services)
    {
        services.AddLocalization();
        
        services.AddIgnisWeb(); 
        services.AddIgnis();

        return services;
    }
}
