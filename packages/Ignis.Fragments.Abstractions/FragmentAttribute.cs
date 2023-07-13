using Ignis.Fragments.Abstractions.Builder;

namespace Ignis.Fragments.Abstractions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
public sealed class FragmentAttribute : Attribute
{
    private static IFragmentBuilder? _emptyFragmentBuilder;
    
    public static IFragmentBuilder Empty() => _emptyFragmentBuilder ??= new EmptyFragmentBuilder();
    
    public static IFragmentBuilder<T> Empty<T>() where T : class => new EmptyFragmentBuilder<T>();

    public IFragmentBuilder Builder { get; }

    public FragmentAttribute(IFragmentBuilder builder)
    {
        Builder = builder ?? throw new ArgumentNullException(nameof(builder));
    }
}