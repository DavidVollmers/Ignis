namespace Ignis.Fragments.Abstractions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
public sealed class AttributeAttribute : Attribute
{
    public string Name { get; }

    public object? Value { get; }

    public AttributeAttribute(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public AttributeAttribute(string name, object? value) : this(name)
    {
        Value = value;
    }
}
