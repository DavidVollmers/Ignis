﻿using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface IRadioGroupOption : IDynamicParentComponent<IRadioGroupOption>, IFocus
{
    bool IsActive { get;  }
    
    bool IsChecked { get; }
    
    string? Id { get; set; }
    
    internal void SetLabel(IRadioGroupLabel label);

    internal void Check();
}