using YamlDotNet.Serialization;

namespace Ignis.Markdown.Processor;

internal class FrontMatterInformation
{
    [YamlMember(Alias = "title")] public string Title { get; set; } = null!;
}
