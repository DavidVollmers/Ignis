using Ignis.Components;

namespace Ignis.Tests.Common;

public sealed class TestTimer : ITimer
{
    private static readonly List<TestTimer> Timers = new();

    private readonly TimerCallback _callback;

    public TestTimer(TimerCallback callback)
    {
        _callback = callback;

        Timers.Add(this);
    }

    public static void Trigger(object? state)
    {
        foreach (var timer in Timers.ToArray())
        {
            timer._callback(state);
        }
    }

    public void Dispose()
    {
        Timers.Remove(this);
    }
}
