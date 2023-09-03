namespace Ignis.Website.Contracts;

public class Section
{
    public string Title { get; set; } = null!;

    public Page[] Pages { get; set; } = Array.Empty<Page>();
}
