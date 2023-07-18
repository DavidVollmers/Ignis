namespace Ignis.Markdown.Processor.Contracts;

public class Section
{
    public string Title { get; set; } = null!;

    public Page[] Links { get; set; } = Array.Empty<Page>();
}
