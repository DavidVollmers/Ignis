namespace Ignis.Components.HeadlessUI;

public interface IDialog : IDynamicParentComponent<IDialog>, IOpenClose, IContentProvider
{
    internal IDialogDescription? Description { get; }

    internal IDialogTitle? Title { get; }

    string Id { get; }

    internal void SetTitle(IDialogTitle title);

    internal void SetDescription(IDialogDescription description);

    internal void CloseFromTransition(Action? continueWith = null);
}
