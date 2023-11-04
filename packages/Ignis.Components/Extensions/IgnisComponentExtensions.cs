namespace Ignis.Components.Extensions;

public static class IgnisComponentExtensions
{
    public static T? GetExtension<T>(this IgnisComponentBase component) where T : class, IComponentExtension
    {
        if (component == null) throw new ArgumentNullException(nameof(component));

        return component.HostContext.ComponentExtensions.OfType<T>().FirstOrDefault();
    }
}
