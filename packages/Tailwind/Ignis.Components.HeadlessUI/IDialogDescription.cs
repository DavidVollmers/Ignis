namespace Ignis.Components.HeadlessUI;

public interface IDialogDescription : IDynamicParentComponent<IDialogDescription>
{
    string? Id { get; set; }
}
