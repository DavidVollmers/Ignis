using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeroIcons;

public abstract class HeroIconBase : IgnisComponentBase
{
    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }
}
