namespace Ignis.Components;

public interface IDynamicComponent
{
    string? AsElement { get; set; }
    
    Type? AsComponent { get; set; }
}
