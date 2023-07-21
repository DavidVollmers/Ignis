using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeroIcons;

public abstract class HeroIconBase : IgnisRigidComponentBase
{
    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }
}
