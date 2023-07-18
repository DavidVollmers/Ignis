using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;
using YamlDotNet.Serialization;

namespace Ignis.Markdown.Processor;

internal class DocsBuilder
{
    private readonly MarkdownPipeline _markdownPipeline = new MarkdownPipelineBuilder().UseYamlFrontMatter().Build();

    private readonly IDeserializer _yamlDeserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();

    public void Build(DirectoryInfo sourceDirectory, DirectoryInfo outputDirectory)
    {
        var markdownFiles = sourceDirectory.EnumerateFiles("*.md", SearchOption.AllDirectories);

        var categories = new Dictionary<string, Category>();

        foreach (var markdownFile in markdownFiles)
        {
            var markdown = File.ReadAllText(markdownFile.FullName);

            var markdownDocument = Markdig.Markdown.Parse(markdown, _markdownPipeline);

            var blocks = markdownDocument.Descendants<YamlFrontMatterBlock>();

            var yamlFrontMatter = blocks.FirstOrDefault();

            if (yamlFrontMatter == null)
            {
                Console.WriteLine("No YAML front matter found in: " + markdownFile.FullName);
                continue;
            }

            var yaml = yamlFrontMatter.Lines.Lines.OrderByDescending(l => l.Line).Select(l => l.ToString() + '\n')
                .Where(l => !l.StartsWith("---")).Aggregate((s, a) => s + a);

            var frontMatterInformation = _yamlDeserializer.Deserialize<FrontMatterInformation>(yaml);

            categories.TryAdd(frontMatterInformation.Category,
                new Category { Title = frontMatterInformation.Category });

            categories[frontMatterInformation.Category].Links.Add(new PageLink
            {
                Title = frontMatterInformation.Title, Link = frontMatterInformation.Permalink
            });

            var html = markdownDocument.ToHtml(_markdownPipeline);

            var outputPath = Path.Combine(frontMatterInformation.Permalink.Split('/')
                .Prepend(outputDirectory.FullName)
                .ToArray());
            
            if (frontMatterInformation.Permalink == "/") outputPath = Path.Combine(outputPath, "index");
            
            var outputFile = new FileInfo(outputPath + ".html");
            
            outputFile.Directory!.Create();

            using var writer = outputFile.CreateText();
            writer.Write(html);
        }
    }

    private class Category
    {
        public string Title { get; set; } = null!;

        public IList<PageLink> Links { get; set; } = new List<PageLink>();
    }

    private class PageLink
    {
        public string Title { get; set; } = null!;

        public string Link { get; set; } = null!;
    }
}
