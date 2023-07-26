using Ignis.Components;
using Ignis.Components.Web;
using Ignis.Website.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ignis.Website;

public static class IgnisWebsiteExtensions
{
    public static IServiceCollection AddIgnisWebsite<T>(this IServiceCollection services) where T : class, IPageService
    {
        services.AddLocalization();
        
        services.AddIgnisWeb(); 
        services.AddIgnis();

        services.AddSingleton<IPageService, T>();

        return services;
    }
}
