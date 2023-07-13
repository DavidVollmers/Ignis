using System.Reflection;
using Ignis.Fragments.Abstractions;
using Ignis.Fragments.Abstractions.Builder;

namespace Ignis.Fragments;

public static partial class IgnisFragments
{
    private static IFragmentBuilder<T>? TryGetFragmentBuilder<T>(object target) where T : class
    {
        var type = target.GetType();
        var fragmentAttribute = type.GetCustomAttribute<FragmentAttribute>();
        return fragmentAttribute?.Builder as IFragmentBuilder<T>;
    }
}