namespace Ignis.Components;

// Substitute for System.Threading.Timer
// https://learn.microsoft.com/en-us/dotnet/api/system.threading.timer?view=net-8.0
#if !NET8_0
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
#endif
