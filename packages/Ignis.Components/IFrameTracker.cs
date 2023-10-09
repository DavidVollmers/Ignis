namespace Ignis.Components;

public interface IFrameTracker
{
    bool IsPending { get; }

    void ExecuteOnNextFrame(IgnisComponentBase target, Action action);

    void OnAfterRender();
}
