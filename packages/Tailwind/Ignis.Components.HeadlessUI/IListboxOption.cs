namespace Ignis.Components.HeadlessUI;

public interface IListboxOption
{
    bool IsActive { get; }
    
    bool IsSelected { get; }
    
    IReadOnlyDictionary<string, object?> Attributes { get; }
}
