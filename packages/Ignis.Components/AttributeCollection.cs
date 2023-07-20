using System.Collections;

namespace Ignis.Components;

public sealed class AttributeCollection : IEnumerable<KeyValuePair<string, object?>>
{
    private readonly IDictionary<string, object?> _attributes = new Dictionary<string, object?>();

    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    // public object? this[string key]
    // {
    //     get
    //     {
    //         return _attributes.TryGetValue(key, out var value)
    //             ? value
    //             : AdditionalAttributes?.FirstOrDefault(a => a.Key == key).Value;
    //     }
    // }

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        foreach (var attribute in _attributes)
        {
            if (AdditionalAttributes?.Any(a => a.Key == attribute.Key) == true) continue;

            yield return attribute;
        }

        if (AdditionalAttributes == null) yield break;

        foreach (var additionalAttribute in AdditionalAttributes)
        {
            yield return additionalAttribute;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
