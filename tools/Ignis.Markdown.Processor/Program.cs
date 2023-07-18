using System.Reflection;
using Ignis.Markdown.Processor;
using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;
using YamlDotNet.Serialization;

var assemblyFile = new FileInfo(Assembly.GetExecutingAssembly().Location);
var markdownDirectory = new DirectoryInfo(Path.Combine(assemblyFile.DirectoryName!, "..", "..", "..", "..", "..", "docs"));
var markdownFiles = markdownDirectory.EnumerateFiles("*.md", SearchOption.AllDirectories);

var pipeline = new MarkdownPipelineBuilder()
    .UseYamlFrontMatter()
    .Build();

var deserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();

foreach (var markdownFile in markdownFiles)
{
    var markdown = File.ReadAllText(markdownFile.FullName);

    var markdownDocument = Markdown.Parse(markdown, pipeline);

    var blocks = markdownDocument.Descendants<YamlFrontMatterBlock>();

    var yamlFrontMatter = blocks.FirstOrDefault();

    if (yamlFrontMatter == null)
    {
        Console.WriteLine("No YAML front matter found in: " + markdownFile.FullName);
        continue;
    }

    var yaml = yamlFrontMatter.Lines.Lines.OrderByDescending(l => l.Line).Select(l => l.ToString() + '\n')
        .Where(l => !l.StartsWith("---")).Aggregate((s, a) => s + a);

    var frontMatterInformation = deserializer.Deserialize<FrontMatterInformation>(yaml);
}
