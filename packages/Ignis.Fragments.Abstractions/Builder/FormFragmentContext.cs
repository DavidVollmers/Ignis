namespace Ignis.Fragments.Abstractions.Builder;

public sealed class FormFragmentContext : FragmentContext
{
    public object Model { get; }

    public Func<object, Task> OnSubmitAsync { get; }

    internal FormFragmentContext(object model, Func<object, Task> onSubmit)
    {
        Model = model;
        OnSubmitAsync = onSubmit;
    }
}
