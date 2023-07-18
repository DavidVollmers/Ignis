using YamlDotNet.Serialization;

namespace Ignis.Markdown.Processor;

internal class FrontMatterInformation
{
    [YamlMember(Alias = "title")] public string Title { get; set; } = null!;
    
    [YamlMember(Alias = "category")] public string Category { get; set; } = null!;
    
    [YamlMember(Alias = "permalink")] public string Permalink { get; set; } = null!;
    
    [YamlMember(Alias = "example")] public ExampleInformation? Example { get; set; }

    public class ExampleInformation
    {
        [YamlMember(Alias = "type")] public string Type { get; set; } = null!;
        
        [YamlMember(Alias = "description")] public string Description { get; set; } = null!;
    }
}
