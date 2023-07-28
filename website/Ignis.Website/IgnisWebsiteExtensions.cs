using System.Reflection;
using Ignis.Components;
using Ignis.Components.Web;
using Ignis.Website.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ignis.Website;

public static class IgnisWebsiteExtensions
{
    public static IServiceCollection AddIgnisWebsite(this IServiceCollection services)
    {
        services.AddLocalization();
        
        services.AddIgnisWeb(); 
        services.AddIgnis();

        services.AddScoped<IPageService, PageService>();

        return services;
    }

    public static string SanitizeTypeName(this TypeInfo type, bool withNamespace = false)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        
        var name = withNamespace ? type.FullName! : type.Name;
        // ReSharper disable once InvertIf
        if (type.IsGenericType)
        {
            var index = name.LastIndexOf('`');
            name = name[..index];

            var args = type.GenericTypeParameters.Select(p => p.Name).ToList();
            args.AddRange(type.GenericTypeArguments.Select(a => SanitizeTypeName(a.GetTypeInfo(), true)));
            name += $"<{string.Join(", ", args)}>";
        }

        return name;
    }
}
