using Ignis.Fragments.Abstractions.Builder;

namespace Ignis.Fragments.Abstractions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
public sealed class FragmentAttribute : Attribute
{
    public static IFragmentBuilder<T> Empty<T>() where T : class => new EmptyFragmentBuilder<T>();

    private readonly Type? _builderType;
    
    private IFragmentBuilder? _builder;

    public FragmentAttribute(IFragmentBuilder builder) : this(builder?.GetType()!)
    {
        _builder = builder;
    }

    public FragmentAttribute(Type builderType)
    {
        if (builderType == null) throw new ArgumentNullException(nameof(builderType));

        if (!builderType.IsAssignableTo(typeof(IFragmentBuilder<>)))
        {
            throw new ArgumentException(
                $"All fragment builders must implement the type generic {nameof(IFragmentBuilder)} interface.",
                nameof(builderType));
        }

        _builderType = builderType;
    }

    //TODO support dependency injection
    internal IFragmentBuilder? GetBuilder()
    {
        if (_builder != null) return _builder;

        return _builder = (IFragmentBuilder?)Activator.CreateInstance(_builderType!);
    }
}
