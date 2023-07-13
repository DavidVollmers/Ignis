namespace Ignis.Fragments.Abstractions.Builder;

public abstract class FragmentContext
{
    public IReadOnlyDictionary<string, object?> Attributes { get; }

    protected FragmentContext(IReadOnlyDictionary<string, object?> attributes)
    {
        Attributes = attributes;
    }
}
