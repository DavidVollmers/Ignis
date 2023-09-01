using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using Ignis.Components;
using Ignis.Components.Web;
using Ignis.Website.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Ignis.Website;

public static partial class IgnisWebsiteExtensions
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
            args.AddRange(type.GenericTypeArguments.Select(a => a.GetTypeInfo().SanitizeTypeName(true)));
            name += $"<{string.Join(", ", args)}>";
        }

        return name;
    }

    public static MarkupString ResolveCodeComment(this string comment)
    {
        if (comment == null) throw new ArgumentNullException(nameof(comment));

        var regex = SeeReferenceRegex();

        var matches = regex.Matches(comment);
        foreach (Match match in matches)
        {
            var cref = match.Groups["cref"].Value;
            var typeInfo = ResolveCRef(cref);
            if (typeInfo == null) continue;

            var name = typeInfo.SanitizeTypeName();
            var link = typeInfo.GetTypeDocumentationLink();
            comment = comment.Replace(match.Value, $"<a href=\"{link}\">{name}</a>");
        }

        return new MarkupString(comment);
    }

    private static TypeInfo? ResolveCRef(string cref)
    {
        if (cref == null) throw new ArgumentNullException(nameof(cref));

        var type = Type.GetType(cref);
        if (type != null) return type.GetTypeInfo();

        var parts = cref.Split(':');
        if (parts.Length != 2) return null;

        var assembly = Assembly.Load(parts[0]);
        type = assembly?.GetType(parts[1]);
        return type?.GetTypeInfo();
    }

    public static string GetTypeDocumentationLink(this TypeInfo typeInfo)
    {
        if (typeInfo == null) throw new ArgumentNullException(nameof(typeInfo));

        return $"/api/{HttpUtility.UrlEncode(typeInfo.AssemblyQualifiedName)}/_";
    }

    [GeneratedRegex("<see cref=\"T:(?<cref>.+?)\"\\s*/>", RegexOptions.Compiled)]
    private static partial Regex SeeReferenceRegex();
}
