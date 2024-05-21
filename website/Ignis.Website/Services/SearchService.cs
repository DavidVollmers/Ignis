using Doki;

namespace Ignis.Website.Services;

internal class SearchService(IPageService pageService, IStaticFileService staticFileService) : ISearchService
{
    private static readonly string[] AssemblyNames =
    [
        "Ignis.Components",
        "Ignis.Components.HeadlessUI",
        "Ignis.Components.HeroIcons",
        "Ignis.Components.Reactivity",
        "Ignis.Components.Server",
        "Ignis.Components.Web",
        "Ignis.Components.WebAssembly",
        "Ignis.Fragments",
        "Ignis.Fragments.Abstractions",
        "Ignis.Fragments.Extensions",
        "Ignis.Utils"
    ];

    private AssemblyDocumentation[]? _assemblies;

    private async Task<AssemblyDocumentation[]> GetAssembliesAsync(CancellationToken cancellationToken)
    {
        var assemblies = new List<AssemblyDocumentation>();

        if (_assemblies != null) return _assemblies;

        foreach (var assemblyName in AssemblyNames)
        {
            var assemblyDocumentation = await staticFileService
                .GetFileContentAsJsonAsync<AssemblyDocumentation>($"/_api/{assemblyName}.json", cancellationToken)
                .ConfigureAwait(false);

            if (assemblyDocumentation != null) assemblies.Add(assemblyDocumentation);
        }

        _assemblies = assemblies.ToArray();

        return _assemblies;
    }

    public async Task<SearchResult[]> SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        var results = new List<SearchResult>();

        var sections = await pageService.GetSectionsAsync(cancellationToken).ConfigureAwait(false);
        if (sections != null)
        {
            foreach (var section in sections)
            {
                foreach (var page in section.Pages)
                {
                    var content = await pageService.GetPageContentAsync(page, cancellationToken).ConfigureAwait(false);
                    if (content != null && content.Contains(query, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(new SearchResult(page, section));
                    }
                }
            }
        }

        var assemblies = await GetAssembliesAsync(cancellationToken).ConfigureAwait(false);

        results.AddRange(from assembly in assemblies
                         from @namespace in assembly.Namespaces
                         from type in @namespace.Types
                         where type.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                         select new SearchResult(type));

        return results.OrderBy(r => r.Title, StringComparer.OrdinalIgnoreCase).ToArray();
    }
}
