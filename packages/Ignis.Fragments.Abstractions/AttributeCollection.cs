using System.Collections;

namespace Ignis.Fragments.Abstractions;

public sealed class AttributeCollection : IReadOnlyDictionary<string, object?>
{
    private readonly IReadOnlyDictionary<string, object?> _innerDictionary;

    public object? this[string key]
    {
        get
        {
            return _innerDictionary.TryGetValue(key, out var value) ? value : null;
        }
    }

    public int Count => _innerDictionary.Count;

    public IEnumerable<string> Keys => _innerDictionary.Keys;

    public IEnumerable<object?> Values => _innerDictionary.Values;

    public AttributeCollection(IEnumerable<KeyValuePair<string, object?>> attributes)
    {
        if (attributes == null) throw new ArgumentNullException(nameof(attributes));

        _innerDictionary = attributes.ToDictionary(k => k.Key, v => v.Value, StringComparer.OrdinalIgnoreCase);
    }

    public AttributeCollection()
    {
        _innerDictionary = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
    }

    public bool ContainsKey(string key) => _innerDictionary.ContainsKey(key);

    public bool TryGetValue(string key, out object? value) => _innerDictionary.TryGetValue(key, out value);

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() => _innerDictionary.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
