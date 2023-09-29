using Ignis.Components;

namespace Ignis.Tests.Common;

internal class TestTimeProvider : TimeProvider
{
    public override Timer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
    {
        return base.CreateTimer(callback, state, TimeSpan.Zero, TimeSpan.Zero);
    }
}
