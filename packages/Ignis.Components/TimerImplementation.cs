namespace Ignis.Components;

internal class TimerImplementation : ITimer
{
    private readonly Timer _timer;

    public TimerImplementation(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
    {
        _timer = new Timer(callback, state, dueTime, period);
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}
