namespace Ignis.Components;

//TODO switch to System.TimeProvider when it's available
// https://learn.microsoft.com/en-us/dotnet/api/system.timeprovider?view=net-8.0
internal abstract class TimeProvider
{
    public virtual ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
    {
        return new TimerImplementation(callback, state, dueTime, period);
    }
}
