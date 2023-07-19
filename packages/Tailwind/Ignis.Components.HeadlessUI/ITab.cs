namespace Ignis.Components.HeadlessUI;

public interface ITab
{
    bool IsSelected { get; }
    
    IReadOnlyDictionary<string, object?> Attributes { get; }
}
