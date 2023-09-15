using System.Text.Json;
using Ignis.Website.Contracts;
using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;
using YamlDotNet.Serialization;

namespace Ignis.Website.Generator;

internal class DocsBuilder
{
    private readonly MarkdownPipeline _markdownPipeline = new MarkdownPipelineBuilder()
        .UseYamlFrontMatter()
        .UsePipeTables()
        .UseDiagrams()
        .Build();

    private readonly IDeserializer _yamlDeserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();

    public void Build(DirectoryInfo sourceDirectory, DirectoryInfo outputDirectory)
    {
        var markdownFiles = sourceDirectory.EnumerateFiles("*.md", SearchOption.AllDirectories);

        var sections = new Dictionary<string, List<Page>>();

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

            var yaml = yamlFrontMatter.Lines.Lines.Select(l => l.ToString() + '\n').Where(l => !l.StartsWith("---"))
                .Aggregate((s, a) => s + a);

            var frontMatterInformation = _yamlDeserializer.Deserialize<FrontMatterInformation>(yaml);

            sections.TryAdd(frontMatterInformation.Category, new List<Page>());

            sections[frontMatterInformation.Category].Add(new Page
            {
                Order = frontMatterInformation.Order,
                Title = frontMatterInformation.Title,
                Link = frontMatterInformation.Permalink,
                ApiTypeNames = frontMatterInformation.Api,
                Inject = frontMatterInformation.Inject == null
                    ? null
                    : new Page.PageInject
                    {
                        TypeName = frontMatterInformation.Inject.Type,
                        Description = frontMatterInformation.Inject.Description
                    }
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

        var sitemapJson = JsonSerializer.Serialize(sections.Select(s => new Section
        {
            Title = s.Key,
            Pages = s.Value.ToArray()
        }));

        var sitemapOutputFile = new FileInfo(Path.Combine(outputDirectory.FullName, "sitemap.json"));

        sitemapOutputFile.Directory!.Create();

        using var sitemapWriter = sitemapOutputFile.CreateText();

        sitemapWriter.Write(sitemapJson);
    }
}
