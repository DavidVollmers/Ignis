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

    public ValueTask DisposeAsync()
    {
        Timers.Remove(this);

        return ValueTask.CompletedTask;
    }

    public bool Change(TimeSpan dueTime, TimeSpan period)
    {
#pragma warning disable MA0025
        throw new NotImplementedException();
#pragma warning restore MA0025
    }
}
