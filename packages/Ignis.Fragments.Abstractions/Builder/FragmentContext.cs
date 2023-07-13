namespace Ignis.Fragments.Abstractions.Builder;

public abstract class FragmentContext
{
    public IReadOnlyDictionary<string, object?> Attributes { get; internal init; } = new Dictionary<string, object?>();
}
