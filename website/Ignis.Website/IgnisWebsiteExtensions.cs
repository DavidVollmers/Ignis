using System.Globalization;
using System.Web;
using Doki;
using Ignis.Components;
using Ignis.Components.Web;
using Ignis.Website.Services;
using Microsoft.AspNetCore.Components;
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

    public static string GetTypeDocumentationLink(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var id = $"{type.FullName}, {type.Assembly.GetName().Name}";

        return $"/api/{HttpUtility.UrlEncode(id)}/_";
    }

    public static string? GetTypeDocumentationLink(this TypeDocumentationReference typeDocumentationReference)
    {
        ArgumentNullException.ThrowIfNull(typeDocumentationReference);

        if (typeDocumentationReference.IsMicrosoft)
            return
                $"https://learn.microsoft.com/{CultureInfo.CurrentUICulture}/dotnet/api/{typeDocumentationReference.FullName}";

        if (!typeDocumentationReference.IsDocumented) return null;

        var id = $"{typeDocumentationReference.FullName}, {typeDocumentationReference.Assembly}";

        return $"/api/{HttpUtility.UrlEncode(id)}/_";
    }

    public static MarkupString ToMarkupString(this XmlDocumentation xmlDocumentation)
    {
        ArgumentNullException.ThrowIfNull(xmlDocumentation);

        return new MarkupString("TODO: Implement ToMarkupString() method");
    }
}
