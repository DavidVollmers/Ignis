namespace Ignis.Components;

public interface IHostContext
{
    bool IsPrerendering { get; }

    bool IsServerSide { get; }
}
