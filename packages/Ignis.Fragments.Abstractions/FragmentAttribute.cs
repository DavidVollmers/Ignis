using Ignis.Fragments.Abstractions.Builder;

namespace Ignis.Fragments.Abstractions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public sealed class FragmentAttribute : Attribute
{
    public IFragmentBuilder Builder { get; }

    public FragmentAttribute(IFragmentBuilder builder)
    {
        Builder = builder ?? throw new ArgumentNullException(nameof(builder));
    }
}