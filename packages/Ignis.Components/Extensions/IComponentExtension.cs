namespace Ignis.Components.Extensions;

public interface IComponentExtension
{
    void OnUpdate(IgnisComponentBase component);

    void OnDispose(IgnisComponentBase component);
}
