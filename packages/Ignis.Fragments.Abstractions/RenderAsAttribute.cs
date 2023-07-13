using Ignis.Fragments.Abstractions.Builder;

namespace Ignis.Fragments.Abstractions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
public sealed class RenderAsAttribute : Attribute
{
    private readonly Type? _builderType;

    private IFragmentBuilder? _builder;

    public RenderAsAttribute(Type builderType)
    {
        if (builderType == null) throw new ArgumentNullException(nameof(builderType));

        var isValid = builderType.GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IFragmentBuilder<>));
        if (!isValid)
        {
            throw new ArgumentException(
                $"All fragment builders must implement the type generic {nameof(IFragmentBuilder)} interface.",
                nameof(builderType));
        }

        _builderType = builderType;
    }

    internal IFragmentBuilder? GetBuilder()
    {
        if (_builder != null) return _builder;

        return _builder = (IFragmentBuilder?)Activator.CreateInstance(_builderType!);
    }
}
