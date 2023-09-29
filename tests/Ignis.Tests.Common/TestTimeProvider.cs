﻿using Ignis.Components;

namespace Ignis.Tests.Common;

internal class TestTimeProvider : TimeProvider
{
    public override ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
    {
        callback(state);
        return new TestTimer();
    }
}
