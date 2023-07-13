namespace Ignis.Fragments.Abstractions.Builder;

public sealed class FormFragmentContext
{
    public object Model { get; }

    public Func<object, Task> OnSubmitAsync { get; }

    internal FormFragmentContext(object model, Func<object, Task> onSubmitAsync)
    {
        Model = model;
        OnSubmitAsync = onSubmitAsync;
    }
}