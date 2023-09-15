namespace Ignis.Website.Contracts;

public class Page
{
    public string Title { get; set; } = null!;

    public string Link { get; set; } = null!;

    public PageInject? Inject { get; set; }

    public string[]? ApiTypeNames { get; set; }

    public int? Order { get; set; }

    public class PageInject
    {
        public string TypeName { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}
