namespace Ignis.Fragments.Abstractions.Builder;

public class FormFragmentContext<T> : FragmentContext where T : class
{
    public T Model { get; }

    public Func<T, Task> OnSubmitAsync { get; }

    internal FormFragmentContext(T model, Func<T, Task> onSubmit)
    {
        Model = model;
        OnSubmitAsync = onSubmit;
    }
}
