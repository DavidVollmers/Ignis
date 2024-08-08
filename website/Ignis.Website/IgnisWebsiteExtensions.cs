using System.Globalization;
using System.Text;
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
        services.AddScoped<ISearchService, SearchService>();

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

        var stringBuilder = new StringBuilder();

        stringBuilder.AppendHtml(xmlDocumentation);

        return new MarkupString(stringBuilder.ToString());
    }

#pragma warning disable MA0051
    private static void AppendHtml(this StringBuilder stringBuilder, DocumentationObject documentationObject)
#pragma warning restore MA0051
    {
        ArgumentNullException.ThrowIfNull(documentationObject);

        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch (documentationObject.ContentType)
        {
            case DocumentationContentType.Xml:
                stringBuilder.Append("<div>");
                var xmlDocumentation = (XmlDocumentation)documentationObject;
                foreach (var content in xmlDocumentation.Contents)
                    stringBuilder.AppendHtml(content);
                stringBuilder.Append("</div>");
                return;

            case DocumentationContentType.Text:
                stringBuilder.Append("<span>");
                var textDocumentation = (TextContent)documentationObject;
                stringBuilder.Append(' ').Append(textDocumentation.Text);
                stringBuilder.Append("</span>");
                return;

            case DocumentationContentType.Link:
                stringBuilder.Append("<a href=\"");
                var link = (Link)documentationObject;
                stringBuilder.Append(link.Url);
                stringBuilder.Append("\">");
                stringBuilder.Append(link.Text);
                stringBuilder.Append("</a>");
                return;

            case DocumentationContentType.CodeBlock:
                stringBuilder.Append("<pre class=\"whitespace-normal\"><code class=\"language-");
                var codeBlock = (CodeBlock)documentationObject;
                stringBuilder.Append(codeBlock.Language);
                stringBuilder.Append("\">");
                stringBuilder.Append(codeBlock.Code);
                stringBuilder.Append("</code></pre>");
                return;

            case DocumentationContentType.Object:
                //TODO resolve member documentation objects
                var typeDocumentationReference = documentationObject switch
                {
                    TypeDocumentationReference reference => reference,
                    _ => throw new InvalidOperationException("Invalid documentation object type.")
                };

                var hRef = typeDocumentationReference.GetTypeDocumentationLink();
                if (hRef != null)
                {
                    stringBuilder.Append("<a href=\"");
                    stringBuilder.Append(hRef);
                    stringBuilder.Append("\">");
                    stringBuilder.Append(typeDocumentationReference.IsDocumented
                        ? typeDocumentationReference.Name
                        : typeDocumentationReference.FullName);
                    stringBuilder.Append("</a>");
                }
                else
                {
                    stringBuilder.Append("<span>");
                    stringBuilder.Append(typeDocumentationReference.FullName);
                    stringBuilder.Append("</span>");
                }

                return;
        }
    }
}
