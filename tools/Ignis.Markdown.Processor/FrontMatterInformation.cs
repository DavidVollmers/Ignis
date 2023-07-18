using YamlDotNet.Serialization;

namespace Ignis.Markdown.Processor;

internal class FrontMatterInformation
{
    [YamlMember(Alias = "title")] public string Title { get; set; } = null!;
    
    [YamlMember(Alias = "category")] public string Category { get; set; } = null!;
    
    [YamlMember(Alias = "permalink")] public string Permalink { get; set; } = null!;
    
    [YamlMember(Alias = "inject")] public InjectInformation? Inject { get; set; }
    
    [YamlMember(Alias = "api")] public string[]? Api { get; set; }

    public class InjectInformation
    {
        [YamlMember(Alias = "type")] public string Type { get; set; } = null!;
        
        [YamlMember(Alias = "description")] public string Description { get; set; } = null!;
    }
}
