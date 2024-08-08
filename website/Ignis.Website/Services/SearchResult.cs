using Doki;
using Ignis.Website.Contracts;

namespace Ignis.Website.Services;

public record SearchResult
{
    public string Title { get; } = null!;

    public string Section { get; } = null!;

    public string Url { get; } = null!;

    public SearchResult(Page page, Section section)
    {
        Title = page.Title;
        Url = $"/docs{page.Link}";
        Section = section.Title;
    }

    public SearchResult(TypeDocumentation typeDocumentation)
    {
        Title = typeDocumentation.Name;
        Url = typeDocumentation.GetTypeDocumentationLink()!;
        Section = typeDocumentation.Namespace!;
    }
}
