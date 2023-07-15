using System.Reflection;

namespace Ignis.Fragments.Abstractions.Builder;

public class LabelFragmentContext : FragmentContext
{
    public PropertyInfo PropertyInfo { get; }

    internal LabelFragmentContext(PropertyInfo propertyInfo)
    {
        PropertyInfo = propertyInfo;
    }
}
