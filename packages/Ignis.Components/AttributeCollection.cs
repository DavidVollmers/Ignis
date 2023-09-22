using System.Collections;

namespace Ignis.Components;

internal class AttributeCollection : IEnumerable<KeyValuePair<string, object?>>
{
    private readonly Func<KeyValuePair<string, object?>>[] _attributes;

    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    public AttributeCollection(IEnumerable<Func<KeyValuePair<string, object?>>> attributes)
    {
        if (attributes == null) throw new ArgumentNullException(nameof(attributes));

        _attributes = attributes.ToArray();
    }

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        foreach (var attributeDelegate in _attributes)
        {
            var attribute = attributeDelegate();

            if (AdditionalAttributes?.Any(a =>
                    string.Equals(a.Key, attribute.Key, StringComparison.OrdinalIgnoreCase)) == true) continue;

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
