using System.Runtime.CompilerServices;
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

    public async IAsyncEnumerable<SearchResult> SearchAsync(string query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
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
                        yield return new SearchResult(page, section);
                    }
                }
            }
        }

        var assemblies = await GetAssembliesAsync(cancellationToken).ConfigureAwait(false);

        foreach (var assemblyDocumentation in assemblies)
        {
            foreach (var namespaceDocumentation in assemblyDocumentation.Namespaces)
            {
                foreach (var typeDocumentation in namespaceDocumentation.Types)
                {
                    if (typeDocumentation.FullName.Contains(query, StringComparison.OrdinalIgnoreCase))
                    {
                        yield return new SearchResult(typeDocumentation);
                    }
                }
            }
        }
    }
}
