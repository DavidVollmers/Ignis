namespace Ignis.Markdown.Processor.Contracts;

public class Page
{
    public string Title { get; set; } = null!;

    public string Link { get; set; } = null!;

    public PageExample? Example { get; set; }
    
    public class PageExample
    {
        public string TypeName { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}
