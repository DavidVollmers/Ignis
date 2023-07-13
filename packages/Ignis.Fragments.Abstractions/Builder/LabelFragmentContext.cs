using System.Reflection;

namespace Ignis.Fragments.Abstractions.Builder;

public sealed class LabelFragmentContext : FragmentContext
{
    public PropertyInfo PropertyInfo { get; }

    internal LabelFragmentContext(PropertyInfo propertyInfo)
    {
        PropertyInfo = propertyInfo;
    }
}
