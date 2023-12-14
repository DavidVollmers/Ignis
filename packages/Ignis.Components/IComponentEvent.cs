namespace Ignis.Components;

public interface IComponentEvent
{
    bool DefaultPrevented { get; }

    void PreventDefault();
}
