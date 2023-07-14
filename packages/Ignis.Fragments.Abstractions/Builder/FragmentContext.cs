namespace Ignis.Fragments.Abstractions.Builder;

public abstract class FragmentContext
{
    public AttributeCollection Attributes { get; internal init; } = new();
}
