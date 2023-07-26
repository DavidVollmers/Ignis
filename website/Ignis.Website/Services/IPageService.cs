using Ignis.Markdown.Processor.Contracts;

namespace Ignis.Website.Services;

public interface IPageService
{
    Section? GetSectionByLink(string link);
    
    Section[] GetSections();
    
    string? GetPageContent(Page page);
}
