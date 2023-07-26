using Ignis.Components;
using Ignis.Components.Web;
using Ignis.Website.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ignis.Website;

public static class IgnisWebsiteExtensions
{
    public static IServiceCollection AddIgnisWebsite(this IServiceCollection services)
    {
        services.AddIgnisWeb();
        services.AddIgnis();

        services.AddSingleton<IPageService, PageService>();

        return services;
    }
}
