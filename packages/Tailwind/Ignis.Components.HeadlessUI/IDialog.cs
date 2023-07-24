﻿namespace Ignis.Components.HeadlessUI;

public interface IDialog : IDynamicParentComponent<IDialog>, IOpenClose
{
    string Id { get; }
    
    internal void SetTitle(IDialogTitle title);
    
    internal void SetDescription(IDialogDescription description);

    internal void CloseFromTransition(Action? continueWith = null);
}
